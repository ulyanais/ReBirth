using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ReBirth.Pages
{
    /// <summary>
    /// Логика взаимодействия для ExercisesPage.xaml
    /// </summary>
    public partial class ExercisesPage : Window
    {
        public ReBirthEntities reBirthEntities;

        public partial class Exercises
        {
            public int exID { get; set; }
            public string exImage { get; set; }
            public string exName { get; set; }
            public string exDescription { get; set; }
        }
        public ExercisesPage(Exercis exercises)
        {
            InitializeComponent();

            exerciseName.Text = exercises.exName;
            descriptionText.Text = exercises.exDescription;
            exPicImg.Source = new BitmapImage(new Uri("pack://application:,,," + exercises.exImage.Replace('\\', '/')));

        }

    }
}
