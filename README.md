# HATE-UML - The UNDERTALE corruptor, but using UndertaleModLib

Hi, \[NO NAME] here. [The guy that made this](https://github.com/RedSpah) is not here, so I'm here to tell you some basic stuff, while I'm messing with the code to add a little [~~Grossley~~ UndertaleModTool](https://github.com/krzys-h/UndertaleModTool) into this tool.

HATE (the UNDERTALE corruptor) is a tool that shuffles the RPG's game data, and as a result, corrupts music, graphics, text, etc. This is a fork of the corruptor, that makes it use the data I/O interface library (UndertaleModLib) that UndertaleModTool uses, so the tool can corrupt DELTARUNE Chapter 1&2 (maybe other GameMaker games - but that's not a goal of this tool). TL;DR this thing should support DELTARUNE if UndertaleModTool catchs up. Interested? ~~Follow the installation guide below and try it today~~ Give me time to finish this.

## Installation

NOTE: This tool is not released yet; these steps are subject to change.

Download the tool from [Releases](https://github.com/Dobby233Liu/HATE-UML/releases).

Windows:
1. Move everything in the ZIP file you downloaded to the directory with the game files - the place you can find `data.win` in.
2. Tada! Open `HATE.exe` and start messing with stuff.

NOTE: macOS and Linux versions are untested. TODO: dotnet CLI.

macOS:
1. Move everything in the ZIP file you downloaded to `<game>.app\Contents\Resources`.
2. Create a "Data" folder in the Resources folder.
3. Copy game.ios into the Data folder.
4. Tada! Open `HATE` and start messing with stuff.

Linux: (Note - make sure you're not root.)
1. Move everything in the ZIP file you downloaded to the directory with the game files - the place you can find an `assets` folder in, or the place you can find a `data.win` in if you want to run a Windows game.
2. Tada! Open `HATE` and start messing with stuff.

## Uninstallation

1. Run HATE with all settings disabled and with Power set to 0.
2. Delete everything that came with the tool.

## FAQ

* **Q:** Windows - HATE doesn't start and/or displays an error message instantly.

    **A:** You may need to install the [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime) (one of the downloads below `Run desktop apps`, depending on your computer). I decided to not bundle framework-related files with the release, because it makes the download's size **huge**.

* **Q:** Linux/macOS - HATE doesn't start and/or displays an error message instantly.

    **A:** I'm not sure if these steps even work, so this may be a issue on my side. Please submit an [issue](https://github.com/Dobby233Liu/HATE-UML/issues) so I can look into it.

## Building

In order to compile the tool yourself, the .NET Core 6 SDK is required. The source tree also contains a `UndertaleModLib` submodule for the version of the ModLib it uses; you will need to download the submodule before building.

### Compiling Via IDE

* Open the UndertaleModTool.sln in the IDE of your choice (Visual Studio, JetBrains Rider, Visual Studio Code etc.)
* Select the `HATE` project.
* Compile.

### Compiling Via Command Line

* Open a terminal and navigate to the project root.
* Execute `dotnet publish HATE`.
  * You can also provide arguments for compiling, such as `--no-self-contained` or `-c release`. For a full list of arguments, consult [this document](https://docs.microsoft.com/dotnet/core/tools/dotnet-publish).

## Source Code

Source code can be found on GitHub: https://github.com/Dobby233Liu/HATE-UML

## License

This program is licensed under the [GNU General Public License v3.0](COPYING). The UndertaleModLib library used in this program is also licensed under the GNU General Public License v3.0.

```
UndertaleModLib: A library for unpacking, decompiling, and modding Undertale
(and other GameMaker: Studio games)
Copyright (C) 2018-2022 krzys-h and contributors
```

```
HATE-UML: The UNDERTALE corruptor, but using UndertaleModLib.
Copyright (C) 2016 RedSpah
Copyright (C) 2022 Dobby233Liu

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
```