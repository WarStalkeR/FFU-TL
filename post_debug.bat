set sourcefile=%~1
set targetdir=%~2
echo F | xcopy /F /Y "%sourcefile%" "%targetdir%..\..\BepInEx\monomod\WTF.FFU_TL.mm.dll"