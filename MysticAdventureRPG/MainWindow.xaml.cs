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
            dtContext.GameWindow = this;

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
            YesNoWindow message = new YesNoWindow("Salvataggio Dati di Gioco", "Vuoi salvare i dati di gioco?");
            message.Owner = GetWindow(this);
            message.ShowDialog();

            if (message.ClickedYes)
            {
                dtContext.SaveGame(null);
            }
        }

        internal void SetActiveGameSessionTo(GameSessionViewModel gameSession)
        {
            // Unsubscribe di OnMessageRaised per evitare duplicazione messaggi di gioco
            _messageBroker.OnMessageRaised -= OnGameMessageRaised;

            dtContext = gameSession;
            DataContext = dtContext;

            // Cancello tutti i game messages precedenti
            GameMessages.Document.Blocks.Clear();

            _messageBroker.OnMessageRaised += OnGameMessageRaised;

            dtContext.GameWindow = this;
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
