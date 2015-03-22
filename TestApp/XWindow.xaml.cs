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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for XWindow.xaml
    /// </summary>
    public partial class XWindow : Window
    {
        public XWindow()
        {
            InitializeComponent();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Share_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Closing(object sender, MouseButtonEventArgs e)
        {
            ExitWindow window = new ExitWindow();
            EffectBorder.Background = Brushes.Black;
            EffectBorder.Opacity = 0.5;
            window.ShowDialog();
        }

        private void Window_Minimizing(object sender, MouseButtonEventArgs e)
        {
            NotificationWindow window = new NotificationWindow();
            EffectBorder.Background = Brushes.Black;
            EffectBorder.Opacity = 0.5;
            window.ShowDialog();
        }
    }
}
