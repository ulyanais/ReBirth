using System;
using System.Windows;

namespace ReBirth.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddNewExercise.xaml
    /// </summary>
    public partial class AddNewExercise : Window
    {
        public AddNewExercise()
        {
            InitializeComponent();
        }

        private void AddExercise_Click(object sender, RoutedEventArgs e)
        {
            if (newNameE.Text != null && newDescriptionE.Text != null && ExerciseImage.Source != null)
            {
                AddExercise.Visibility = Visibility.Visible;
            }
            else
            {
                throw new Exception("Не все поля были заполнены!");
            }
        }
    }
}
