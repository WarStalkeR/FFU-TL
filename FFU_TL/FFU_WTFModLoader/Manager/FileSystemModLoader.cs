using FFU_Tyrian_Legacy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WTFModLoader.Manager {
	public class FileSystemModLoader {

		public List<Type> LoadModTypesFromDirectory(string directory, string steamdirectory) {
			List<Type> modTypes = new List<Type>();
			if (FFU_TL_Defs.isDebug) ModLog.Warning($"directory: {directory}");
			if (FFU_TL_Defs.isDebug) ModLog.Warning($"steamdirectory: {steamdirectory}");
			modTypes = FilterTypesInDirectory(directory, IsModType).ToList();
			if (Directory.Exists(steamdirectory)) {
				modTypes.AddRange(FilterTypesInDirectory(steamdirectory, IsModType).ToList());
			}
			if (FFU_TL_Defs.isDebug) foreach (var modType in modTypes) ModLog.Warning($"modType: {modType.FullName}");
			return modTypes;
		}


		private static IEnumerable<Type> FilterTypesInDirectory(string directory, Func<Type, bool> predicate) {
			List<Type> modTypes = new List<Type>();
			var fileSearch = Directory.EnumerateFiles(directory, "*.dll", SearchOption.AllDirectories);
			foreach (string foundFile in fileSearch) {
				if (FFU_TL_Defs.isDebug) ModLog.Warning($"Found DLL: {foundFile}");
				try {
					if (!foundFile.Contains("WTFModLoader.dll") && !foundFile.Contains("0Harmony.dll")) {
						Assembly loadedFile = null;
						if (WTFModLoader.legacyLoad) {
							loadedFile = Assembly.LoadFile(foundFile);
							if (FFU_TL_Defs.isDebug) ModLog.Warning($"LoadFile: {loadedFile.FullName}");
						} else {
							loadedFile = Assembly.UnsafeLoadFrom(foundFile);
							if (FFU_TL_Defs.isDebug) ModLog.Warning($"UnsafeLoadFrom: {loadedFile.FullName}");
						}
						if (!foundFile.Contains(loadedFile.Location)) {
							WTFModLoader._modManager.conflictingAssemblies.Value.Add(new Tuple<Assembly, string>(loadedFile, foundFile));
						}
						modTypes = modTypes.Concat(FilterTypes(loadedFile, predicate)).ToList();
					}
				} catch (ReflectionTypeLoadException ex) {
					StringBuilder sb = new StringBuilder();
					foreach (Exception exSub in ex.LoaderExceptions) {
						sb.AppendLine(exSub.Message);
						FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
						if (exFileNotFound != null) {
							if (!string.IsNullOrEmpty(exFileNotFound.FusionLog)) {
								sb.AppendLine("Fusion Log:");
								sb.AppendLine(exFileNotFound.FusionLog);
							}
						}
						sb.AppendLine();
					}
					string errorMessage = sb.ToString();
					ModLog.Error($"Failed to load mod file `{foundFile}`.");
					ModLog.Error(errorMessage);
					continue;
				}
			}
			if (FFU_TL_Defs.isDebug) foreach (var modType in modTypes) ModLog.Warning($"modType1: {modType.FullName}");
			return modTypes;
		}
		private static IEnumerable<Type> FilterTypes(Assembly asm, Func<Type, bool> predicate) {
			//if (FFU_TL_Defs.isDebug) ModLog.Warning($"FilterTypes: {asm.FullName}");
			//var typeList = asm.GetTypes().Where(x => typeof(IWTFMod).IsAssignableFrom(x));
			//var typeList = asm.GetTypes().Where(x => !string.IsNullOrEmpty(x.GetInterface("IWTFMod").ToString()));
			//var typeList = asm.GetTypes();
			//var typeList = asm.GetTypes().Where(x => x.GetInterface("IWTFMod").ToString().Contains("IWTFMod"));
			//try { if (FFU_TL_Defs.isDebug) foreach (Type typeEntry in typeList) ModLog.Warning($"typeEntry: {typeEntry.GetInterface("IWTFMod").ToString().Contains("IWTFMod")} {typeEntry}"); } catch { }
			//try { if (FFU_TL_Defs.isDebug) foreach (Type typeEntry in typeList) ModLog.Warning($"typeEntry: {typeEntry}"); } catch { }
			return asm.GetTypes().Where(predicate);
			//return asm.GetTypes().Where(x => x.GetInterface("IWTFMod").ToString().Contains("IWTFMod"));
			//return asm.GetTypes().Where(x => x.GetInterface("IWTFMod") != null && x.GetInterface("IWTFMod").ToString().Contains("IWTFMod"));
		}

		private static bool IsModType(Type type) {
			return type.GetInterface("IWTFMod") != null && type.GetInterface("IWTFMod").ToString().Contains("IWTFMod");
			//return typeof(IWTFMod).IsAssignableFrom(type);
		}

	}
}