namespace VComm.Core.Windows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { DraggyWindow(); }; //Draggy windows

            SetupSettings();
            if (Variables.FirstRun) ShowIntro();
        }

        private void DraggyWindow()
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void ShowIntro()
        {
            FirstRun.Visibility = Visibility.Visible;
        }

        private void IntroDoneBtn_Click(object sender, RoutedEventArgs e)
        {
            Variables.FirstRun = false;
            FirstRun.Visibility = Visibility.Collapsed;
        }


        private void SetupSettings()
        {
            BuildInfo.Text = $"VERSION: {Variables.Version} (EARLY-ACCESS/TEST BUILD)";

            if (Data.isPTTActive())
            {
                PTTCheck.IsChecked = true;
                PTTKey.IsEnabled = true;
                PTTKey.Content =  Data.getPTTKey().ToString().ToKeycode().ToString().ToUpper();
            }
            else PTTKey.IsEnabled = false;

            VPackList.ItemsSource = Variables.VPacks;
            VPackList.SelectedItem = Variables.ActiveVPack;

            OverlayCheck.IsChecked = (Variables.Overlay.OverlayGrid.Visibility == Visibility.Visible);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private async void Window_Closed(object sender, EventArgs e)
        {
            await Variables.Overlay.voiceEngine.ReloadEngine();
            if (Variables.OverlayVisable)
                Variables.Overlay.OverlayGrid.Visibility = Visibility.Visible;
            else
                Variables.Overlay.OverlayGrid.Visibility = Visibility.Hidden;
        }

        private async void PTTCheck_Checked(object sender, RoutedEventArgs e)
        {
            Data.StoreConfig("PTTActive", "true");
            PTTKey.IsEnabled = true;
        }

        private async void PTTCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Data.StoreConfig("PTTActive", "false");
            PTTKey.IsEnabled = false;
        }

        private void PTTKey_Click(object sender, RoutedEventArgs e)
        {
            PTTKey.Content = "PRESS ANY KEY";
            PTTKey.Focus();
        }

        private async void PTTKey_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            uint virtualKey = e.Key.ToVirtualKey();
            string keycode = e.Key.ToString().ToUpper();
            PTTKey.Content = keycode;
            Data.StoreConfig("PTTKey", virtualKey.ToString());
        }

        private void VPackList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Variables.ActiveVPack = VPackList.SelectedItem as VPack;
            Data.StoreConfig("ActiveVPack", Variables.ActiveVPack.name);
        }

        private void OverlayCheck_Checked(object sender, RoutedEventArgs e)
        {
            Variables.OverlayVisable = true;
            Data.StoreConfig("OverlayActive", "true");
        }

        private void OverlayCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            Variables.OverlayVisable = false;
            Data.StoreConfig("OverlayActive", "false");
        }

        
    }
}
