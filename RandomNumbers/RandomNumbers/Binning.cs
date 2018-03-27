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

using System;        // ArgumentOutOfRangeException

namespace RandomNumbers
{
    class Binning
    {
        #region public members
        public Binning( int numOfBins, double minValue, double maxValue )
        {
            if( maxValue <= minValue )
            {
                throw new ArgumentOutOfRangeException( "maxValue smaller than minValue (" + maxValue + ", " + minValue + ")" );
            }
            MinValue = minValue;
            MaxValue = maxValue;            

            NumOfBins = numOfBins;            
            Delta = (MaxValue - MinValue) / NumOfBins;

            Bins = new NumberBin[ NumOfBins ];
            for (int i = 0; i < Bins.Length; i++ )
            {
                NumberBin nb = new NumberBin
                {
                    Amount = 0L,
                    Name = i.ToString(),

                    lower =  i * Delta,
                    upper = (i + 1) * Delta
                };
                Bins[ i ] = nb;

            } // for all bins

            TotalCount = 0UL;

        } // ctor Binning

        public Boolean AddNumber( double d )
        {            
            // Find index of bin:
            for( int i = 0; i < Bins.Length; i++ )
            {
                if( d <= Bins[ i ].upper )
                {
                    Bins[ i ].Amount++;
                    TotalCount++;

                    return true;
                }

            } // for all bins

            return false;

        } // AddNumber

        public void Clear()
        {
            for( int i = 0; i < Bins.Length; i++ )
            {
                Bins[ i ].Amount = 0L;
            }

            TotalCount = 0UL;

        } // Clear

        public struct NumberBin
        {
            public double lower;
            public double upper;

            private long amount;
            private string name;

            public long Amount { get => amount; set => amount = value; }
            public string Name { get => name; set => name = value; }

        } // class NumberBin

        #region Getter/Setter
        public int NumOfBins { get => numOfBins; set => numOfBins = value; }
        public ulong TotalCount { get => totalCount; set => totalCount = value; }
        public double MinValue { get => minValue; set => minValue = value; }
        public double MaxValue { get => maxValue; set => maxValue = value; }
        public double Delta { get => delta; set => delta = value; }
        public NumberBin[] Bins { get => bins; set => bins = value; }
        #endregion
        #endregion

        #region private members
        private int numOfBins;

        private double minValue;
        private double maxValue;
        private double delta;

        private ulong totalCount;

        private NumberBin[] bins;
        #endregion
    } // class Binning

} // namespace RandomNumbers
