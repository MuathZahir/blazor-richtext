using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace RTBlazor.RTTextBox
{
    public partial class RTTextBox
    {
        [Parameter] 
        public RenderFragment ChildContent { get; set; }

        public RichTextDocument Document { get; private set; } = new();
        public ElementReference PageReference { get; set; }

        public List<TextDecoration> TextDecorations { get; set; } = new();

        public string SelectedColor { get; set; } = "#6B6B6B";

        public int SelectedFontSize { get; set; } = 16;
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await InitializeJs();
                Document = new RichTextDocument();
            }
        }

        internal void AddDecoration(TextDecoration decoration)
        {
            TextDecorations.Add(decoration);
        }

        private async Task UpdateDecorationButtons(SelectionRange? range = null)
        {
            var style = await GetAppliedStyles(range ?? null);

            foreach (var decoration in TextDecorations)
            {
                decoration.IsAppliedToSelection = style.TextDecoration != TextDecorationType.None
                                  && style.TextDecoration?.HasFlag(decoration.TextDecorationType) == true;
                decoration.Update.Invoke();
            }

            SelectedColor = style.FontColor ?? "#6B6B6B";

            SelectedFontSize = style.FontSize ?? 16;
        }

        private async Task OnKeyClicked(KeyboardEventArgs e)
        {
            if (e.Code is "ArrowLeft" or "ArrowRight" or "ArrowUp" or "ArrowDown")
            {
                await UpdateDecorationButtons();
            }
        }

        private void ResetDecorationButtons()
        {
            foreach (var decoration in TextDecorations)
            {
                decoration.IsAppliedToSelection = false;
            }
        }

    }
}