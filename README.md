# Random-Numbers
A quite simple [UWP](https://www.visualstudio.com/vs/features/universal-windows-platform/) application implemented in C# that can generate and statistically evaluate random numbers. The generator is selected at runtime via [strategy design pattern](https://en.wikipedia.org/wiki/Strategy_pattern). A two-dimensional graphic facilitates the recognition of recurring patterns in number sequences.

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

## Mathematics
### Scaling of numbers 
The program scales an integer random value `l` generated in the closed interval `[lmin, lmax]` to the floating point interval `[dmin, dmax]`. The two interval limits are currently defined as constant values `0.0d` and `1.0d`. Scaling is performed by the static method `RandomNumbers.Statistics.Scale()`.

### Binning
The scaled random numbers are divided into `numOfBins = 50` intervals defined in class [Binning](RandomNumbers/RandomNumbers/Binning.cs). Each interval no. `i` contains random numbers in the half-open range `[dmin + i*δ, dmin + (i + 1)*δ)` with `δ = (dmax - dmin)/numOfBins`. 

### Estimation of stochastic quantities
The expected value `m` = **μ** of the scaled random numbers is calculated using the recursion formula 

    m<k> = (1/k)*(x<k> + (k-1)*m<k-1>) 
    
where `x<k>` is the new random number generated. 

The variance `s` = **σ*σ** of the random numbers is also calculated recursively using 

    s<k> = (1-(1/k))*s<k-1> + (k+1)*SQR(m<k> - m<k-1>)
    
where `SQR` denotes squaring. The newly generated random number `x<k>` is only included in this formula indirectly via the new estimate `m<k>` of the expected value, which must therefore be calculated before the variance is updated:

    // Update estimations of expected value and variance:
    AveragePrev = viewModel.Average;
    viewModel.Average  = Statistics.UpdateAverage( k + 1, AveragePrev, r );
    viewModel.Variance = Statistics.UpdateVariance( k + 1, viewModel.Variance, AveragePrev, viewModel.Average );

The estimates are implemented as static methods in the class `RandomNumbers.Statistics`.

## Limitations
The following limitations currently exist: 
1. Constant size of image bitmap (512 x 512 pixels)
1. Constant number of bins (50)
1. No [software documentation](https://en.wikipedia.org/wiki/Software_documentation)
1. Still too much code behind
1. Number generation just single threaded
1. No [unit testing](https://en.wikipedia.org/wiki/Unit_testing)
1. Not tested on platforms other than Windows 10 Professional _(x64)_
1. No performance optimization - there are many intermediate calculations for debugging purposes. 

But if experiments with random number generators are to be done quickly, then it does what it should.

### Adding a random number generator
There are two behaviors of randomness implemented: [System.Random](https://msdn.microsoft.com/en-us/library/system.random.aspx) _(= Pseudo)_ and [System.Security.Cryptography.RNGCryptoServiceProvider](https://msdn.microsoft.com/de-de/library/system.security.cryptography.rngcryptoserviceprovider.aspx) _(= Secure)_. Another random number generator is provided in three steps:
1. Define a class that implements the [IRandomness](RandomNumbers/RandomNumbers/Randomness.cs) interface. Please take the definition of [MyRandomness](RandomNumbers/RandomNumbers/MyRandomness.cs) as an example.
1. In [MainPage.xaml](RandomNumbers/RandomNumbers/MainPage.xaml):  
Add a list element representing the random number generator to the `rngSelector` combo box.
1. In [MainPage.xaml.cs](RandomNumbers/RandomNumbers/MainPage.xaml.cs):  
Extend the `switch (idx)` statement in the callback function `Button_Click`. The function starts or cancels the generation of random numbers in a separate thread.

Observe the distribution of the white pixels in the graphic: If patterns are recognizable, the generator still offers room for improvement. 

#### RNG of GNU C Library ####
For the implementation of `MyRandomness` I took from [Wikipedia](https://en.wikipedia.org/wiki/Linear_congruential_generator#Parameters_in_common_use) the parameters `a` _(= multiplier)_, `c` _(= increment)_, and `m` _(= modulus)_ of the linear congruential generator

    x<k+1> = (a*x<k> + c) % m
    
which is part of the GNU C Library:
 
    a = 1,103,515,245 
    c = 12,345
    m = 2**31 = Int32.MaxValue + 1
    
## Acknowledgement
I don't want to advertise here, but [Head First Design Patterns](http://wickedlysmart.com/head-first-design-patterns/) gives a very good introduction to the [strategy pattern](https://en.wikipedia.org/wiki/Strategy_pattern). I like this book very much.

## Wikipedia articles 
* [Linear congruential generator](https://en.wikipedia.org/wiki/Linear_congruential_generator)
* [Expected value](https://en.wikipedia.org/wiki/Expected_value)
* [Variance](https://en.wikipedia.org/wiki/Variance)

## Author
[Markus A. Stulle](mailto:markus@stulle.zone) // [smartcontract.world](https://smartcontract.world) | Munich, March 2018.


