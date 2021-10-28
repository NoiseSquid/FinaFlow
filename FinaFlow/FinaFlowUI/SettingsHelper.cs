using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinaFlowUI
{
    public static class SettingsHelper
    {
        private static Windows.Storage.ApplicationDataContainer localSettings;

        public static string CurrentDatabaseFilepath 
        { 
            get {
                return (string)localSettings.Values["currentDatabaseFilepath"]; 
            } 
            set {
                localSettings.Values["currentDatabaseFilepath"] = value; 
            } 
        }

        public static void Initialise()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }
    }

    public class SettingsSerialisable
    {
        public string currentDatabaseFilepath = "";
    }
}
