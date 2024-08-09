using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using UltimateTic_Tac_Toe.MVVM.Models;
using UltimateTic_Tac_Toe.MVVM.ViewModels;
using Microsoft.VisualBasic;
using System;

namespace UltimateTic_Tac_Toe
{
    public partial class StatWindow : Window
    {
        public StatViewModel viewModel;
        public StatWindow(User user)
        {
            InitializeComponent();
            viewModel = new StatViewModel(user);

            DataContext = viewModel;
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Height = 450;
            StatCalendar.Visibility = Visibility.Hidden;

            var FullCollection = new ObservableCollection<GameStat>(viewModel.User.Games);
            viewModel?.Games?.Clear();
            foreach (var game in FullCollection)
            {
                viewModel.Games.Add(game);
            }

            if (ComboBox1.SelectedIndex == 1)
            {
                var Collection = new ObservableCollection<GameStat>(viewModel.User.Games);
                foreach (var game in Collection)
                {
                    if (game.w != viewModel?.User.Login)
                        viewModel?.Games?.Remove(game);
                }
            }
            else 
            if (ComboBox1.SelectedIndex == 2)
            {
                var Collection = new ObservableCollection<GameStat>(viewModel.User.Games);
                foreach (var game in Collection)
                {
                    if (game.w == viewModel?.User.Login)
                        viewModel?.Games?.Remove(game);
                }
            }
            else
            if (ComboBox1.SelectedIndex == 3)
            {
                var Collection = new ObservableCollection<GameStat>(viewModel.User.Games);
                foreach (var game in Collection)
                {
                    if (game.p0 != viewModel?.User.Login)
                        viewModel?.Games?.Remove(game);
                }
            }
            else
            if (ComboBox1.SelectedIndex == 4)
            {
                var Collection = new ObservableCollection<GameStat>(viewModel.User.Games);
                foreach (var game in Collection)
                {
                    if (game.p0 == viewModel?.User.Login)
                        viewModel?.Games?.Remove(game);
                }
            }
            else
            if (ComboBox1.SelectedIndex == 5)
            {
                this.Height = 570;
                StatCalendar.Visibility = Visibility.Visible;
            }
        }

        private void StatCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var FullCollection = new ObservableCollection<GameStat>(viewModel.User.Games);
            viewModel?.Games?.Clear();
            foreach (var game in FullCollection)
            {
                viewModel.Games.Add(game);
            }

            String dateTime = StatCalendar.SelectedDate.Value.ToShortDateString();

            var Collection = new ObservableCollection<GameStat>(viewModel.User.Games);
            foreach (var game in Collection)
            {
                String gameTime = game.Date.ToShortDateString();
                //MessageBox.Show($"{dateTime} - {gameTime}");

                if (dateTime != gameTime) 
                    viewModel?.Games?.Remove(game);
            }
        }
    }


}
