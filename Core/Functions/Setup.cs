namespace VComm.Core.Functions
{
    internal class Setup
    {
        public async Task RuntimeChecks()
        {
            await this.Log("Running Init...");
            await CheckForVPacks();
            await ApplySettings();
        }

        public async Task<bool> ApplySettings()
        {
            await this.Log("Applying settings...");

            string? preferred = Data.GetConfig("ActiveVPack");
            if (preferred != null) 
            {
                
                VPack? pack = Variables.VPacks.FirstOrDefault(p => p.name == preferred);
                if (pack != null)
                {
                    await this.Log($"Saved VPack: {preferred}");
                    Variables.ActiveVPack = pack;
                }
            }

            
            bool overlayActive = true;
            bool.TryParse(Data.GetConfig("OverlayActive"), out overlayActive);
            Variables.OverlayVisable = overlayActive;
            await this.Log($"Overlay hidden: {!overlayActive}");

            await this.Log($"All setting applied!");
            return true;
        }

        public async Task<bool> CheckForVPacks()
        {
            await this.Log("Checking for VPacks...");
            string vPackDir = Variables.VPacksDirectory;
            if (!Directory.Exists(vPackDir)) Directory.CreateDirectory(vPackDir);

            DirectoryInfo packDirectoryInfo = new DirectoryInfo(vPackDir);
            FileInfo[] vPacks = packDirectoryInfo.GetFiles();

            if (vPacks.Length > 0) 
            {
                foreach (FileInfo vPack in vPacks) 
                {
                    VPack? pack = await Data.LoadVPack(vPack.FullName);
                    if (pack != null)
                    {
                        if (Variables.VPacks.Any(p => p.name == pack.name))
                        {
                            await this.Log($"VPack already exists! Ignoring \"{pack.name}\"");
                        }
                        else
                        {
                            await this.Log($"Adding \"{vPack.Name}\" to VPacks...");
                            Variables.VPacks.Add(pack);
                        }
                    }
                    else
                    {
                        await this.Log($"Couldn't load \"{vPack.Name}\" VPack!");
                    }
                }
            }
            else
            {
                await this.Log("No VPacks found, creating one for you...");


                VPack vPack = new VPack();

                vPack.name = "Ready Or Not";
                vPack.author = "Devlin";
                vPack.vRequests = Variables.ReadyOrNot_vRequests;

                await Data.SaveVPack(vPack);
                Variables.VPacks.Add(vPack);


                Variables.FirstRun = true;
            }
            return true;
        }
    }
}
