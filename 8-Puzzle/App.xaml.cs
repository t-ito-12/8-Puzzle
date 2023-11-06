namespace _8_Puzzle
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        /// <inheritdoc/>
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            const int newWidth = 800;
            const int newHeight = 840;

            window.Width = newWidth;
            window.Height = newHeight;

            return window;
        }
    }
}