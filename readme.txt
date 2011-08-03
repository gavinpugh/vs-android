vs-android v0.91 - 2nd August 2011
==================================

vs-android is intended to provide a collection of scripts and utilities to support integrated development of
Android NDK C/C++ software under Microsoft Visual Studio.

Currently vs-android only works under Visual Studio 2010. Earlier versions lack the MSBuild integration with 
the C/C++ compilation systems.


Required Support SDKs
=====================

Cygwin is not required at all to use vs-android, thankfully!

At a bare minimum the Android NDK needs to be installed. This will allow compilation of C/C++ code:
* http://developer.android.com/sdk/ndk/index.html


In order to build an apk package to run on an Android device, you'll also require:

The Android SDK:
* http://developer.android.com/sdk/index.html

The Java JDK (the x86 version, *not* the x64 one!):
* http://www.oracle.com/technetwork/java/javase/downloads/index.html

Apache Ant:
* http://ant.apache.org/



Documentation
=============

Documentation for vs-android can be found here:
  * http://code.google.com/p/vs-android/



Version History
===============

v0.91 - 2nd August 2011

  * Windows debugger is now usable. Fixed 'CLRSupport' error.
  * Error checking to ensure the 32-bit JDK is used.
  * Added JVM Heap options to Ant Build step. Initial and Maximum sizes are able to be set there now.
  * Added asafhel...@gmail's 'clickable errors from compiler' C# code.
  * Modified clickable errors code to also work with #include errors, which specify the column number too.
  * Added clickable error support to linker too.


v0.9 - 20th July 2011

  * Major update, hence the skip in numbers. Closing in on a v1.0 release.
  * Verified working with Android NDK r5b, r5c, and r6.
  * Much of vs-android functionality moved from MSBuild script to C# tasks. Similar approach now to Microsoft's
    existing Win32 setup.
  * Dependency checking rewritten to use tracking log files.
  * Dependency issues fixed, dependency checking also now far quicker.
  * Android Property sheets now completely replace the Microsoft ones, no more rafts of unused sheets.
  * Property sheets populated with many options. Switches are no longer hard-coded within vs-android script.
  * STL support added. Choice between 'None', 'Minimal', 'libstdc++', and 'stlport'.
  * Support for x86 compilation with r6 NDK.
  * Full support for v7-a arm architecture, as well as the existing v5.
  * Support for Android API directories other than just 'android-9'.
  * Separated support for 'dynamic libraries' and 'applications'. Applications build to apk files.
  * Response files used in build, no more command-line length limitations.
  * Deploy and run within Visual Studio, adb is now invoked by vs-android.
  * 'Echo command lines' feature fixed.
  * All support SDK/libs (NDK, SDK, Ant, JDK) are okay living in directories with spaces in them now.
  * All bugs logged within Google Code are addressed.


v0.21 - 10th Feb 2011

  * Fixed issues with the 'ant build' step.
  * Added a sensible error message if the NDK envvar isn't set, or is set incorrectly.


v0.2 - 1st Feb 2011

  * Changed default preprocessor symbols to work the same way Microsoft's stuff does. Should fix any 
    issues with intellisense as well.
  * Added support for scanning header dependencies


v0.1 - 30th Jan 2011

  * Initial version.
  * All major functionality present, barring header dependency checking. 



Contributors
============

asafhel...@gmail.com - Initial 'clickable errors from compiler' C# code.


References
==========

"Inside the Microsoft Build Engine: Using MSBuild and Team Foundation Build"
Authors: Sayed Ibrahim Hashimi, William Bartholomew

"Microsoft C/C++ Visual Studio 2010 Build Implementation"
Located here: %ProgramFiles(x86)%\MSBuild\Microsoft.Cpp



License
=======

vs-android is released under the zlib license.
http://en.wikipedia.org/wiki/Zlib_License



Copyright (c) 2011 Gavin Pugh http://www.gavpugh.com/

This software is provided 'as-is', without any express or implied
warranty. In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you must not
   claim that you wrote the original software. If you use this software
   in a product, an acknowledgment in the product documentation would be
   appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and must not be
   misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
   distribution.
