using Microsoft.AspNetCore.Components;
using System.Text;

namespace RTBlazor.RTTextBox;

public class HtmlParser
{
    /// <summary>
    /// Converts HTML to a sorted list of spans
    /// </summary>
    /// <param name="htmlMarkup"></param>
    /// <returns></returns>
    public static SortedList<int, StyledSpan> Parse(MarkupString htmlMarkup)
    {
        var spans = new SortedList<int, StyledSpan>();

        var tags = new Stack<string>();

        var html = htmlMarkup.Value;

        var index = 0;
        var textIndex = 0;

        while (index < html.Length)
        {
            if (html[index] == '<')
            {
                // Get the tag name
                var tagName = "";

                index++;
                while (html[index] != '>')
                {
                    tagName += html[index];
                    index++;
                }

                index++;

                // Check if the tag is a closing tag
                if (tagName[0] == '/')
                {
                    // Remove the tag from the stack
                    tags.Pop();
                }
                else
                {
                    // Add the tag to the stack
                    tags.Push(tagName);
                }
            }
            else
            {
                var style = new Style();

                foreach (var tag in tags)
                {
                    style = style.Add(new Style(tag));
                }

                if (spans.Count == 0 || !style.Equals(spans.Last().Value.Style))
                {
                    spans.Add(textIndex, new StyledSpan("", textIndex, style));
                }

                // Get the text
                var text = "";
                while (index < html.Length && html[index] != '<')
                {
                    text += html[index];
                    index++;
                    textIndex++;
                }

                var lastSpan = spans.Last(); ;

                var startIndex = lastSpan.Key + lastSpan.Value.Text.Length;

                // Get the index of the span that contains the start index
                var spanIndex = spans.Keys.LastOrDefault(x => x <= startIndex);
                var span = spans[spanIndex];

                span.InsertText(text, startIndex - spanIndex);
            }
        }

        return spans;
    }

    //public static Dictionary<int, StyledSpan> Parse(MarkupString htmlMarkup)
    //{
    //    var spans = new Dictionary<int, StyledSpan>();
    //    var tags = new Stack<string>();
    //    var html = htmlMarkup.Value;
    //    var index = 0;
    //    var textIndex = 0;
    //    var style = new Style();

    //    while (index < html.Length - 1)
    //    {
    //        if (html[index] == '<')
    //        {
    //            // Get the tag name
    //            var tagName = "";
    //            index++;
    //            while (html[index] != '>')
    //            {
    //                tagName += html[index];
    //                index++;
    //            }

    //            index++;

    //            // Check if the tag is a closing tag
    //            if (tagName[0] == '/')
    //            {
    //                // Remove the tag from the stack
    //                tags.Pop();
    //                style = GetStyle(tags);
    //            }
    //            else
    //            {
    //                // Add the tag to the stack
    //                tags.Push(tagName);
    //                style = GetStyle(tags);
    //            }
    //        }
    //        else
    //        {
    //            if (spans.Count == 0 || !style.Equals(spans.Last().Value.Style))
    //            {
    //                spans.Add(textIndex, new StyledSpan("", textIndex, style));
    //            }

    //            // Get the text
    //            var textBuilder = new StringBuilder();
    //            while (index < html.Length - 1 && html[index] != '<')
    //            {
    //                textBuilder.Append(html[index]);
    //                index++;
    //                textIndex++;
    //            }

    //            var lastSpan = spans.LastOrDefault();
    //            var startIndex = lastSpan.Value?.Index + lastSpan.Value?.Text.Length ?? 0;

    //            lastSpan.Value?.InsertText(textBuilder.ToString(), startIndex);

    //        }

    //        index++;
    //    }

    //    return spans;
    //}

    //private static Style GetStyle(Stack<string> tags)
    //{
    //    var style = new Style();

    //    foreach (var tag in tags)
    //    {
    //        style = style.Add(new Style(tag));
    //    }

    //    return style;
    //}
}