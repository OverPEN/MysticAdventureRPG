using CommonClasses.Enums;
using CommonClasses.EventArgs;
using Engine.Models;
using MysticAdventureRPG.ViewModels;
using MysticAdventureRPG.Views;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MysticAdventureRPG
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameSessionViewModel dtContext;
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        public MainWindow()
        {
            InitializeComponent();

            dtContext = DataContext as GameSessionViewModel;

            _messageBroker.OnMessageRaised += OnGameMessageRaised;
        }

        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            _messageBroker.OnGameMessageRaised(sender, e, ref GameMessages);
        }

        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            dtContext.CraftItemUsing(recipe);
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            dtContext.ExecuteFromKeyboard(e.Key, ref PlayerTabControl);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            SavePlayerService.SavePlayer(dtContext.CurrentPlayer);
            SaveWorldService.SaveWorld(dtContext.CurrentWorld);
        }
    }
}
