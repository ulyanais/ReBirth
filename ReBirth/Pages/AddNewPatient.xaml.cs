using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ReBirth.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddNewPatient.xaml
    /// </summary>
    public partial class AddNewPatient : Window
    {
        //private ObservableCollection<Patient> patients;
        User currentUser;
        Patient selectedUser;

        public ReBirthEntities reBirthEntities;
        ObservableCollection<Exercis> excerciseCollection;
        

        /// <summary>
        /// инициализация AddNewPatient
        /// </summary>
        /// <param name="currentUser">действующая учетная запись</param>
        /// <param name="selectedUser">выбранный пользователь, если null, то новый</param>

        public AddNewPatient(User currentUser, Patient selectedUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            this.selectedUser = selectedUser;


            excerciseCollection = new ObservableCollection<Exercis>();
            SelectEXForPatListView.ItemsSource = excerciseCollection;

            reBirthEntities = new ReBirthEntities();
            diagnozOptions.ItemsSource = reBirthEntities.Diagnosis.Select(d => d.diagnosName).ToList();
            EditNewPatientM();
        }


        /// <summary>
        /// Редактирование пациента. для удобства пользователя данные для редактирования прописываются в TextBox. 
        /// Вывод упражнений в лист для добавления новых упражнений пациенту.
        /// </summary>
        private void EditNewPatientM()
        {
            var specialistId = currentUser.Specialists.First().ID;

            var exerciseList = ReBirthEntities.GetContext()
                .Exercises
                .Where(e => e.specialistID == specialistId)
                .ToList();

            using (var addEditContex = new ReBirthEntities())
            {
                try
                {
                    if (selectedUser != null)
                    {
                        User tmpUser = addEditContex.Users.Where(u => u.userID == selectedUser.userID).FirstOrDefault();
                        Patient tmpPatient = addEditContex.Patients.Where(p => p.userID == tmpUser.userID).FirstOrDefault();
                        Session tmpSession = addEditContex.Sessions.Where(s => s.patientID == tmpPatient.ID).FirstOrDefault();
                        Diagnosi tmpDiag = addEditContex.Diagnosis.Where(d => d.diagID == tmpPatient.diagnosisID).FirstOrDefault();

                        foreach(var ses in addEditContex.Sessions.Where(s => s.patientID == selectedUser.ID).ToList())
                        {
                            excerciseCollection.Add(ses.Exercis);
                        }
                        SelectEXForPatListView.ItemsSource = excerciseCollection;
                        SelectEXForPatListView.Items.Refresh();

                        tmpUser.userID = selectedUser.userID;
                        newLoginP.Text = tmpUser.login;
                        newPasswordP.Text = tmpUser.password;
                        newNameP.Text = tmpPatient.nameP;
                        newSurnameP.Text = tmpPatient.surnameP;
                        newpatronymicP.Text = tmpPatient.patronymicP;
                        kindAcNewP.Text = tmpDiag.diagnosName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            foreach (var item in exerciseList)
            {
                exNewPatListView.Items.Add(item);
            }
        }

        /// <summary>
        /// Добавление упражнения пациенту
        /// </summary>
        private void AddExForPat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var exItem = (Exercis)exNewPatListView.SelectedValue;

                foreach (var item in excerciseCollection)
                {
                    if(item.exID == exItem.exID)
                    {
                        throw new Exception("Данное упражнение уже добавлено!");
                    }
                }
                    excerciseCollection.Add(exItem);
                    SelectEXForPatListView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Удаление упражнения
        /// </summary>
        private void DeleteExForPat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var exItem = (Exercis)SelectEXForPatListView.SelectedValue;

                if (!SelectEXForPatListView.Items.Contains(exItem))
                {
                    throw new Exception("Невозможно удалить упражнение!");
                }
                else
                {
                    excerciseCollection.Remove(exItem);
                    SelectEXForPatListView.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// обновление данных после добавления/редактирования
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            //проверка на пустые строки
            if (newNameP.Text.Length <= 0 && newSurnameP.Text.Length <= 0 && newpatronymicP.Text.Length <= 0 &&
                newLoginP.Text.Length <= 0 && newPasswordP.Text.Length <= 0 && kindAcNewP.Text.Length <= 0)
            {
                throw new Exception("Не все поля были заполнены!");
            }
            if (kindAcNewP.Text.Length <= 0)
            {
                throw new Exception("Не указан тип реабилитации");
            }

            using (var addEditContex = new ReBirthEntities())
            {
                try
                {
                    //редактирование
                    if (selectedUser != null)
                    {
                        User tmpUser = addEditContex.Users.Where(u => u.userID == selectedUser.userID).FirstOrDefault();
                        Patient tmpPatient = addEditContex.Patients.Where(p => p.userID == tmpUser.userID).FirstOrDefault();
                        Session tmpSession = addEditContex.Sessions.Where(s => s.patientID == tmpPatient.ID).FirstOrDefault();
                        Diagnosi tmpDiag = addEditContex.Diagnosis.Where(d => d.diagID == tmpPatient.diagnosisID).FirstOrDefault();

                        tmpPatient.nameP = newNameP.Text;
                        tmpPatient.surnameP = newSurnameP.Text;
                        tmpPatient.patronymicP = newpatronymicP.Text;
                        tmpDiag.diagnosName = kindAcNewP.Text;
                        tmpUser.login = newLoginP.Text;
                        tmpUser.password = newPasswordP.Text;

                        //для добавления нескольких упражнений в сессию пациента
                        if (SelectEXForPatListView.Items != null)
                        {

                            foreach (var exercise in excerciseCollection)
                            {
                                if (addEditContex.Sessions.Any(sE => sE.excerciseID == exercise.exID) == false)
                                {
                                    addEditContex.Sessions.Add(new Session
                                    {
                                        patientID = tmpPatient.ID,
                                        specialistID = addEditContex.Specialists.Where(s => s.userID == currentUser.userID).Select(s => s.ID).FirstOrDefault(),
                                        excerciseID = addEditContex.Exercises.Where(ex => ex.exName == exercise.exName).Select(ex => ex.exID).FirstOrDefault()
                                    });
                                }
                            }

                            addEditContex.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Вы не выбрали ни одно упражнение для пациента!");
                        }

                        addEditContex.SaveChanges();
                    }
                    //добавление
                    else
                    {
                        var newUser = new User
                        {
                            login = newLoginP.Text,
                            password = newPasswordP.Text,
                            roleID = 2,
                        };

                        addEditContex.Users.Add(newUser);


                        kindAcNewP.Text = diagnozOptions.Text;

                        var newPatient = new Patient
                        {
                            userID = newUser.userID,
                            nameP = newNameP.Text,
                            surnameP = newSurnameP.Text,
                            patronymicP = newpatronymicP.Text,
                            diagnosisID = reBirthEntities.Diagnosis.Where(d => d.diagnosName == kindAcNewP.Text).First().diagID
                        };

                        addEditContex.Patients.Add(newPatient);

                        //для добавления нескольких упражнений в сессию пациента
                        if (SelectEXForPatListView.Items != null)
                        { 
                            foreach (var exercise in excerciseCollection)
                            {
                                addEditContex.Sessions.Add(new Session
                                {
                                    patientID = newPatient.ID,
                                    specialistID = addEditContex.Specialists.Where(s => s.userID == currentUser.userID).Select(s => s.ID).FirstOrDefault(),
                                    excerciseID = addEditContex.Exercises.Where(ex => ex.exName == exercise.exName).Select(ex => ex.exID).FirstOrDefault()
                                });
                            }

                            addEditContex.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Вы не выбрали ни одно упражнение для пациента!");
                        }

                        if (addEditContex.Users.Where(u => u.login.Trim().ToLower() == newLoginP.Text.Trim().ToLower()).FirstOrDefault() != null)
                        {
                            throw new Exception("Имя пользователя уже занято!");
                        }

                        addEditContex.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                addEditContex.SaveChanges();
            }

            this.DialogResult = true;

            reBirthEntities.SaveChanges();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void diagnozOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            kindAcNewP.Text = diagnozOptions.SelectedItem.ToString();
        }

        private void rndmLogin_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                int idPat = selectedUser.ID;

                string abc = "patient" + idPat.ToString();

                newLoginP.Text = abc;
            }
            else
            {
                string numPat = "1234567890";
                int kol = 3;
                string abc = "patient";

                string result = String.Empty;
                string resNumPat = String.Empty;

                Random rnd = new Random();
                int lng = numPat.Length;
                for (int i = 0; i < kol; i++)
                    resNumPat += numPat[rnd.Next(lng)];
                result += abc + resNumPat;
                newLoginP.Text = result;
            }
            
        }

        private void rndmPassword_Click(object sender, RoutedEventArgs e)
        {
            string abc = "qwertyuiopasdfghjklzxcvbnm1234567890";

            int kol = 6; // кол-во символов
            string result = "";

            Random rnd = new Random();
            int lng = abc.Length;
            for (int i = 0; i < kol; i++)
                result += abc[rnd.Next(lng)];
            newPasswordP.Text = result;
        }
    }
}
