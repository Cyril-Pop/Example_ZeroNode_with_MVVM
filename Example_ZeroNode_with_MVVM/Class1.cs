using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Revit.Elements;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using DS = Autodesk.DesignScript.Geometry;
using Application = System.Windows.Application;
using Custom_UINode.Element.View;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using System.Collections;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using Autodesk.DesignScript.Runtime;
using Dynamo.Configuration;
using Dynamo.Logging;


namespace Custom_UINode.Element
{
    public class UserUI
    {
        // define logger variable
        private static DynamoLogger logger { set; get; } = null;
        // acces of logger outside the class 
        internal static DynamoLogger GetLogger()
        {
            return logger;
        }
        private UserUI()
        {
        }
        public static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().FullName;
        }

        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[] { "key", "values" })]
        public static Dictionary<string, object> SelectFromDictionary(DesignScript.Builtin.Dictionary DSDict)
        {
            List<string> keys = DSDict.Keys.ToList();
            List<object> values = DSDict.Values.ToList();
            // create log
#if DEBUG
            logger = new DynamoLogger(new DebugSettings(),
                                                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                    true,
                                                    false,
                                                    false);
#endif
            //
            logger?.Log("SelectFromDictionary -> Start Logger", LogLevel.File);
            logger?.Log($"SelectFromDictionary -> dll_package_version {GetAssemblyVersion()}", LogLevel.File);
            string json_dict = JsonConvert.SerializeObject(DSDict, Formatting.Indented);
            string json_values = JsonConvert.SerializeObject(values, Formatting.Indented);
            logger?.Log($"SelectFromDictionary -> json_dict { json_dict }", LogLevel.File);
            logger?.Log($"SelectFromDictionary -> json_values { json_values}", LogLevel.File);
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

            for (int i = 0; i < keys.Count; i++)
            {
                dictionary[keys[i]] = ((IEnumerable)values[i]).Cast<object>().Select(x => x.ToString()).ToList();
            }
            SelectFromDictionaryWindow view = new SelectFromDictionaryWindow(dictionary);
            view.ShowDialog();
            ViewModel.SelectFromDictionaryViewModel vm = view.GetViewModel();
            //create a new dictionary and return it from view model
            var d = new Dictionary<string, object>();
            d.Add("key", vm.SelectedKey);
            d.Add("values", vm.SelectedValues.Select(int.Parse).ToList());
            logger?.Dispose();
            return d;
        }
    }
}
