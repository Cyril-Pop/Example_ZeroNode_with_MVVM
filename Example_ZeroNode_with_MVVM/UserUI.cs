using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Application = System.Windows.Application;
using Custom_UINode.Element.View;
using System.Linq;
using System.Collections;
using System.Text;
using System.Windows.Input;
using Autodesk.DesignScript.Runtime;
using Dynamo.Configuration;
using Dynamo.Logging;
using System.Collections.ObjectModel;



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
                                                    false,
                                                    false,
                                                    false);
#endif
            //
            logger?.Log("SelectFromDictionary -> Start Logger", LogLevel.File);
            logger?.Log($"SelectFromDictionary -> dll_package_version {GetAssemblyVersion()}", LogLevel.File);
            string input_json_dict = JsonConvert.SerializeObject(DSDict, Formatting.Indented);
            string json_values = JsonConvert.SerializeObject(values, Formatting.Indented);
            logger?.Log($"SelectFromDictionary -> input_json_dict {input_json_dict}", LogLevel.File);

            // convert DesignScript.Builtin.Dictionary to .Net Dictionnary
            //Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            Dictionary<string, ObservableCollection<string>> dictionary = new Dictionary<string, ObservableCollection<string>>();
            for (int i = 0; i < keys.Count; i++)
            {
               // dictionary[keys[i]] = ((IEnumerable)values[i]).Cast<object>().Select(x => x.ToString()).ToList();
               List<string> list_values = ((IEnumerable)values[i]).Cast<object>().Select(x => x.ToString()).ToList();
               dictionary[keys[i]] = new ObservableCollection<string>(list_values);
            }
            SelectFromDictionaryWindow view = new SelectFromDictionaryWindow(dictionary);
            view.ShowDialog();
            // get viewModel
            ViewModel.SelectFromDictionaryViewModel vm = view.GetViewModel();
            //create a new dictionary from view model,convert values and return it 
            var d = new Dictionary<string, object>();
            d.Add("key", vm.SelectedKey);
            d.Add("values", vm.SelectedValues.Select(int.Parse).ToList());
            string out_json_dict = JsonConvert.SerializeObject(d, Formatting.Indented);
            logger?.Log($"SelectFromDictionary -> out_json_dict {out_json_dict}", LogLevel.File);
            logger?.Dispose();
            return d;
        }
    }
}
