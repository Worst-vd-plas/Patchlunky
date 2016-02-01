# Patchlunky

Patchlunky is a mod and skin manager for [Spelunky HD](http://www.spelunkyworld.com/).

The Mod Manager is designed to:
 * Make installing/uninstalling mods easy and fast.
 * Allow multiple different mods to be patched to the game.
 * Make mod distribution simple, by having a dedicated file type for mods. (*.plm)

The Skin Manager allows changing the skins of the 20 player characters in the game.


You can download the latest release here: [Patchlunky-1.0.0.0-beta.zip](https://github.com/Worst-vd-plas/Patchlunky/releases/download/v1.0.0.0/Patchlunky-1.0.0.0-beta.zip)

See [README.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/README.txt) for instructions on how to use Patchlunky.

# Compiling Patchlunky

Patchlunky was written using Microsoft Visual C# 2010, targeting .NET Framework 4.

## Dependencies

Patchlunky uses the following third-party libraries:

[DotNetZip](https://dotnetzip.codeplex.com/)

You need to include a reference to Ionic.Zip.dll

[SpelunkyWad](https://github.com/Contron/SpelunkyWad)

An old version of SpelunkyWad is currently included with the solution.

## Notes

The source code is far from ideal, as I don't have much prior experience
with C# or coding in general. Writing Patchlunky has been kind of a big
learning experience for me, but perhaps also a showcase of too little
planning with regards to the program structure. I'll have to clean up and
refactor the code here and there.

Currently the program lacks a lot of error handling, and will probably raise
exceptions in most unexpected situations. I'll improve this as I work on the code.

The SpelunkyWad library included with the source is not the latest version, due
to the latest version not being compatible with the version of MS Visual C# I am using.
I'll have to look into upgrading my IDE for this.

# License

Patchlunky is licensed under Simplified BSD License (see [LICENSE.Patchlunky.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.Patchlunky.txt))

Patchlunky makes use of the following third-party code:


SpelunkyWad  
MIT License (see [LICENSE.SpelunkyWad.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.SpelunkyWad.txt))  
https://github.com/Contron/SpelunkyWad

DotNetZip  
Microsoft Public License (see [LICENSE.DotNetZip.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.DotNetZip.txt))  
https://dotnetzip.codeplex.com/

ZLIB  
zlib License (see [LICENSE.DotNetZip.zlib.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.DotNetZip.zlib.txt))  
http://zlib.net/

BZIP2  
BSD License/Apache License 2.0 (see [LICENSE.DotNetZip.bzip2.txt](https://github.com/Worst-vd-plas/Patchlunky/blob/master/License/LICENSE.DotNetZip.bzip2.txt))  
http://www.bzip.org/

