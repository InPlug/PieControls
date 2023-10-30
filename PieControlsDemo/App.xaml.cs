using NetEti.ApplicationControl;
using System;
using System.Windows;

namespace PieControlsDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger? _logger;

        /// <summary>
        /// Wird beim Start der Anwendung durchlaufen.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            App._logger = new Logger(false, "#PieControl#");
            InfoController.GetInfoSource().RegisterInfoReceiver(App._logger, InfoTypes.Collection2InfoTypeArray(InfoTypes.All));
        }
        // Wird in jedem Fall beim Beenden von Vishnu durchlaufen.
        // Versucht noch, Aufräumarbeiten auszuführen, endet aber u.U. abrupt
        // ohne fertig zu werden.
        private static void OnProcessExit(object? sender, EventArgs e)
        {
            App._logger?.Dispose();
        }

    }
}
