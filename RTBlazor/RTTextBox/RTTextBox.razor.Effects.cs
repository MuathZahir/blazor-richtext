using Microsoft.AspNetCore.Components;

namespace RTBlazor.RTTextBox
{
    public partial class RTTextBox
    {
        public async Task ApplyStyle(Style style)
        {
            var range = await GetSelectedText(PageReference);
            var html = await GetHtml(PageReference);
            
            if(Document.Html != html)
                Document.UpdateSpans(new MarkupString(html));

            Document.ApplyStyle(range.StartOffset, range.EndOffset, style);

            await UpdateHtml(Document.ToHtml().Value, PageReference);

            await PageReference.FocusAsync();
            await SetSelectedText(PageReference, range);
            await UpdateDecorationButtons(range);
        }

        public async Task<Style> GetAppliedStyles(SelectionRange? selection = null)
        {
            var range = selection ?? await GetSelectedText(PageReference);
            var html = await GetHtml(PageReference);

            if (Document.Html != html)
                Document.UpdateSpans(new MarkupString(html));

            return Document.GetStyleApplied(range.StartOffset, range.EndOffset);
        }
    }
}
