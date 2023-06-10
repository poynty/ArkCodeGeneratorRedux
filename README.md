# ArkCodeGeneratorRedux

Full rights and credit to the original author of ArkCodeGenerator.exe as provided by https://www.arkmod.net/sourcefiles

# What is it?

This is a cheeky rework of the ArkCodeGenerator. It adds a string to the end of the output file containing the info S+ requires to pull resources.
i.e. PullResourceAdditions=

# Build Tips

1 -> Clone this repository

2 -> Build with Visual Studio (.net 4.7.2), VS Code or the command line.

# Usage

Exactly the same as ArkCodeGenerator. Place exe in mod directory, for example <Z:\SteamLibrary\steamapps\common\ARK\ShooterGame\Content\Mods\710880648> and run. All things being good it will produce a text file with filename \<ModName\>_\<ModID\>.txt. 
  
 Note this tool only works in the Mods folder of the game. It will not work correctly in the local workshop cache on your machine. I'll make a new tool to handle both cases at some point.
