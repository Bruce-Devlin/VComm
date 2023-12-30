namespace VComm.Core.Functions
{
    internal static class Data
    {
        /// <summary>
        /// Loads a VPack from a file path.
        /// </summary>
        /// <param name="filepath">The URI to the file you wish to load a VPack</param>
        /// <returns>The VPack parsed from the file</returns>
        public static async Task<VPack>? LoadVPack(Uri filepath)
        {
            VPack? vPack = null;
            string json = await File.ReadAllTextAsync(filepath.LocalPath);
            vPack = JsonConvert.DeserializeObject<VPack>(json);
                
            return vPack;
        }

        /// <summary>
        /// Loads a VPack from a string.
        /// </summary>
        /// <param name="json">The JSON string containing the VPack</param>
        /// <returns>The VPack parsed from the string</returns>
        public static async Task<VPack>? LoadVPack(string json)
        {
            VPack? vPack = null;
            vPack = JsonConvert.DeserializeObject<VPack>(json);

            return vPack;
        }

        /// <summary>
        /// Saves a VPack to the VPacks folder.
        /// </summary>
        /// <param name="vPack">The VPack you would like to save</param>
        public static async Task SaveVPack(VPack vPack)
        {
            string vPackDir = Variables.VPacksDirectory;
            string pureFilepath = vPackDir + "\\" + vPack.name + ".vcomm";

            string json = JsonConvert.SerializeObject(vPack, formatting: Formatting.Indented);
            await File.WriteAllTextAsync(pureFilepath, json);
        }

        /// <summary>
        /// Stores a value to the config file.
        /// </summary>
        /// <param name="name">The title/key for the data you would like to store</param>
        /// <param name="data">The string data you would like to store</param>
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

        /// <summary>
        /// Gets a values from the config file.
        /// </summary>
        /// <param name="name">The title/key for the data you would like to retrieve</param>
        /// <returns>The string data from the config file</returns>
        public static string? GetConfig(string name)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = Variables.ConfigFile;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;

            string? value = null;
            if (settings[name] != null) value = settings[name].Value;

            return value;
        }

        /// <summary>
        /// Is Push-To-Talk currently active?
        /// </summary>
        /// <returns>true if config contains (PTTActive == true)</returns>
        public static bool isPTTActive()
        {
            bool pttActive = false;
            bool.TryParse(Data.GetConfig("PTTActive"), out pttActive);
            return pttActive;
        }

        /// <summary>
        /// Gets the saved Push-To-Talk key ID.
        /// </summary>
        /// <returns>The Virtual Key ID saved in the config</returns>
        public static int getPTTKey()
        {
            int pttKey = -1;
            string? storedKey = Data.GetConfig("PTTKey");
            int.TryParse(storedKey, out pttKey);
            return pttKey;
        }

    }
}
