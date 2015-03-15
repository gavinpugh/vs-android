<p align='right'>
<br>
</p>

# Introduction #

<img src='http://www.gavpugh.com/img/vs-android/paneicon.png' align='right'>

<i>vs-android</i> is intended to provide a collection of scripts and utilities to support integrated development of Android NDK C/C++ software under Microsoft Visual Studio.<br>
<br>
vs-android supports only Visual Studio 2010, 2012 and 2013. Earlier versions lack the MSBuild integration with the C/C++ compilation systems.<br>
<br>
Also, the free "Express" editions of Visual Studio are not supported. If you wish to use a free version of Visual Studio, use the new "Community Edition" of Visual Studio 2013. That works fine with external plugins, like vs-android.<br>
<br>
<br>
<br>
<h1>Features</h1>

<ul><li>Compile and link Android C/C++ projects within Visual Studio.<br>
</li><li>Integrated development, no makefiles. Works as another 'Platform' type.<br>
</li><li>Android settings co-exist within the same VS projects as other platforms.<br>
</li><li>Supports static library projects, and links them in if marked as project dependencies.<br>
</li><li>Intellisense and 'External Dependencies' correctly pull in Android NDK headers.<br>
</li><li>Cygwin install is not required.<br>
</li><li>Over twice as fast as using ndk-build under Cygwin. For full rebuilds, and incremental changes.<br>
</li><li>Ctrl-F7: compile single file functions, has no dependency checking wait.<br>
</li><li>Applications build to .apk package files, and can be deployed and ran using vs-android.<br>
</li><li>Supports selection of STL types, Arm Architecture, and API versions.<br>
</li><li>Support for the ARM, MIPS and x86 toolchains.</li></ul>

<h1>Download</h1>

<ul><li>Latest version: <a href='http://www.gavpugh.com/downloads/vs-android-0.964.zip'>vs-android-0.964.zip</a>
</li><li>Samples: <a href='http://www.gavpugh.com/downloads/vs-android_samples.zip'>vs-android_samples.zip</a> - Set of sample projects<br>(last updated for v0.94 - 24th July 2012 - Still works with latest)</li></ul>

<h1>Installation</h1>

<ul><li><a href='Installation.md'>A guide to installing vs-android</a></li></ul>

<h1>Usage</h1>

<ul><li><a href='HowTo_Samples.md'>Compiling a simple Android app with vs-android</a>
</li><li><a href='Troubleshooting.md'>Troubleshooting</a></li></ul>

<h1>Tech Notes / Future Plans</h1>

<ul><li><a href='TechNotesFuturePlans.md'>Technical notes about implementation, and future plans for vs-android</a></li></ul>

<h1>Version History</h1>

v0.964 - 28th December 2014<br>
<br>
<ul><li>Updated to support the r10d NDK.<br>
</li><li>GCC 4.6 has been deprecated by Google. It is still supported, but the default GCC version vs-android uses, is now GCC 4.8.<br>
</li><li>Added support for GCC 4.9.<br>
</li><li>Added support for the "android-21" target (aka "Android 5.0", "Lollipop").<br>
</li><li>Integrated "tlog" generation fix, thanks to "drew@thedunlops".<br>
</li><li>NOTE: This version has "Ignore All Default Libraries" defaulting to "No" for GCC 4.8 and 4.9. Linker errors occur otherwise, when using any C++ features. GCC 4.6 still defaults this to "Yes", as it doesn't exhibit issues.</li></ul>

v0.963 - 19th July 2014<br>
<br>
<ul><li>Fix for issue with the "Windows 64-bit" NDK. The "TRACKER : error TRK0002: Failed to execute command" error.<br>
</li><li>Thanks to Ilya Konstantinov for the fix.<br>
</li><li>Added warning message for using the new "(64-bit Target)" NDK.<br>
</li><li>Confusingly there are 32-bit and 64-bit Windows flavors of the "(32-bit Target)" and "(64-bit Target)" NDKs.<br>
</li><li>vs-android only supports the "(32-bit Target)" NDK currently.<br>
</li><li>Also tested vs-android against the new <code>r10</code> NDK. All toolchains and platforms are current.</li></ul>

v0.962 - 9th June 2014<br>
<br>
<ul><li>Added support for lone-installs of Visual Studio 2013.<br>
</li><li>Thanks to ted@lindenlab, and "mellean". I simplified the approach in the end, using project imports rather than duplicating code.<br>
</li><li>Fixed pre/post-build steps not functioning correctly.<br>
</li><li>Thanks to "mellean" for the fix.<br>
</li><li>Tested against new "r9d" NDK. Appears to work fine, please let me know on the "Issues" page if I am mistaken.</li></ul>

