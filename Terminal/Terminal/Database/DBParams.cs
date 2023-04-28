using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal
{
    public class DbTables //table names
    {
        //settings table
        public static string Settings { get; set; } = "Settings";
        public static string Users { get; set; } = "Users";
        public static string SapConnections { get; set; } = "SapConnections";
    }

    public class DbParams //table fields
    {
        //App settings PK
        public static string Settings { get; set; } = "Settings";
    }

    public class WSDLKeys //table fields
    {
        //WSDL material Key
        public static string Ids { get; set; } 
    }


}
