using System;
using System.Windows;
using System.Windows.Controls;
using WordsCount.ViewModel;

namespace WordsCount
{
    public partial class MainWindow : Window
    {
        private TextViewModel viewModel;            // viewmodel class instance
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new TextViewModel();        // initialize a viewmodel instance
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)       // in case of first error appearance
            {
                var textbox = sender as TextBox;
                int selectionStart = GetSelectionStart(textbox.Text);       // make selection of last entered id
                textbox.SelectionStart = selectionStart;
                textbox.SelectionLength = textbox.Text.Length - selectionStart + 1;
            }
        }
        private static int GetSelectionStart(string text)           // calculate selection position
        {
            return Math.Max(text.LastIndexOf(','), text.LastIndexOf(';')) + 1;
        }

        private void TextList_SizeChanged(object sender, SizeChangedEventArgs e)            // adjust listview column while window size changed
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            double workingWidth = listView.ActualWidth - 35; // take into account a scrollbar width

            gView.Columns[0].Width = Math.Abs(workingWidth - 200);      // 1-st column has adjustable width
            gView.Columns[1].Width = 100;       // 2-nd and 3-rd columns have fixed width
            gView.Columns[2].Width = 100;
        }
    }
}
