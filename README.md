# Star Valor Predictive Missile

An BepInEx plugin for [Star Valor](https://store.steampowered.com/app/833360/Star_Valor/).

Changes the missile AI to lead targets and spaces missiles apart when fired at larger targets.

------------------------------

Installing Plugins
---
These mods require the BepInEx mod framework.
Install the latest 5.x (x86) release, see here for instructions: [BepinEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html)

**Important**: Star Valor is a 32-bit application. Make sure you choose the 32-bit version of BepinEx.

After installing BepInEx, download the latest release of the plugin (the .dll file) from the link below and put it in the BepInEx/plugins folder inside of your Star Valor game directory.

* As with all mods, make sure to **back up your save game** before trying a new mod.
* Only download mods from reliable sources: as with everything on the internet, be careful.

After installing, if your mods don't work, try the following:
> Navigate to the where you installed BepinEx, and open doorstop_config.ini with a text editor

> Change "ignoreDisableSwitch=false" to "ignoreDisableSwitch=true"

------------------------------

## Predictive Missile Mod Usage

- By default all missiles (both yours and NPCs) will use the new missile aiming logic.

- There are options in the config file to disable the missile aiming for NPCs and to disable missile spacing.

------------------------------

## Developer : How to Build & Customize

**Minimum Requirement :**

- Visual Studio that Supports .NET Standard 2.0 and .NET library project, w/ dependencies fulfilled.

- [BepInEx NuGet repo](https://nuget.bepinex.dev/) as one of your Visual Studio NuGet source (https://nuget.bepinex.dev/v3/index.json)

**Dependencies**

This library project depends on several NuGet packages and a directly referred assembly/.dll file. While the NuGet packages are auto-resolved and auto-restored by Visual Studio, the assembly is not. The assembly is from :

- `Assembly-CSharp.dll` from the game's `Star Valor_Data\Managed\` folder

The project is configured to looks for the required assembly inside `<.csproj root folder>..\..\Libraries\Assembly-CSharp.dll` folder. You can fulfill those dependencies simply by copying the file into it.

Consult the build/Visual Studio error message or the `.csproj` file for the full list of the required dependencies.
