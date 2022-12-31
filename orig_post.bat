if $(ConfigurationName) == Debug (
	echo F | xcopy /F /Y "$(TargetPath)" "$(SolutionDir)..\..\BepInEx\monomod\WTF.FFU_TL.mm.dll"
) else if $(ConfigurationName) == Release (
	echo F | xcopy /F /Y "$(TargetPath)" "$(SolutionDir)..\..\WTF.FFU_TL.mm.dll"
	"$(SolutionDir)..\MonoMod\MonoMod.exe" "$(SolutionDir)..\..\WTF.exe"
	del "$(SolutionDir)..\..\WTF.FFU_TL.mm.dll" /F
	del "$(SolutionDir)..\..\MONOMODDED_WTF.pdb" /F
	move /Y "$(SolutionDir)..\..\MONOMODDED_WTF.exe" "$(SolutionDir)..\..\WTF_modded.exe"
) else if $(ConfigurationName) == Publish (
	echo F | xcopy /F /Y "$(TargetPath)" "$(SolutionDir)..\..\WTF.FFU_TL.mm.dll"
	"$(SolutionDir)..\MonoMod\MonoMod.exe" "$(SolutionDir)..\..\WTF.exe"
	del "$(SolutionDir)..\..\MONOMODDED_WTF.pdb" /F
	move /Y "$(SolutionDir)..\..\MONOMODDED_WTF.exe" "$(SolutionDir)..\..\WTF_modded.exe"
)