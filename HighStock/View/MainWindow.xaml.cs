using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HighStock.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.MainViewModel();
        }
        //private void Window_Loaded(object sender, RoutedEventArgs e) {
        //    System.Windows.Forms.Integration.WindowsFormsHost host =
        //        new System.Windows.Forms.Integration.WindowsFormsHost();
        //    //KHAxLib.KHAxControl khAxCtr = new KHAxLib.KHAxControl();
        //    //host.Child = khAxCtr;
        //    //this.SPanel.Children.Add(host);
        //    //if (khAxCtr.CommConnect() == 0) {
        //    //    // Connection success
        //    //} else {
        //    //    // Connection fail
        //    //}
        //}
    }
    
}
