using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Collections.Generic;

namespace WpfAppBindingTest
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedOption;
        private ObservableCollection<object> _gridData;

        public ObservableCollection<string> DropDownOptions { get; set; }

        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
                RefreshDataAsync();
            }
        }

        public ObservableCollection<object> GridData
        {
            get => _gridData;
            set
            {
                _gridData = value;
                OnPropertyChanged(nameof(GridData));
            }
        }

        public MainViewModel()
        {
            DropDownOptions = new ObservableCollection<string> { "Option 1", "Option 2", "Option 3" };
            GridData = new ObservableCollection<object>();
            UpdateRowCommand = new RelayCommand<object>(UpdateRowAsync);
        }


        public ICommand UpdateRowCommand { get; private set; }

        private async void UpdateRowAsync(object rowData)
        {
            if (rowData is DataItem dataItem)
            {
                // Simulate a remote call to update the data
                await Task.Delay(500);

                // Update the data item (replace this with your actual update logic)
                dataItem.Column2 = $"Updated at {DateTime.Now}";

                // Notify the UI that the item has changed
                OnPropertyChanged(nameof(GridData));
            }
        }

        private async Task RefreshDataAsync()
        {
            // Simulate a remote call
            await Task.Delay(1000);

            // Replace this with your actual data fetching logic
            var newData = await FetchDataFromRemoteSourceAsync(SelectedOption);

            GridData.Clear();
            foreach (var item in newData)
            {
                GridData.Add(item);
            }
        }

        private async Task<IEnumerable<object>> FetchDataFromRemoteSourceAsync(string option)
        {
            // Implement your remote data fetching logic here
            // This is just a placeholder
            return new List<object> { new DataItem { Column1 = option, Column2 = "Data" } };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class DataItem : INotifyPropertyChanged
    {
        private string _column1;
        private string _column2;

        public string Column1
        {
            get => _column1;
            set
            {
                _column1 = value;
                OnPropertyChanged(nameof(Column1));
            }
        }

        public string Column2
        {
            get => _column2;
            set
            {
                _column2 = value;
                OnPropertyChanged(nameof(Column2));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Simple implementation of ICommand for the button
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);
        public void Execute(object parameter) => _execute((T)parameter);
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}