v0.961 - 24th February 2014<br>
<br>
<ul><li>Added Precompiled Header support. Thanks to help from Richard Forster.<br>
</li><li>PCH support works similarly to the Win32/x64 compilers. You can enable "Create" for the compilation unit which should create the PCH, and "Use" for the project so have all files use that PCH.<br>
</li><li>Projects mistakenly setting the <code>ObjectFileName</code> to a directory, will now have an explanatory error.<br>
</li><li>/libs/armeabi-v7a is now used when the architecture is set to "armeabi-v7a". This addresses an issue when submitting apk's to the Play Store. Thanks to Ilja Plutschouw.<br>
</li><li>The default <code>PlatformToolset</code> for newly added configurations is now correctly set. Thanks to Chuck Evans.<br>
</li><li>The <code>BrowseInformation</code> flag is now ignored for <code>ClCompile</code>. Oft-imported setting when creating an Android platform on a Win32 project. Compilation fails if it was enabled. Thanks to C.Aragones for the headsup.</li></ul>

v0.96 - 4th January 2014<br>
<br>
<ul><li>Visual Studio 2013 has preliminary support now. It requires VS2012 to be installed alongside it. Unfortunately Microsoft have radically changed how MSBuild scripts implement new platforms. VS2012 express is probably fine.<br>
</li><li>Fix for running on the <code>r9</code> NDKs. The minimum requirement is now NDK r9c.<br>
</li><li>New NDK sees 4.3.3 and 4.7 GCC toolchains removed, and 4.8 added. vs-android now reflects these changes.<br>
</li><li>GCC 4.8 however still seems to have issues when using the STL. It appears fine if you don't use STL. Suggestions welcome on how to address it, my Google-fu has failed me when looking up the link errors. Stick with GCC 4.6 if you need STL support.<br>
</li><li>Added support for missing Android API targets: 12, 13, and 15 through 19. Goes up to Android 4.4 API now.</li></ul>

v0.951 - 22nd May 2013<br>
<br>
<ul><li>Fix for running on machines with a lone install of vs2010. I was doing all my testing on machines which have a dual vs2010 and vs2012 install. Apologies.</li></ul>

v0.95 - 22nd May 2013<br>
<br>
<ul><li>Visual Studio 2012 is now fully supported.<br>
</li><li>There are separate installers for vs2010 and vs2012. Scripts are identical between version, via use of conditionals in MSBuild. However both use different supporting DLL files.<br>
</li><li>.vcxproj and .sln files are cross-compatible between vs2012 and vs2010. Support for vs2010 will continue for vs-android in the foreseeable future.<br>
</li><li>The x64 version of the NDK is now supported.<br>
</li><li>Fix for Google breaking change to paths: "4.6.x-google" -> "4.6".<br>
</li><li>The default GCC toolchain version has been changed from 4.4.3 to 4.6.<br>
</li><li>The build scripts are now compliant with NDK r8e. Previous NDK versions are no longer supported.<br>
</li><li>GCC 4.7 is now an available toolchain choice. However, it is not currently usable with the STL. Google state that 4.7 is still experimental. I'd welcome any suggested fix, if it does indeed work in ndk-build.</li></ul>

v0.94 - 24th July 2012<br>
<br>
<ul><li>Completely reworked the deploy and run portions of vs-android.<br>
</li><li>Deploy has its own configuration pane: "Android Deployment", and saves these settings to the .user file.<br>
</li><li>The Deployment process in general is much more robust. "Build->Cancel" also now works correctly when deploying.<br>
</li><li>"Build->Deploy Solution/Deploy Project" now works on Android APK projects, to simply run the deploy step. Enable the "Deploy" checkbox for your project in the Solution "Configuration Manager" to enable this.<br>
</li><li>You can now run a deployed app by using "Debug->Run" (F5)! It's a bit of a hacky method, but appears to work fine.<br>
</li><li>NDK r8b was a breaking change for vs-android. This version now requires r8b or newer to be installed.<br>
</li><li>Fixed breaking changes to the location of libstdc++ STL libraries.<br>
</li><li>Fix for Google breaking change to x86 paths: "i686-android-linux" -> "i686-linux-android".<br>
</li><li>Added support for the new GCC 4.6 toolchains.<br>
</li><li>Added support for the new MIPS toolchains.<br>
</li><li>Tested and hopefully fixed issues such that vs-android works fully with a 64-bit JDK install.<br>
</li><li>Fix to make it possible to build projects in paths that contain spaces. Thanks to 'null77'.<br>
</li><li>Added 'Forced Include File' to "C/C++ -> Advanced" property sheet. Thanks to 'danfilner'.<br>
</li><li>Fix to make sure ARM5/ARM7 GCC flags are passed correctly to the compiler. Thanks to 'Drew Dunlop'.</li></ul>

