

## Installation ##

  * Follow the [Installation](Installation.md) instructions to install vs-android.

  * Close down any instances of Visual Studio you may have had open before or during installation. You need to start with fresh after you install.

## Samples ##

  * Go here and download the sample projects:
[vs-android\_samples.zip](http://vs-android.googlecode.com/files/vs-android_samples.zip)

  * Let's start with san-angeles. Go into that directory and open up the .sln file inside it.

<img src='http://www.gavpugh.com/img/vs-android/san_angeles_devenv.png' align='center'>


<h2>San Angeles</h2>

<ul><li>San Angeles is a simple OpenGL ES 1.0 graphical demo</li></ul>

<ul><li>With the solution opened in Visual Studio, go into the 'Build' menu, and click 'Build Solution'.</li></ul>

<ul><li>Observe the build process. You'll likely have an error if the installation failed. The errors should be pretty verbose and describe measures of how to fix them.</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/san_angeles_success.png' align='center'>

<ul><li>The projects are setup to auto-deploy and run onto a connected device. You can start a virtual device through the 'SDK Manager' if you wish. San Angeles works fine on the virtual device.</li></ul>


<h2>Other Samples</h2>

<ul><li>Go nuts with the other samples too. They're all ready to go as well.</li></ul>

<ul><li>'hello-gl2' requires OpenGL ES 2.0 compatible hardware. It will not work on a virtual Android device.</li></ul>


<h2>Property Sheets</h2>

<ul><li>For those familiar with editing project properties in Visual Studio, vs-android completely overrides the Win32 ones you're used to:</li></ul>

<img src='http://www.gavpugh.com/img/vs-android/prop_page.png' align='center'>

<ul><li>Take a look around. I'd suggest taking one of the samples to start a new project you may want to work on. In a future release of vs-android there will be a 'New Project' wizard, but not just yet.</li></ul>

<h2>Troubleshooting</h2>

If you run into problems, issues, errors, whatever. Take a look here:<br>
<br>
<ul><li><a href='Troubleshooting.md'>Troubleshooting</a>