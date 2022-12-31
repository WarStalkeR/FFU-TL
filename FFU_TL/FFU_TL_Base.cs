#pragma warning disable CS0108
#pragma warning disable CS0626

using CoOpSpRpG;
using FFU_Terra_Liberatio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Concurrent;
using System.IO;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Base {
        public static void LoadModConfiguration() {
            ModLog.Warning($"Client Mode: {(CONFIG.openMP ? "Multiplayer" : "Singleplayer")}");
            Support.ValidateDirPath(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modConfDir);
            if (File.Exists(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modConfDir + FFU_TL_Defs.modConfFile)) {
                IniFile modConfig = new IniFile();
                modConfig.Load(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modConfDir + FFU_TL_Defs.modConfFile);
                string modConfigLog = $"Loading mod configuration from {FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modConfDir + FFU_TL_Defs.modConfFile}";
                if (string.IsNullOrEmpty(modConfig["InitConfig"]["modVersion"].Value) || modConfig["InitConfig"]["modVersion"].ToString() != FFU_TL_Defs.modVersion) {
                    ModLog.Warning(modConfigLog);
                    CreateModConfiguration();
                    return;
                }
                FFU_TL_Defs.secretStash = modConfig["InitConfig"]["secretStash"].ToBool(FFU_TL_Defs.secretStash);
                modConfigLog += $"\n > Property [secretStash] loaded with value: {FFU_TL_Defs.secretStash}";
                ModLog.Warning(modConfigLog);
            } else CreateModConfiguration(false);
        }
        public static void CreateModConfiguration(bool isObsolete = true) {
            ModLog.Warning($"Mod configuration file {(isObsolete ? "is obsolete!" : "doesn't exist!")}");
            ModLog.Warning($"Creating template mod configuration file at {FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modConfDir + FFU_TL_Defs.modConfFile}");
            IniFile modConfig = new IniFile();
            modConfig["InitConfig"]["modVersion"] = FFU_TL_Defs.modVersion;
            modConfig["InitConfig"]["secretStash"] = FFU_TL_Defs.secretStash;
            modConfig.Save(FFU_TL_Defs.exeFilePath + FFU_TL_Defs.modConfDir + FFU_TL_Defs.modConfFile);
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
            SCREEN_MANAGER.TileArt[4] = Support.PatchLight(SCREEN_MANAGER.TileArt[4], Datas.tTorpedoSiloLight);
            SCREEN_MANAGER.TileArt[3] = Support.PatchLight(SCREEN_MANAGER.TileArt[3], Datas.tHallwaysT2Light);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art01);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art02);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art03);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art04);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art05);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art06);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art07);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art08);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art09);
            SCREEN_MANAGER.TileArt[1] = Support.PatchSheet(SCREEN_MANAGER.TileArt[1], Datas.tHallwaysT2Art10);
            SCREEN_MANAGER.TileArt[2] = Support.PatchSheet(SCREEN_MANAGER.TileArt[2], Datas.tHugeEnergyTurret);
            SCREEN_MANAGER.GameArt[31] = Support.PatchSheet(SCREEN_MANAGER.GameArt[31], Datas.sAllDataCores);
            SCREEN_MANAGER.GameArt[31] = Support.PatchSheet(SCREEN_MANAGER.GameArt[31], Datas.sNewHandDigger);
            SCREEN_MANAGER.AnimSheets[15] = Support.PatchSheet(SCREEN_MANAGER.AnimSheets[15], Datas.sNewTorpMissiles);
            //Support.DumpImageToFile(SCREEN_MANAGER.AnimSheets[15], "Missile_Sheet_Patched.png");
            //Support.DumpImageToFile(SCREEN_MANAGER.TileArt[0], "Tile_Sheet_0_Patched.png");
            //Support.DumpImageToFile(SCREEN_MANAGER.TileArt[1], "Tile_Sheet_1_Patched.png");
            //Support.DumpImageToFile(SCREEN_MANAGER.TileArt[2], "Tile_Sheet_2_Patched.png");
            //Support.DumpImageToFile(SCREEN_MANAGER.TileArt[5], "Tile_Light_0_Patched.png", true);
            //Support.DumpImageToFile(SCREEN_MANAGER.TileArt[3], "Tile_Light_1_Patched.png", true);
            //Support.DumpImageToFile(SCREEN_MANAGER.TileArt[4], "Tile_Light_2_Patched.png", true);
            //Support.DumpImageToFile(SCREEN_MANAGER.GameArt[31], "Items_Sheet_Patched.png");
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