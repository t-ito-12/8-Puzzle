using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace _8_Puzzle.ViewModels
{
    /// <summary>
    /// 8パズルのメインViewModel
    /// </summary>
    internal class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 盤面の大きさ
        /// </summary>
        /// <remarks>
        /// 8パズルなので大きさは3*3=9
        /// </remarks>
        public const int SIZE = 9;

        /// <summary>
        /// 盤面上の<see cref="PanelButton"/>のコレクション
        /// </summary>
        public ObservableCollection<PanelButton> PanelButtons { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// パネルボタンの押下に対応するコマンド
        /// </summary>
        public ICommand ButtonClick { get; set; }

        /// <summary>
        /// パネルボタンとそれに隣接するパネルボタンのDictionary
        /// </summary>
        /// <remarks>
        /// パネルボタンの配置は以下 <br/>
        /// 0 1 2 <br/>
        /// 3 4 5 <br/>
        /// 6 7 8 
        /// </remarks>
        public static IDictionary<int, IList<int>> AdjacentPanelDictionary { get; } = new Dictionary<int, IList<int>>();

        /// <summary>
        /// <see cref="MainViewModel"/>のコンストラクタ
        /// </summary>
        public MainViewModel()
        {
            InitAdjacentPanelDictionary(SIZE);
            var puzzle = CreateNPuzzle(SIZE);

            PanelButtons = new ObservableCollection<PanelButton>();
            for (int i = 0; i < SIZE; i++)
            {
                // 末尾のパネルボタンは空白ボタン
                if (i == SIZE - 1) { PanelButtons.Add(new BlankButton(i, (puzzle[i] + 1).ToString())); }

                PanelButtons.Add(new PanelButton(i, (puzzle[i] + 1).ToString()));
            }

            ButtonClick = new Command(
                execute: (s) =>
                {
                    var button = s as PanelButton;
                    if (button == null) { return; }
                    var index = PanelButtons.IndexOf(button);
                    BlankButton blankButton;
                    if (!TryGetAdjacentBlankButtonIfExsists(index, out blankButton)) { return; }
                    MovePanelButton(button, blankButton);
                    OnPropertyChanged(nameof(Button));
                    if (IsPuzzleSolved(SIZE))
                    {
                        // パズル完成時にメッセージを表示する
                        Application.Current.MainPage.DisplayAlert("Congratulations", "パズルが完成しました！", "OK");
                    }
                }
            );
        }

        /// <summary>
        /// 指定した位置に隣接した<see cref="BlankButton"/>を取得する
        /// </summary>
        /// <param name="index"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        private bool TryGetAdjacentBlankButtonIfExsists(int index, out BlankButton button)
        {
            var adjacentPositions = AdjacentPanelDictionary[index];
            var adjacentButtons = new List<PanelButton>();
            foreach (var adjacentPosition in adjacentPositions)
            {
                adjacentButtons.Add(PanelButtons.FirstOrDefault(x => x.Position == adjacentPosition));
            }
            var blankButton = adjacentButtons.FirstOrDefault(x => x is BlankButton);
            button = blankButton as BlankButton;
            return button != null;
        }

        /// <summary>
        /// パネルボタンを移動する(パネルボタンの位置を<see cref="BlankButton"/>と交換する)
        /// </summary>
        /// <param name="panelButton">対象のボタン</param>
        /// <param name="blankButton">空白ボタン</param>
        private void MovePanelButton(PanelButton panelButton, PanelButton blankButton)
        {
            var srcIndex = PanelButtons.IndexOf(panelButton);
            var srcPosition = panelButton.Position;
            var destIndex = PanelButtons.IndexOf(blankButton);
            var destPosition = blankButton.Position;
            PanelButtons[srcIndex] = blankButton;
            PanelButtons[destIndex] = panelButton;
            PanelButtons[srcIndex].Position = srcIndex;
            PanelButtons[destIndex].Position = destIndex;
        }

        /// <summary>
        /// <see cref="AdjacentPanelDictionary"/>を初期化する
        /// </summary>
        /// <param name="size">盤面の大きさ</param>
        private void InitAdjacentPanelDictionary(int size)
        {
            for (int i = 0; i < size; i++)
            {
                var adjacentPanels = new List<int>();

                // 隣接する最大4つのパネルボタンをリストに追加する
                if (i + 1 < size) { adjacentPanels.Add(i + 1); }
                if (i - 1 >= 0) { adjacentPanels.Add(i - 1); }
                if (i + Math.Sqrt(SIZE) < size) { adjacentPanels.Add(i + Convert.ToInt32(Math.Sqrt(SIZE))); }
                if (i - Math.Sqrt(SIZE) >= 0) { adjacentPanels.Add(i - Convert.ToInt32(Math.Sqrt(SIZE))); }

                AdjacentPanelDictionary.Add(i, adjacentPanels);
            }
        }

        /// <summary>
        /// Nパズルを作成する
        /// </summary>
        /// <param name="size">盤面のサイズ</param>
        /// <returns>作成したNパズル</returns>
        private IList<int> CreateNPuzzle(int size)
        {
            var puzzle = new List<int>();
            var orderedNumber = new List<int>();
            while (!CanSolveNPuzzle(puzzle, size))
            {
                puzzle = new List<int>();
                orderedNumber = new List<int>();
                for (int i = 0; i < size; i++) { orderedNumber.Add(i); }
                var rand = new Random();
                while (orderedNumber.Count > 0)
                {
                    var index = rand.Next(0, orderedNumber.Count);
                    puzzle.Add(orderedNumber[index]);
                    orderedNumber.RemoveAt(index);
                }
                var blankIndex = puzzle.IndexOf(size - 1);
                var endPanelButton = puzzle[size - 1];
                puzzle[size - 1] = size - 1;
                puzzle[blankIndex] = endPanelButton;
            }
            return puzzle;
        }

        /// <summary>
        /// Nパズルを解くことができるかを返す
        /// </summary>
        /// <param name="targetPuzzle">対象のパズル</param>
        /// <param name="size">パズルのサイズ</param>
        /// <returns>true:解くことができる false:解くことができない</returns>
        private bool CanSolveNPuzzle(IList<int> targetPuzzle, int size)
        {
            var puzzle = new List<int>(targetPuzzle);
            if (puzzle.Count != size) { return false; }
            var solvedPuzze = new List<int>();
            for (int i = 0; i < size; i++) { solvedPuzze.Add(i); }
            var NumberOfExchanges = 0;
            for (int i = 0; i < size; i++)
            {
                if (solvedPuzze[i] != puzzle[i])
                {
                    var src = puzzle[i];
                    var destIndex = puzzle.IndexOf(solvedPuzze[i]);
                    var dest = puzzle[destIndex];
                    puzzle[destIndex] = src;
                    puzzle[i] = dest;
                    NumberOfExchanges++;
                }
            }
            return NumberOfExchanges % 2 == 0;
        }

        /// <summary>
        /// パズルが解かれたかどうか
        /// </summary>
        /// <param name="size">パズルのサイズ</param>
        /// <returns>true:解かれた false:解かれていない</returns>
        private bool IsPuzzleSolved(int size)
        {
            for (int i = 0; i < size; i++)
            {
                if (PanelButtons[i].Text != (i + 1).ToString()) { return false; }
            }
            return true;
        }

        /// <summary>
        /// <see cref="PropertyChanged"/>イベントを通知する
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
