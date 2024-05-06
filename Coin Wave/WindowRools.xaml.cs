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

namespace Coin_Wave
{
    /// <summary>
    /// Логика взаимодействия для WindowRools.xaml
    /// </summary>
    public partial class WindowRools : Window
    {
        public WindowRools(Window parentWindow)
        {
            InitializeComponent();
            double parentWindowCenterX = parentWindow.Left + parentWindow.Width / 2;
            double parentWindowCenterY = parentWindow.Top + parentWindow.Height / 2;

            // Устанавливаем координаты для текущего окна
            this.Left = parentWindowCenterX - this.Width / 2;
            this.Top = parentWindowCenterY - this.Height / 2;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
