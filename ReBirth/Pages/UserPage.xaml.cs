using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ReBirth.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public ReBirthEntities reBirthEntities;
        ReBirthEntities context = new ReBirthEntities();

        public static User currentUser;

        ObservableCollection<Exercis> excerciseCollection;
        public UserPage(User user)
        {
            InitializeComponent();
            currentUser = user; // текущий пользователь, который авторизовался в приложении
            LoadEx();
        }
        /// <summary>
        /// метод, получающий упражнения, назначенные пациенту из БД
        /// </summary>
        public void LoadEx()
        {
            excerciseCollection = new ObservableCollection<Exercis>();

            reBirthEntities = new ReBirthEntities();

            List<Session> sessions = reBirthEntities.Sessions.Where(s => s.excerciseID == s.Exercis.exID).ToList();

            foreach (Session ses in sessions)
            {
                Exercis exToAdd = new Exercis
                {
                    exID = ses.Exercis.exID,
                    exName = ses.Exercis.exName,
                    exImage = ses.Exercis.exImage,
                    exDescription = ses.Exercis.exDescription
                };
                excerciseCollection.Add(exToAdd);
            }
            exerciseListView.ItemsSource = excerciseCollection;
        }

        /// <summary>
        /// метод для открытия окна с подробным описанием упражнения
        /// </summary>
        public void MessStartEx()
        {
            string message = "Изучить упражнение и приступить к выполнению?";
            string caption = "Приготовьтесь выполнять упражнение!";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult defaultResult = MessageBoxResult.No;

            // Отображение MessageBox
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon, defaultResult);

            this.FontFamily = new FontFamily("Comic Sans MS");

            // Если да, открывается окно с упражнением
            if ((result == MessageBoxResult.Yes))
            {
                var exercise = reBirthEntities.Exercises.Single(a => a.exID == ((Exercis)exerciseListView.SelectedValue).exID);
                if ((bool)new ExercisesPage(exercise).ShowDialog())
                {
                    LoadEx();
                }
            }
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

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

        private void exerciseListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessStartEx();
        }

        private void ExInfo_Click(object sender, RoutedEventArgs e)
        {
            MessStartEx();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            new UserHelp().ShowDialog();
        }
    }
}
