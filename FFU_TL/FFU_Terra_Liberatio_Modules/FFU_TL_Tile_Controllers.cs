#pragma warning disable CS0169
#pragma warning disable CS0414
#pragma warning disable CS0649

using CoOpSpRpG;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FFU_Terra_Liberatio {
    internal class FFU_TL_Tile_Controllers {
        public static void updateModules(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules) {
            ModLog.Message($"Applying module changes: Ship/Weapon Controllers...");
            modControllerN(modules, 255, 75, 0);
            modControllerW(modules, 255, 75, 1);
            modControllerW(modules, 255, 75, 2);
            modControllerW(modules, 255, 75, 3);
            modControllerW(modules, 255, 75, 4);
            modControllerW(modules, 255, 75, 5);
            modControllerW(modules, 255, 75, 6);
            modControllerW(modules, 255, 75, 7);
            modControllerW(modules, 255, 75, 8);
        }
        public static void assignCustomCosts(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, Dictionary<Module, Dictionary<InventoryItemType, float>> rRes, Dictionary<Module, Dictionary<InventoryItemType, float>> rExtra) {
            ModLog.Message($"Applying custom resource costs: Ship/Weapon Controllers...");
            modController(modules[255][75][0], rRes, rExtra);
            modController(modules[255][75][1], rRes, rExtra);
            modController(modules[255][75][2], rRes, rExtra);
            modController(modules[255][75][3], rRes, rExtra);
            modController(modules[255][75][4], rRes, rExtra);
            modController(modules[255][75][5], rRes, rExtra);
            modController(modules[255][75][6], rRes, rExtra);
            modController(modules[255][75][7], rRes, rExtra);
            modController(modules[255][75][8], rRes, rExtra);
        }
        public static void modControllerN(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modControllerN(modules[r][g][b] as ConsoleConnect, new Color(r, g, b));
        }
        public static void modControllerW(Dictionary<byte, Dictionary<byte, Dictionary<byte, Module>>> modules, byte r, byte g, byte b) {
            FFU_TL_Defs.unlistDynamic.Add(new Color(r, g, b));
            FFU_TL_Defs.unlistDynamic = FFU_TL_Defs.unlistDynamic.ToList();
            modControllerW(modules[r][g][b] as ConsoleConnect, new Color(r, g, b), b);
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
        public static void modController(Module rMod, Dictionary<Module, Dictionary<InventoryItemType, float>> rRes, Dictionary<Module, Dictionary<InventoryItemType, float>> rExtra) {
            try {
                FFU_TL_Modules.cleanModuleResList(rMod);
                if (rRes.ContainsKey(rMod)) rRes.Remove(rMod);
                if (rExtra.ContainsKey(rMod)) rExtra.Remove(rMod);
                rExtra.Add(rMod, new Dictionary<InventoryItemType, float>() {
                    {InventoryItemType.gold_ore, 5f},
                });
                TILEBAG.AssignResources(rMod);
            } catch (Exception ex) {
                ModLog.Error($"Error assigning custom resources! Exception: {ex}");
            }
        }
    }
}
