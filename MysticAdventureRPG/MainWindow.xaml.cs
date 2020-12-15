using CommonClasses.Enums;
using CommonClasses.EventArgs;
using MysticAdventureRPG.ViewModels;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

            GameSessionViewModel dtContext = DataContext as GameSessionViewModel;

            dtContext.OnMessageRaised += OnGameMessageRaised;
        }

        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            Paragraph par = new Paragraph(new Run(e.Message));
            switch (e.Type)
            {
                case GameMessageType.Info:                  
                    par.Foreground = Brushes.Black;
                    par.FontWeight = FontWeights.Normal;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 18f;
                    GameMessages.Document.Blocks.Add(par);
                    break;
                case GameMessageType.Encounter:
                    par.Foreground = Brushes.OrangeRed;
                    par.FontWeight = FontWeights.Bold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 18f;
                    GameMessages.Document.Blocks.Add(par);
                    break;
                case GameMessageType.DamageSuffer:
                    par.Foreground = Brushes.Red;
                    par.FontWeight = FontWeights.Bold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 18f;
                    GameMessages.Document.Blocks.Add(par);
                    break;
                case GameMessageType.DamageDeal:
                    par.Foreground = Brushes.DarkGreen;
                    par.FontWeight = FontWeights.Medium;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 18f;
                    GameMessages.Document.Blocks.Add(par);
                    break;
            }
            
            GameMessages.ScrollToEnd();
        }
    }
}
