namespace VComm
{
    /// <summary>
    /// Interaction logic for Preloader.xaml
    /// </summary>
    public partial class Preloader : Window
    {
        public bool initComplete = false;


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
            while (!initComplete) { await Task.Delay(500); }
            await this.Log("Init Complete!");

            Variables.Overlay = new Overlay();
            Variables.Overlay.Show();
            this.Close();
        }
    }
}
