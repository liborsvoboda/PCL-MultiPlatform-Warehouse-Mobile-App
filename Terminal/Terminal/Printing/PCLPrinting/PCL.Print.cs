using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Terminal.Interfaces;
using Xamarin.Forms;

namespace Terminal.PclPrintService
{
    public class PclPrintService
    {

        public static void PrintFromUrl(string url)
        {
            using (var webClient = new WebClient())
            {
                byte[] content = webClient.DownloadData(new Uri(url));
                Printing(content);
            }
        }

        public static void PrintFromFile(string path)
        {
            byte[] content = File.ReadAllBytes(path);
            Printing(content);
        }

        public static string Printing(byte[] content)
        {
            string ipAddress = App.Settings.labelPrinterIp;
            int portNumber = Convert.ToInt32(App.Settings.labelPrinterPort);

            IPclPrintService iPclPrintService = DependencyService.Get<IPclPrintService>();
            if (iPclPrintService == null) { return "no printer service found"; }

            try
            {
                if (content.Length > 0) {
                    iPclPrintService.Print(ipAddress, portNumber, content); 
                } 
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}