//Git URL: https://github.com/BlackteaGit/WaywardTerranFrontierModLoader/tree/20b221a0e4a0d262155f9da12bfec27ed8e7d5a9

using CoOpSpRpG;
using FFU_Tyrian_Legacy;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WaywardExtensions;
using WTFModLoader.Config;
using WTFModLoader.Infrastructure;
using WTFModLoader.Manager;

namespace WTFModLoader {
	public static class WTFModLoader {
		public static ModManager _modManager;

		public static bool legacyLoad;
		public static string CurrentBuildVersion { get; private set; }
		public static string ModsDirectory { get; private set; }
		public static string SteamModsDirectory { get; private set; }
		public static void Initialize() {
			//WTFModLoaderInjector entry
			string rootDirectory = Directory.GetCurrentDirectory();
			SteamModsDirectory = Path.GetFullPath(Path.Combine(rootDirectory, Path.Combine(@"..\..\workshop\content\392080")));
			ModsDirectory = Path.GetFullPath(Path.Combine(rootDirectory, Path.Combine(@"Mods")));
			ModLog.Warning($"rootDirectory: {rootDirectory}");
			ModLog.Warning($"SteamModsDirectory: {SteamModsDirectory}");
			ModLog.Warning($"ModsDirectory: {ModsDirectory}");
			if (ModsDirectory == null || SteamModsDirectory == null) LegacyLoad();
			EnsureFolderSetup();
			ModDbManager.Init();
			ModDbManager.updateCfgDb();
			ModDbManager.loadCfgData();
			HarmonyPatcher.PatchGameRootMenu();
			Logger.InitializeLogging(Path.Combine(ModsDirectory, "WTFModLoader.log"));
			SimpleInjector.Container container = CompositionRoot.GetContainer();
			container.Options.ResolveUnregisteredConcreteTypes = true;
			_modManager = new ModManager(ModsDirectory, SteamModsDirectory, new JsonConfigProvider(), new FileSystemModLoader(), container);
			_modManager.Initialize();
		}
		private static void LegacyLoad() {
			legacyLoad = true;
			CurrentBuildVersion = "0.4";
			string manifestDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
					?? throw new InvalidOperationException("Could not determine operating directory. Is your folder structure correct? " +
					"Try verifying game files in Steam, if you're using it.");

			string rootdirectoryFile = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine(@"0Harmony.dll")));
			string rootdirectoryChangedFile = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine(@"0Harmony.dll.old")));
			if (File.Exists(rootdirectoryChangedFile) && !File.Exists(rootdirectoryFile)) {
				File.Copy(rootdirectoryChangedFile, rootdirectoryFile, true);
				File.Delete(rootdirectoryChangedFile);
			}
			SteamModsDirectory = Path.GetFullPath(Path.Combine(manifestDirectory, Path.Combine(@"..\..\workshop\content\392080")));
			ModsDirectory = Path.GetFullPath(Path.Combine(manifestDirectory, Path.Combine(@"Mods")));
			//HarmonyPatcher.PatchBACKDROP(); // possibility to modyfy backdrop loading process if loaded by Injector (earlier entry point)
		}
		private static void EnsureFolderSetup() {
			if (!Directory.Exists(ModsDirectory)) {
				Directory.CreateDirectory(ModsDirectory);
			}
		}
	}
}