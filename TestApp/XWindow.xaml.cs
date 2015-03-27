using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private void ShareWindow_Click(object sender, MouseButtonEventArgs e)
        {
            double screenX = SystemParameters.WorkArea.Width;
            double screenY = SystemParameters.WorkArea.Height;

            ShareWindow share = new ShareWindow();
            if (this.Left + 910 > screenX)
            {
                share.Left = this.Left - 510;
            }
            else
            {
                share.Left = this.Left + 410;
            }
            
            share.Top = this.Top;
            share.Show();
        }

        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ExitWindow window = new ExitWindow();
            EffectBorder.Background = Brushes.Black;
            EffectBorder.Opacity = 0.5;
            window.ShowDialog();
        }

        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NotificationWindow window = new NotificationWindow();
            EffectBorder.Background = Brushes.Black;
            EffectBorder.Opacity = 0.5;
            window.ShowDialog();
        }

        private void UserBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginTextBlock.Visibility = Visibility.Collapsed;
            LoginTextBlock.IsEnabled = false;
            UserMenu.Visibility = Visibility.Visible;
            UserMenu.IsEnabled = true;
        }

        private void Share_MouseDown(object sender, MouseButtonEventArgs e)
        { }

        private void Clip_MouseDown(object sender, MouseButtonEventArgs e)
        { }

        private void Delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShareList.Items.RemoveAt(ShareList.SelectedIndex);
        }
    }
}
