# HATE - UndertaleModLib edition

Hi, \[NO NAME] here. [The guy that made this](https://github.com/RedSpah) is not here, so I'm here to tell you some basic stuff, while I'm messing with the code to add a little [~~Grossley~~ UndertaleModTool](https://github.com/krzys-h/UndertaleModTool) into this tool.

HATE (the UNDERTALE corruptor) is a tool that shuffles the RPG's game data, and as a result, corrupts music, graphics, text, etc. This is a fork of the corruptor, that makes it use the data I/O interface library (UndertaleModLib) that UndertaleModTool uses, so the tool can corrupt DELTARUNE Chapter 1&2 (maybe other GameMaker games - but that's not a goal of this tool). TL;DR this thing should support DELTARUNE if UndertaleModTool catchs up. Interested? ~~Follow the installation guide below and try it today~~ Come back later so I can finish this.

## Installation

NOTE: This tool is not released yet; these steps are subject to change.

Download the tool from [Releases](https://github.com/Dobby233Liu/HATE-UML/releases).

Windows:
1. Move everything in the ZIP file you downloaded to the directory with the game files - the place you can find `data.win` in.
2. Tada! Open `HATE.exe` and start messing with stuff.

NOTE: macOS and Linux versions are untested.

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

    **A:** If the error dialog box has the `HATE` title, please refer to the text below. If not, please submit an [issue](https://github.com/Dobby233Liu/HATE-UML/issues) so I can look into it.

* **Q:** Linux/macOS - HATE doesn't start and/or displays an error message instantly.

    **A:** I'm not sure if these steps even work, so this may be a issue on my side. Please submit an [issue](https://github.com/Dobby233Liu/HATE-UML/issues) so I can look into it.