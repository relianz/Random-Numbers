/*

The MIT License

Copyright 2018, Dr.-Ing. Markus A. Stulle, München (markus@stulle.zone)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies 
or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

using System;                                               // ArgumentOutOfRangeException
using System.IO;                                            // Stream
using System.Diagnostics;                                   // Stopwatch
using System.Globalization;                                 // CultureInfo
using System.Linq;                                          // ToList
using System.Reflection;                                    // Assembly
using System.Threading;                                     // CancellationTokenSource
using System.Threading.Tasks;                               // Task
using System.Runtime.InteropServices.WindowsRuntime;        // _wb.PixelBuffer

using Windows.UI.Core;                                      // CoreDispatcherPriority
using Windows.UI.Xaml;                                      // RoutedEventArgs
using Windows.UI.Xaml.Controls;                             // Page
using Windows.UI.Xaml.Media.Imaging;                        // WriteableBitmap
using WinRTXamlToolkit.Controls.DataVisualization.Charting; // ColumnSeries

namespace RandomNumbers
{
    public sealed partial class MainPage : Page
    {
        #region Public members
        public MainPage()
        {
            this.InitializeComponent();
            txtVersion.Text = "Version " + GetAttributeValueFromAssy( "AssemblyFileVersionAttribute" );

            // Query bitmap size:
            RndBitmapWidth  = (int)rndBitmap.Width;
            RndBitmapHeight = (int)rndBitmap.Height;

            viewModel = new ViewModel( RndBitmapWidth, RndBitmapHeight );
            NumOfBins = 50;

            // Must adjust output formatting of average and variance if intervall changed:
            Dmin = 0.0d;
            Dmax = 1.0d;
            Delta = (Dmax - Dmin) / NumOfBins;

            // Create bins to count frequencies:
            binning = new RandomNumberBin[ NumOfBins ];

            for (int i = 0; i < NumOfBins; i++)
            {
                double lower = i * Delta;
                double upper = lower + Delta;
                string name = i.ToString();

                binning[ i ] = new RandomNumberBin()
                {
                    Lower = lower,
                    Upper = upper,
                    Name = name
                };

            } // for all bins

            cs = ColumnChart.Series[ 0 ] as ColumnSeries;
            cs.ItemsSource = binning;
            cs.Title = "Frequency";

            Rng = new SampleRNG();

            int imageSize = RndBitmapWidth * RndBitmapHeight * 4;
            imageArray = new byte[ imageSize ];

        } // ctor MainPage

        public string GetAttributeValueFromAssy( string name )
        {
            Assembly currentAssembly = typeof( App ).GetTypeInfo().Assembly;

            var customAttributes = currentAssembly.CustomAttributes;
            var list = customAttributes.ToList();

            var result = list.FirstOrDefault( x => x.AttributeType.Name == name );
            var value = result.ConstructorArguments[ 0 ].Value;

            return (string)value;

        } // GetAttributeValueFromAssy
        
        #region Getter/Setter
        public int NumOfBins { get => numOfBins; set => numOfBins = value; }
        public double Dmin { get => dmin; set => dmin = value; }
        public double Dmax { get => dmax; set => dmax = value; }
        public double Delta { get => delta; set => delta = value; }

        public bool IsRunning { get => isRunning; set => isRunning = value; }
        public Task Runner { get => runner; set => runner = value; }

        public double AveragePrev { get => averagePrev; set => averagePrev = value; }
        public RNG Rng { get => rng; set => rng = value; }
        public Stopwatch Watch { get => watch; set => watch = value; }
        public int RndBitmapWidth { get => rndBitmapWidth; set => rndBitmapWidth = value; }
        public int RndBitmapHeight { get => rndBitmapHeight; set => rndBitmapHeight = value; }
        #endregion

        #endregion

        #region Private members
        private RandomNumberBin[] binning;
        private RNG rng;

        private int numOfBins;

        private double dmin;
        private double dmax;
        private double delta;

        private double averagePrev;

        private int rndBitmapWidth;
        private int rndBitmapHeight;
        private byte[] imageArray;

        private ColumnSeries cs;
        private Boolean isRunning;
        private Task runner;
        private Stopwatch watch;
        private const string unit = " [1/s]";

        private ViewModel viewModel;

        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        private void MinMaxDefaults_Toggled( object sender, RoutedEventArgs e )
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    // Set default values for random number intervall:
                    viewModel.Lmin = System.Int32.MinValue + 1;
                    viewModel.Lmax = System.Int32.MaxValue - 1;

                    // Disable editing:
                    minRandomNumber.IsEnabled = false;
                    maxRandomNumber.IsEnabled = false;
                }
                else
                {
                    // Enable editing:
                    minRandomNumber.IsEnabled = true;
                    maxRandomNumber.IsEnabled = true;
                }

            } // toggleSwitch != null

        } // minMaxDefaults_Toggled

        private async void Button_Click( object sender, RoutedEventArgs e )
        {
            if (IsRunning)
            {
                // Stop random number generation:
                tokenSource.Cancel();
            }
            else
            {
                // Strategy pattern - set behavior of random number generator:
                int idx = rngSelector.SelectedIndex;
                switch (idx)
                {
                    case 0:
                        Rng.Randomness = new PseudoRandomness();
                        break;

                    case 1:
                        Rng.Randomness = new SecureRandomness();
                        break;

                    case 2:
                        Rng.Randomness = new MyRandomness();
                        break;

                    case 3:
                        Rng.Randomness = new NoRandomness();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException( "Invalid randomness index" );
                }

                // create stop watch to compute number generation rate:
                Watch = new Stopwatch();

                Boolean isHighRes = Stopwatch.IsHighResolution;
                long frequency = Stopwatch.Frequency;
                long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;

                // Clear bins:
                for (int i = 0; i < binning.Length; i++)
                    binning[ i ].Amount = 0L;

                // Reset image:
                viewModel.PixelTested = 0L;
                viewModel.PixelSet = 0L;

                ClearImage();
                WriteImageToBitmap();

                // Create token to facilitate task cancellation:
                tokenSource = new CancellationTokenSource();
                token = tokenSource.Token;

                viewModel.PercentageDone = 0;
                startStopButton.Content = "Stop Generation";
                cs.ItemsSource = null;
                IsRunning = true;

                // Start random number generation:
                Watch.Start();
                Runner = Task.Run( () => GenerateRandomNumbers( token ), token );

            } // isRunning

            try {
                await Runner;
            }
            catch (OperationCanceledException x) {
                ;
            }

            // Display results:
            cs.ItemsSource = binning;
            WriteImageToBitmap();

            averageText.Text  = viewModel.Average.ToString( "0.########" );
            varianceText.Text = viewModel.Variance.ToString( "0.########" );
            rndMatches.Text   = viewModel.PixelSet.ToString();

            startStopButton.Content = "Start Generation";
            IsRunning = false;

        } // Button_Click

        private async Task GenerateRandomNumbers( CancellationToken ct )
        {
            // Compute 1 percent increment:
            long onePercent = viewModel.NumOfRandomNumbers / 100L;

            viewModel.Variance = 0.0d;
            viewModel.Average = AveragePrev = 0.0d;

            int p = -1;
            int nextP = 0;
            Boolean randomNumberMatch = false;

            // Create random numbers:
            for (long k = 0; k < viewModel.NumOfRandomNumbers; k++)
            {
                int li = Rng.GetRandomNumer( viewModel.Lmin, viewModel.Lmax );
                long l = (long)li;

                double r = Statistics.Scale( l, viewModel.Lmax, viewModel.Lmin, Dmax, Dmin );

                // Compute offset in bitmap:
                nextP = OffsetInBitmap( RndBitmapWidth, RndBitmapHeight, viewModel.NumOfRandomNumbers, k );

                // Next pixel reached?
                if (nextP > p)
                {
                    p = nextP;
                    SetPixelNoMatch( p );
                    randomNumberMatch = false;
                    
                } // next pixel reached

                if (!randomNumberMatch)
                {
                    if (li == viewModel.RndEquals)
                    {
#if DEBUG
                        // compute coordinates (for debugging purposes):
                        int y = p / RndBitmapWidth;
                        int x = p - (RndBitmapWidth * y);
#endif
                        randomNumberMatch = true;
                        SetPixelMatch( p );

                        viewModel.PixelSet++;
                    }

                    viewModel.PixelTested++;

                } // !randomNumberMatch 

                // Update estimations of expected value and variance:
                AveragePrev = viewModel.Average;
                viewModel.Average  = Statistics.UpdateAverage( k + 1, AveragePrev, r );
                viewModel.Variance = Statistics.UpdateVariance( k + 1, viewModel.Variance, AveragePrev, viewModel.Average );

                // Find index of bin:
                int i = (int)((r - Dmin) / Delta);
                if (i == NumOfBins)
                    i--;

                binning[ i ].Amount++;

                if (ct.IsCancellationRequested)
                {
                    await Dispatcher.RunAsync( CoreDispatcherPriority.Normal, () => {
                        numberCount.Text = k.ToString();
                    } );

                    ct.ThrowIfCancellationRequested();
                    break;

                } // task cancelled

                // Update GUI...
                if ((k % onePercent) == 0)
                {
                    // ...progress control:
                    await Dispatcher.RunAsync( CoreDispatcherPriority.Normal, () => {
                        progressControl.Value++;
                    } );

                    // ...average computed so far:
                    await Dispatcher.RunAsync( CoreDispatcherPriority.Normal, () => {
                        averageText.Text = viewModel.Average.ToString( "0.########" );
                    } );

                    // ...variance computed so far:
                    await Dispatcher.RunAsync( CoreDispatcherPriority.Normal, () => {
                        varianceText.Text = viewModel.Variance.ToString( "0.########" );
                    } );

                    // ...rate of number generation:
                    TimeSpan ts = Watch.Elapsed;
                    double rate = k / ts.TotalSeconds;

                    await Dispatcher.RunAsync( CoreDispatcherPriority.Normal, () => {
                        numbersPerSecond.Text = ((int)Math.Floor( rate )).ToString( "N1", CultureInfo.InvariantCulture ) + unit;
                    } );

                    // ...random number matches:
                    await Dispatcher.RunAsync( CoreDispatcherPriority.Normal, () => {
                        rndMatches.Text = viewModel.PixelSet.ToString();
                    } );

                } // Update GUI

            } // for all random numbers

        } // GenerateRandomNumbers

        private static int OffsetInBitmap( int w, int h, long N, long k )
        {
            double nom   = (w * h - 1.0d);
            double denom = (N - 1.0d);

            double p = (nom / denom) * k;
            int offset = (int)Math.Floor( p );
#if DEBUG
            ;
#endif
            return offset;

        } // OffsetInBitmap

        private void SetPixelMatch( int offset )
        {           
            int index = 4 * offset;

            // Set a white pixel:
            imageArray[ index ] = 255;        // Blue
            imageArray[ index + 1 ] = 255;    // Green
            imageArray[ index + 2 ] = 255;    // Red
            imageArray[ index + 3 ] = 255;    // Intensity?

        } // SetPixelMatch

        private void SetPixelNoMatch( int offset )
        {
            int index = 4 * offset;

            // Set a black pixel:
            imageArray[ index ] = 0;
            imageArray[ index + 1 ] = 0;
            imageArray[ index + 2 ] = 0;
            imageArray[ index + 3 ] = 255;

        } // SetPixelNoMatch

        private void ClearImage()
        {
            Array.Clear( imageArray, 0, imageArray.Length );

        } // ClearImage

        private async void WriteImageToBitmap()
        {
            long p = viewModel.PixelTested;
            long q = viewModel.PixelSet;

            WriteableBitmap _wb = new WriteableBitmap( RndBitmapWidth, RndBitmapHeight );
            using (Stream stream = _wb.PixelBuffer.AsStream())
            {
                await stream.WriteAsync( imageArray, 0, imageArray.Length );
            }
            rndBitmap.Source = _wb;

        } // WriteImageToBitmap
#endregion

    } // class MainPage

    public class RandomNumberBin  
    {
        private double lower;
        private double upper;
        private long amount;
        private string name;
      
        public double Lower { get => lower; set => lower = value; }
        public double Upper { get => upper; set => upper = value; }
        public long Amount { get => amount; set => amount = value; }
        public string Name { get => name; set => name = value; }

    } // class RandomNumberBin

} // namepsace RandomNumbers

/* [EOF] */
