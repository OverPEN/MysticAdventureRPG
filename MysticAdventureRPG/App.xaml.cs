using Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MysticAdventureRPG
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string exceptionMessageText =
                $"Errore durante l'esecuzione: {e.Exception.Message}\r\n\r\nat: {e.Exception.StackTrace}";

            LoggingService.Log(e.Exception);

            // TODO: Create a Window to display the exception information.
            MessageBox.Show(exceptionMessageText, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            this.Properties["SAVE_GAME_FILE_EXTENSION"] = "marpg";
            this.Properties["SAVE_GAME_FILES_FOLDER"] = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SaveFiles");
        }
    }
}
