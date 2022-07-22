# HATE-UML

[HATE (the UNDERTALE corruptor)](https://www.reddit.com/r/Undertale/comments/41lb16/hate_the_undertale_corruptor/) is a tool that shuffles the RPG's game data, and as a result, corrupts music, graphics, text, etc. This is a fork of the corruptor, that makes it use the data I/O interface library (UndertaleModLib) that [UndertaleModTool](https://github.com/krzys-h/UndertaleModTool) uses, in hope that so the tool can corrupt DELTARUNE Chapter 1&2 (maybe other games made with GameMaker - but that's not a goal of this tool).

TL;DR this is a modification of the good old HATE that you may have came across, that should support the latest version of DELTARUNE (at least if UndertaleModTool catchs up).

I heard you want to destroy the fabric of time-space continuum and bring living hell to the second dark world you have access to? Or do you want to make the underground a boiling mess (maybe again, for some reason)? If so, ~~follow the installation guide below and try it today~~ Just give me time to finish this.

**HEALTHY \[REDACTED]'S WARNING:** Using the Shuffle GFX option may cause flashing lights in-game. Please also back your saves up if you want to try this tool, as there is a chance that this tool will break them.

## The original              corruptor

Bing gave me this page when I searched for HATE on the search engine, however this fork is not ready yet.
So it's best to not disappoint the newcomers, I have to give them something to use.

At least you can check the original corruptor out. Note that it only supports UNDERTALE versions <=1.08, and the original release of DELTARUNE Chapter 1 in 2018.

* [Original version](https://www.reddit.com/r/Undertale/comments/41lb16/hate_the_undertale_corruptor/);
* [DELTARUNE Chapter 1 special version](https://www.reddit.com/r/Deltarune/comments/9v1vd7/hate_the_deltarune_corruptor/);
* The [source code](https://github.com/RedSpah/HATE).

Also, shout out to RedSpah. Dude definitely heavily suffered during the development of the original.

## Installation

**NOTE:** This tool is not released yet, and these steps are subject to change.

**NOTE:** macOS and Linux versions are **untested**; you have been warned.

First of all, download the tool from the [Releases](https://github.com/Dobby233Liu/HATE-UML/releases) page.

### Windows

You may need to install the [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime) (one of the downloads below `Run desktop apps`, depending on your computer). I decided to not bundle framework-related code with the release, because it makes the download's size *absurdly huge*.

1. Move everything in the release ZIP file to you game directory (the place you can find `data.win` and `UNDERTALE.exe` or `DELTARUNE.exe` in).
2. Tada, you're set! Open `HATE.exe` and start messing with stuff.

### macOS

Install the .NET 6 Runtime if you don't have it; see the [Install .NET on macOS](https://docs.microsoft.com/en-us/dotnet/core/install/macos) document on Microsoft Docs.
Make sure to install `libgdiplus` too.

1. Move everything in the release ZIP file to `<game>.app\Contents\Resources`.
2. Tada, you're set! Execute `HATE.sh` and start messing with stuff.

### Linux

Install the .NET 6 Runtime if you don't have it; see the [Install .NET on Linux](https://docs.microsoft.com/en-us/dotnet/core/install/linux) document on Microsoft Docs.
Make sure to install `libgdiplus` too.

1. Move everything in the release ZIP file to the directory with the game files - the place you can find an `assets` folder in, or the place you can find a `data.win` in if you want to corrupt a Windows game (in that case, also install Wine).
2. Tada, you're set! Execute `HATE.sh` and start messing with stuff.

## Uninstallation

1. Run HATE with all settings disabled and with Power set to 0. This will restore your original data files.
2. Delete everything that came with the tool.

## FAQ

* **Q:** Linux/macOS - `HATE.sh` doesn't run.

    **A:** You may need to run `chmod +x HATE.sh` in a terminal.

* **Q:** HATE doesn't start and/or displays an error message instantly.

    **A:** Check whether you have correctly performed the installation steps. If there are error boxes that say anything meaningful, please read their contents.
           If you're sure you did everything correctly, then this may be an issue on my side. Please submit an [issue](https://github.com/Dobby233Liu/HATE-UML/issues) on GitHub so I can look into it.

## Building

In order to compile the tool yourself, the .NET Core 6 SDK is required. The source tree also contains a `UndertaleModTool` submodule which the tool uses to include the ModLib in the project; you will need to download the submodule before building.

### Compiling Via IDE

* Open HATE.sln in the IDE of your choice (Visual Studio, JetBrains Rider, Visual Studio Code etc.)
* Select the `HATE` project.
* Compile.

### Compiling Via Command Line

* Open a terminal and navigate to the project root.
* Execute `dotnet publish HATE`.
  * You can also provide arguments for compiling, such as `--no-self-contained` or `-c release`.
    For a full list of arguments, consult [this document](https://docs.microsoft.com/dotnet/core/tools/dotnet-publish).

## Source Code

Source code can be found on GitHub:
https://github.com/Dobby233Liu/HATE-UML

## License

This program is licensed under the [GNU General Public License v3.0](COPYING.txt).
The original program this is based on is licensed under the [MIT License](LICENSE.MIT.txt).
