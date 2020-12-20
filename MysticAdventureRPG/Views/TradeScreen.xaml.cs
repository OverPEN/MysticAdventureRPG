using Engine.Factories;
using Engine.Models;
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
using Xceed.Wpf.Toolkit;

namespace MysticAdventureRPG.Views
{
    /// <summary>
    /// Logica di interazione per TradeScreen.xaml
    /// </summary>
    public partial class TradeScreen : Window
    {
        public GameSessionViewModel Session => DataContext as GameSessionViewModel;

        public TradeScreen()
        {
            InitializeComponent();
        }

        private void Bt_Vendi_Click(object sender, RoutedEventArgs e)
        {
            Item item = ItemFactory.ObtainItem((((FrameworkElement)sender).DataContext as Item).ItemID, (((FrameworkElement)sender).DataContext as Item).Quantity);

            if (item != null)
            {
                ContentPresenter Cell = PlayerInventory.Columns[3].GetCellContent(PlayerInventory.SelectedItem) as ContentPresenter;
                IntegerUpDown QuantityToSell = (IntegerUpDown)Cell.ContentTemplate.FindName("SellQuantitySelector", Cell);

                Session.CurrentPlayer.ReceiveGold(item.Price * QuantityToSell.Value.GetValueOrDefault());
                item.Quantity = (byte)QuantityToSell.Value.GetValueOrDefault();
                Session.CurrentTrader.AddItemToInventory(item);
                Session.CurrentPlayer.RemoveItemFromInventory(item);
            }
        }

        private void Bt_Compra_Click(object sender, RoutedEventArgs e)
        {
            Item item = ItemFactory.ObtainItem((((FrameworkElement)sender).DataContext as Item).ItemID, (((FrameworkElement)sender).DataContext as Item).Quantity);

            if (item != null)
            {
                ContentPresenter Cell = TraderInventory.Columns[3].GetCellContent(((FrameworkElement)sender).DataContext) as ContentPresenter;
                IntegerUpDown QuantityToBuy = (IntegerUpDown)Cell.ContentTemplate.FindName("BuyQuantitySelector", Cell);

                if (Session.CurrentPlayer.Gold >= item.Price * QuantityToBuy.Value.GetValueOrDefault())
                {
                    Session.CurrentPlayer.SpendGold(item.Price * QuantityToBuy.Value.GetValueOrDefault());
                    item.Quantity = (byte)QuantityToBuy.Value.GetValueOrDefault();
                    Session.CurrentTrader.RemoveItemFromInventory(item);
                    Session.CurrentPlayer.AddItemToInventory(item);
                }
                else
                {
                    System.Windows.MessageBox.Show($"Non hai abbastanza Oro per acquistare {QuantityToBuy.Value.GetValueOrDefault()} {item.Name}!");
                }
            }
        }

        private void Bt_Chiudi_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
