using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Lplfw.UI
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image ig = sender as Image;
            if (ig.Tag.ToString() == "close")
            {
                Uri ur = new Uri("images/close1.png", UriKind.Relative);
                BitmapImage bp = new BitmapImage(ur);
                ig.Source = bp;
            }
            else
            {
                Uri ur = new Uri("images/mini1.png", UriKind.Relative);
                BitmapImage bp = new BitmapImage(ur);
                ig.Source = bp;
            }
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image ig = sender as Image;
            if (ig.Tag.ToString() == "close")
            {
                Uri ur = new Uri("images/close0.png", UriKind.Relative);
                BitmapImage bp = new BitmapImage(ur);
                ig.Source = bp;
            }
            else
            {
                Uri ur = new Uri("images/mini0.png", UriKind.Relative);
                BitmapImage bp = new BitmapImage(ur);
                ig.Source = bp;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image ig = sender as Image;
            if (ig.Tag.ToString() == "close")
            {
                this.Close();
            }
            else this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            var _name = txtName.Text;
            var _pass = txtPass.Password;
            if (_name != "" && _pass != "")
            {
                using (var _db = new DAL.ModelContainer())
                {
                    var _temp = _db.UserSet.FirstOrDefault(i => i.Name == _name);
                    if (_temp != null)
                    {
                        String _thepass = DAL.User.Decrypt(_temp.Password);
                        if (_thepass != _pass)
                        {
                            MessageBox.Show("用户名密码错误！");

                        }
                        else
                        {
                            Utils.CurrentUser = _temp;
                            Utils.OpenMainWindow();
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("用户名密码错误！");
                    }
                }
            }
            else
            {
                MessageBox.Show("输入为空！");
            }
        }

        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
