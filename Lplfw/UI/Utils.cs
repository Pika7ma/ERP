using System;
using System.Windows;

namespace Lplfw.UI
{
    public class Utils
    {
        static public MainWindow MainWindow { set; get; }
        static public Window Subwindow { get; set; }

        static public void OpenSubwindow(Window window)
        {
            Subwindow = window;
            window.Closed += SwitchToMainWindow;
            Application.Current.MainWindow = window;
            MainWindow.Visibility = Visibility.Hidden;
            window.Show();
        }

        private static void SwitchToMainWindow(object sender, EventArgs e)
        {
            Application.Current.MainWindow = MainWindow;
            MainWindow.Visibility = Visibility.Visible;
            Subwindow.Close();
        }

    }
}