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
using System.Windows.Shapes;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for ShareWindow.xaml
    /// </summary>
    public partial class ShareWindow : Window
    {
        public ShareWindow()
        {
            InitializeComponent();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
