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
using System.ComponentModel;                                // INotifyPropertyChanged

namespace RandomNumbers
{
    class ViewModel : INotifyPropertyChanged
    {
        #region private attributes
        private long numOfRandomNumbers;

        private int lmin;
        private int lmax;

        private double average;
        private double variance;

        private int percentageDone;

        private int rndEquals;
        private long pixelTested;
        private long pixelSet;

        private string numbersFileName;
        #endregion

        #region Getter/Setter
        public long NumOfRandomNumbers
        {
            get => numOfRandomNumbers;
            set
            {
                if (value != numOfRandomNumbers)
                {
                    // Must generate at least 100 numbers for nonzero 1 percent integer increment:
                    if (value < 100)
                        numOfRandomNumbers = 100;
                    else
                        numOfRandomNumbers = value;

                    OnPropertyChanged( "NumOfRandomNumbers" );
                }
            }

        } // NumOfRandomNumbers

        public int Lmin
        {
            get => lmin;
            set
            {
                if (value != lmin)
                {
                    lmin = value;
                    OnPropertyChanged( "Lmin" );

                    ComputeAndSetRndEquals();
                }
            }
        } // Lmin

        public int Lmax
        {
            get => lmax;
            set
            {
                if (value != lmax)
                {
                    lmax = value;
                    OnPropertyChanged( "Lmax" );

                    ComputeAndSetRndEquals();
                }
            }
        } // Lmax

        public double Average
        {
            get => average;
            set
            {
                if (value != average)
                {
                    average = value;
                    OnPropertyChanged( "Average" );
                }
            }
        } // Average

        public double Variance
        {
            get => variance;
            set
            {
                if (value != variance)
                {
                    variance = value;
                    OnPropertyChanged( "Variance" );
                }
            }
        } // Variance

        public int PercentageDone
        {
            get => percentageDone;
            set
            {
                if (value != percentageDone)
                {
                    if ((value >= 0) && (value <= 100))
                    {
                        percentageDone = value;
                        OnPropertyChanged( "PercentageDone" );
                    }
                    else
                        throw new ArgumentOutOfRangeException( "PercentageDone" );
                }
            }
        } // PercentageDone

        public int RndEquals
        {
            get => rndEquals;
            set
            {
                if (value != rndEquals)
                {
                    rndEquals = value;
                    OnPropertyChanged( "RndEquals" );
                }                  
            }
        } // RndEquals

        public long PixelTested
        {
            get => pixelTested;
            set
            {
                if (value != pixelTested)
                {
                    pixelTested = value;
                    OnPropertyChanged( "PixelTested" );
                }
            }

        } // PixelTested

        public long PixelSet
        {
            get => pixelSet;
            set
            {
                if (value != pixelSet)
                {
                    pixelSet = value;
                    OnPropertyChanged( "PixelSet" );
                }
            }

        } // PixelSet

        public string NumbersFileName
        {
            get => numbersFileName;
            set
            {
                if (value != numbersFileName)
                {
                    numbersFileName = value;
                    OnPropertyChanged( "NumbersFileName" );
                }
            }

        } // NumbersFileName

        #endregion

        public ViewModel( int imageWidth, int imageHeight )
        {
            // default setting: match number of image pixels
            NumOfRandomNumbers = imageWidth * imageHeight; 

            Lmin = -40;
            Lmax = +60;

            PercentageDone = 0;

            PixelTested = 0L;
            PixelSet = 0L;

            NumbersFileName = "".PadRight( 14 );

        } // ctor

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged( string propertyName )
        {
            if (PropertyChanged != null)
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
        }

        private void ComputeAndSetRndEquals()
        {
            int lower = lmin;
            int upper = lmax;

            int m = (lmax + lmin) / 2;
                                 
            RndEquals = m;

        } // ComputeAndSetRndEquals

        public void AssertFieldValues()
        {
            if( Lmax <= Lmin)
                Lmax = Lmin + 1;

        } // AssertFieldValues

    } // class ViewModel
}

/* [EOF] */