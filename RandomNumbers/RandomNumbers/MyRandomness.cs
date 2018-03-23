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
        private int number;

        private const int a = 1103515245;
        private const int c = 12345;
        private const int m = Int32.MaxValue;

        public MyRandomness( int seed = 0) => this.number = seed;

        public int RandomNumber( int min, int max )
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException();
            }

            long r = a * number + c;
            if (r < 0L)
                r = -r;

            number = (int)(r % m);
            
            int value = number % max + min;

            return value;

        } // IRandomness.RandomNumber

    } // class PseudoRandomness

} // namespace RandomNumbers

/* [EOF] */
