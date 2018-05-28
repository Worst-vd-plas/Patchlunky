
Patchlunky - Mod Manager for Spelunky HD
Version 1.0.0.2 Beta
Copyright (c) 2018, Worst-vd-plas

Web site: https://github.com/Worst-vd-plas/Patchlunky


Contents:
=========
 1. Description
 2. System Requirements
 3. Using Patchlunky
 4. Making mods for Patchlunky
 5. Lua Scripting Support
 6. Patchlunky URL Protocol
 7. Planned Features
 8. Changelog
 9. Credits
10. License
11. Contact


1. Description
==============

Patchlunky is a mod manager for Spelunky HD.

Features of Patchlunky include:

  * Easily install/uninstall mods.
  * Support for most existing Spelunky HD mods.
  * Multiple mods can be patched to the game at the same time.[*]
  * Change any of the 20 player characters in the game.
  * Load .png files as custom character skins.
  * Dedicated file types for mods and characters (*.plm and *.plc).
  * XML based mods and characters that support additional features.
  * Custom URL protocol for downloading mods or characters directly into Patchlunky.
  * Lua scripting support for mods (it is quite limited atm)


[*] If multiple mods contain the same file(s) or resource(s), the last mod patched will override the previous mod(s).


2. System Requirements
======================

* .NET Framework 4
* around 400 MB free space for backups and temporary files.


3. Using Patchlunky
===================

When you launch Patchlunky for the first time, the setup wizard will ask you for the
path to the game. If you have the steam or GOG version of Spelunky HD, you can let
Patchlunky find the game folder automatically, otherwise you'll need to browse to it manually.

The setup wizard will also ask about backing up the original game files, this is required
for Patchlunky to function at all. Before you let Patchlunky perform the backup make sure
that your game is not modded! Patchlunky will use the data from the files that it backs up
as a base for patching mods to.

Finally the setup wizard will ask about registering the Patchlunky URL protocol with your
system. This allows opening special links in your web browser that interact with Patchlunky.
What this means in practise is that by clicking on a link you can let Patchlunky download
a mod to the correct location without you having to manually save it in the right folder.

After you are done with these steps, the Patchlunky main window will appear. There are four
tabs and also a bottom part below the tabs that is visible regardless of the active tab.


Bottom part
-----------
The bottom part includes buttons for patching or launching the game, a progress bar for
showing patch progress and a log message display.

