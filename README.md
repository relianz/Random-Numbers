# Random-Numbers
A quite simple [UWP](https://www.visualstudio.com/vs/features/universal-windows-platform/) application that can generate and statistically evaluate random numbers. The generator is selected at runtime via [strategy design pattern](https://en.wikipedia.org/wiki/Strategy_pattern). A two-dimensional graphic facilitates the recognition of recurring patterns in number sequences.
![Screenshot](/180321_0643%20GUI.PNG)
The implementation is based on these components:
* [UWP Community Toolkit](https://github.com/Microsoft/UWPCommunityToolkit) _(using the [RadialProgressBar](https://docs.microsoft.com/en-us/windows/uwpcommunitytoolkit/controls/radialprogressbar))_
* [WinRT XAML Toolkit](https://github.com/xyzzer/WinRTXamlToolkit) _(for the binning chart diagram)_

Development has been done with [Microsoft Visual Studio Community 2017](https://www.visualstudio.com/vs/community), which is a fine tool.

Limitations:
1. Poor estimation of variance 
1. Constant size of image bitmap (512 x 512 pixels)
1. Only two flavors of randomness: [System.Random](https://msdn.microsoft.com/en-us/library/system.random.aspx), [System.Security.Cryptography.RNGCryptoServiceProvider](https://msdn.microsoft.com/de-de/library/system.security.cryptography.rngcryptoserviceprovider.aspx)

[Markus A. Stulle](mailto:markus@stulle.zone) // [smartcontract.world](https://smartcontract.world)


