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

        public static List<VRequest> ReadyOrNot_vRequests = new List<VRequest>()
        {
            new VRequest()
            {
                phrases = ["red", "red team"],
                macro = new Macro()
                {
                    keycodes = new List<string>() { "{F7}" }
                },
            },
            new VRequest()
            {
                phrases = ["blue", "blue team"],
                macro = new Macro()
                {
                    keycodes = new List<string>() { "{F6}" }
                },
            },
            new VRequest()
            {
                phrases = ["gold", "gold team", "all"],
                macro = new Macro()
                {
                    keycodes = new List<string>() { "{F5}" }
                },
            },
            new VRequest()
            {
                phrases = ["do that"],
                macro = new Macro()
                {
                    keycodes = new List<string>() { "{Z}" }
                },
            },
            new VRequest()
            {
                phrases = ["fall in", "regroup", "on me"],
                macro = new Macro()
                {
                    msToWait = 1,
                    keycodes = new List<string>() { "{HOME}", "{2}", "{1}" }
                },
            },
            new VRequest()
            {
                phrases = ["move there", "move", "go there"],
                macro = new Macro()
                {
                    keycodes = new List<string>() { "{HOME}", "{1}" }
                },
            },
            new VRequest()
            {
                phrases = ["cover", "cover there"],
                macro = new Macro()
                {
                    keycodes = new List<string>() { "{HOME}", "{3}" }
                },
            },
            new VRequest()
            {
                phrases = ["stop", "hold"],
                macro = new Macro()
                {
                    keycodes = new List<string>() { "{HOME}", "{4}" }
                },
            },
        };
    }
}
