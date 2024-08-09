using System.Windows;
using System.Windows.Input;
using UltimateTic_Tac_Toe.MVVM.Models;
using UltimateTic_Tac_Toe.MVVM.Models.Game;
using UltimateTic_Tac_Toe.MVVM.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UltimateTic_Tac_Toe;

namespace TicTacToe
{
    public enum Figure { Cross, Circle, TwoLines }
    public enum Status { Nothing, Tie, CrossWin, CircleWin }
    public enum GameType { Small, Big }
    public enum PlayerType { Player }

    public partial class MainWindow : Window
    {
        public ApplicationViewModel viewModel;
        public ObservableCollection<User> Users;

        public MainWindow()
        {
            InitializeComponent();

            using (ApplicationContext db = new ApplicationContext())
            {
                //User user1 = new User() { Name = "Tom", Login = "LoginTom", Password = "PasswordTom", Games = new List<GameStat>(), Reg = DateTime.Now };
                //User user2 = new User() { Name = "Alice", Login = "LoginAlice", Password = "PasswordAlice", Games = new List<GameStat>(), Reg = DateTime.Now };
                //User user3 = new User() { Name = "Kain", Login = "LoginKain", Password = "PasswordKain", Games = new List<GameStat>(), Reg = DateTime.Now };
                
                //GameStat gameStat1 = new GameStat() { p0 = user1.Login, p1 = user2.Login, w = user1.Login, Date = DateTime.Now };
                //GameStat gameStat2 = new GameStat() { p0 = user2.Login, p1 = user1.Login, w = user1.Login, Date = DateTime.Now };
                //GameStat gameStat3 = new GameStat() { Users = new List<User> { user2, user1, user2 }, Date = DateTime.Now };
                
                //user1.Games.Add(gameStat1);
                //user1.Games.Add(gameStat2);
                //user1.Games.Add(gameStat3);

                //user2.Games.Add(gameStat1);
                //user2.Games.Add(gameStat2);
                //user2.Games.Add(gameStat3);

                //db.Users.AddRange(user1, user2, user3);
                //db.Games.AddRange(gameStat1, gameStat2);
                //db.SaveChanges();

                Users = new ObservableCollection<User>();
                foreach (var user in db.Users)
                {
                    //MessageBox.Show($"{user.Name}");
                    Users.Add(user);
                    user.Games = new List<GameStat>();
                }

                foreach (var game in db.Games)
                {
                    //MessageBox.Show($"{game.p0} vs {game.p1}");

                    foreach (var user in Users)
                    {
                        if (user.Login == game.p0 || user.Login == game.p1)
                            user.Games.Add(game);
                    }
                }

                //foreach (var user in Users)
                //{
                //    foreach (var game in db.Games)
                //    {
                //        if (game.p0 == user.Login)
                //            user.Games.Add(game);
                //    }
                //}
            }

            viewModel = new ApplicationViewModel { Users = this.Users, InGameUsers = new ObservableCollection<User>(), 
                sqaureSize = 75, GameInfo = "Для начала игры нажмите на кнопку \"Начать игру\"" };

            DataContext = viewModel;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            if (viewModel.game != null)
                viewModel.game.ClickHandler(((int)p.X - 200) / viewModel.sqaureSize, ((int)p.Y - 30) / viewModel.sqaureSize); //Handle the click
            
        }

        private void btnBig_Click(object sender, RoutedEventArgs e)
        {
            grdGrid.Children.Clear();
            viewModel.game = new GameManager(GameType.Big, viewModel.sqaureSize, grdGrid, PlayerType.Player, PlayerType.Player);
            
            Add.IsEnabled = false;
            Убрать.IsEnabled = false;
            Swap.IsEnabled = false;
            Убрать.IsEnabled = false;
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
            Application.Current.Shutdown();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            grdGrid.Children.Clear();
            viewModel.game = null;

            Add.IsEnabled = true;
            Убрать.IsEnabled = true;
            Swap.IsEnabled = true;
            Убрать.IsEnabled = true;
            //viewModel.game.EndHandler(Status.CrossWin);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedUser != null)
            if (!viewModel.InGameUsers.Contains(viewModel.SelectedUser))
            {
                if (viewModel.InGameUsers.Count == 2)
                {
                    viewModel.Users.Add(viewModel.InGameUsers[0]);
                    viewModel.InGameUsers.Remove(viewModel.InGameUsers[0]);
                    viewModel.InGameUsers.Add(viewModel.SelectedUser);
                    viewModel.Users.Remove(viewModel.SelectedUser);
                }
                else
                {
                    viewModel.InGameUsers.Add(viewModel.SelectedUser);
                    viewModel.Users.Remove(viewModel.SelectedUser);
                }
            }

            if (viewModel.InGameUsers.Count == 2)
            {
                Start.IsEnabled = true;
                Swap.IsEnabled = true;
                Cancel.IsEnabled = true;
                Surr.IsEnabled = true;
            }
        }

        private void Убрать_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.InGameSelectedUser != null)
            {
                viewModel.Users.Add(viewModel.InGameSelectedUser);
                viewModel.InGameUsers.Remove(viewModel.InGameSelectedUser);
            }

            if (viewModel.InGameUsers.Count < 2)
            {
                Start.IsEnabled = false;
                Swap.IsEnabled = false;
                Cancel.IsEnabled = false;
                Surr.IsEnabled = false;
            }
        }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            User user = viewModel.InGameUsers[0];
            viewModel.InGameUsers[0] = viewModel.InGameUsers[1];
            viewModel.InGameUsers[1] = user;
        }

        private void InGameListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Убрать.IsEnabled)
                Убрать_Click(sender, e);
        }

        private void LobbyListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Add.IsEnabled)
                Add_Click(sender, e);
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.InGameSelectedUser != null)
            {
                StatWindow statWindow = new StatWindow(viewModel.InGameSelectedUser);
                statWindow.Show();
            }

            if (viewModel.InGameSelectedUser == null)
            {
                if (viewModel.SelectedUser != null)
                {
                    StatWindow statWindow = new StatWindow(viewModel.SelectedUser);
                    statWindow.Show();
                }
            }
        }

        private void Выйти_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedUser != null)
            {
                viewModel.Users.Remove(viewModel.SelectedUser);
            }
        }

        private void Surr_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.game != null)
            {
                if (viewModel.game.turn == Figure.Cross)
                {
                    viewModel.game.EndHandler(Status.CircleWin);
                }
                else
                {
                    viewModel.game.EndHandler(Status.CrossWin);
                }
            }
        }
    }
}