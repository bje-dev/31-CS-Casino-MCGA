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
using Tragamonedas.Dominio;

namespace Tragamonedas
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string _nro_terminal;
        private string _nro_casino;
        Terminal t = new Terminal();
        public Login()
        {
            InitializeComponent();
            // Agrega el manejador de eventos PreviewTextInput a los TextBox
            txtboxmaq.PreviewTextInput += TextBox_PreviewTextInput;
            txtboxcas.PreviewTextInput += TextBox_PreviewTextInput;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string ok;
            _nro_terminal = txtboxmaq.Text;
            _nro_casino = txtboxcas.Text;

            //Hacer el llamado al backoffice y comprobar que este habilitado
            //Si esta OK
            try
            {
                t.nroMaquina = Int32.Parse(_nro_terminal);
                t.nroCasino = Int32.Parse(_nro_casino);
                t.accion = "login";
                var jsonRequest = JSONbuilder.serialize(t);
                ok = caller.ExecuteClient(jsonRequest);

                if (ok.Contains("true"))
                {
                    MainWindow m = new MainWindow(this, _nro_terminal, _nro_casino);
                    this.Hide();
                    m.ShowDialog();
                }
                else
                {
                    MessageBox.Show("La terminal y/o Casino No son Válidos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectarse al servidor!");
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verificar si el texto introducido es numérico
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Si no es numérico, manejar el evento para evitar la entrada
            }
        }

        // Función para verificar si una cadena es numérica
        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
    }
}
