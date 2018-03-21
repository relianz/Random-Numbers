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

using System;                       // ArgumentOutOfRangeException
using System.Security.Cryptography; // RNGCryptoServiceProvider

namespace RandomNumbers
{
    class SecureRandomness: IRandomness
    {
        private readonly RNGCryptoServiceProvider 
            secureProvider = new RNGCryptoServiceProvider();

        public int RandomNumber( int min, int max )
        {
            if (min > max) {
                throw new ArgumentOutOfRangeException();
            }
            return (int)Math.Floor( (min + ((double)max - min) * NextDouble()) );

        } // IRandomness.RandomNumber

        private double NextDouble()
        {
            var data = new byte[ sizeof( uint ) ];
            secureProvider.GetBytes( data );

            var randUint = BitConverter.ToUInt32( data, 0 );
            return randUint / (uint.MaxValue + 1.0);

        } // NextDouble

    } // class SecureRandomness

} // namespace RandomNumbers

/* [EOF] */