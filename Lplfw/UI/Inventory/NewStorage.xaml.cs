using Lplfw.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Lplfw.UI.Inventory
{
    /// <summary>
    /// NewStorage.xaml 的交互逻辑
    /// </summary>
    public partial class NewStorage : Window
    {
        private StorageViewModel storage;
        public NewStorage()
        {
            InitializeComponent();
            Title = "新建仓库";
            storage = new StorageViewModel();
            btnConfirm.Click += SubmitNewStorage;
            BindingControls();
        }

        public NewStorage(StorageView storageView)
        {
            InitializeComponent();
            Title = "修改仓库";
            storage = new StorageViewModel(new Storage {
                Id = storageView.Id,
                Name = storageView.Name,
                UserId = storageView.UserId,
                Location = storageView.Location,
                Description = storageView.Description
            });
            btnConfirm.Click += SubmitEditStorage;
            BindingControls();
        }

        private void BindingControls()
        {
            using (var _db = new ModelContainer())
            {
                cbUser.ItemsSource = _db.UserSet.ToList();
            }
            txtName.Binding(storage, "TxtName");
            cbUser.Binding(storage, "CbUser");
            txtLocation.Binding(storage, "TxtLocation");
            txtDescription.Binding(storage, "TxtDescription");
        }

        private void SubmitNewStorage(object sender, RoutedEventArgs e)
        {
            if (storage.CanSubmit)
            {
                var _rtn = storage.CreateNew();
                if (_rtn == true)
                {
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("新建仓库失败!", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtMessage.Text = storage.CheckMessage;
            }

        }

        private void SubmitEditStorage(object sender, RoutedEventArgs e)
        {
            if (storage.CanSubmit)
            {
                var _rtn = storage.SaveChanges();
                if (_rtn == true)
                {
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("修改仓库失败!", null, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                txtMessage.Text = storage.CheckMessage;
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
