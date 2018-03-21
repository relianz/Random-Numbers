# Random-Numbers
A quite simple [UWP](https://www.visualstudio.com/vs/features/universal-windows-platform/) application that can generate and statistically evaluate random numbers. The generator is selected at runtime via [strategy design pattern](https://en.wikipedia.org/wiki/Strategy_pattern). A two-dimensional graphic facilitates the recognition of recurring patterns in number sequences.

![Screenshot](/180321_2122%20GUI.PNG)
## Components used
The implementation is based on these software components:
* [UWP Community Toolkit](https://github.com/Microsoft/UWPCommunityToolkit) _(using the [RadialProgressBar](https://docs.microsoft.com/en-us/windows/uwpcommunitytoolkit/controls/radialprogressbar))_
* [WinRT XAML Toolkit](https://github.com/xyzzer/WinRTXamlToolkit) _(for the binning chart diagram)_

## Development tools
Development has been done with [Microsoft Visual Studio Community 2017](https://www.visualstudio.com/vs/community) on a Windows 10 Skylake machine. 

## Limitations
The following limitations currently exist: 
1. Poor estimation of variance 
1. Constant size of image bitmap (512 x 512 pixels)
1. Only two flavors of randomness: [System.Random](https://msdn.microsoft.com/en-us/library/system.random.aspx) _(= Pseudo)_ and [System.Security.Cryptography.RNGCryptoServiceProvider](https://msdn.microsoft.com/de-de/library/system.security.cryptography.rngcryptoserviceprovider.aspx) _(= Secure)_
1. No [software documentation](https://en.wikipedia.org/wiki/Software_documentation)
1. Too much code behind
1. Number generation just single threaded
1. No [unit testing](https://en.wikipedia.org/wiki/Unit_testing)
1. Not tested on platforms other than Windows 10 Professional _(x64)_

But if experiments with random number generators are to be done quickly, then it does what it should.

## Author
[Markus A. Stulle](mailto:markus@stulle.zone) // [smartcontract.world](https://smartcontract.world) | Munich, March 2018.


