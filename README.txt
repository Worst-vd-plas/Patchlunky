
Patchlunky - Spelunky HD Mod Manager
Version 1.0.0.1 Beta
Copyright (c) 2016, Worst-vd-plas

Web site: https://github.com/Worst-vd-plas/Patchlunky


Contents:
=========
 1. Description
 2. System Requirements
 3. Using Patchlunky
 4. Making mods for Patchlunky
 5. Planned Features
 6. Changelog
 7. Credits
 8. License
 9. Contact


1. Description
==============

Patchlunky is a Spelunky HD Mod and Skin manager which is partially inspired from the Slipstream Mod Manager for FTL.

The Mod Manager is designed to:

  * Make installing/uninstalling mods easy and fast.
  * Allow multiple different mods to be patched to the game.(*)
  * Make mod distribution simple, by having a dedicated file type for mods. (*.plm)

The Skin Manager allows changing the skin of any of the 20 player characters found in the game.
It supports the default skins aswell as any skins it finds in the Skins folder.


(*) If multiple mods contain the same file(s) or resource(s), the last mod patched will override the previous mod(s).


2. System Requirements
======================

* .NET Framework 4
* around 400 MB free space for backups and temporary files.


3. Using Patchlunky
===================

When you launch Patchlunky for the first time, the setup wizard will notify you about 
needing the path to the game. If you have the steam or GOG version of Spelunky HD, you can 
let Patchlunky find the game folder automatically, otherwise you'll need to browse to 
it manually.

The setup wizard will also ask about backing up the original game files, this is required
for Patchlunky to function at all. Before you let Patchlunky perform the backup make sure
that your game is not modded! Patchlunky will use the data from the files that it backs up
as a base for patching mods to.

After you are done with these two steps, the Patchlunky main window will appear. There are
three tabs, aswell as two buttons below the tabs that are visible regardless of the active tab.

Patch Game button: 
  Patches the currently checked Mods and Skins to the game. 
  (Make sure that you don't have Spelunky opened when pressing this)

Launch Game button: 
  Launches Spelunky HD.


Mods
----
The Mods tab displays a checked listbox of mods that Patchlunky finds from the Mods folder
located inside the Patchlunky folder. If you add a mod while Patchlunky is running, you will
need to restart Patchlunky to refresh the list of mods.

At the top left there is a combo box that displays the active mod configuration. The mod 
configurations specify which mods are checked, and what the order of the mods is. You can save 
custom mod configurations by typing a name in the combo box and pressing the 'Save' button next 
to it.

You can select a mod in the mod list and click the up/down buttons to move it before/after
another mod to change the order the mods are patched to the game. This only makes difference
if two mods affect the same resources in the game, and you wish to have one override the other.


Character Skins
---------------
The Character Skins tab displays a tile view of the game characters. You can double-click on
a character to switch to the skin browser, which looks the same but the background is cyan color
instead. It includes the default skins and also any skins it finds from the Skins folder located 
inside the Patchlunky folder. Double-clicking a skin will assign it to the character you 
previously chose.

The Character Skins tab also includes a combo box for selecting the active skin configuration.
It works the same way as mod configurations do, but for skins.


Settings
--------
At the top left of the settings tab you can see the status of Patchlunky. If everything in the
status box is "OK.", Patchlunky should be functional.

Setup Patchlunky button:
  You can press this button to force the Patchlunky Setup wizard to run again.

Restore default game files:
  This will replace the game files with the backups that were created with the Patchlunky setup
  wizard. If you want skins but no mods, you should instead uncheck all the mods, and press the
  Patch Game button so that it only patches the skins.

Mods can replace default skins:
  This setting allows mods to replace any character skins that have no custom skins assigned to
  them. If you want to see character skins embedded inside a mod, make sure this setting is
  enabled, and also select 'Reset ALL characters to default' in the Character Skins tab.

Autosave mod configuration:
  Enables saving the current mod configuration when patching or closing patchlunky.

Autosave skin configuration:
  Enables saving the current skin configuration when patching or closing patchlunky.


4. Making mods for Patchlunky
=============================

An easy way to start a new mod for Patchlunky is to make a folder inside the Mods folder, using
the name of the new mod as the folder name. This new folder inside the Mods folder will contain
the contents of your mod.

Inside your mod folder you can create subfolders and files the same way as they would show up in
the Spelunky\Data directory. A Patchlunky mod may contain modded versions of most of the spelunky
data files.

It is STRONGLY RECOMMENDED that you DO NOT include modified alltex or allsounds WADs however, as
it goes against the idea of being able to combine multiple different mods together.

Instead of including the WAD files, you should create a folder where you would normally have the
WAD file at, and inside that folder create subfolders that match the groups that your mod modifies
and then include the .png or .wav files that your mod changes. See 'Patchlunky Example Mod 1' and
'Patchlunky Example Mod 2' for examples on how to do this.

In the main folder of your mod, you should create a textfile named "readme.txt" in which you can
write a description of the mod. You can also include a .png image named "picture.png" for your mod,
which should be 16:9 in aspect ratio, and recommended resolution is 512x288.
Patchlunky will display the text (and picture if supplied) in the Mod tab when the mod is selected.

When you are done making your mod, you should zip the CONTENTS of your mod folder (but not the mod 
folder itself), and rename the .zip file extension of the archive to ".plm".

For example "my mod.zip" -> "my mod.plm"

Then move the .plm file to the Patchlunky Mods folder, and move the main folder of your mod out of the
Patchlunky Mods folder, leaving only the .plm file there. Launch Patchlunky, and verify that the .plm
file for your mod is working as intended.

If everything works, you are ready to distribute the .plm file containing your mod!
(Note that you don't need to put the .plm file into a zip for distribution, because it is one already)



5. Planned Features
===================
 
 * A feature that allows mods to replace specific strings instead of the whole file.
 * A feature that allows mods to replace a portion of an image instead of the whole file.
 * A tab for adjusting some Spelunky settings such as windowed mode and resolution.


6. Changelog
============

- version 1.0.0.1 Beta
 * Minor update
 * Added skin configurations
 * Autosave settings for saving configurations on patch/exit.
 * Setting for allowing mods to replace default skins.

- version 1.0.0.0 Beta
 * Initial release


7. Credits
==========

 * Connor 'Contron' Haigh - Made the SpelunkyWad C# library, which has a big role in Patchlunky.
 * Derek Yu - Spelunky Creator, Lead Artist
 * Andy Hull -  Spelunky Lead Programmer, Designer
 * Eirik Suhrke - Spelunky Musician


8. License
==========

Patchlunky is licensed under Simplified BSD License (see LICENSE.Patchlunky.txt)

Patchlunky makes use of the following third-party code:


SpelunkyWad
MIT License (see LICENSE.SpelunkyWad.txt)
https://github.com/Contron/SpelunkyWad


DotNetZip 
Microsoft Public License (see LICENSE.DotNetZip.txt)
https://dotnetzip.codeplex.com/


ZLIB
zlib License (see LICENSE.DotNetZip.zlib.txt)
http://zlib.net/


BZIP2
BSD License/Apache License 2.0 (see LICENSE.DotNetZip.bzip2.txt)
http://www.bzip.org/


9. Contact
==========

If you have Bug reports, feature requests or questions, you can post an issue at GitHub:
  https://github.com/Worst-vd-plas/Patchlunky/issues?state=open


You can also contact me on reddit:
  https://www.reddit.com/message/compose/?to=worst-vd-plas
