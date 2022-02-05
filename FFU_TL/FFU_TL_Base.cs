#pragma warning disable CS0108
#pragma warning disable CS0626

using CoOpSpRpG;
using FFU_Tyrian_Legacy;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Concurrent;
using System.IO;

namespace FFU_Tyrian_Legacy {
    public class FFU_TL_Base {
        public static readonly string exeFilePath = Directory.GetCurrentDirectory() + @"\";
        public static readonly string modConfDir = @"Mods\";
        public static readonly string modConfFile = @"FFU_Tyrian_Legacy.ini";
        public static void LoadModConfiguration() {
            if (!Directory.Exists(exeFilePath + modConfDir)) Directory.CreateDirectory(exeFilePath + modConfDir);
            if (File.Exists(exeFilePath + modConfDir + modConfFile)) {
                IniFile modConfig = new IniFile();
                modConfig.Load(exeFilePath + modConfDir + modConfFile);
                string modConfigLog = $"Loading mod configuration from {exeFilePath + modConfDir + modConfFile}";
                if (string.IsNullOrEmpty(modConfig["InitConfig"]["modVersion"].Value) || modConfig["InitConfig"]["modVersion"].ToString() != FFU_TL_Defs.modVersion) {
                    ModLog.Message(modConfigLog);
                    CreateModConfiguration();
                    return;
                }
                FFU_TL_Defs.secretStash = modConfig["InitConfig"]["secretStash"].ToBool(FFU_TL_Defs.secretStash);
                modConfigLog += $"\n > Property [secretStash] loaded with value: {FFU_TL_Defs.secretStash}";
                ModLog.Message(modConfigLog);
            } else CreateModConfiguration(false);
        }
        public static void CreateModConfiguration(bool isObsolete = true) {
            ModLog.Warning($"Mod configuration file {(isObsolete ? "is obsolete!" : "doesn't exist!")}");
            ModLog.Warning($"Creating template mod configuration file at {exeFilePath + modConfDir + modConfFile}");
            IniFile modConfig = new IniFile();
            modConfig["InitConfig"]["modVersion"] = FFU_TL_Defs.modVersion;
            modConfig["InitConfig"]["secretStash"] = FFU_TL_Defs.secretStash;
            modConfig.Save(exeFilePath + modConfDir + modConfFile);
        }
    }
}

namespace CoOpSpRpG {
    public class patch_LoaderProc : LoaderProc {
        public extern void orig_threadProcVerbose(ConcurrentQueue<string> messageQueue, ConcurrentQueue<string> errorMessages);
        public void threadProcVerbose(ConcurrentQueue<string> messageQueue, ConcurrentQueue<string> errorMessages) {
        /// Load mod configuration from specific INI file.
            alive = true;
            if (alive) {
                messageQueue.Enqueue("loading mod configuration");
                FFU_TL_Base.LoadModConfiguration();
            }
            orig_threadProcVerbose(messageQueue, errorMessages);
        }
        private extern void orig_loadArt(ConcurrentQueue<string> messageQueue);
        private void loadArt(ConcurrentQueue<string> messageQueue) {
        /// Load additional art from binary code/hex.
            orig_loadArt(messageQueue);
            SCREEN_MANAGER.TileArt[0] = Support.PatchTexture(SCREEN_MANAGER.TileArt[0], Datas.tGroupsSelectors);
            SCREEN_MANAGER.TileArt[3] = Support.PatchLight(SCREEN_MANAGER.TileArt[3], Datas.tMissleTubesLight);
        }
    }
    public class patch_Game1 : Game1 {
        private extern void orig_loadStuff();
        private void loadStuff() {
        /// Initialize priority code before everything else.
            orig_loadStuff();
        }
        public static GraphicsDevice refGraphicsDevice() {
            return SCREEN_MANAGER.Device;
        }

    }
}