Patch Game button:
  Patches the currently selected Mods and Characters to the game.
  (Make sure that you don't have Spelunky opened when pressing this)

Launch Game button: 
  Launches Spelunky HD.


Mods
----
The Mods tab displays a checked listbox of mods that Patchlunky has found from the Mods folder
located inside the Patchlunky folder. If you add a new mod while Patchlunky is running, you
can refresh the list of mods by clicking the 'Reload Mods' button found in the Downloads tab.

At the top left there is a combo box that shows the active saved mod list. The saved mod lists
specify which mods are checked, and what the order of the mods is. You can create custom saved
mod lists by typing a name in the combo box and pressing the 'Save' button next to it.

You can select a mod in the list of mods and click the up/down buttons to move it before/after
another mod to change the order in which the mods are patched to the game. This only makes
difference if two mods affect the same resources in the game, and you wish to have one override
the other.


Characters
----------
The Characters tab displays a tile view of the game characters. In order to change one of the
characters, you can double-click a character to switch to the character browser. The character
browser looks the same but the background is cyan instead. It includes the default characters
and also any characters it finds from the Skins folder located inside the Patchlunky folder.
Double-clicking a character in the browser will make it replace the character that you double-
clicked before.

The Characters tab also includes a combo box for selecting the active saved character list.
It works the same way as saved mod lists do, but for characters.


Downloads
---------
The Downloads tab displays a list of downloads for the current session. Downloads can be moved
up/down in the list to change their queueing order. You can remove a download from the list by
pressing the 'Remove' button, but note that files are not removed for completed downloads.

To remove files you can press the 'Open Mods folder' or 'Open Characters folder' button to open
the appropiate folder in Windows Explorer, and then delete the file of your choice from there.

Reload Mods button:
  Scans the 'Mods' folder for mods and then refreshes the list of mods.

Reload Characters button:
  Scans the 'Skins' folder for characters and then refreshes the list of characters.


Settings
--------
At the top left of the settings tab you can see the status of Patchlunky. If everything in the
status box is "OK.", Patchlunky should be functional.

Setup Patchlunky button:
  You can press this button to force the Patchlunky Setup wizard to run again.

Restore default game files:
  This will replace the game files with the backups that were created with the Patchlunky setup
  wizard. If you want custom characters but no mods, you should instead uncheck all the mods,
  and press the Patch Game button so that it only patches the characters.

Mods can replace default characters:
  Allows mods to replace any default characters that have not been assigned a custom character.
  If you want to see characters included inside mods, make sure to enable this setting. Select
  'Reset ALL characters' in the Characters tab to ensure that you have the default
  characters.

Autosave mod list:
  Enables saving the current mod list when patching or closing Patchlunky.

Autosave character list:
  Enables saving the current character list when patching or closing Patchlunky.


4. Making mods for Patchlunky
=============================

Simple mods
-----------
An easy way to start a new mod for Patchlunky is to make a folder inside the Mods folder, using
the name of the new mod as the folder name. This new folder inside the Mods folder will contain
the contents of your mod.

Inside your mod folder you can create subfolders and files the same way as they would show up in
the Spelunky\Data directory. A Patchlunky mod may contain modded versions of most of the spelunky
data files.

It is STRONGLY RECOMMENDED however that you DO NOT include modified alltex or allsounds WADs, as
they go against the idea of being able to combine multiple different mods together.

Instead of including the WAD files, you should create a folder at the location where the WAD file
would be, naming that folder like the WAD file, but without the file extension. So for example
naming the folder 'alltex' or 'allsounds'. Inside that folder create subfolders that match the
WAD groups that your mod affects, and then include the .png or .wav files that your mod changes.
See 'Patchlunky Example Mod 1' and 'Patchlunky Example Mod 2' for examples on how to do this.

In the main folder of your mod, you should create a textfile named "readme.txt" in which you can
write a description of the mod. You can also include a .png image named "picture.png" for your mod,
which should be 16:9 in aspect ratio, and recommended resolution is 512x288.
Patchlunky will display the text (and picture if supplied) in the Mod tab when the mod is selected.

When you are done making your mod, you should zip the CONTENTS of your mod folder (but not the mod 
folder itself), and rename the .zip file extension of the archive to ".plm".

For example "my mod.zip" -> "my mod.plm"

Then move the .plm file to the Patchlunky Mods folder, and move the main folder of your mod somewhere
elsewhere outside the Patchlunky Mods folder, leaving only the .plm file there. Launch Patchlunky,
and verify that the .plm file for your mod is working properly.

If everything works, you are ready to distribute the .plm file containing your mod!
(Note: There is no need to put the .plm file into a zip for distribution, because it is one already)


XML-mods
--------
Mods that include a 'mod.xml' file, will be treated according to the properties they specify in the
xml file. See 'Patchlunky XML Mod' for an example.

Supported tags:
  * Name : Name of the mod.
  * Id : Unique ID for the mod, used for detecting duplicates, etc.
  * Version : Version number or string for the mod.
  * Date : The date the mod was finished, in YYYY-MM-DD format.
  * Author : Name of the author(s) of the mod.
  * Email : Contact email for the mod.
  * WebUrl : Web address for the mod.
  * Description : The description of the mod.
  * TextFile : Path to text file with description, overrides the 'Description' tag.
  * PreviewImage : Image displayed for the mod in the mod manager.
  * PatchlunkyVersion : Minimum Patchlunky version required for the mod to work.
  * PatchDefaultFiles : Set to true to copy any matching file from the mod folders.
  * PatchScript : Path to a Lua patchscript file for the mod.


XML-characters
--------------
Characters can be created in a way similar to XML-mods, by including a 'character.xml' file inside
a zip file with the '.plc' file extension. See 'hiredhand.plc' for an example.

Supported tags:
  * Name : Name of the character.
  * Id : Unique ID for the character, used for detecting duplicates, etc.
  * Version : Version number or string for the character.
  * Date : The date the character was finished, in YYYY-MM-DD format.
  * Author : Name of the author(s) of the character.
  * Email : Contact email for the character.
  * WebUrl : Web address for the character.
  * Description : The description of the character.
  * PreviewImage : Image displayed for the character in the mod manager.
  * PatchlunkyVersion : Minimum Patchlunky version required for the character to work.
  * CharacterImage : The spritesheet for the character.
  * LeaderboardImage : Image to display in leaderboards, 128x48 pixels.
                       To match with original images, the image should have:
                       - a 128px wide and 6px high transparent area
                       - a 128px wide and 36px high portrait area
                       - a 128px wide and 6px high transparent area
                       (Note that other people will not see the image unless they
                        have exactly the same characters patched into the game)

5. Lua Scripting Support
==========================
!TODO: Add proper documentation here!


6. Patchlunky URL Protocol
==========================
Patchlunky supports URL commands that can be used like web page links.

patchlunky://download/mod/<url>
patchlunky://download/char/<url>
  Opens a dialog for downloading the mod or character from the URL.
  For example: patchlunky://download/mod/https://www.site.net/download.php?file=214

For these links to work, you need to register the URL protocol with the setup wizard.


7. Planned Features
===================
 
 * A feature that allows mods to replace specific strings instead of the whole file.
 * A tab for adjusting some Spelunky settings such as windowed mode and resolution.


8. Changelog
============

- version 1.0.0.2 Beta
 * Support GOG version of Spelunky
 * XML-based mods and characters
 * Patchlunky URL protocol
 * Download Manager
 * Mod content type detection
 * Limited Lua scripting support for mods

- version 1.0.0.1 Beta
 * Minor update
 * Added skin configurations
 * Autosave settings for saving configurations on patch/exit.
 * Setting for allowing mods to replace default skins.

- version 1.0.0.0 Beta
 * Initial release


9. Credits
==========

 * Connor 'Contron' Haigh - Made the SpelunkyWad C# library, which has a big role in Patchlunky.
 * Derek Yu - Spelunky Creator, Lead Artist
 * Andy Hull -  Spelunky Lead Programmer, Designer
 * Eirik Suhrke - Spelunky Musician


10. License
==========

Patchlunky is licensed under Simplified BSD License (see LICENSE.Patchlunky.txt)

Patchlunky makes use of the following third-party code:


SpelunkyWad
MIT License (see LICENSE.SpelunkyWad.txt)
https://github.com/Contron/SpelunkyWad


Lua
MIT License (see LICENSE.Lua.txt)
https://www.lua.org/


NLua
MIT License (see LICENSE.NLua.txt)
https://github.com/NLua/NLua


KopiLua
MIT License (see LICENSE.NLua.txt)
http://www.ppl-pilot.com/kopilua.aspx


DotNetZip 
Microsoft Public License (see LICENSE.DotNetZip.txt)
https://dotnetzip.codeplex.com/


ZLIB
zlib License (see LICENSE.DotNetZip.zlib.txt)
http://zlib.net/


BZIP2
BSD License/Apache License 2.0 (see LICENSE.DotNetZip.bzip2.txt)
http://www.bzip.org/


MersenneTwister stlalv C# port
Mersenne Twister License (See LICENSE.MersenneTwister.txt)
http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/VERSIONS/C-LANG/c-lang.html


11. Contact
===========

If you have Bug reports, feature requests or questions, you can post an issue at GitHub:
  https://github.com/Worst-vd-plas/Patchlunky/issues?state=open


You can also contact me on reddit:
  https://www.reddit.com/message/compose/?to=worst-vd-plas
