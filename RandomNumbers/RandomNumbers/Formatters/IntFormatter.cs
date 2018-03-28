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

using System;                                               // Type
using System.Globalization;                                 // CultureInfo

using Windows.UI.Xaml.Data;                                 // IValueConverter

namespace RandomNumbers.Formatters
{
    internal class IntFormatter : IValueConverter
    {
        // This converts the Integer object to the string to display
        public object Convert( object value, Type targetType, object parameter, string language )
        {
            // Retrieve the format string and use it to format the value.
            string formatString = parameter as string;
            if( !string.IsNullOrEmpty( formatString ) )
            {
                return string.Format( new CultureInfo( language ), formatString, value );
            }

            // If the format string is null or empty, simply call ToString() on the value.
            return value.ToString();

        } // Convert

        public object ConvertBack( object value, Type targetType, object parameter, string language )
        {
            int n;
            bool isNumeric = int.TryParse( value.ToString(), out n );

            if (isNumeric)
            {
                return n;
            }
            else
            {
                return 0;
            }
        }

    } // class IntFormatter

} // namespace RandomNumbers.Formatters
