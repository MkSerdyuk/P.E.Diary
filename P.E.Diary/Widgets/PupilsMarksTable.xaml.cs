using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for PupilsMarksTable.xaml
    /// </summary>
    public partial class PupilsMarksTable : UserControl
    {
        public SchoolClass CurrentSchoolClass;
        private List<Test> _tests;

        public PupilsMarksTable()
        {
            InitializeComponent();
        }

        public Test GetSelectedTest()
        {
            if (Table.SelectedCells[0].Column != null)
            {
                return _tests[Table.SelectedCells[0].Column.DisplayIndex - 2];
            }
            return null;
        }

        public void DeleteSelected()
        {
            if (FoolProof.DeletionProtection(Table.SelectedCells[0].Column.Header.ToString()))
            {
                Test test = GetSelectedTest();
                SqlReader.DeleteTest(test.Normative.Id, test.Date);
                LoadTable(CurrentSchoolClass);
            }
        }

        public void LoadTable(SchoolClass schoolClass)
        {
            CurrentSchoolClass = schoolClass;
            _tests = SqlReader.GetSchoolClassTests(schoolClass);
            Table.ItemsSource = null; //очистка
            Table.Columns.Clear();

            PupilMarksRow[] data = new PupilMarksRow[CurrentSchoolClass.Pupils.Count];
            Binding binding = new Binding("Data[0]");
            DataGridTextColumn surnameColumn = new DataGridTextColumn { Header = "Фамилия", Binding = binding };
            Table.Columns.Add(surnameColumn);
            binding = new Binding("Data[1]");
            DataGridTextColumn nameColumn = new DataGridTextColumn { Header = "Имя", Binding = binding };
            Table.Columns.Add(nameColumn);
            for (int i = 0; i < _tests.Count; i++)
            {
                binding = new Binding("Data[" + (i + 2) + "]");
                string newColumnHeader = _tests[i].Normative.Name + " " + _tests[i].Date.ToString();
                Table.Columns.Add(new DataGridTextColumn { Header = newColumnHeader, Binding = binding });
            }
            for (int i = 0; i < CurrentSchoolClass.Pupils.Count; i++)
            {
                PupilMarksRow row = new PupilMarksRow(_tests.Count + 2);
                row.Data[0] = CurrentSchoolClass.Pupils[i].Surname;
                row.Data[1] = CurrentSchoolClass.Pupils[i].Name;
                for (int j = 0; j < _tests.Count; j++)
                {
                    string header = _tests[j].Normative.Name + " " + _tests[j].Date.ToString();
                    int mark = CurrentSchoolClass.Pupils[i].GetMark(_tests[j].Normative,
                        SqlReader.GetTestResult(_tests[j].Normative.Id, CurrentSchoolClass.Pupils[i].Id,
                        _tests[j].Date));
                    if (mark > 5)
                    {
                        row.Data[j + 2] = "5+1";
                    }
                    else
                    {
                        row.Data[j + 2] = mark.ToString();
                    }
                }
                data[i] = row;
            }
            Table.ItemsSource = data;
            Table.Columns.RemoveAt(Table.Columns.Count - 1); //последня строчка - служебная
            Table.Items.Refresh();
        }
    }

    public class PupilMarksRow
    {
        public PupilMarksRow(int length)
        {
            Data = new string[length];
        }
        public string[] Data { get; set; }
    }
}
