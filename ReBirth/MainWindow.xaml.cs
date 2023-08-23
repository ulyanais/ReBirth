using System;
using System.Windows;
using System.Windows.Threading;

namespace ReBirth
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static User currentUser;
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Pages.Authorisation registration = new Pages.Authorisation();

            registration.Show();

            timer.Stop();
            this.Close();
        }
    }
}
