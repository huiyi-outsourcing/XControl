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

        private void NewShare_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NewShareControl.Visibility = Visibility.Visible;
            ShareList.Visibility = Visibility.Collapsed;
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ExitWindow window = new ExitWindow();
            EffectBorder.Background = Brushes.Black;
            EffectBorder.Opacity = 0.5;
            window.ShowDialog();
        }

        private void Detail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShareList.Items.RemoveAt(ShareList.SelectedIndex);
        }
    }
}
