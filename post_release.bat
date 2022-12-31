set sourcefile=%~1
set targetdir=%~2
echo F | xcopy /F /Y "%sourcefile%" "%targetdir%..\..\WTF.FFU_TL.mm.dll"
waitfor SomethingThatIsNeverHappening /t 2 2>NUL
"%targetdir%..\MonoMod\MonoMod.exe" "%targetdir%..\..\WTF.exe"
del "%targetdir%..\..\WTF.FFU_TL.mm.dll" /F
del "%targetdir%..\..\MONOMODDED_WTF.pdb" /F
move /Y "%targetdir%..\..\MONOMODDED_WTF.exe" "%targetdir%..\..\WTF_modded.exe"