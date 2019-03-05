using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSimpleUI.TestWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = Vm = new VM();
        }

        VM Vm = null;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txt.Text = "view text";
            //Vm.TestString = "test string";
            Console.WriteLine(picker.SelectedDateTime);
            Debug.WriteLine($"TestDateTime={Vm.TestDateTime}");
        }
    }

    public class VM : INotifyPropertyChanged
    {
        public VM()
        {
            TestDateTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private DateTime? _TestDateTime;
        public DateTime? TestDateTime
        {
            get { return _TestDateTime; }
            set
            {
                _TestDateTime = value;
                OnPropertyChanged("TestDateTime");
                Debug.WriteLine($"TestDateTime={TestDateTime}");
            }
        }

        private string _TestString;
        public string TestString
        {
            get { return _TestString; }
            set
            {
                _TestString = value;
                OnPropertyChanged("TestString");
            }
        }


    }

}
