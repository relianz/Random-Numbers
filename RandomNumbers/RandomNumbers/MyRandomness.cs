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

using System; // ArgumentOutOfRangeException

namespace RandomNumbers
{
    class MyRandomness : IRandomness
    {
        private ulong number;

        // constant values 
        // from https://en.wikipedia.org/wiki/Linear_congruential_generator#cite_note-9
        //
        private const int a = 1103515245;                           // multiplier
        private const int c = 12345;                                // increment
        private const ulong m = (System.UInt64)Int32.MaxValue + 1;  // modulus

        // from https://docs.microsoft.com/de-de/dotnet/api/system.int32?view=netframework-4.7.1
        //
        // Int32 is an immutable value type that represents signed integers with values 
        // that range from negative 2,147,483,648 (which is represented by the Int32.MinValue constant) 
        // through positive 2,147,483,647 (which is represented by the Int32.MaxValue constant).

        public MyRandomness( int seed = 0 )
        {
            number = (ulong)seed;

        } // ctor

        public int RandomNumber( int min, int max )
        {
            ulong delta;
            if (min > max)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                delta = (ulong)(max - min); // 60 - (-40) == 100, 60 - 40 == 20, 60 - 0 == 60, -40 - (-60) == 20
            }
               
            ulong r = a * number + c;
            number  = r % m;
            
            int value = ScaleNumber( min, max );

            return value;

        } // IRandomness.RandomNumber

        private int ScaleNumber( int min, int max )
        {
            double w = number / (m - 1.0d);            
            double d = max - min;

            int n = (int)Math.Round( w*d ) + min;

            return n;

        } // Scale

    } // class PseudoRandomness

} // namespace RandomNumbers

/* [EOF] */
