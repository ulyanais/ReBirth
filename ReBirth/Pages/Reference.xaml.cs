using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReBirth.Pages
{
    /// <summary>
    /// Логика взаимодействия для Reference.xaml
    /// </summary>
    public partial class Reference : Page
    {
        public static User currentUser;
        public static Patient selectedPat;

        public ReBirthEntities reBirthEntities;
        public Reference(User user, Patient patient)
        {
            currentUser = user;  // текущий пользователь
            selectedPat = patient;

            InitializeComponent();

            LoadRef();
        }

        public void LoadRef()
        {
            reBirthEntities = new ReBirthEntities();

            surName.Text = reBirthEntities.Patients.Where(p => p.ID == selectedPat.ID).Select(p => p.surnameP).Single();
            name.Text = reBirthEntities.Patients.Where(p => p.ID == selectedPat.ID).Select(p => p.nameP).Single();
            patronymic.Text = reBirthEntities.Patients.Where(p => p.ID == selectedPat.ID).Select(p => p.patronymicP).Single();
            fullDiagnosis.Text = reBirthEntities.Diagnosis.Where(d => d.diagID == selectedPat.diagnosisID).Select(d => d.diagnosName).Single();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DoctorPage(currentUser));
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            ReferencePage newPrint = new ReferencePage();

            newPrint.patientNameBox.Text = selectedPat.surnameP + " " + selectedPat.nameP + " " + selectedPat.patronymicP;
            newPrint.patientDateBox.Text = birth.Text;
            newPrint.patientAdressBox.Text = adress.Text;
            newPrint.patientOccupationBox.Text = occupation.Text;
            newPrint.patientDateDateBox.Text = dateDisease.Text;
            newPrint.patientStBox.Text = dateDirection.Text;
            newPrint.patientFullDiagBox.Text = fullDiagnosis.Text;
            newPrint.patientAnamnesisBox.Text = anamnesis.Text;
            newPrint.patientRecBox.Text = recommendations.Text;
            newPrint.doctorName.Text = currentUser.Specialists.Select(s => s.surnameS).FirstOrDefault() + " " + currentUser.Specialists.Select(s => s.nameS).FirstOrDefault() + " " + currentUser.Specialists.Select(s => s.patronymicS).FirstOrDefault();

            PrintDialog printDialog = new PrintDialog();

            newPrint.ShowDialog();

            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(newPrint.prtGrd, Title);
            }
        }
    }
}
