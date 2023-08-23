using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace ReBirth.Pages
{
    /// <summary>
    /// свойство(запрос) сортировки 
    /// </summary>
    public class SortItem : INotifyPropertyChanged
    {
        private string sortProperty;                                       
        public string SortProperty
        {
            get
            { return sortProperty; }
            set
            {
                if (sortProperty != value)
                {
                    sortProperty = value;
                    OnPropertyChanged("sortProperty");
                }
            }
        }
        /// <summary>
        /// наименование сортировки для вывода
        /// </summary>
        private string sortTitle;   
        public string SortTitle
        {
            get
            { return sortTitle; }
            set
            {
                if (sortTitle != value)
                {
                    sortTitle = value;
                    OnPropertyChanged("sortTitle");
                }
            }
        }
        public SortItem(string prop, string title)
        {
            sortProperty = prop;
            sortTitle = title;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    /// <summary>
    /// Логика взаимодействия для DoctorPage.xaml
    /// </summary>
    public partial class DoctorPage : Page
    {
        public ReBirthEntities reBirthEntities;

        public static User currentUser;

        ObservableCollection<Patient> patientsCollection = new ObservableCollection<Patient>();
        ObservableCollection<Exercis> excerciseCollection = new ObservableCollection<Exercis>();

        string diagnosisFilter = "";

        ObservableCollection<SortItem> sortCollection = new ObservableCollection<SortItem>
            {
                new SortItem(null, "Без сортировки"),
                new SortItem("patronymicP", "Фамилия"),
                new SortItem("diagnosName", "Диагноз"),
            };

        SortItem orderByItem;

        /// <summary>
        /// Логика взаимодействия для DoctorPage.xaml
        /// </summary>
        /// <param name="user">текущий пользователь</param>
        public DoctorPage(User user)
        {
            currentUser = user; 
            InitializeComponent();
            patientsCollection = new ObservableCollection<Patient>();
            patientListView.ItemsSource = patientsCollection;
            excerciseCollection = new ObservableCollection<Exercis>();
            exerciseListView.ItemsSource = excerciseCollection;

            orderByItem = sortCollection[0];
            Sort.ItemsSource = sortCollection;
            FilterBox.SelectedIndex = 0;

            LoadPat();
        }
        /// <summary>
        /// метод для работы с listView c пациентами и упражнениеями
        /// </summary>
        public void LoadPat()
        {
            excerciseCollection.Clear();
            patientsCollection.Clear();

            reBirthEntities = new ReBirthEntities();
            

            var specialistId = currentUser.Specialists.First().ID;
            var patientList = ReBirthEntities.GetContext()
                    .Patients
                    .Where(p => p.Sessions.Any(s => s.specialistID == specialistId) == true)
                    .Where(p => FilterBox.SelectedIndex != 0 ? p.diagnosisID == FilterBox.SelectedIndex : p.ID != 0)
                    .Where(p => p.nameP.ToLower().Contains(Search.Text)
                    || p.patronymicP.ToLower().Contains(Search.Text)
                    || p.surnameP.ToLower().Contains(Search.Text)
                    || p.Diagnosi.diagnosName.ToLower().Contains(Search.Text))
                    .ToList();


            var exerciseList = ReBirthEntities.GetContext()
                .Exercises
                .Where(e => e.specialistID == specialistId)
                .Where(e => e.exName.ToLower().Contains(Search.Text)
                || e.exDescription.ToLower().Contains(Search.Text))
                .ToList();

            diagnosisFilter = FilterBox.SelectedItem as String;
            if (FilterBox.Items.Count <= 1)
            {
                foreach (var item in reBirthEntities.Diagnosis)
                {
                    FilterBox.Items.Add(item.diagnosName);
                }
            }

            if (Sort != null)
            {
                if (Sort.SelectedIndex != 0)
                {
                    if (byAscending.IsChecked == true)
                        switch (Sort.SelectedIndex)
                        {
                            case 1:
                                patientList = patientList
                            .OrderBy(p => p.patronymicP
                            ).ToList();
                                break;
                            case 2:
                                patientList = patientList
                            .OrderBy(p => p.Diagnosi.diagnosName
                            ).ToList();
                                break;
                        }
                    else if (byDescending.IsChecked == true)
                        switch (Sort.SelectedIndex)
                        {
                            case 1:
                                patientList = patientList
                            .OrderByDescending(p => p.patronymicP
                            ).ToList();
                                break;
                            case 2:
                                patientList = patientList
                            .OrderByDescending(p => p.Diagnosi.diagnosName
                            ).ToList();
                                break;
                        }
                }
            }

            foreach (var item in patientList)
            {
                patientsCollection.Add(item);
            }


            foreach (var item in exerciseList)
            {
                excerciseCollection.Add(item);
            }
        }

        /// <summary>
        /// Кнопки добавления, редактирования и удаления
        /// </summary>
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            string message = "Добавить пациента?";
            string caption = "Добавление данных";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult defaultResult = MessageBoxResult.No;
            // Отображение MessageBox
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon, defaultResult);

            this.FontFamily = new FontFamily("Comic Sans MS");

            // Если да, открывается окно редактирования
            if ((result == MessageBoxResult.Yes))
            {
                Pages.AddNewPatient addPatient = new Pages.AddNewPatient(currentUser, null);
                patientListView.SelectedValue = null;
                addPatient.ShowDialog();

                if (addPatient.DialogResult == true)
                {
                    reBirthEntities.SaveChanges();
                    patientListView.Items.Refresh();
                    LoadPat();
                }
            }
        }
        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            string message = "Редактировать данные?";
            string caption = "Изменение данных";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult defaultResult = MessageBoxResult.No;
            // Отображение MessageBox
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon, defaultResult);

            this.FontFamily = new FontFamily("Comic Sans MS");

            // Если да, открывается окно редактирования
            if ((result == MessageBoxResult.Yes))
            {
                if (patientListView.SelectedValue != null)
                {
                    Patient tmpPat = (Patient)patientListView.SelectedItem;
                    Pages.AddNewPatient editPatient = new Pages.AddNewPatient(currentUser, tmpPat);
                    editPatient.ShowDialog();
                    patientListView.Items.Refresh();

                    if (editPatient.DialogResult == true)
                    {
                        patientListView.Items.Refresh();
                        reBirthEntities.SaveChanges();
                        LoadPat();
                    }
                }
            }
        }

        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            string message = "Удалить данные?";
            string caption = "Изменение данных";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult defaultResult = MessageBoxResult.No;
            // Отображение MessageBox
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon, defaultResult);

            this.FontFamily = new FontFamily("Comic Sans MS");

            using (var addEditContex = new ReBirthEntities())
            {
                try
                {
                    Patient tmpPat = (Patient)patientListView.SelectedItem;
                    if (currentUser != null)
                    {
                        User tmpUser = addEditContex.Users.Where(u => u.userID == tmpPat.userID).FirstOrDefault();
                        Patient tmpPatient = addEditContex.Patients.Where(p => p.userID == tmpUser.userID).FirstOrDefault();
                        Session tmpSession = addEditContex.Sessions.Where(z => z.patientID == tmpPatient.ID).FirstOrDefault();

                        addEditContex.Sessions.Remove(tmpSession);
                        addEditContex.SaveChanges();

                        addEditContex.Patients.Remove(tmpPatient);
                        addEditContex.SaveChanges();

                        addEditContex.Users.Remove(tmpUser);
                        addEditContex.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                patientListView.Items.Refresh();
            }

        }
        private void AddExercise_Click(object sender, RoutedEventArgs e)
        {

        }
        private void EditExercise_Click(object sender, RoutedEventArgs e)
        {
            string message = "Редактировать данные?";
            string caption = "Изменение данных";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult defaultResult = MessageBoxResult.No;
            // Отображение MessageBox
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon, defaultResult);

            this.FontFamily = new FontFamily("Comic Sans MS");

            // Если да, открывается окно добавления
            if ((result == MessageBoxResult.Yes))
            {
                if (exerciseListView.SelectedValue != null)
                {
                    Exercis tmpEx = (Exercis)exerciseListView.SelectedItem;
                    Pages.AddNewExercise editEx = new Pages.AddNewExercise();
                    editEx.ShowDialog();
                }
            }
        }
        private void DeleteExercise_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PatientView_Click(object sender, RoutedEventArgs e)
        {
            exerciseListView.Visibility = Visibility.Collapsed;
            patientListView.Visibility = Visibility.Visible;
        }

        private void ExerciseView_Click(object sender, RoutedEventArgs e)
        {
            patientListView.Visibility = Visibility.Collapsed;
            exerciseListView.Visibility = Visibility.Visible;
        }

        void Close_Click(object sender, RoutedEventArgs e)
        {
            string message = "Вы уверены, что хотите закрыть программу?";
            string caption = "Завершение сеанса";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult defaultResult = MessageBoxResult.No;
            // Отображение MessageBox
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon, defaultResult);

            this.FontFamily = new FontFamily("Comic Sans MS");

            // Если да, открывается окно добавления
            if ((result == MessageBoxResult.Yes))
            {
                Application.Current.Shutdown();
            }
        }
        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
        private void ExerciseTypeName_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void SortMenu_Click(object sender, RoutedEventArgs e)
        {
            if (SortPanel.Visibility == Visibility.Visible)
            {
                SortPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                SortPanel.Visibility = Visibility.Visible;
            }
            
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadPat();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadPat();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //orderByItem = Sort.SelectedItem as SortItem;
            LoadPat();
        }

        private void FilterMenu_Click(object sender, RoutedEventArgs e)
        {
            if (FilterBox.Visibility == Visibility.Visible)
            {
                FilterBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                FilterBox.Visibility = Visibility.Visible;
            }
            LoadPat();
        }

        private void SearchMenu_Click(object sender, RoutedEventArgs e)
        {
            if (SearchPanel.Visibility == Visibility.Visible)
            {
                SearchPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                SearchPanel.Visibility = Visibility.Visible;
            }
            
        }
        private void byAscending_Checked(object sender, RoutedEventArgs e)
        {
            LoadPat();
        }

        private void byDescending_Checked(object sender, RoutedEventArgs e)
        {
            LoadPat();
        }
        /// <summary>
        /// формирование справки 027/у для вывода на печать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reference_Click(object sender, RoutedEventArgs e)
        {
            if (patientListView.SelectedValue != null)
            {
                Patient selectedPatient = (Patient)patientListView.SelectedItem;                
                this.NavigationService.Navigate(new Reference(currentUser, selectedPatient));
            }
            else
            {
                throw new Exception("Не выбран пациент для формирования справки 027 / у!");
                
            }
        }
    }
}
