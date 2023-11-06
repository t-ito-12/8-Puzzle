namespace _8_Puzzle.ViewModels
{
    /// <summary>
    /// パネルボタンクラス
    /// </summary>
    internal class PanelButton
    {
        /// <summary>
        /// ボタン本体
        /// </summary>
        public Button Button { get; set; } = new Button();

        /// <summary>
        /// ボタンの位置
        /// </summary>
        public int Position { get; set; } = 0;

        /// <summary>
        /// ボタンの色
        /// </summary>
        public Color Color { get; set; } = Colors.SkyBlue;

        /// <summary>
        /// ボタンを利用可能か
        /// </summary>
        /// <remarks>true:利用可能 false:利用不可</remarks>
        public bool IsEnable { get; set; } = true;

        /// <summary>
        /// ボタンテキスト
        /// </summary>
        public string Text
        {
            get
            {
                return Button.Text;
            }
            set { Button.Text = value; }
        }

        /// <summary>
        /// ボタンテキストの色
        /// </summary>
        public Color TextColor { get; set; } = Colors.Black;

        /// <summary>
        /// <see cref="PanelButton"/>のコンストラクタ
        /// </summary>
        /// <param name="position">パネルボタンの位置</param>
        /// <param name="text">パネルボタンのテキスト</param>
        public PanelButton(int position, string text) 
        {
            this.Position = position;
            this.Text = text;
        }
    }
}
