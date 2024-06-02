using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Dictionary<string, ObservableCollection<string>> _data;
        private ObservableCollection<string> _selectedValues;
        private string _selectedKey;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, ObservableCollection<string>> Data
        {
            get => _data;
            private set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SelectedValues
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
                    SelectedValues = new ObservableCollection<string>(Data[_selectedKey]);
                    OnPropertyChanged();
                }
            }
        }

        // Constructor that accepts a dictionary
        [IsVisibleInDynamoLibrary(false)]
        public SelectFromDictionaryViewModel(Dictionary<string, ObservableCollection<string>> data)
        {
            _data = data;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}