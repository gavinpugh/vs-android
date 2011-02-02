vs-android v0.2 - 1st Feb 2011
==============================

vs-android is intended to provide a collection of scripts and utilities to support integrated development of
Android NDK C/C++ software under Microsoft Visual Studio.

Currently vs-android only works under Visual Studio 2010. Earlier versions lack the MSBuild integration with 
the C/C++ compilation systems.

The only required component is the Android NDK. Neither Cygwin, Java, nor the full Android SDK are needed to 
compile and link C/C++ code. 



Documentation
=============

Documentation for vs-android can be found here:
   * http://code.google.com/p/vs-android/



Version History
===============

v0.2 - 1st Feb 2011

  * Changed default preprocessor symbols to work the same way Microsoft's stuff does. Should fix any 
    issues with intellisense as well.
  * Added support for scanning header dependencies


v0.1 - 30th Jan 2011

  * Initial version.
  * All major functionality present, barring header dependency checking. 



References
==========
"Inside the Microsoft Build Engine: Using MSBuild and Team Foundation Build"
Authors: Sayed Ibrahim Hashimi, William Bartholomew



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
