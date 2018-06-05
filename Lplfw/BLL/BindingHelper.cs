using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Lplfw
{
    static public class BindingHelper
    {
        /// <summary>
        /// 将TextBox的Text属性绑定到指定对象的string属性上
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        static public void Binding(this TextBox control, object obj, string property = null)
        {
            var _binding = new Binding();
            _binding.Source = obj;
            if (property != null)
            {
                _binding.Path = new PropertyPath(property);
            }
            control.SetBinding(TextBox.TextProperty, _binding);
        }

        /// <summary>
        /// 将TextBlock的Text属性绑定到指定对象的string属性上
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        static public void Binding(this TextBlock control, object obj, string property = null)
        {
            var _binding = new Binding();
            _binding.Source = obj;
            _binding.Mode = BindingMode.OneWay;
            if (property != null)
            {
                _binding.Path = new PropertyPath(property);
            }
            control.SetBinding(TextBlock.TextProperty, _binding);
        }

        /// <summary>
        /// 将Label的Content属性绑定到指定对象的string属性上
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        static public void Binding(this Label control, object obj, string property = null)
        {
            var _binding = new Binding();
            _binding.Source = obj;
            if (property != null)
            {
                _binding.Path = new PropertyPath(property);
            }
            control.SetBinding(Label.ContentProperty, _binding);
        }

        /// <summary>
        /// 将CheckBox的IsChecked属性绑定到指定对象的bool属性上
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        static public void Binding(this CheckBox control, object obj, string property = null)
        {
            var _binding = new Binding();
            _binding.Source = obj;
            if (property != null)
            {
                _binding.Path = new PropertyPath(property);
            }

            control.SetBinding(CheckBox.IsCheckedProperty, _binding);
        }

        /// <summary>
        /// 将ComboBox的SelectedValue属性绑定到指定对象的属性上
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        static public void Binding(this ComboBox control, object obj, string property = null)
        {
            var _binding = new Binding();
            _binding.Source = obj;
            if (property != null)
            {
                _binding.Path = new PropertyPath(property);
            }

            control.SetBinding(ComboBox.SelectedValueProperty, _binding);
        }

        /// <summary>
        /// 将ComboBox可选项绑定到一个列表
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        static public void BindingList<T>(this ComboBox control, IEnumerable<T> list, string display, string selected)
        {
            control.ItemsSource = list;
            control.DisplayMemberPath = display;
            control.SelectedValuePath = selected;
        }

        /// <summary>
        /// 将DatePicker的SelectedDate属性绑定到指定对象的DateTime属性上
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        static public void Binding(this DatePicker control, object obj, string property = null)
        {
            var _binding = new Binding();
            _binding.Source = obj;
            if (property != null)
            {
                _binding.Path = new PropertyPath(property);
            }
            control.SetBinding(DatePicker.SelectedDateProperty, _binding);
        }
    }
}
