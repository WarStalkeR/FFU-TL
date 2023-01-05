## Description
**Fight For Universe: Terra Liberatio** is a mod that extends original **Wayward Terran Frontier: Zero Falls** in different ways. Mostly it is focused on implementing features and modules that aren't fully complete, or were disabled for one or another reason. In addition, it also contains features for modders that allow to do some interesting things, such as reading compressed textures directly from HEX string in code and patching main textures with smaller ones on game's load, without need to modify them permanently on your hard drive.

## Features
**Cloning Bays** can be acquired, installed and used from your ship directly, if you're not far from it (i.e. if ship is present in location, where session is still active and processed by the game). In future it will be available even at locations, that aren't active.  
**Logistics Terminal** now can be acquired, installed and used from your ship to build other ships and/or unload ore from your own ship to global ore storage. I also intend to add feature that will allow to fetch ores from global storage and move them into ship for trading.  
**MagRail Stations** now can be acquired, installed and used from your ship for fast in-ship travel between stations. They also can be used to access any cargo bay. When your crew is on **hold**, stations will ignore them and will only move player.  
**All Crew Cabins** are now acquirable, installable on your ship and have some flavor texts added. I'm also thinking about implementing feature that will increase rewards from hitchhikers based on quality cabins provide.  
**Utility Room** now can be acquired, installed and used from your ship to get infinite supply of repair tools, fire extinguishers and hand-held mining lasers. Has secret option, but it is up to you discover it.  
**All Cargo Bays** are now acquirable and installable. Cargo bay capacity was greatly rebalanced. Working ore mining bonus from cargo bays will be implemented later.  
**Space Bars/Taverns** now can be acquired, installed and used from your ship to re-recruit your own agents or heed One's nagging.  
**Crew Rooms** are now divided into T1 that requires iron for building and T2 that requires titanium for building.  
**Airlocks** are now divided into T1 that requires iron for building and T2 that requires titanium for building.  
**Graviton Missiles** now have their own missile tube and missile magazine texture that makes it obvious, when converter is used.  
**Group Selectors** were renamed into Group Controllers and now each of them has their own unique image.  

## Installation (Basic)
1) Download https://www.nuget.org/packages/SharpDX.Direct3D9/4.0.1, unpack it, copy SharpDX.Direct3D9.dll from \lib\net45 to WTF's folder.
2) Download https://www.nuget.org/packages/SharpDX.Mathematics/4.0.1, unpack it, copy SharpDX.Mathematics.dll from \lib\net45 to WTF's folder.
3) Download any BepInEx_NetLauncher_???_6.0.0.zip from https://builds.bepinex.dev/projects/bepinex_be, unpack it, and copy only "BepInEx" folder to WTF's folder.
4) Download MonoMod from https://github.com/MonoMod/MonoMod/releases/tag/v22.01.29.01 and unpack it to anywhere.
5) Download already compiled mod's DLL from https://github.com/WarStalkeR/FFU-TL/releases/tag/v0.0.1.0 and place it in WTF's folder.
6) Via command line, launch MonoMod.exe \Path\To\WTF.exe, it will create MONOMODDED_WTF.exe that you can run. Enjoy!

## Installation (Advanced)
1) Download https://www.nuget.org/packages/SharpDX.Direct3D9/4.0.1, unpack it, copy SharpDX.Direct3D9.dll from \lib\net45 to WTF's folder.
2) Download https://www.nuget.org/packages/SharpDX.Mathematics/4.0.1, unpack it, copy SharpDX.Mathematics.dll from \lib\net45 to WTF's folder.
3) Download this repository, unpack, and place it in any folder in WTF's directory: \Zero Falls\<Folder>\FFU_TL\FFU_Terra_Liberatio.sln
4) Download MonoMod from https://github.com/MonoMod/MonoMod/releases/tag/v22.01.29.01 and unpack dll/exe files into \Zero Falls\<Folder>\MonoMod\
5) Open solution in Visual Studio 2022, remove BepInEx/Harmony references, comment out related code in ModLog.cs.
6) Switch compilation target to Release, compile DLL. Project will autorun script that will create monomod patched WTF_modded.exe in WTF's folder.
7) Launch WTF_modded.exe directly and enjoy the modded game. Compatible with blacktea's mod loader and DLL mods.

## Installation (Developer)
1) Download https://www.nuget.org/packages/SharpDX.Direct3D9/4.0.1, unpack it, copy SharpDX.Direct3D9.dll from \lib\net45 to WTF's folder.
2) Download https://www.nuget.org/packages/SharpDX.Mathematics/4.0.1, unpack it, copy SharpDX.Mathematics.dll from \lib\net45 to WTF's folder.
3) Download this repository, unpack, and place it in any folder in WTF's directory: \Zero Falls\<Folder>\FFU_TL\FFU_Terra_Liberatio.sln
4) Download MonoMod from https://github.com/MonoMod/MonoMod/releases/tag/v22.01.29.01 and unpack dll/exe files into \Zero Falls\<Folder>\MonoMod\
5) Download latest code from https://github.com/BepInEx/BepInEx, unpack and open in Visual Studio 2022.
6) In it, modify BepInEx.NetLauncher project to target .NET 4.6 and edit project to use <PlatformTarget>x64</PlatformTarget>.
7) Compile it, copy all files, except BepInEx.NetLauncher.exe/pdb into folder \Zero Falls\BepInEx\core\
8) Copy BepInEx.NetLauncher.exe/pdb into \Zero Falls\ folder. Launch the BepInEx.NetLauncher.exe to initialize BepInEx for the first time.
9) Open file \Zero Falls\BepInEx\config\BepInEx.cfg and in [Logging.Console] set Enabled = true
10) Download code from https://github.com/BepInEx/BepInEx.MonoMod.Loader/tree/v6-net45-compat, open it in VS2022, compile it.
11) From compiled files, copy MonoMod.Utils.dll and MonoMod.exe (rename to MonoMod.dll) into \Zero Falls\BepInEx\core\
12) From compiled files, copy BepInEx.MonoMod.Loader.dll into \Zero Falls\BepInEx\patchers\
13) Open Terra Liberatio solution in Visual Studion 2022, compile it either in Debug or Release mode.
14) Debug target will create only DLL file in BepInEx's folder, while Release target will also create monomod patched WTF_modded.exe in game's dir.
15) Launch it via BepInEx.NetLauncher.exe. Optionally in steam you can set Launch Option for WTF to: %command%\..\BepInEx.NetLauncher.exe %command%

**Important Note**: for some reason, while game is launched via *BepInEx.NetLauncher.exe*, it doesn't work with blacktea's WTF Mod Loader (and other DLL mods that injected/loaded as *objects* and converted via *as*). So use Launcher version only if you do some coding/debugging. For normal playthrough play game via *WTF_modded.exe*