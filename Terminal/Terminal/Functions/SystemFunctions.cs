using Novell.Directory.Ldap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Terminal.DbModels;
using Terminal.Models;
using Terminal.Singleton;
using Terminal.Views;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Terminal
{
    class SystemFunctions
    {
        public static T Clone<T>(ref T source)
        {
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, serializeSettings), deserializeSettings);
        }


        public static bool checkOnline(string address, int port)
        {


            try
            {
                if (!Plugin.Connectivity.CrossConnectivity.IsSupported)
                {
                    return true;
                }

                var current = Plugin.Connectivity.CrossConnectivity.Current;
                if (current.IsConnected == false)
                {
                    return false;
                }
                else
                {
                    if (current.IsRemoteReachable(address, port, 3000).Result == false)
                    {
                        return false;
                    }
                    else return true;

                }
            }
            catch (Exception) {
                return false;
            }
            finally
            {
                Plugin.Connectivity.CrossConnectivity.Dispose();
            }
        }

        public static void checkPlatform()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    GlobalResources.Current.Platform = "iOS";
                    break;
                case Device.Android:
                    GlobalResources.Current.Platform = "Android";
                    break;
                case Device.UWP:
                    GlobalResources.Current.Platform = "UWP";
                    break;
                default:
                    GlobalResources.Current.Platform = "";
                    break;
            }

        }



      
    }

}
