using System.Windows;
using System.Windows.Input;
using TicTacToe;
using UltimateTic_Tac_Toe.MVVM.Models;

namespace UltimateTic_Tac_Toe
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim().TrimEnd().ToString();
            string login = LoginTextBox.Text.Trim().TrimEnd().ToString();
            string password = PasswordTextBox.Text.Trim().TrimEnd().ToString();
            bool fl = true;
            User newUser;
            //MessageBox.Show($"{login}, {password}");

            if (name == "")
            {
                MessageBox.Show("Имя не должно быть пустым.");
                return;
            }

            if (login == "")
            {
                MessageBox.Show("Логин не должен быть пустым.");
                return;
            }

            if (password.Length < 4)
            {
                MessageBox.Show("Пароль должен быть больше 3 символов длинной.");
                return;
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in db.Users)
                {
                    if (user.Login == login)
                    {
                        MessageBox.Show("Выберите другой логин.");
                        fl = false;
                        LoginTextBox.Text = "";
                        break;
                    }
                }

                if (fl == true)
                {
                    newUser = User.Register(name, login, password);
                    ((MainWindow)Application.Current.MainWindow).viewModel.Users.Add(newUser);
                    
                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
