#pragma warning disable CS0169
#pragma warning disable CS0414
#pragma warning disable CS0649

using CoOpSpRpG;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FFU_Terra_Liberatio {
    public class FFU_TL_Tile_Controllers {
        public static void updateModules() {
            ModLog.Message($"Applying module changes: Ship/Weapon Controllers.");
            modControllerN(255, 75, 0);
            modControllerW(255, 75, 1);
            modControllerW(255, 75, 2);
            modControllerW(255, 75, 3);
            modControllerW(255, 75, 4);
            modControllerW(255, 75, 5);
            modControllerW(255, 75, 6);
            modControllerW(255, 75, 7);
            modControllerW(255, 75, 8);
        }
        public static void assignCustomCosts() {
            ModLog.Message($"Applying custom resource costs: Ship/Weapon Controllers.");
            FFU_TL_Modules.applyModuleResList(255, 75, 0, gold: 5f);
            FFU_TL_Modules.applyModuleResList(255, 75, 1, gold: 5f);
            FFU_TL_Modules.applyModuleResList(255, 75, 2, gold: 5f);
            FFU_TL_Modules.applyModuleResList(255, 75, 3, gold: 5f);
            FFU_TL_Modules.applyModuleResList(255, 75, 4, gold: 5f);
            FFU_TL_Modules.applyModuleResList(255, 75, 5, gold: 5f);
            FFU_TL_Modules.applyModuleResList(255, 75, 6, gold: 5f);
            FFU_TL_Modules.applyModuleResList(255, 75, 7, gold: 5f);
            FFU_TL_Modules.applyModuleResList(255, 75, 8, gold: 5f);
        }
        public static void modControllerN(byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modControllerN(FFU_TL_Defs.rMod[r][g][b] as ConsoleConnect, new Color(r, g, b));
        }
        public static void modControllerW(byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modControllerW(FFU_TL_Defs.rMod[r][g][b] as ConsoleConnect, new Color(r, g, b), b);
        }
        public static void modControllerN(ConsoleConnect rCtrl, Color refColor) {
            rCtrl.cost = 500;
            rCtrl.toolTip = $"Navigation Controller";
            rCtrl.techLevel = 5;
            rCtrl.tileSheet = 0;
            rCtrl.tip = new ToolTip();
            rCtrl.tip.tip = rCtrl.toolTip;
            rCtrl.tip.botLeftText = $"Elbit Systems Interstellar";
            rCtrl.tip.description = $"Once connected to the console (placed near it), allows console user to control ship's main and auxiliary thrusters. If connected to weapons or modules, allows to use them, while steering ship.";
            rCtrl.tip.tierIcontype = TierIcon.resource;
            rCtrl.tip.addStat($"Relay Group", "Ship Navigation", false);
            rCtrl.tip.addStat($"Processing Unit", "Core-RTOS", false);
            rCtrl.tip.addStat($"Processing Frequency", "148.2 THz", false);
            rCtrl.tip.addStat($"Signal Transfer Speed", "86.7 TBps", false);
            rCtrl.tip.addStat($"Signal Transfer Latency", "0.017 fs", false);
            rCtrl.tiles = Support.PatchTiles(Datas.tGroupsSelectors, refColor);
        }
        public static void modControllerW(ConsoleConnect rCtrl, Color refColor, int rG) {
            rCtrl.cost = 50;
            rCtrl.toolTip = $"{(rG == 1 ? $"1st" : (rG == 2 ? $"2nd" : (rG == 3 ? $"3rd" : $"{rG}th")))} Weapon Controller";
            rCtrl.techLevel = 5;
            rCtrl.tileSheet = 0;
            rCtrl.tip = new ToolTip();
            rCtrl.tip.tip = rCtrl.toolTip;
            rCtrl.tip.botLeftText = $"Elbit Systems Interstellar";
            rCtrl.tip.description = $"Once connected to the console (placed near it), allows console user to control connected weapons and modules, as long as console has enough system capacity for connected modules.";
            rCtrl.tip.tierIcontype = TierIcon.resource;
            rCtrl.tip.addStat($"Relay Group", $"{(rG == 1 ? $"1st" : (rG == 2 ? $"2nd" : (rG == 3 ? $"3rd" : $"{rG}th")))} Weapon Group", false);
            rCtrl.tip.addStat($"Processing Unit", "Core-RTOS", false);
            rCtrl.tip.addStat($"Processing Frequency", "148.2 THz", false);
            rCtrl.tip.addStat($"Signal Transfer Speed", "86.7 TBps", false);
            rCtrl.tip.addStat($"Signal Transfer Latency", "0.017 fs", false);
            rCtrl.tiles = Support.PatchTiles(Datas.tGroupsSelectors, refColor);
        }
    }
}
