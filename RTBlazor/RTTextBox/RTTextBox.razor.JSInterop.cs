using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RTBlazor.RTTextBox
{
    public partial class RTTextBox
    {
        [Inject]
        private IJSRuntime? JsRuntime { get; set; }

        private IJSObjectReference? _jsModule;

        private async Task InitializeJs()
        {
            if (JsRuntime is null)
            {
                throw new NullReferenceException("JsRuntime is null");
            }

            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./RTTextBox/RTTextBox.razor.js");
        }

        public async Task UpdateHtml(string html, ElementReference element)
        {
            await _jsModule.InvokeVoidAsync("updateInnerHTML", element, html);
        }

        public async Task<string> GetHtml(ElementReference element)
        {
            return CleanHtml(await _jsModule.InvokeAsync<string>("getInnerHTML", element));
        }

        public async Task<SelectionRange> GetSelectedText(ElementReference element)
        {
            return await _jsModule.InvokeAsync<SelectionRange>("getSelectionRangeWithin", element);
        }

        public async Task SetSelectedText(ElementReference element, SelectionRange range)
        {
            await _jsModule.InvokeVoidAsync("selectText", element, range.StartOffset, range.EndOffset);
        }

        private string CleanHtml(string html)
        {
            return html.Replace("<!--!-->", "").Replace("&nbsp;", "").Trim();
        }

        public async Task SetClickListener(ElementReference element)
        {
            await _jsModule.InvokeVoidAsync("addClickEventListener", element);
        }
    }
}