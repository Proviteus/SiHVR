# VR Plugin for the game SummerInHeat
Major parts of this plugin were forked from [KKS_VR](https://github.com/IllusionMods/KKS_VR) with the OpenXR integration forked from [VRGIN_OpenXR](https://github.com/ManlyMarco/VRGIN_OpenXR).

Some modifications were made to get this working in SummerInHeat and so far it has only been tested with the 1st generation HTC Vive.

The plugin is very janky and I can't say it works very well, but it allows the game to load into VR. Seated Mode is all that works for now unfortunately.

## Installation

1. Make sure the game is patched and BepInEx is installed.
2. Drag the files from the zip into the proper directory
3. Create a shortcut from the game's application file (SummerInHeat.exe) located in the GameData folder, and add --vr to the end of the line in "Target:". It should look something like this after: "C:\Your\Directory\GameData\SummerInHeat.exe" --vr
4. Either start SteamVR (If you use it) or run the game straight from the shortcut you made.
