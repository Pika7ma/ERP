using Lplfw.DAL;
using System;
using System.Linq;
using System.Threading;
using System.Windows;


namespace Lplfw.UI.User
{
    /// <summary>
    /// NewUser.xaml 的交互逻辑
    /// </summary>
    public partial class NewUser : Window
    {
        private UserViewModel user;

        public NewUser()
        {
            InitializeComponent();
            user = new UserViewModel();
            SetControls();
        }

        private void SetControls()
        {
            cbGroup.Binding(user, "CbGroup");
            txtName.Binding(user, "TxtName");
            txtPassword.Binding(user, "TxtPassword");
            txtTel.Binding(user, "TxtTel");
            new Thread(new ThreadStart(ReadGroupsThread)).Start();
        }

        private void ReadGroupsThread()
        {
            using (var _db = new ModelContainer())
            {
                var _userGroups = _db.UserGroupSet.ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbGroup.ItemsSource = _userGroups;
                    cbGroup.SelectedIndex = 0;
                });
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (user.CanSubmit)
            {
                var _rtn = user.CreateNew();
                DialogResult = _rtn;
            }
            else
            {
                txtMessage.Text = user.TxtCheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}