using System.Drawing;

namespace RTBlazor.RTTextBox
{
    public class Style
    {
        public TextDecorationType? TextDecoration { get; set; }
        public int? FontSize { get; set; }
        public string? FontColor { get; set; }

        public Style()
        {
            TextDecoration = TextDecorationType.None;
            FontSize = 16;
            FontColor = "#000000";
        }

        public Style(TextDecorationType? textDecoration = null, int? fontSize = null, string? color = null)
        {
            TextDecoration = textDecoration;
            FontSize = fontSize;
            FontColor = color;
        }

        public Style(string style) : this()
        {
            ExtractStyle(style);
        }

        public Style(Style style)
        {
            TextDecoration = style.TextDecoration;
            FontSize = style.FontSize;
            FontColor = style.FontColor;
        }

        // Convert to html
        public string ToHtml(string content)
        {
            if(content == "") return "";

            var spanNumber = 1;
            var html = "<span style=\"";
            var isEmpty = true;

            if (TextDecoration != null)
            {
                if (TextDecoration.Value.HasFlag(TextDecorationType.Bold))
                {
                    html += "font-weight: bold; ";
                    isEmpty = false;
                }

                if (TextDecoration.Value.HasFlag(TextDecorationType.Italic))
                {
                    html += "font-style: italic; ";
                    isEmpty = false;
                }
            }

            if (FontSize != null)
            {
                html += $"font-size: {FontSize.Value}px; ";
                isEmpty = false;
            }

            if (FontColor != null)
            {
                html += "color: " + FontColor + "; ";
                isEmpty = false;
            }

            html += "\">";

            if (isEmpty)
            {
                html = "";
                spanNumber--;
            }

            if (TextDecoration != null)
            {
                // Can't be added together in the same span
                if (TextDecoration.Value.HasFlag(TextDecorationType.Underline))
                {
                    html += "<span style=\"text-decoration-line: underline;\">";
                    spanNumber++;
                }

                if (TextDecoration.Value.HasFlag(TextDecorationType.StrikeThrough))
                {
                    html += "<span style=\"text-decoration-line: line-through;\">";
                    spanNumber++;
                }

            }

            html += content;

            for (int i = 0; i < spanNumber; i++)
            {
                html += "</span>";
            }

            return html;
        }

        // Convert from HTML to Style
        public void ExtractStyle(string style)
        {
            if (style == "") return;

            // remove style=" and "
            style = style[12..].Replace("\"", "");
            var styles = style.Split(";");
            styles = styles[..^1];
            if (styles.Length == 0)
            {
                return;
            }

            foreach (var s in styles)
            {
                var split = s.Split(":");
                var key = split[0].Trim();
                var value = split[1].Trim();
                switch (key)
                {
                    case "font-weight":
                        if (value == "bold")
                        {
                            TextDecoration |= TextDecorationType.Bold;
                        }
                        break;
                    case "font-style":
                        if (value == "italic")
                        {
                            TextDecoration |= TextDecorationType.Italic;
                        }
                        break;
                    case "font-size":
                        FontSize = int.Parse(value[..^2]);
                        break;
                    case "color":
                        FontColor = value;
                        break;
                }
            }

            if (style.Contains("text-decoration-line: underline;"))
            {
                TextDecoration |= TextDecorationType.Underline;
            }

            if (style.Contains("text-decoration-line: line-through;"))
            {
                TextDecoration |= TextDecorationType.StrikeThrough;
            }
        }

        // Add two styles together
        public Style Add(Style other)
        {
            return new Style(
                TextDecoration | (other.TextDecoration ?? TextDecorationType.None),
                other.FontSize ?? FontSize,
                other.FontColor ?? FontColor
                );
        }

        // Subtract two styles
        public Style Subtract(Style other)
        {
            return new Style(TextDecoration ^ other.TextDecoration, FontSize, FontColor);
        }
        
        public bool HasStyle(Style style)
        {
            // Check if font size is equal, or both null
            if (FontSize.HasValue && style.FontSize.HasValue && FontSize.Value != style.FontSize.Value)
                return false;

            // Check if font color is equal, or both null
            if (FontColor != null && style.FontColor != null && FontColor != style.FontColor)
                return false;

            // Check if the calling object has all the flags in the passed object's text decoration
            if (TextDecoration != null && style.TextDecoration != null)
            {
                if(style.TextDecoration == TextDecorationType.None)
                    return TextDecoration == TextDecorationType.None;
                
                return TextDecoration.Value.HasFlag(style.TextDecoration.Value);
            }

            // All properties match or are null, return true
            return true;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Style style)
            {
                return TextDecoration == style.TextDecoration && FontSize == style.FontSize && FontColor == style.FontColor;
            }
            return false;
        }

        private static string HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }

    [Flags]
    public enum TextDecorationType
    {
        None = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        StrikeThrough = 8,
    }
}