v0.93 - 13th November 2011<br>
<ul><li>NDK <code>r7</code> was a breaking change for vs-android. This version now requires <code>r7</code> or newer to be installed.<br>
</li><li>Fixed breaking changes to the location of STL libraries. Also fixed new linking issues introduced by STL changes.<br>
</li><li>Removed support for defunct arm 4.4.0 toolset.<br>
</li><li>Added support for android-14, Android API v4.0.<br>
</li><li>Added support for the dynamic (shared) version of the GNU libstdc++ STL.<br>
</li><li>Tested against newest JDK - jdk-7u1-windows-i586.<br>
</li><li>Added support for building assembly files. '.s' and '.asm' extensions will be treated as assembly code.<br>
</li><li>Correct passing of ANT_OPTS to the Ant Build step. Thanks to 'mark.bozeman'.<br>
</li><li>Corrected expected apk name for release builds.<br>
</li><li>Added to Ant Build property page; the ability to add extra flags to the calls to adb.<br>
</li><li>Fixed bug with arm arch preprocessor defines not making it onto the command line.<br>
</li><li>Fixed bad quote removal on paths, in the C# code. Thanks to 'hoesing@kleinbottling'.<br>
</li><li>Removed stlport project from Google Code - This was an oversight by Google in the <code>r6</code> NDK, prebuilt is back again.<br>
</li><li>Updated sample projects to work with <code>R15</code> SDK tools.</li></ul>

v0.92 - 3rd August 2011<br>
<ul><li>Fixed jump-to-line clickable errors. Reworked code to use regular expressions instead. Tried a number of different compiler/linker warnings and errors and all seems to be good<br>
</li><li>Default warning level is now 'Normal Warnings', instead of 'Disable All Warnings'. Whoops!<br>
</li><li>Fixed rtti-related warnings when compiling .c files with 'Enable All Warnings (-Wall)', turned on.</li></ul>

v0.91 - 2nd August 2011<br>
<ul><li>Windows debugger is now usable. Fixed 'CLRSupport' error.<br>
</li><li>Error checking to ensure the 32-bit JDK is used.<br>
</li><li>Added JVM Heap options to Ant Build step. Initial and Maximum sizes are able to be set there now.<br>
</li><li>Added asafhel...@gmail's 'clickable errors from compiler' C# code.<br>
</li><li>Modified clickable errors code to also work with #include errors, which specify the column number too.<br>
</li><li>Added clickable error support to linker too.</li></ul>

v0.9 - 20th July 2011<br>
<ul><li>Major update, hence the skip in numbers. Closing in on a v1.0 release.<br>
</li><li>Verified working with Android NDK r5b, r5c, and <code>r6</code>.<br>
</li><li>Much of vs-android functionality moved from MSBuild script to C# tasks. Similar approach now to Microsoft's existing Win32 setup.<br>
</li><li>Dependency checking rewritten to use tracking log files.<br>
</li><li>Dependency issues fixed, dependency checking also now far quicker.<br>
</li><li>Android Property sheets now completely replace the Microsoft ones, no more rafts of unused sheets.<br>
</li><li>Property sheets populated with many options. Switches are no longer hard-coded within vs-android script.<br>
</li><li>STL support added. Choice between 'None', 'Minimal', 'libstdc++', and 'stlport'.<br>
</li><li>Support for x86 compilation with <code>r6</code> NDK.<br>
</li><li>Full support for v7-a arm architecture, as well as the existing v5.<br>
</li><li>Support for Android API directories other than just 'android-9'.<br>
</li><li>Separated support for 'dynamic libraries' and 'applications'. Applications build to apk files.<br>
</li><li>Response files used in build, no more command-line length limitations.<br>
</li><li>Deploy and run within Visual Studio, adb is now invoked by vs-android.<br>
</li><li>'Echo command lines' feature fixed.<br>
</li><li>All support SDK/libs (NDK, SDK, Ant, JDK) are okay living in directories with spaces in them now.<br>
</li><li>All bugs logged within Google Code are addressed.</li></ul>

v0.21 - 10th Feb 2011<br>
<ul><li>Fixed issues with the 'ant build' step.<br>
</li><li>Added a sensible error message if the NDK envvar isn't set, or is set incorrectly.</li></ul>

v0.2 - 1st Feb 2011<br>
<ul><li>Changed default preprocessor symbols to work the same way Microsoft's stuff does. Should fix any issues with intellisense as well.<br>
</li><li>Added support for scanning header dependencies.</li></ul>

v0.1 - 30th Jan 2011<br>
<ul><li>Initial version.<br>
</li><li>All major functionality present, barring header dependency checking.</li></ul>

<h1>License</h1>

(c) 2014 <a href='https://plus.google.com/116894316812948433768?rel=author'>Gavin Pugh</a>


vs-android is released under the zlib license.<br>
<a href='http://en.wikipedia.org/wiki/Zlib_License'>http://en.wikipedia.org/wiki/Zlib_License</a>