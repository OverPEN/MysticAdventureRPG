using CommonClasses.Enums;
using CommonClasses.EventArgs;
using MysticAdventureRPG.ViewModels;
using MysticAdventureRPG.Views;
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
        public MainWindow()
        {
            InitializeComponent();

            GameSessionViewModel dtContext = DataContext as GameSessionViewModel;

            dtContext.OnMessageRaised += OnGameMessageRaised;
        }

        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            Paragraph separator = new Paragraph(new Run(""));
            separator.FontSize = 2.5f;
            Paragraph par = new Paragraph(new Run(e.Message));
            switch (e.Type)
            {
                case GameMessageType.Info:                  
                    par.Foreground = Brushes.DimGray;
                    par.FontWeight = FontWeights.Normal;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontStyle = FontStyles.Italic;
                    par.FontSize = 12f;                   
                    break;
                case GameMessageType.ImportantInfo:
                    par.Foreground = Brushes.Black;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 12f;
                    break;
                case GameMessageType.BattleInfo:
                    par.Foreground = Brushes.Black;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
                case GameMessageType.BattleNegative:
                    par.Foreground = Brushes.Red;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
                case GameMessageType.BattlePositive:
                    par.Foreground = Brushes.ForestGreen;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
            }
            GameMessages.Document.Blocks.Add(separator);
            GameMessages.Document.Blocks.Add(par);
            GameMessages.ScrollToEnd();
        }       
    }
}
