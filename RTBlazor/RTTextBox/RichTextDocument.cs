using Microsoft.AspNetCore.Components;

namespace RTBlazor.RTTextBox
{
    public class RichTextDocument
    {
        public SortedList<int, StyledSpan> Spans { get; set; }
        public string? Html { get; private set; }

        public RichTextDocument()
        {
            Spans = new SortedList<int, StyledSpan>();
        }

        // Convert from HTML to RichTextDocument
        public RichTextDocument(MarkupString htmlMarkup)
        {
            Spans = HtmlParser.Parse(htmlMarkup);
        }

        // Convert from RichTextDocument to HTML
        public MarkupString ToHtml()
        {
            var val = "";
            foreach (var span in Spans.Values)
            {
                val += span.Style.ToHtml(span.Text);
            }

            Html = val;

            return new MarkupString(val);
        }

        public void UpdateSpans(MarkupString htmlMarkup)
        {
            Spans = HtmlParser.Parse(htmlMarkup);
        }

        public void InsertSpan(int textIndex, Style style)
        {
            if (Spans.TryGetValue(textIndex, out var span))
            {
                span.Style = style;
            }
            else
            {
                Spans.Add(textIndex, new StyledSpan("", textIndex, style));
            }
        }
        
        public void RemoveText(int startIndex, int endIndex)
        {
            foreach (var span in GetIncludedSpans(startIndex, endIndex))
            {
                var spanStartIndex = span.Index;
                var spanEndIndex = spanStartIndex + span.Text.Length;
                
                if (spanStartIndex < startIndex && spanEndIndex > endIndex)
                {
                    span.RemoveText(startIndex, endIndex);
                }
                else if (spanStartIndex < startIndex)
                {
                    span.Text = span.Text[..(startIndex - spanStartIndex)];
                }
                else if (spanEndIndex > endIndex)
                {
                    span.Text = span.Text[(endIndex - spanStartIndex)..];
                }
                else
                {
                    Spans.Remove(span.Index);
                }
            }
        }

        public void ApplyStyle(int startIndex, int endIndex, Style style, bool invert = false)
        {
            var allHaveStyle = true; // If all the spans already have the style

            var spans = GetIncludedSpans(startIndex, endIndex);

            foreach (var span in spans)
            {
                if (!invert && span.Style.HasStyle(style)) // If the span already has the style,
                    continue;

                allHaveStyle = false;

                var spanStartIndex = span.Index;
                var spanEndIndex = spanStartIndex + span.Text.Length;

                Spans.Remove(span.Index);

                if (spanStartIndex < startIndex && spanEndIndex > endIndex)
                {
                    var span1 = new StyledSpan(span.Text[..(startIndex - spanStartIndex)],
                        spanStartIndex, span.Style);

                    var span2 = new StyledSpan(span.Text[(startIndex - spanStartIndex)..(endIndex - spanStartIndex)],
                        startIndex, invert ? span.Style.Subtract(style) : span.Style.Add(style)); 

                    var span3 = new StyledSpan(span.Text[(endIndex - spanStartIndex)..],
                        endIndex, span.Style);

                    Spans.Add(span1.Index, span1);
                    Spans.Add(span2.Index, span2);
                    Spans.Add(span3.Index, span3);
                }
                else if (spanStartIndex < startIndex)
                {
                    var span1 = new StyledSpan(span.Text[..(startIndex - spanStartIndex)],
                        spanStartIndex, span.Style);

                    var span2 = new StyledSpan(span.Text[(startIndex - spanStartIndex)..],
                        startIndex, invert ? span.Style.Subtract(style) : span.Style.Add(style));

                    Spans.Add(span1.Index, span1);
                    Spans.Add(span2.Index, span2);
                }
                else if (spanEndIndex > endIndex)
                {
                    var span1 = new StyledSpan(span.Text[..(endIndex - spanStartIndex)], 
                        spanStartIndex, invert ? span.Style.Subtract(style) : span.Style.Add(style));

                    var span2 = new StyledSpan(span.Text[(endIndex - spanStartIndex)..], 
                        endIndex, span.Style);
                    
                    Spans.Add(span1.Index, span1);
                    Spans.Add(span2.Index, span2);
                }
                else
                {
                    var newSpan = new StyledSpan(span.Text, span.Index, invert ? span.Style.Subtract(style) : span.Style.Add(style));
                    Spans.Add(newSpan.Index, newSpan);
                }
            }

            if (allHaveStyle)
            {
                ApplyStyle(startIndex, endIndex, style, true);
            }
        }

        public List<StyledSpan> GetIncludedSpans(int startIndex, int endIndex)
        {
            var spanIndex = Spans.Keys.LastOrDefault(x => x <= startIndex);
            var includedSpans = Spans.Where((i) => (i.Key < endIndex && i.Key >= spanIndex)).ToList();

            return includedSpans.Select(x => x.Value).ToList();
        }

        public Style GetStyleApplied(int startIndex, int endIndex)
        {
            var spans = GetIncludedSpans(startIndex, endIndex);

            if(spans.Count == 0) return new Style();

            var style = new Style(spans[0].Style);

            var sameFont = true;
            var sameColor = true;

            foreach (var span in spans)
            {
                if (span.Style.HasStyle(style)) continue;

                style.TextDecoration &= span.Style.TextDecoration;

                if(span.Style.FontSize != style.FontSize) sameFont = false;
                if(span.Style.FontColor != style.FontColor) sameColor = false;
            }

            if (!sameFont)
                style.FontSize = null;

            if (!sameColor)
                style.FontColor = null;

            return style;
        }

        public override string ToString()
        {
            var val = "";
            foreach (var styledSpan in Spans)
            {
                val += styledSpan + " ";
            }
            return val;
        }
    }

    public class StyledSpan
    {
        public string Text { get; set; }
        public int Index { get; set; }
        public Style Style { get; set; }

        public StyledSpan(string text, int index, string style)
        {
            Text = text;
            Index = index;
            Style = new Style(style);
        }

        public StyledSpan(string text, int index, Style style)
        {
            Text = text;
            Index = index;
            Style = style;
        }

        public void InsertText(string text, int startIndex)
        {
            if (startIndex >= 0 && startIndex <= Text.Length)
            {
                Text = Text.Insert(startIndex, text);
            }
            else
            {
                // Handle the invalid startIndex value.
                // For example, you can throw an ArgumentOutOfRangeException:
                throw new ArgumentOutOfRangeException(nameof(startIndex), @"The start Index value is out of range.");
            }
        }

        public void RemoveText(int startIndex, int endIndex)
        {
            Text = Text.Remove(startIndex - Index, endIndex - startIndex);
        }

        public override string ToString()
        {
            return $"[{Text} ({Index}, {Style})]";
        }

        public override bool Equals(object? obj)
        {
            if (obj is StyledSpan span)
            {
                return span.Text == Text && span.Index == Index && span.Style.Equals(Style);
            }
            else
            {
                return false;
            }
        }
    }
}
