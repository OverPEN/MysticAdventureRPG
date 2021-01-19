using Engine.Factories;
using Engine.Models;
using MysticAdventureRPG.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace MysticAdventureRPG.Views
{
    /// <summary>
    /// Logica di interazione per TradeScreen.xaml
    /// </summary>
    public partial class TradeScreenView : Window
    {
        public GameSessionViewModel Session => DataContext as GameSessionViewModel;

        public TradeScreenView()
        {
            InitializeComponent();
        }

        private void Bt_Vendi_Click(object sender, RoutedEventArgs e)
        {
            GroupedItem groupedItem = ItemFactory.ObtainItem((((FrameworkElement)sender).DataContext as GroupedItem).Item.ItemID, (((FrameworkElement)sender).DataContext as GroupedItem).Quantity);

            if (groupedItem != null)
            {
                ContentPresenter Cell = PlayerInventory.Columns[3].GetCellContent(PlayerInventory.SelectedItem) as ContentPresenter;
                IntegerUpDown QuantityToSell = (IntegerUpDown)Cell.ContentTemplate.FindName("SellQuantitySelector", Cell);

                if (QuantityToSell.Value.GetValueOrDefault() != 0)
                {
                    Session.CurrentPlayer.ReceiveGold(groupedItem.Item.Price * QuantityToSell.Value.GetValueOrDefault());
                    groupedItem.Quantity = (byte)QuantityToSell.Value.GetValueOrDefault();
                    Session.CurrentTrader.AddItemToInventory(groupedItem);
                    Session.CurrentPlayer.RemoveItemFromInventory(groupedItem);
                }
            }
        }

        private void Bt_Compra_Click(object sender, RoutedEventArgs e)
        {
            GroupedItem groupedItem = ItemFactory.ObtainItem((((FrameworkElement)sender).DataContext as GroupedItem).Item.ItemID, (((FrameworkElement)sender).DataContext as GroupedItem).Quantity);

            if (groupedItem != null)
            {
                ContentPresenter Cell = TraderInventory.Columns[3].GetCellContent(((FrameworkElement)sender).DataContext) as ContentPresenter;
                IntegerUpDown QuantityToBuy = (IntegerUpDown)Cell.ContentTemplate.FindName("BuyQuantitySelector", Cell);
                if (QuantityToBuy.Value.GetValueOrDefault() != 0)
                {
                    if (Session.CurrentPlayer.Gold >= groupedItem.Item.Price * QuantityToBuy.Value.GetValueOrDefault())
                    {
                        Session.CurrentPlayer.SpendGold(groupedItem.Item.Price * QuantityToBuy.Value.GetValueOrDefault());
                        groupedItem.Quantity = (byte)QuantityToBuy.Value.GetValueOrDefault();
                        Session.CurrentTrader.RemoveItemFromInventory(groupedItem);
                        Session.CurrentPlayer.AddItemToInventory(groupedItem);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show($"Non hai abbastanza Oro per acquistare {QuantityToBuy.Value.GetValueOrDefault()} {groupedItem.Item.Name}!");
                    }
                } 
            }
        }

        private void Bt_Chiudi_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
