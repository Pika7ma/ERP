using Lplfw.BLL;
using Lplfw.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Lplfw.UI
{
    /// <summary>
    /// TempWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TempWindow : Window
    {
        private ExcelWriter writer;

        public TempWindow()
        {
            InitializeComponent();
            var _list = new List<Product>
            {
                new Product
                {
                    Status = "生产中",
                    Name="A",
                    Type="X-001"
                },
                new Product
                {
                    Status = "停产",
                    Name="B",
                    Type="X-002"
                },
            };

            dgTest.ItemsSource = _list;
        }

        private void NewExcel(object sender, RoutedEventArgs e)
        {
            writer = new ExcelWriter(ExcelWriter.Type.Product, dgTest.ItemsSource as List<Product>);
            writer.Write();
        }
    }
}
