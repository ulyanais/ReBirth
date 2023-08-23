using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ReBirth.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorisation.xaml
    /// </summary>
    public class People
    {
        public string[] name = new string[3];
        public static int amount;
        public static void Resolution(People[] peoples) //объявляем экземпляры класса
        {
            for (int i = 0; i < peoples.Length; i++)
            {
                peoples[i] = new People();
            }
        }
    }

    public partial class Authorisation : Window
    {
        People[] peoples = new People[10];
        ReBirthEntities context = new ReBirthEntities();
        CollectionViewSource peoplesViewSource;

        public Authorisation()
        {
            InitializeComponent();
            People.Resolution(peoples); //вызываем объявление всех экземпляров
            peoplesViewSource = ((CollectionViewSource)FindResource("peoplesViewSource"));
            DataContext = this;
            this.context.SaveChanges();
        }
        /// <summary>
        /// Метод, выдающий важную информацию для пациента
        /// </summary>
        public void MessageInfo()
        {
            string message = "Привет, юзер моего приложения!\nЧтобы реабилитация была максимально эффективной " +
                "и полезной для тебя, советую пользоваться приложением ТОЛЬКО после согласования упражнений " +
                "с лечащим врачом!\n\nВсе упражнения делаются строго по времени и обязательны к выполнению." +
                " Минимальное количество времени, выделенное на тренировку 30 минут.\n\n" +
                "C любовью, ReBirth!";
            string caption = "info for patient";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult defaultResult = MessageBoxResult.No;
            // Отображение MessageBox
            MessageBoxResult result = MessageBox.Show(message, caption, buttons, icon, defaultResult);

            this.FontFamily = new FontFamily("Lucida Bright");

            if (result == MessageBoxResult.No)
            {
                Application.Current.Shutdown();
            }
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
        /// <summary>
        /// авторизация пользователя, проверка на роль
        /// </summary>
        private void LogIn_button(object sender, RoutedEventArgs e)
        {
            try
            {
                //проверка логина в базе
                using (ReBirthEntities context = new ReBirthEntities())
                {

                    if (UserLogin.Text == string.Empty || UserPasswordBox.Password == string.Empty)
                    {
                        throw new Exception("Пустые поля! Проверьте данные!");
                    }

                    User logInUser = null;
                    string tempUserLogin = Convert.ToString(UserLogin.Text).ToLower().Trim();
                    string tempPassword = Convert.ToString(UserPasswordBox.Password).Trim();


                    logInUser = context.Users.Where(b => b.login == tempUserLogin && b.password == tempPassword).FirstOrDefault();
                    //0-админ 1-врач 2-пациент
                    if (logInUser != null)
                    {
                        switch (logInUser.roleID)
                        {
                            case 0:
                                frame.Navigate(new AdminPage(logInUser));
                                break;
                            case 1:
                                frame.Navigate(new DoctorPage(logInUser));
                                break;

                            case 2:
                                frame.Navigate(new UserPage(logInUser));
                                MessageInfo();
                                break;

                            default:
                                throw new Exception("Неверный логин или пароль!");
                        }
                        UserLogin.Text = String.Empty;
                        UserPasswordBox.Password = String.Empty;
                    }
                    else
                        throw new Exception("Неверный логин или пароль!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }
        private void Login_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void StackPanel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(IsGood);
        }
        /// <summary>
        /// запрет ввода русских букв в логине
        /// </summary>
        private void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            var stringData = (string)e.DataObject.GetData(typeof(string));
            if (stringData == null || !stringData.All(IsGood))
                e.CancelCommand();
        }
        bool IsGood(char c)
        {
            if (c >= '0' && c <= '9')
                return true;
            if (c >= 'a' && c <= 'z')
                return true;
            if (c >= 'A' && c <= 'Z')
                return true;
            return false;
        }

        private void UserPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

            if (UserPasswordBox.Password == string.Empty)
            {
                textOver.Visibility = Visibility.Visible;
            }
            else
            {
                textOver.Visibility = Visibility.Collapsed;
            }

        }
    }
}
