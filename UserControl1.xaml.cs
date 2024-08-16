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

namespace Autocad_searching_for_layers_15_08_2024
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : Window
    {
        ListSeachLayers dumpPolylines = new ListSeachLayers();
        public UserControl1()
        {
            InitializeComponent();
        }

        private void buttonClearTextbox_Click(object sender, RoutedEventArgs e)
        {
            textSeach.Text = string.Empty;
            WinCloseTwo.massSeach = new string[1] { "0" };
        }

        private void buttonClouseWin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            WinCloseTwo.countWin = 0;
        }

        private void buttonSeach_Click(object sender, RoutedEventArgs e)
        {
            WinCloseTwo.massSeach = textSeach.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            dumpPolylines.ListLay();
        }
    }
}
