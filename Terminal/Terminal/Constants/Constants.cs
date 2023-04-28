using System;
using System.IO;
using Terminal.Singleton;
using Xamarin.Forms;

namespace Terminal
{

    public static class Constants
    {
      
        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                return Path.Combine(AppStatus.BasePath, AppStatus.DatabaseFilename)+(Device.RuntimePlatform !="UWP" ? "?socket_timeout=50000":"");
            }
        }
    }

}
