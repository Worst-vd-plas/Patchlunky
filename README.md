# Patchlunky

Patchlunky is a mod manager for [Spelunky HD](http://www.spelunkyworld.com/).

Features of Patchlunky include:
 * Easily install/uninstall mods.
 * Support for most existing Spelunky HD mods.
 * Multiple mods can be patched to the game at the same time.
 * Change any of the 20 player characters in the game.
 * Load .png files as custom character skins.
 * Dedicated file types for mods and characters (*.plm and *.plc).
 * XML based mods and characters that support additional features.
 * Custom URL protocol for downloading mods or characters directly into Patchlunky.
 * Lua scripting support for mods (it is quite limited atm)

See [README.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/README.txt) for instructions on how to use Patchlunky.

# Downloads

You can download the latest release here: [Patchlunky Releases](https://github.com/Worst-vd-plas/Patchlunky/releases)

# Compiling Patchlunky

Patchlunky was written using Microsoft Visual C# 2010, targeting .NET Framework 4.

To compile patchlunky you need to include the libraries for DotNetZip and NLua,
they go in the correspondingly named directories.

## Dependencies

Patchlunky uses the following third-party libraries:

[DotNetZip](https://dotnetzip.codeplex.com/)
Project includes a reference to Ionic.Zip.dll

[SpelunkyWad](https://github.com/Contron/SpelunkyWad)
Older version of SpelunkyWad is included in the solution.

[NLua](https://github.com/NLua/NLua)
Project includes a reference to NLua.dll and KeraLua.dll

[Lua](https://www.lua.org/)
Project includes a reference to lua52.dll

[MersenneTwister stlalv C# port](http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/VERSIONS/C-LANG/c-lang.html)
Project includes the source file.

## Notes

The source code is far from ideal, as I don't have all that much experience
with C# or coding in general. There is a lot of cleaning and refactoring to
be done with the source code.

Currently the program lacks a lot of error handling, and may raise exceptions
in unexpected situations. This will be improved as I work on the code.

The SpelunkyWad library included with the source is not the latest version, due
to the latest version not being compatible with Microsoft Visual C# 2010, which
I am using. I'll have to look into upgrading my IDE at some point.

# License

Patchlunky is licensed under Simplified BSD License (see [LICENSE.Patchlunky.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.Patchlunky.txt))

Patchlunky makes use of the following third-party code:


SpelunkyWad  
MIT License (see [LICENSE.SpelunkyWad.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.SpelunkyWad.txt))  
https://github.com/Contron/SpelunkyWad

Lua
MIT License (see [LICENSE.Lua.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.Lua.txt))
https://www.lua.org/

NLua
MIT License (see [LICENSE.NLua.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.NLua.txt))
https://github.com/NLua/NLua

KopiLua
MIT License (see [LICENSE.NLua.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.NLua.txt))
http://www.ppl-pilot.com/kopilua.aspx

DotNetZip  
Microsoft Public License (see [LICENSE.DotNetZip.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.DotNetZip.txt))  
https://dotnetzip.codeplex.com/

ZLIB  
zlib License (see [LICENSE.DotNetZip.zlib.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.DotNetZip.zlib.txt))  
http://zlib.net/

BZIP2  
BSD License/Apache License 2.0 (see [LICENSE.DotNetZip.bzip2.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.DotNetZip.bzip2.txt))  
http://www.bzip.org/

MersenneTwister stlalv C# port
Mersenne Twister License (See [LICENSE.MersenneTwister.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.MersenneTwister.txt))
http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/VERSIONS/C-LANG/c-lang.html
