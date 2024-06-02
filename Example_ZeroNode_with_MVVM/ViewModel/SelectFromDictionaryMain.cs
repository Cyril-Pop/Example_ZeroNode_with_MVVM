using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Custom_UINode.ViewModel
{
    [IsVisibleInDynamoLibrary(false)]
    public class SelectFromDictionaryViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, List<string>> _data;
        private List<string> _selectedValues;
        private string _selectedKey;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, List<string>> Data
        {
            get => _data;
            private set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        public List<string> SelectedValues
        {
            get => _selectedValues;
            set
            {
                _selectedValues = value;
                OnPropertyChanged();
            }
        }

        public string SelectedKey
        {
            get => _selectedKey;
            set
            {
                if (_selectedKey != value)
                {
                    _selectedKey = value;
                    SelectedValues = new List<string>(Data[_selectedKey]);
                    OnPropertyChanged();
                }
            }
        }

        // Constructor that accepts a dictionary
        [IsVisibleInDynamoLibrary(false)]
        public SelectFromDictionaryViewModel(Dictionary<string, List<string>> data)
        {
            _data = data;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}