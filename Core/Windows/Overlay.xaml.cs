namespace VComm.Core.Windows
{
    /// <summary>
    /// Interaction logic for Overlay.xaml
    /// </summary>
    public partial class Overlay : Window
    {
        internal VoiceEngine voiceEngine = new VoiceEngine();

        public int Volume 
        { 
            set 
            { 
                VolumeBar.SetPercent(value);
            } 
        }

        public async Task SetSpeechText(string text)
        {
            RecognizedText.Text = text;
            await Task.Delay(1000);
            RecognizedText.Text = "";
        }

        public Overlay()
        {
            InitializeComponent();
            
            if (!Variables.OverlayVisable) OverlayGrid.Visibility = Visibility.Hidden;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await voiceEngine.StartEngine();
            if (Variables.FirstRun)
            {
                Settings settings = new Settings();
                settings.Show();
            }
        }

        public ICommand ShowSettingsCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => System.Windows.Application.Current.MainWindow == null,
                    CommandAction = () =>
                    {
                        Settings settings = new Settings();
                        settings.Show();
                    }
                };
            }
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private async void ExitApp(object sender, RoutedEventArgs e)
        {
            await voiceEngine.StopEngine();
            Environment.Exit(0);
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            Focus();
        }
    }
}
