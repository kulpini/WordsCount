using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WordsCount.Model;

namespace WordsCount.ViewModel
{
    public class TextViewModel : INotifyPropertyChanged, IDataErrorInfo  // viewmodel class, it connects view and model together
    {
        private string statusMessage;
        public string StatusMessage         // a string  Property that notifies view about current state
        {
            get { return statusMessage; }
            set
            {
                statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));   // notify about property changing
            }
        }
        private string idString;
        public string IDString      // a Property that keeps a user input of string id`s
        {
            get { return idString; }
            set
            {
                idString = value;
                OnPropertyChanged(nameof(IDString));
            }
        }
        private List<TextElement> list;         // summary data set 

        private RelayCommand makeTextAnalysis;      // a command for getting data from model
        public List<TextElement> ElementsList
        {
            get { return list; }
            set
            {
                list = value;
                OnPropertyChanged(nameof(ElementsList));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;   // the event of property change
        public TextViewModel()          // viewmodel class constructor
        {
            list = new List<TextElement>();         //  create an empty data set
            idString = "";                          // initialize a user input string with blank value
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand MakeTextAnalysis            // getting summary data set command
        {
            get
            {
                return makeTextAnalysis ??
                    (makeTextAnalysis = new RelayCommand(async obj =>
                    {
                        if (IDString.Trim().Length > 0)
                        {
                            StatusMessage = "... получение данных ...";
                            ElementsList = await GetTextDataAsync();
                            StatusMessage = "";
                        }
                        else
                            StatusMessage = "Не заданы идентификаторы!";
                    }));
            }
        }
        public string this[string propertyName]         // property for validating user input
        {
            get
            {
                string error = string.Empty;
                switch (propertyName)
                {
                    case "IDString":
                        if (!string.IsNullOrWhiteSpace(IDString) && !Regex.IsMatch(IDString, @"\A[0-9\s,;]{1,}\z"))     // check for digits,spaces,commas and semicolons only in user input string
                        {
                            StatusMessage = "Некорректный идентификатор! Используйте цифры, пробелы, а также символы ',' и ';'";        //  inform about incorrect symbol in user input
                            error = "Error";
                        }
                        else
                        {
                            StatusMessage = "";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        private async Task<List<TextElement>> GetTextDataAsync()        // getting data set, addressing to Model
        {
            List<int> idList = MakeIDList();            //  make only numerical id`s from user input string
            int[] idArray = idList.Where(x => x > 0 && x <= 20).OrderBy(x => x).Distinct().ToArray<int>();   // filter received id`s
            return await Task.Run(() => TextElements.GetList(idArray));         // run async task for data set receipt 
        }

        private List<int> MakeIDList()
        {
            string[] strings = IDString.Split(new char[] { ',', ';' });         // split string to substring array 
            List<int> numbers = new List<int>();
            foreach (string item in strings)
            {
                if (IsNumber(item, out int number))     // check string for numerical value
                {
                    numbers.Add(number);                //  add received id to list
                }
            }
            return numbers;
        }
        private static bool IsNumber(string text, out int number)
        {
            number = 0;
            if (Regex.IsMatch(text.Trim(), @"^\d+$"))    // check string for numerical symbols only existence
            {
                number = Convert.ToInt32(text.Trim().TrimStart('0'));           // delete leading zeroes
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
