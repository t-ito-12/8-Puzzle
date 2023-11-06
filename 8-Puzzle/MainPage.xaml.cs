using _8_Puzzle.ViewModels;

namespace _8_Puzzle
{
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// このViewのViewModel
        /// </summary>
        private MainViewModel ViewModel { get; set; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModel;
        }
    }
}