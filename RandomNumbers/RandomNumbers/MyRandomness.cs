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

        private const int a = 1103515245;
        private const int c = 12345;
        private const ulong m = (System.UInt64)Int32.MaxValue + 1;

        public MyRandomness( int seed = 0 )
        {
            number = (ulong)seed;

        } // ctor

        public int RandomNumber( int min, int max )
        {
            int delta;
            if (min > max)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                delta = max - min; // 60 - (-40) == 100, 60 - 40 == 20, 60 - 0 == 60, -40 - (-60) == 20
            }
               
            ulong r = a * number + c;
            number  = r % m;

            int value = ((int)number % (delta + 1)) + min;

            return value;

        } // IRandomness.RandomNumber

    } // class PseudoRandomness

} // namespace RandomNumbers

/* [EOF] */
