namespace VComm.Core
{
    internal class Variables
    {
        public static string AppDirectory = Directory.GetCurrentDirectory();
        public static string AppFilePath = Environment.ProcessPath;
        public static string LogFolder = AppDirectory + "\\Logs";
        public static string LogFilePath = LogFolder + "\\" + $"{Process.GetCurrentProcess().StartTime.ToShortDateString().Replace("/", "-")}--{Process.GetCurrentProcess().StartTime.ToLongTimeString().Replace(":", "-")}.txt";
        public static string Version = FileVersionInfo.GetVersionInfo(AppFilePath).FileVersion;
        public static string VPacksDirectory = Directory.GetCurrentDirectory() + "\\VPacks";
        public static string ConfigFile = Directory.GetCurrentDirectory() + "\\VComm.config";
        public static Overlay Overlay;
        public static bool OverlayVisable = true;

        public static string ChimePath
        { 
            get 
            {
                if (customChimePath != "") return customChimePath;
                else return defaultChimePath;
            }
            set { customChimePath = value; }
        }
        private static string customChimePath = "";
        private static string defaultChimePath = "VComm.Core.Assets.Sounds.chime.wav";


        public static bool FirstRun = false;
        public static List<VPack> VPacks = new List<VPack>();

        private static VPack _activeVPack = null;
        public static VPack ActiveVPack { 
            get 
            {
                if (_activeVPack == null)
                {
                    if (VPacks.Count == 0) return null;
                    else return VPacks[0];
                }
                else return _activeVPack;   
            }
            set
            {
                _activeVPack = value;
            }
        }
    }
}
