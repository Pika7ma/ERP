using System;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI
{
    public class Utils
    {
        static public DAL.User CurrentUser { set; get; }
        static public MainWindow MainWindow { set; get; }
        static public Window Subwindow { get; set; }
        static public Object TempObject { get; set; }

        static public void OpenMainWindow()
        {
            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        static public void OpenSubwindow(Window window)
        {
            Subwindow = window;
            window.Closed += SwitchToMainWindow;
            Application.Current.MainWindow = window;
            MainWindow.Visibility = Visibility.Hidden;
            window.Show();
        }

        static private void SwitchToMainWindow(object sender, EventArgs e)
        {
            Application.Current.MainWindow = MainWindow;
            MainWindow.Visibility = Visibility.Visible;
            Subwindow.Close();
        }

        static public int? GetTreeViewSelectedValue(ref TreeView tv)
        {
            int? index;
            if (tv.SelectedItem == null)
                index = null;
            else
                index = (int)((TreeViewItem)tv.SelectedItem).DataContext;
            return index;
        }

        public class KeyValue
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}