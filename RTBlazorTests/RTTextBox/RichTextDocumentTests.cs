using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTBlazor.RTTextBox;

namespace RTBlazorTests.RTTextBox
{
    [TestClass()]
    public class RichTextDocumentTests
    {
        [TestMethod]
        public void InsertSpan_SpanExists_SpanIsUpdated()
        {
            var richTextDocument = new RichTextDocument();
            richTextDocument.Spans.Add(0, new StyledSpan("Test", 0, new Style(TextDecorationType.None, 12, "#FF0000")));
            var style = new Style(TextDecorationType.Underline, 14, "#000000");
            richTextDocument.InsertSpan(0, style);
            Assert.AreEqual(1, richTextDocument.Spans.Count);
            Assert.AreEqual("Test", richTextDocument.Spans[0].Text);
            Assert.AreEqual(style, richTextDocument.Spans[0].Style);
        }

        [TestMethod]
        public void InsertSpan_SpanDoesNotExist_SpanIsAdded()
        {
            var richTextDocument = new RichTextDocument();
            var style = new Style(TextDecorationType.Underline, 14, "#000000");
            richTextDocument.InsertSpan(0, style);
            Assert.AreEqual(1, richTextDocument.Spans.Count);
            Assert.AreEqual("", richTextDocument.Spans[0].Text);
            Assert.AreEqual(style, richTextDocument.Spans[0].Style);
        }

        [TestMethod]
        public void InsertSpan_SpanDoesNotExist_SpanIsAddedAtCorrectIndex()
        {
            var richTextDocument = new RichTextDocument();
            var style = new Style(TextDecorationType.Underline, 14, "#000000");
            richTextDocument.InsertSpan(0, style);
            Assert.AreEqual(1, richTextDocument.Spans.Count);
            Assert.AreEqual("", richTextDocument.Spans[0].Text);
            Assert.AreEqual(style, richTextDocument.Spans[0].Style);
        }

        [TestMethod()]
        public void ToHtmlTest()
        {
            var richTextDocument = new RichTextDocument();
            richTextDocument.Spans.Add(0, new StyledSpan("Test", 0, new Style(TextDecorationType.None, 12, "#FF0000")));
            richTextDocument.Spans.Add(4, new StyledSpan("Underlined", 4, new Style(TextDecorationType.Underline, 14, "#000000")));
            richTextDocument.Spans.Add(14, new StyledSpan("StrikeThrough", 14, new Style(TextDecorationType.StrikeThrough, 16, "#0000FF")));

            var result = richTextDocument.ToHtml();

            Assert.AreEqual("<span style=\"font-size: 12px; color: #FF0000; \">Test</span>" +
                            "<span style=\"font-size: 14px; color: #000000; \">" +
                            "<span style=\"text-decoration-line: underline;\">" +
                            "Underlined</span></span>" +
                              "<span style=\"font-size: 16px; color: #0000FF; \">" +
                              "<span style=\"text-decoration-line: line-through;\">" +
                              "StrikeThrough</span></span>", result.Value);
        }
    }
}