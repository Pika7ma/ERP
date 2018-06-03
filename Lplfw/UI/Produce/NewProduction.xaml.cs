using System;
using System.Linq;
using System.Windows;
using Lplfw.DAL;
using System.Threading;

namespace Lplfw.UI.Produce
{
    /// <summary>
    /// NewProduction.xaml 的交互逻辑
    /// </summary>
    public partial class NewProduction : Window
    {
        public NewProduction()
        {
            InitializeComponent();
        }

        private void SetControlSource(object sender, RoutedEventArgs e)
        {
            new Thread(new ThreadStart(InitProductThread)).Start();
            new Thread(new ThreadStart(InitAssemblyLineThread)).Start();
            new Thread(new ThreadStart(InitRequsitionThread)).Start();
        }

        private void InitProductThread()
        {
            using (var _db = new ModelContainer())
            {
                var _list = _db.ProductSet.ToList();
                Dispatcher.BeginInvoke((Action) delegate()
                {
                    cbProduct.ItemsSource = _list;
                });
            }
        }

        private void InitAssemblyLineThread()
        {
            using (var _db = new ModelContainer())
            {
                var _list = _db.AssemblyLineSet.ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbAssemblyline.ItemsSource = _list;
                });
            }
        }

        private void InitRequsitionThread()
        {
            using (var _db = new ModelContainer())
            {
                var _list = _db.RequisitionSet.ToList();
                Dispatcher.BeginInvoke((Action)delegate ()
                {
                    cbRequsition.ItemsSource = _list;
                });
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            using (var _db = new ModelContainer())
            {
                var _production = new Production
                {
                    ProductId = (int)cbProduct.SelectedValue,
                    RequisitionId = (int)cbRequsition.SelectedValue,
                    AssemblyLineId = (int)cbAssemblyline.SelectedValue,
                    Status = "未质检",
                    StartAt = (DateTime)dtStartAt.SelectedDate,
                    ThinkFinishedAt = (DateTime)dtThinkFinishedAt.SelectedDate,
                    FinishedAt = null,
                    Description = txtDescription.Text
                };
                _db.ProductionSet.Add(_production);
                _db.SaveChanges();
            }
            DialogResult = true;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
