

## Installation ##

  * Follow the [Installation](Installation.md) instructions to install vs-android.

  * This guide also makes use of 'ant' to build a deployable .apk file. You can skip the ant-related steps if you so wish.

## Startup ##

  * After you have installed vs-android, start up Visual Studio 2010.

## New Project ##

  * Go to the 'File' menu, choose 'New -> Project...'

  * Pick 'Visual C++', and 'Empty Project'

  * The location you place the project isn't important. I suggest using a path that doesn't have spaces, since I haven't tested paths which contain spaces.

  * Uncheck 'Create directory for solution'. This puts the sln file and the vcxproj in the same directory. It will still create one directory off the base path you give. So in my case below, it'll end up in "C:\projects\sanangeles"

<img src='http://www.gavpugh.com/img/vs-android/NewProj.png' align='center'>

<h2>Adding Files</h2>

<ul><li>Download <a href='http://vs-android.googlecode.com/files/sanangeles.zip'>sanangeles.zip</a>.</li></ul>

<ul><li>You'll want to unzip this so that the directory containing '''AndroidManifest.xml''' sits alongside the sln and vcxproj files that were just created. So that same directory would have the 'jni', 'res', and 'src' sub-directories too.</li></ul>

<ul><li>Go into the 'jni' directory and drag over the following files into your project as shown:</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/DragFiles.png' align='center'>

<h2>Adding The Android Platform</h2>

<ul><li>Now go to the 'Build' menu and pick 'Configuration Manager...'.</li></ul>

<ul><li>In the top right of this dialog, the solution dropdown box, choose 'New...'</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/NewPlat.png' align='center'>

<ul><li>In the next dialog choose 'Android' from the dropdown list. If it is not present, then there was a problem with installing vs-android.</li></ul>

<ul><li>Copy settings from 'empty', and ensure 'Create new project platforms' is checked.</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/PickAndroid.png' align='center'>

<h2>OpenGL Support</h2>

<h3>Preprocessor Symbol</h3>

<ul><li>Out of the box, the default setup doesn't hook in OpenGL support. You have to modify project settings to link in the library, and in the case of this sanangeles demo define a preprocessor symbol.</li></ul>

<ul><li>Right click on the 'sanangeles' project in the 'Solution Explorer' pane. This is the one that will be shown in bold, right above 'External Dependencies'. Choose 'Properties' from this menu.</li></ul>

<ul><li>In this dialog, navigate into the 'C++' part of the tree. Click on 'Preprocessor'.</li></ul>

<ul><li>In the right-hand part click on the "Preprocessor Definitions" line, and then on the little arrow that appears in the top-right corner.</li></ul>

<ul><li>Choose 'Edit...'</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/prepro.png' align='center'>

<ul><li>Type <b>DISABLE_IMPORTGL</b> into the top box. The 'importgl.h' header needs this defined in order to link statically with the OpenGL ES library. Click OK when you're done typing.</li></ul>

<h3>Linker</h3>

<ul><li>Navigate into the 'Linker' part of the tree now. Go to the 'Command Line' option:</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/AddOpts.png' align='center'>

<ul><li>Type <b>-lGLESv1_CM</b> into the lower box. The letter after the initial dash is a lower case 'L'. This tells the linker to link in that OpenGL library.</li></ul>

<h3>Ant Build</h3>

<i>(skip these steps if you do not have ant installed)</i>

<ul><li>In the same 'Linker' section of this properties dialog, click on 'Android Ant Build' on the left-hand side.</li></ul>

<ul><li>Change the top option 'Ant Build' to <b>ant debug</b>. You can just double click on the entry to cycle through the options.</li></ul>

<ul><li>For the 'Path To Ant Batch File', change this to the correct path of your ant.bat. If you installed ant to 'c:\ant'. It'll be 'c:\ant\bin\ant.bat'.</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/AntDebug.png' align='center'>

<ul><li>Finally, you'll also need to edit the 'local.properties' file that's at the root of the sanangeles project. There's a line denoting the path to your Android SDK. This will almost certainly be different from mine, modify it to path to your install.</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/localprops.png' align='center'>

<h2>Building</h2>

<ul><li>Click 'OK' to exit the properties dialog.</li></ul>

<ul><li>Go to the 'Build' menu. Choose 'Build Solution'.</li></ul>

<ul><li>It'll:<br>
<ol><li>Compile the .c files.<br>
</li><li>Link the finished .so native executable.<br>
</li><li>Run ant to build the final deployable apk file. <i>(or not, if you chose to skip this)</i></li></ol></li></ul>

<img src='http://www.gavpugh.com/img/vs-android/Finished.png' align='center'>

<h2>Deploy</h2>

If you built the apk file with ant, here are some instructions on how to deploy it.<br>
<br>
<ul><li>Ensure you have an emulated device set up, or a real Android phone connected via debug-USB.</li></ul>

<ul><li>The final apk file will be placed here, from your ant build:<br>
<code>sanangeles\bin\DemoActivity-debug.apk</code></li></ul>

<ul><li>Note the path to the Android SDK that you needed to enter in the 'properties.local' file earlier. These are the two command lines to run to deploy and run the app. They assume you're in the root directory of the project.</li></ul>

<code>$(SDK_PATH)\platform-tools\adb install -r bin\DemoActivity-debug.apk</code>

<code>$(SDK_PATH)\platform-tools\adb shell am start -n com.example.SanAngeles/.DemoActivity</code>

<ul><li>You'll then see the demo app running on your phone!</li></ul>

<ul><li>You may want to put these command lines into a batch file, and set up a post-build step in Visual Studio.