namespace _8_Puzzle.ViewModels
{
    /// <summary>
    /// 空白ボタン
    /// </summary>
    internal class BlankButton : PanelButton
    {
        public BlankButton(int position, string text) : base(position, text)
        {
            this.Color = Color.FromRgba(0, 0, 0, 0);
            this.IsEnable = false;
            this.TextColor = Color.FromRgba(0, 0, 0, 0);
        }
    }
}
