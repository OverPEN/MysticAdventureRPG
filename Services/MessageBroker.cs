using CommonClasses.Enums;
using CommonClasses.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Services
{
    public class MessageBroker
    {
        private static readonly MessageBroker s_messageBroker = new MessageBroker();

        private MessageBroker()
        {
        }

        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        public static MessageBroker GetInstance()
        {
            return s_messageBroker;
        }

        public void RaiseMessage(string message, GameMessageTypeEnum type)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message, type));
        }

        public void OnGameMessageRaised(object sender, GameMessageEventArgs e, ref RichTextBox gameConsole)
        {
            Paragraph separator = new Paragraph(new Run(""));
            separator.FontSize = 3f;
            Paragraph par = new Paragraph(new Run(e.Message));
            switch (e.Type)
            {
                case GameMessageTypeEnum.Info:
                    par.Foreground = Brushes.DimGray;
                    par.FontWeight = FontWeights.Normal;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontStyle = FontStyles.Italic;
                    par.FontSize = 12f;
                    break;
                case GameMessageTypeEnum.ImportantInfo:
                    par.Foreground = Brushes.Black;
                    par.FontWeight = FontWeights.Normal;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 12.5f;
                    par.TextDecorations = TextDecorations.Underline;
                    break;
                case GameMessageTypeEnum.BattleInfo:
                    par.Foreground = Brushes.Black;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
                case GameMessageTypeEnum.BattleNegative:
                    par.Foreground = Brushes.Red;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
                case GameMessageTypeEnum.BattlePositive:
                    par.Foreground = Brushes.ForestGreen;
                    par.FontWeight = FontWeights.SemiBold;
                    par.FontFamily = new FontFamily("Tahoma");
                    par.FontSize = 13f;
                    break;
            }
            gameConsole.Document.Blocks.Add(separator);
            gameConsole.Document.Blocks.Add(par);
            gameConsole.ScrollToEnd();
        }
    }
}
