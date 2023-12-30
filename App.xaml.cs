namespace VComm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        async void App_Start(object sender, StartupEventArgs e)
        {
            HandleArgs(e.Args);
            HandleErrors();
            Preloader preloader = new Preloader();
            preloader.Show();

            Init init = new Init();
            await init.Start();
            preloader.initComplete = true;
        }

        public void HandleErrors()
        {
            AppDomain.CurrentDomain.UnhandledException += 
                new UnhandledExceptionEventHandler(UnhandledExceptions);

            this.Dispatcher.UnhandledException += 
                new DispatcherUnhandledExceptionEventHandler(UnhandledPrivateDispatcherExceptions);

            System.Windows.Application.Current.DispatcherUnhandledException +=
                new DispatcherUnhandledExceptionEventHandler(UnhandledPublicDispatcherExceptions);

            TaskScheduler.UnobservedTaskException += 
                new EventHandler<UnobservedTaskExceptionEventArgs>(UnhandledTaskExceptions);
        }

        private async void UnhandledTaskExceptions(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            await this.Log("UNHADLED TASK EXCEPTION!", error: true);
            await this.Log($"{e.Exception.ToString()}", error:true);
        }

        private async void UnhandledPrivateDispatcherExceptions(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            await this.Log("UNHADLED PRIVATE EXCEPTION!", error: true);
            await this.Log($"{e.Exception.ToString()}", error: true);
        }
        private async void UnhandledPublicDispatcherExceptions(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            await this.Log("UNHADLED PUBLIC EXCEPTION!");
            await this.Log($"{e.Exception.ToString()}", error: true);
            await this.Log($"{e.Exception.ToString()}", error: true);
        }

        private async void UnhandledExceptions(object? sender, UnhandledExceptionEventArgs e)
        {
            await this.Log("UNHADLED EXCEPTION!");
            await this.Log($"Message: {e.ExceptionObject.ToString()}", error: true);
            await this.Log($"Terminating: {e.IsTerminating}", error: true);
        }

        public void HandleArgs(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg == "-console")
                {
                    Helpers.AllocConsole();
                }
            }
        }
    }

    public static class Helpers
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();

        private static List<string> logCache = new List<string>();

        public static async Task Log(
                this object callingObj,
                string message,
                bool error = false,
                ConsoleColor color = ConsoleColor.White,
                bool stamped = true,
                bool hiddenFromCMD = false
            )
        {
            string parentName = callingObj.GetType().Name.ToUpper();

            if (!hiddenFromCMD)
            {
                string stamp = "";
                if (stamped)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    stamp = $"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}] VComm - {parentName}: ";
                    Console.Write(stamp);
                }

                if (error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR!");
                }

                Console.ForegroundColor = color;

                string cleanMessage = stamp + message;
                logCache.Add(cleanMessage);
                Console.WriteLine(message);
                Debug.WriteLine(cleanMessage);
                //if (!Directory.Exists(Variables.LogFolder)) Directory.CreateDirectory(Variables.LogFolder);
                //await File.AppendAllTextAsync(Variables.LogFilePath, cleanMessage + Environment.NewLine);
                if (error) Console.ReadKey();
            }
        }
    }
}
