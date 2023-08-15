using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTBlazor.RTTextBox;

namespace RTBlazorTests.RTTextBox
{
    [TestClass()]
    public class HtmlParserTests
    {
        [TestMethod]
        public void Parse_ReturnsSortedListOfSpans()
        {
            // Arrange
            var htmlMarkup = new MarkupString("<span style=\"color: #FF0000; \"><span style=\"text-decoration-line: underline;\">" +
                                              "Italic</span> and <span style=\"font-weight: bold; \">Bold</span></span>");
            
            var expectedSpans = new SortedList<int, StyledSpan>
            {
                {0, new StyledSpan("Italic", 0, new Style(TextDecorationType.Underline, 16, color: "#FF0000"))},
                {6, new StyledSpan(" and ", 6, new Style(TextDecorationType.None, 16, color: "#FF0000"))},
                {11, new StyledSpan("Bold", 11, new Style(TextDecorationType.Bold, 16, color: "#FF0000"))}
            };

            // Act
            var actualSpans = HtmlParser.Parse(htmlMarkup);

            // Assert
            CollectionAssert.AreEqual(expectedSpans, actualSpans);
        }
    }
}