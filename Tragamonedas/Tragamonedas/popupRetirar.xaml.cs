using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Tragamonedas
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        bool retira;
        public Window1(double monto)
        {
            InitializeComponent();
            Closing += OnWindowClosing;
            txtMontoGanado.Text = monto.ToString("N0");
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            //this.Close();
        }
        private void closeWindow()
        {
            this.Close();
        }
        private void btnRetirar_Click(object sender, RoutedEventArgs e)
        {
            retira = true;
            closeWindow();
        }

        private void btnContinuar_Click(object sender, RoutedEventArgs e)
        {
            retira = false;
            closeWindow();
        }
        public bool isRetirar()
        {
            return retira;
        }
    }
}
