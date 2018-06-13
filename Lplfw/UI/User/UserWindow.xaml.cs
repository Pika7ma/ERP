using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Lplfw.UI.User
{

    public partial class UserWindow : Window
    {
        public List<Utils.KeyValue> PrivilegeModes { get; set; }

        public UserWindow()
        {
            InitializeComponent();
            DataContext = this;
            PrivilegeModes = new List<Utils.KeyValue>
            {
                new Utils.KeyValue { Id = 0, Name = "不可见" },
                new Utils.KeyValue { Id = 1, Name = "只读" },
                new Utils.KeyValue { Id = 2, Name = "可修改" },
            };
            new Thread(new ThreadStart(CheckPrivilege)).Start();
        }

        private void TabRouter(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (tabEditPassword.IsSelected)
                {

                }
                else if (tabUserGroupAdmin.IsSelected)
                {
                    RefreshTvGroups();
                }
                else if (tabUserAdmin.IsSelected)
                {
                    SetUserControls();
                    RefreshTvUserGroups();
                }
            }
        }

        #region 修改密码
        private void ClearControls()
        {
            pwbOld.Password = "";
            pwbNew.Password = "";
            pwbRepeat.Password = "";
        }

        private void CancelEditPassword(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private string txtCheckMessageEditPassword;
        private bool CanSubmitEditPassword()
        {
            var _new = pwbNew.Password;
            var _repeat = pwbRepeat.Password;
            if (_new == null || _new == "")
            {
                txtCheckMessageEditPassword = "请输入新密码";
                return false;
            }
            if (_repeat == null || _repeat == "")
            {
                txtCheckMessageEditPassword = "请重复新密码";
                return false;
            }
            if (_repeat != _new) {
                txtCheckMessageEditPassword = "两次密码不一致";
                return false;
            }
            txtCheckMessageEditPassword = null;
            return true;
        }

        private void SubmitEditPassword(object sender, RoutedEventArgs e)
        {
            var _user = Utils.CurrentUser;
            var _inputedPassword = DAL.User.Encrypt(pwbOld.Password);
            if (_inputedPassword != _user.Password)
            {
                MessageBox.Show("原密码错误！");
                ClearControls();
            }
            else
            {
                if (!CanSubmitEditPassword())
                {
                    MessageBox.Show(txtCheckMessageEditPassword, null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var _newPassword = DAL.User.Encrypt(pwbNew.Password);
                    try
                    {
                        using (var _db = new ModelContainer())
                        {
                            var _old = _db.UserSet.FirstOrDefault(i => i.Id == _user.Id);
                            _old.Password = _newPassword;
                            _db.SaveChanges();
                            Utils.CurrentUser.Password = _newPassword;
                            MessageBox.Show("修改密码成功", null, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("修改密码失败", null, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        #endregion

        #region 用户组管理
        public void RefreshTvGroups()
        {
            tvGroups.Items.Clear();
            UserGroup.SetTree(ref tvGroups);
        }

        private void SelectUserGroupPrivilege(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var _id = Utils.GetTreeViewSelectedValue(ref tvGroups);
            if (_id == null) {
                dgPrivilege.ItemsSource = null;
            }
            else
            {
                dgPrivilege.ItemsSource = PrivilegeView.GetById((int)_id);
            }
        }

        private void NewUserGroup(object sender, RoutedEventArgs e)
        {
            var _win = new NewUserGroup();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                RefreshTvGroups();
            }
        }

        private void DeleteUserGroup(object sender, RoutedEventArgs e)
        {
            var _id = Utils.GetTreeViewSelectedValue(ref tvGroups);
            if (_id == null) return;

            try
            {
                using (var _db = new ModelContainer())
                {
                    var _users = _db.UserSet.Where(i => i.UserGroupId == _id).ToList();
                    if (_users.Count == 0)
                    {
                        var _privileges = _db.UserGroupPrivilegeItemSet.Where(i => i.UserGroupId == _id).ToList();
                        _db.UserGroupPrivilegeItemSet.RemoveRange(_privileges);
                        var _old = _db.UserGroupSet.FirstOrDefault(i => i.Id == _id);
                        _db.UserGroupSet.Remove(_old);
                        _db.SaveChanges();
                        MessageBox.Show("用户组删除成功!", null, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("该用户组下存在用户, 无法删除!", null, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditPrivilege(object sender, RoutedEventArgs e)
        {
            var _privileges = dgPrivilege.ItemsSource as List<PrivilegeView>;
            if (_privileges == null) return;
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _id = Utils.GetTreeViewSelectedValue(ref tvGroups);
                    var _old = _db.UserGroupPrivilegeItemSet.Where(i => i.UserGroupId == _id).ToList();
                    for (var _i = 0; _i < _old.Count; _i++)
                    {
                        var _pid = _old[_i].PrivilegeId;
                        var _newMode = _privileges.FirstOrDefault(i => i.PrivilegeId == _pid);
                        if (_newMode != null)
                        {
                            _old[_i].Mode = _newMode.Mode;
                        }
                    }
                    _db.SaveChanges();
                }
                MessageBox.Show("权限修改成功!", null, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("权限修改失败!", null, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region 用户管理
        public void RefreshTvUserGroups()
        {
            tvUserGroups.Items.Clear();
            UserGroup.SetTree(ref tvUserGroups);
        }

        private void SelectUserGroup(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var _id = Utils.GetTreeViewSelectedValue(ref tvUserGroups);
            if (_id == null)
            {
                dgUsers.ItemsSource = null;
                return;
            }
            else
            {
                using (var _db = new ModelContainer())
                {
                    var _users = _db.UserSet.Where(i => i.UserGroupId == _id).ToList();
                    dgUsers.ItemsSource = _users;
                }
            }
        }

        private void SelectUser(object sender, SelectionChangedEventArgs e)
        {
            var _user = dgUsers.SelectedItem as DAL.User;
            if (_user == null)
            {
                ClearResetPasswordControls();
            }
            else
            {
                txtName.Text = _user.Name;
                txtTel.Text = _user.Tel;
                cbGroup.SelectedValue = _user.UserGroupId;
            }
        }

        private void ClearResetPasswordControls()
        {
            txtName.Text = "";
            txtTel.Text = "";
            cbGroup.SelectedItem = null;
        }

        private void SetUserControls()
        {
            using (var _db = new ModelContainer())
            {
                cbGroup.ItemsSource = _db.UserGroupSet.ToList();
            }
        }

        private void ShowAllUsers(object sender, RoutedEventArgs e)
        {
            var _item = tvUserGroups.SelectedItem as TreeViewItem;
            if(_item != null) _item.IsSelected = false;
            using (var _db = new ModelContainer())
            {
                var _users = _db.UserSet.ToList();
                dgUsers.ItemsSource = _users;
            }
        }

        private void NewUser(object sender, RoutedEventArgs e)
        {
            var _win = new NewUser();
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {
                ShowAllUsers(null, null);
                MessageBox.Show("新建用户成功");
            }
            else {
                MessageBox.Show("新建用户失败");
            }
        }

        private void ResetPassword(object sender, RoutedEventArgs e)
        {
            var _user = dgUsers.SelectedItem as DAL.User;
            if (_user == null) return;
            try
            {
                using (var _db = new ModelContainer())
                {
                    var _old = _db.UserSet.FirstOrDefault(i => i.Id == _user.Id);
                    _old.Password = DAL.User.Encrypt("111111");
                    _db.SaveChanges();
                    MessageBox.Show("密码已重置为 '111111'", null, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("密码重置失败", null, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelResetPassword(object sender, RoutedEventArgs e)
        {
            ClearResetPasswordControls();
        }

        private void SubmitResetPassword(object sender, RoutedEventArgs e)
        {
            var _user = dgUsers.SelectedItem as DAL.User;
            if (_user == null) return;
            var _newName = txtName.Text;
            var _groupId = cbGroup.SelectedValue as int?;
            if (_newName == null || _newName == "")
            {
                MessageBox.Show("请输入新的用户名");
            }
            else if (_groupId == null)
            {
                MessageBox.Show("请选择用户组");
            }
            else
            {
                try
                {
                    using (var _db = new ModelContainer())
                    {
                        var _exist = _db.UserSet.FirstOrDefault(i => i.Name == _newName);
                        if (_exist != null)
                        {
                            MessageBox.Show("该用户名已存在");
                        }
                        else
                        {
                            var _old = _db.UserSet.FirstOrDefault(i => i.Id == _user.Id);
                            _old.Name = _newName;
                            _old.Tel = txtTel.Text;
                            _old.UserGroupId = (int)_groupId;
                            MessageBox.Show("用户信息修改成功");
                        }
                    }
                } catch (Exception)
                {
                    MessageBox.Show("用户信息修改失败");
                }
            }
        }
        #endregion

        #region 权限控制
        private void CheckPrivilege()
        {
            using (var _db = new ModelContainer())
            {
                var _temp = _db.UserGroupPrivilegeItemSet.First(i => i.PrivilegeId == 6 && i.UserGroupId == Utils.CurrentUser.UserGroupId);
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    if (_temp.Mode == 1)
                    {
                        OnlyRead();
                    }
                    else if (_temp.Mode == 0)
                    {
                        CannotRead();
                    }
                });
            }
        }
        /// <summary>
        /// 只读
        /// </summary>
        private void OnlyRead()
        {
            btnNewGroup.Visibility = Visibility.Hidden;
            btnDelGroup.Visibility = Visibility.Hidden;
            btnEditPrivilege.Visibility = Visibility.Hidden;
            btnNewUser.Visibility = Visibility.Hidden;
            txtName.Visibility = Visibility.Hidden;
            txtTel.Visibility = Visibility.Hidden;
            cbGroup.Visibility = Visibility.Hidden;
            btnResetPassword.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            btnConfrim.Visibility = Visibility.Hidden;
        }

        private void CannotRead()
        {
            tabUserGroupAdmin.Visibility = Visibility.Hidden;
            tabUserAdmin.Visibility = Visibility.Hidden;
            Width = 340;
            Height = 320;
        }

        #endregion
    }
}
