using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTBlazor.RTTextBox;

namespace RTBlazorTests.RTTextBox
{
    [TestClass()]
    public class StyleTests
    {
        #region ToHtml
        [TestMethod]
        public void ToHtml_NonUnderlinedNonStrikeThroughStyle_ReturnsCorrectHtml()
        {
            var style = new Style(TextDecorationType.None, 12, "#FF0000");
            var content = "Test";

            var result = style.ToHtml(content);

            Assert.AreEqual("<span style=\"font-size: 12px; color: #FF0000; \">Test</span>", result);
        }

        [TestMethod]
        public void ToHtml_UnderlinedStyle_ReturnsCorrectHtml()
        {
            var style = new Style(TextDecorationType.Underline, 14, "#000000");
            var content = "Underlined";

            var result = style.ToHtml(content);

            Assert.AreEqual("<span style=\"font-size: 14px; color: #000000; \">" +
                            "<span style=\"text-decoration-line: underline;\">" +
                "Underlined</span></span>", result);
        }

        [TestMethod]
        public void ToHtml_StrikeThroughStyle_ReturnsCorrectHtml()
        {
            var style = new Style(TextDecorationType.StrikeThrough, 16, "#0000FF");
            var content = "StrikeThrough";

            var result = style.ToHtml(content);

            Assert.AreEqual("<span style=\"font-size: 16px; color: #0000FF; \">" +
                            "<span style=\"text-decoration-line: line-through;\">" +
                "StrikeThrough</span></span>", result);
        }

        [TestMethod]
        public void ToHtml_AllNullExceptDecoration_ReturnsCorrectHtml()
        {
            var style = new Style(TextDecorationType.StrikeThrough, null, null);
            var content = "Test";

            var result = style.ToHtml(content);

            Assert.AreEqual("<span style=\"text-decoration-line: line-through;\">Test</span>", result);
        }

        #endregion

        #region ExtractStyle
        [TestMethod]
        public void ExtractStyle_NonEmptyStyleString_StylePropertiesSetCorrectly()
        {
            var style = new Style();
            var styleString = "font-weight: bold; font-size: 16pt; color: #FF00FF;" +
                "text-decoration-line: underline; text-decoration-line: line-through;";

            style.ExtractStyle($"span style=\"{styleString}\"");

            Assert.AreEqual(style.TextDecoration, TextDecorationType.Underline | TextDecorationType.StrikeThrough
                | TextDecorationType.Bold);
            Assert.AreEqual(style.FontSize, 16);
            Assert.AreEqual(style.FontColor, "#FF00FF");
        }
        #endregion

        #region HasStyle
        [TestMethod]
        public void HasStyle_StylesHaveDifferentFontSizes_ReturnsFalse()
        {
            var style1 = new Style(TextDecorationType.Bold | TextDecorationType.Italic, 14, "#0000FF");
            var style2 = new Style(TextDecorationType.Bold | TextDecorationType.Italic, 12, "#0000FF");

            var result = style1.HasStyle(style2);

            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void HasStyle_StylesHaveDifferentFontColors_ReturnsFalse()
        {
            var style1 = new Style(TextDecorationType.Bold | TextDecorationType.Italic, 14, "#00FF00");
            var style2 = new Style(TextDecorationType.Bold | TextDecorationType.Italic, 14, "#0000FF");

            var result = style1.HasStyle(style2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasStyle_StylesHaveDifferentDecorations_ReturnsFalse()
        {
            var style1 = new Style(TextDecorationType.Bold | TextDecorationType.Italic, 14, "#FF0000");
            var style2 = new Style(TextDecorationType.StrikeThrough, 14, "#FF0000");

            var result = style1.HasStyle(style2);

            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void HasStyle_StyleHaveNoneDecorations_ReturnsFalse()
        {
            var style1 = new Style(TextDecorationType.Bold | TextDecorationType.Italic, 14, "#FF0000");
            var style2 = new Style(TextDecorationType.None, 14, "#FF0000");

            var result = style1.HasStyle(style2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasStyle_StylesHaveSimilarProperties_ReturnsTrue()
        {
            var style1 = new Style(TextDecorationType.Bold | TextDecorationType.Italic, 14, "#FF0000");
            var style2 = new Style(TextDecorationType.Bold, 14, "#FF0000");

            var result = style1.HasStyle(style2);
            
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasStyle_StylesHaveNullProperties_ReturnsTrue()
        {
            var style1 = new Style(null, 14, "#FF0000");
            var style2 = new Style(TextDecorationType.Italic, null, null);
            var result = style1.HasStyle(style2);
            Assert.IsTrue(result);
        }
        #endregion

        #region Add
        [TestMethod]
        public void Add_TwoStylesHaveDifferentFontSizes_ReturnsCorrectStyle()
        {
            var style1 = new Style(TextDecorationType.Bold | TextDecorationType.Italic, 14, "#FF0000");
            var style2 = new Style(null, 12, null);

            var result = style1.Add(style2);

            Assert.AreEqual(result.FontSize, 12);
            Assert.IsFalse(result.TextDecoration == TextDecorationType.None);
            Assert.AreEqual(result.FontColor, "#FF0000");
        }
        #endregion

        #region Subtract
        [TestMethod]
        public void Invert_TwoStylesHaveDifferentFontDecorations_ReturnsCorrectStyle()
        {
            var style1 = new Style(TextDecorationType.Underline | TextDecorationType.Bold | TextDecorationType.Italic,
                14, "#FF0000");
            var style2 = new Style(TextDecorationType.Bold, 14, "#000000");

            var result = style1.Subtract(style2);

            Assert.AreEqual(result.TextDecoration, TextDecorationType.Underline | TextDecorationType.Italic);
            Assert.AreEqual(result.FontSize, 14);
            Assert.AreEqual(result.FontColor, "#FF0000");
        }
        #endregion

        #region Equals
        [TestMethod]
        public void Equals_StylesHaveSameProperties_ReturnsTrue()
        {
            var style1 = new Style(TextDecorationType.StrikeThrough, 10, "#0000FF");
            var style2 = new Style(TextDecorationType.StrikeThrough, 10, "#0000FF");

            var result = style1.Equals(style2);

            Assert.IsTrue(result);
        }
        #endregion
    }
}