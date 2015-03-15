**NOTE:** Please keep in mind that I'm working on vs-android for free, in my own time. I'm just one guy, so please be patient with the development of this software.


**Priorities for v1.0:**

  * "New Project" wizard, for Android. Nothing fancy, just a barebones 'Hello World' project.
  * Installer. .msi, nullsoft, something simple.


**Wishlist, in a rough order:**

  * Look into issues with 64-bit Java. No intention of officially supporting it, but I need to check out that having the 32 and 64-bits installed isn't a problem.
  * Ability to set Java VM size in Ant Build property sheets
  * Fix Ant Build dependencies such that editing .java files (or any dependents: xml, etc...) will cause Ant to run again.
  * Clickable errors, jump to line support. Error log line number support.
  * NativeActivity Support
  * --start-group, --end-group support for linker.
  * Clean up 'in-IDE' intellisense warnings when editing cpp files. It'll be impossible to get it 100% warning free, but there's likely some measures to get it to play nice.
  * 'Android Debugger' implementation in MSBuild. A first pass should just kick off adb to run the app.
  * GUI GDB debugger - DDD, Insight? Needs research. (WinGDB is cool, but is proprietary closed source).
  * Support for 'deploy' checkbox in 'Configuration Manager'. To decouple the deploy from the Ant Build.
  * Project 'Output Directory' setting does nothing? Check this is so, consider deleting it.
  * Ability to override environment variables for SDK/NDK/Ant/JDK on a per-project basis in property sheets.
  * Look at using the TrackingFiles stuff in C# with the Compile task. Have had one aborted attempt, but should try again. It'll simplify the code a lot.
  * Add a full complement of GCC compiler and linker switches to the property pages.
  * adb/ddms logging capture to a Visual Studio output window.
  * Facility to create ant build projects (AndroidManifest/build.xml) from within Visual Studio, driven by property pages.
  * A "Build Customization" for .java files, so they can be added to the project and individually compiled. Possibly with a view for removing the dependency on using ant to build apk's.


**May not be issues, but I'd like to check out:**

  * Release builds. I haven't attempted setting up signing. Check there's nothing bad going on.
  * Deploying to emulator. Does it have problems that require an explicit uninstall of existing packages?
  * Some 'Win32' specific properties creep into the Android settings in .vcxproj files. Investigate.
  * x86. It builds and links. That's as far as I've got. If the emulator supports it, I should try and see if it runs.
  * Someone on my blog hinted that the 'Library Paths' passed to the linker don't make it through. Seems odd, as I know the STL, etc... rely on it. Will double-check.