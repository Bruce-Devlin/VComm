namespace VComm.Core.Functions
{
    internal static class Data
    {
        
        public static async Task<VPack>? LoadVPack(string filepath)
        {
            VPack? vPack = null;
            string json = await File.ReadAllTextAsync(filepath);

            if (File.Exists(filepath))
                vPack = JsonConvert.DeserializeObject<VPack>(json);

            return vPack;
        }

        public static async Task<VPack>? LoadVPackFromString(this string json)
        {
            VPack? vPack = null;
            vPack = JsonConvert.DeserializeObject<VPack>(json);

            return vPack;
        }

        public static async Task SaveVPack(VPack vPack)
        {
            string vPackDir = Variables.VPacksDirectory;
            string pureFilepath = vPackDir + "\\" + vPack.name + ".vcomm";

            string json = JsonConvert.SerializeObject(vPack, formatting: Formatting.Indented);
            await File.WriteAllTextAsync(pureFilepath, json);
        }

        public static void StoreConfig(string name, string data)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = Variables.ConfigFile;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;
            if (settings[name] == null) settings.Add(name, data);
            else settings[name].Value = data;
            configuration.Save(ConfigurationSaveMode.Modified);
        }

        public static string? GetConfig(string variable)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = Variables.ConfigFile;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;

            string? value = null;
            if (settings[variable] != null) value = settings[variable].Value;

            return value;
        }

        public static bool isPTTActive()
        {
            bool pttActive = false;
            bool.TryParse(Data.GetConfig("PTTActive"), out pttActive);
            return pttActive;
        }

        public static int getPTTKey()
        {
            int pttKey = -1;
            string? storedKey = Data.GetConfig("PTTKey");
            int.TryParse(storedKey, out pttKey);
            return pttKey;
        }

    }
}
