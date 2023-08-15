namespace RTBlazor.RTTextBox
{
    public class TextDecoration
    {
        public TextDecorationType TextDecorationType { get; }
        public string Icon { get; }
        public bool IsAppliedToSelection { get; set; }
        public Action Update { get; set; }

        public TextDecoration(TextDecorationType textDecorationType, string icon)
        {
            TextDecorationType = textDecorationType;
            Icon = icon;
        }
    }
}
