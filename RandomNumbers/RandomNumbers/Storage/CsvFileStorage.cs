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

using System;                            // DateTime
using System.Collections.Generic;        // List
using System.Threading.Tasks;            // Task

using Windows.Storage;                   // StorageFile
using Windows.Storage.Pickers;           // FileSavePicker
using Windows.Storage.Provider;          // FileUpdateStatus
using Windows.Storage.Streams;           // IRandomAccessStream

namespace RandomNumbers.Storage
{
    public class CsvFileStorage : INumberStorage
    {
        #region Public members
        public async Task<bool> Open()
        {
            if( file == null )
            {
                throw new InvalidOperationException( "No file selected" );
            }

            // Prevent updates to the number file until we finish making changes and call CompleteUpdatesAsync:
            CachedFileManager.DeferUpdates( file );

            var s = await file.OpenAsync( FileAccessMode.ReadWrite );
            stream = (IRandomAccessStream)s;

            outputStream = stream.GetOutputStreamAt( 0 );
            writer = new DataWriter( outputStream );

            return true;

        } // Open

        public async Task<bool> Close()
        {
            if( writer == null ) {
                throw new InvalidOperationException( "Not opened" );
            }

            await writer.StoreAsync();

            if (outputStream != null)
                await outputStream.FlushAsync();
            else
                return false;

            FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync( file );
            if( status != FileUpdateStatus.Complete ) {
                return false;
            }

            writer = null;
            outputStream = null;

            return true;

        } // Close

        public async Task<string> SelectLocation( string basename = "CsvFileStorage.csv" )
        {
            var savePicker = new FileSavePicker();

            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the number file as:
            savePicker.FileTypeChoices.Add( "CSV File", new List<string>() { ".csv" } );

            // Create default file name if the user does not type one in or select a file to replace:
            string now = DateTime.Now.ToString( "yyMMdd_HHmm" );
            savePicker.SuggestedFileName = now + "-" + basename;

            file = await savePicker.PickSaveFileAsync();
            if( file != null )
            {
                location = file.Path;

            } // file != null

            return location;

        } // SelectLocation

        public void StoreNumbers( int n, double d )
        {
            if( writer == null )
            {
                throw new InvalidOperationException( "File not opened" );
            }

            string line = n.ToString() + ";" + d.ToString() + "\n";
            writer.WriteString( line );

        } // StoreNumbers

        #region Getter/Setter
        public string Location { get => location; }
        #endregion
        #endregion

        #region Private members
        private StorageFile file;
        private IRandomAccessStream stream;
        private IOutputStream outputStream;
        private DataWriter writer;
        private string location;
        #endregion

    } // class CsvFileStorage

} // namespace RandomNumbers.Storage
