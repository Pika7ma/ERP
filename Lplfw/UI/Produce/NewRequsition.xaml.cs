using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Lplfw.DAL;


namespace Lplfw.UI.Produce
{
    /// <summary>
    /// NewRequsition.xaml 的交互逻辑
    /// </summary>
    public partial class NewRequsition : Window
    {
        public bool type { set; get; }
        public int id { set; get; }
        private List<MaterialItems> materialitemslist = new List<MaterialItems>();
        public NewRequsition(bool isRequsition)
        {
            InitializeComponent();
            if (isRequsition)
            {
                Title = "新建借料单";
            }
            else
            {
                Title = "新建还料单";
            }
        }
        public void MaterialItemsListAdd(string materialname, int amount)
        {
            MaterialItems newitem = new MaterialItems();
            newitem.Name = materialname;
            newitem.Quantity = amount;
            materialitemslist.Add(newitem);
            dgSalesItems.ItemsSource = null;
            dgSalesItems.ItemsSource = materialitemslist;
        }

        private void NewItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewRequsitionItem(isNew: true);
            _win.TransfEvent += MaterialItemsListAdd;
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
        public void MaterialItemsListEdit(String materialname, int amount)
        {
            foreach (var _item in materialitemslist)
            {
                if (_item.Name == materialname)
                {
                    _item.Quantity = amount;
                    dgSalesItems.ItemsSource = null;
                    dgSalesItems.ItemsSource = materialitemslist;
                    break;
                }
            }
        }
        private void EditItem(object sender, RoutedEventArgs e)
        {
            var _win = new NewRequsitionItem(isNew: false);
            _win.TransfEvent += MaterialItemsListEdit;
            var _rtn = _win.ShowDialog();
            if (_rtn == true)
            {

            }
        }
        /// <summary>
        /// 绑定确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sure(object sender, RoutedEventArgs e)
        {
            using (var db = new ModelContainer())
            {
                if (type)
                {
                    Requisition req = new Requisition();
                    string assemname = this.AssemblyLineCombo.Text.ToString();
                    var assem = db.AssemblyLineSet.Where(i => i.Name == assemname).ToList();
                    req.AssemblyLineId = assem.First().Id;
                    req.UserId = 1;                //待定，之后需改为Utils.CurrentUser
                    req.Description = this.descriptionText.Text.ToString();
                    req.CreateAt = DateTime.Now;
                    req.FinishedAt = DateTime.Now.AddDays(2.3).AddHours(10.1);
                    req.Status = "unreceived";
                    db.RequisitionSet.Add(req);
                    db.SaveChanges();
                    var _requistition = db.RequisitionSet.ToList().Last();
                    var _rid = _requistition.Id;
                    foreach (var _item in materialitemslist)
                    {
                        RequisitionItem reqitem = new RequisitionItem();
                        reqitem.RequisitionId = _rid;
                        reqitem.Quantity = _item.Quantity;
                        var _materials = db.MaterialSet.Where(i => i.Name == _item.Name).ToList();
                        reqitem.MaterialId = _materials.First().Id;
                        var _materialstock = db.MaterialStockSet.Where(i => i.MaterialId == reqitem.MaterialId).ToList();
                        reqitem.StorageId = _materialstock.First().StorageId;
                        db.RequisitionItemSet.Add(reqitem);
                    }
                    db.SaveChanges();
                }
                else
                {
                    Return ret = new Return();
                    ret.Status = "Borrowed";
                    ret.RequisitionId = 3;
                    ret.UserId = 1;
                    ret.CreateAt = DateTime.Now;
                    ret.FinishedAt = DateTime.Now.AddDays(2.3).AddHours(10.1);
                    ret.Description = this.descriptionText.Text.ToString();
                    db.ReturnSet.Add(ret);
                    db.SaveChanges();
                    foreach (var _item in materialitemslist)
                    {
                        ReturnItem _newretitem = new ReturnItem();
                        _newretitem.ReturnId = db.ReturnSet.ToList().Last().Id;
                        _newretitem.Quantity = _item.Quantity;
                        var _materials = db.MaterialSet.Where(i => i.Name == _item.Name).ToList();
                        _newretitem.MaterialId = _materials.First().Id;
                        var _materialstocks = db.MaterialStockSet.Where(i => i.MaterialId == _newretitem.MaterialId).ToList();
                        _newretitem.StorageId = _materialstocks.First().StorageId;
                        db.ReturnItemSet.Add(_newretitem);
                    }
                    db.SaveChanges();
                }
            }
            this.DialogResult = true;
        }
        /// <summary>
        /// 绑定取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotSure(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 界面初始化时初始化combobox值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<ComBoxData> list = new List<ComBoxData>();
            using (var db = new ModelContainer())
            {
                var _assemblylinelist = db.AssemblyLineSet.ToList();
                int i = 0;
                foreach (var _assemblyline in _assemblylinelist)
                {
                    i += 1;
                    list.Add(new ComBoxData { Id = i, Name = _assemblyline.Name });
                }
            }
            this.AssemblyLineCombo.ItemsSource = list;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (dgSalesItems.SelectedItems.Count > 0)
            {
                var item = (MaterialItems)dgSalesItems.SelectedItems[0];
                foreach (var _item in materialitemslist)
                {
                    if (_item.Name == item.Name)
                    {
                        materialitemslist.Remove(_item);
                        dgSalesItems.ItemsSource = null;
                        dgSalesItems.ItemsSource = materialitemslist;
                        break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// ComBox 要添加的值的格式
    /// </summary>
    public class ComBoxData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// datagrid 要显示的值的格式
    /// </summary>
    public class MaterialItems
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}