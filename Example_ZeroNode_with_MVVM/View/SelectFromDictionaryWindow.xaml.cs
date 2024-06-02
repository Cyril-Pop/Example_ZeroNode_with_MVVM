using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Autodesk.DesignScript.Runtime;
using System.Text.RegularExpressions;
using Dynamo.Logging;
using System.Collections.ObjectModel;


namespace Custom_UINode.Element.View
{
    /// <summary>
    /// Interaction for Window
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public partial class SelectFromDictionaryWindow : Window
    {
        private ViewModel.SelectFromDictionaryViewModel viewModel;
        // acces of viewModel outside the class 
        internal ViewModel.SelectFromDictionaryViewModel GetViewModel()
        {
            return viewModel;
        }
        private Dictionary<string, ObservableCollection<string>> _dictData;
        private DynamoLogger logger = UserUI.GetLogger();
        /// <summary>
        /// construtor
        /// </summary>
        /// <param name="dictData"></param>
        public SelectFromDictionaryWindow(Dictionary<string, ObservableCollection<string>> dictData)
        {
            InitializeComponent();
            // Setting the DataContext
            _dictData = dictData;
            viewModel = new ViewModel.SelectFromDictionaryViewModel(_dictData);
            this.DataContext = viewModel;
        }
        // No need event handler with this MVVM context, keep comment below for information
        /*
        private void cmbSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string select_value = this.cmbSelect.SelectedItem.ToString();
                logger?.Log($"cmbSelect_SelectionChanged -> select_value {select_value}", LogLevel.File);

                List<string> value;
                if (_dictData.TryGetValue(select_value, out value))
                {
                    logger?.Log("cmbSelect_SelectionChanged -> values founded by key"LogLevel.File);
                    viewModel.SelectedValues = value;
                }
                else
                {
                    logger?.LogWarning("cmbSelect_SelectionChanged -> values NOT founded by key", WarningLevel.Moderate);
                }

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message);
                throw new Exception(ex.Message);
            }

        }
        */
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
