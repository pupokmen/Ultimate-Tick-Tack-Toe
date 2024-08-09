using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TicTacToe;
using UltimateTic_Tac_Toe.MVVM.Models;

namespace UltimateTic_Tac_Toe
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.ToString();
            string password = PasswordTextBox.Text.ToString();
            //MessageBox.Show($"{login}, {password}");
            bool fl = false;
            User newUser = new User();

            using (ApplicationContext db = new ApplicationContext())
            {
                //MessageBox.Show("Соединение открыто");
                foreach (var user in db.Users)
                {
                    //MessageBox.Show($"{user.Login}, {user.Password}");
                    if (user.Login == login && user.Password == password)
                    {
                        foreach (var u in ((MainWindow)Application.Current.MainWindow).viewModel.Users)
                        {
                            if (u.Login == user.Login)
                            {
                                MessageBox.Show("Такой пользователь уже вошел!");
                                return;
                            }
                        }

                        foreach (var u in ((MainWindow)Application.Current.MainWindow).viewModel.InGameUsers)
                        {
                            if (u.Login == user.Login)
                            {
                                MessageBox.Show("Такой пользователь уже вошел!");
                                return;
                            }
                        }

                        newUser = user;
                        fl = true;
                        break;
                    }
                }
            }

            if (fl)
            {
                newUser.Games = new List<GameStat>();
                using (ApplicationContext db = new ApplicationContext())
                {
                    foreach (var game in db.Games)
                    {
                        //MessageBox.Show($"{game.p0} vs {game.p1}");
                        if (newUser.Login == game.p0 || newUser.Login == game.p1)
                            newUser.Games.Add(game);
                    }
                }
                ((MainWindow)Application.Current.MainWindow).viewModel.Users.Add(newUser);
                MessageBox.Show("Вход выполнен!");
                this.Close();
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
