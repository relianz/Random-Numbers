# Random-Numbers
A quite simple [UWP](https://www.visualstudio.com/vs/features/universal-windows-platform/) application that can generate and statistically evaluate random numbers. The generator is selected at runtime via [strategy design pattern](https://en.wikipedia.org/wiki/Strategy_pattern). A two-dimensional graphic facilitates the recognition of recurring patterns in number sequences.

![Screenshot](/180322_1701%20GUI.PNG)
## Building the program
The executable program can be built in five steps:
1. Clone the repository. Personally, I like to use [SourceTree](https://www.sourcetreeapp.com/) as a free git client.
1. Open the solution file [RandomNumbers.sln](RandomNumbers/RandomNumbers.sln) with Visual Studio.  
**Note**: UWP applications cannot be created from UNC paths _(Visual Studio reports „DEP0700: Registration of the app failed. [0x80073CF0] error 0x80070003“)_.
1. Using the NuGet package manager:    
Add the components `Microsoft.NETCore.UniversalWindowsPlatform`, `Microsoft.Toolkit.Uwp.UI.Controls`, and `WinRTXamlToolkit.Controls.DataVisualization.UWP`.
1. Select configuration, e.g. _Debug/x64_.
1. Press `F5`.

## Development tools
### Software design
When designing the software, a Wacom Intuos graphics tablet and [Bamboo Paper](https://www.wacom.com/en/products/apps-services/bamboo-paper) were very pleasant companions - I will never use real paper again in this development phase! The draft works are recorded in the document [RandomNumbers.will](RandomNumbers.will), unfortunately mostly in German, I apologize.

### Implementation
Implementation has been done with [Microsoft Visual Studio Community 2017](https://www.visualstudio.com/vs/community) _(Version 15.6.3)_ on platform Windows 10 Professional _(Version 1709)_. However, the latest C# features are not used. 

### Components used
The implementation is based on these software components:
* [UWP Community Toolkit](https://github.com/Microsoft/UWPCommunityToolkit) _(using the [RadialProgressBar](https://docs.microsoft.com/en-us/windows/uwpcommunitytoolkit/controls/radialprogressbar))_
* [WinRT XAML Toolkit](https://github.com/xyzzer/WinRTXamlToolkit) _(for the binning chart diagram)_

## Limitations
The following limitations currently exist: 
1. Constant size of image bitmap (512 x 512 pixels)
1. Only two flavors of randomness: [System.Random](https://msdn.microsoft.com/en-us/library/system.random.aspx) _(= Pseudo)_ and [System.Security.Cryptography.RNGCryptoServiceProvider](https://msdn.microsoft.com/de-de/library/system.security.cryptography.rngcryptoserviceprovider.aspx) _(= Secure)_
1. No [software documentation](https://en.wikipedia.org/wiki/Software_documentation)
1. Too much code behind
1. Number generation just single threaded
1. No [unit testing](https://en.wikipedia.org/wiki/Unit_testing)
1. Not tested on platforms other than Windows 10 Professional _(x64)_

But if experiments with random number generators are to be done quickly, then it does what it should.

### Adding a random number generator
A new random number generator is added in three steps:
1. Define a class that implements the [IRandomness](RandomNumbers/RandomNumbers/Randomness.cs) interface. Take the definition of [SecureRandomness](RandomNumbers/RandomNumbers/SecureRandomness.cs) as an example.
1. In [MainPage.xaml](RandomNumbers/RandomNumbers/MainPage.xaml):  
Add a list element representing the random number generator to the `rngSelector` combo box.
1. In [MainPage.xaml.cs](RandomNumbers/RandomNumbers/MainPage.xaml.cs):  
Extend the `switch (idx)` statement in the callback function `Button_Click`. The function starts or cancels the generation of random numbers in a separate thread.

I don't want to advertise here, but [Head First Design Patterns](http://wickedlysmart.com/head-first-design-patterns/) gives a very good introduction to the [strategy pattern](https://en.wikipedia.org/wiki/Strategy_pattern). I like this book very much.

## Author
[Markus A. Stulle](mailto:markus@stulle.zone) // [smartcontract.world](https://smartcontract.world) | Munich, March 2018.


