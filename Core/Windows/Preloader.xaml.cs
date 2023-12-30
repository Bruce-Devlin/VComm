namespace VComm
{
    /// <summary>
    /// Interaction logic for Preloader.xaml
    /// </summary>
    public partial class Preloader : Window
    {
        public bool preInitComplete = false;

        public Preloader()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { DraggyWindow(); }; //Draggy windows
        }

        private void DraggyWindow()
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            while (!preInitComplete) { await Task.Delay(500); }
            await this.Log("PreInit Complete!");

            Variables.Overlay = new Overlay();
            Variables.Overlay.Show();
            this.Close();
        }
    }
}
