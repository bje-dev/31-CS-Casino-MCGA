using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Newtonsoft.Json;
using Tragamonedas.Dominio;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Tragamonedas
{
    public partial class MainWindow : Window
    {
        Window loginForm;
        double _maximo;
        double _minimo;
        double totalInt;
        int ficha1, ficha2, ficha3;
        Terminal t = new Terminal();
        DispatcherTimer temp;
        private MediaPlayer mediaPlayer;

        // Rutas para los archivos de audio de ganador y perdedor
        private string audioAmbiente = @"..\..\audioAmbiente.mp3";
        private string audioGanador = @"..\..\audioGanador.mp3";
        private string audioPerdedor = @"..\..\audioPerdedor.mp3";

        public MainWindow(Window previous, string nroTerminal, string nroCasino)
        {
            // Configura la reproducción del archivo de audio
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(audioAmbiente, UriKind.RelativeOrAbsolute));
            mediaPlayer.Volume = 0.7;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            // Inicia la reproducción
            mediaPlayer.Play();

            loginForm = previous;
            t.nroCasino = Int32.Parse(nroCasino);
            t.nroMaquina = Int32.Parse(nroTerminal);
            InitializeComponent();
            Closing += OnWindowClosing;
            actualizarTerminal();
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            // Reinicia la reproducción cuando el archivo llega al final
            mediaPlayer.Position = TimeSpan.Zero;
            mediaPlayer.Play();
        }

        private void startThreadUpdate()
        {
            int startin = 50 - DateTime.Now.Second;
            var t = new System.Threading.Timer(o => actualizarTerminal(),
                 null, startin * 1000, 60000);
        }

        private void actualizarTerminal()
        {
            t.accion = "actualizarTerminal";
            var jsonRequest = JSONbuilder.serialize(t);
            var newMaxMin = caller.ExecuteClient(jsonRequest);
            t = JsonConvert.DeserializeObject<Terminal>(newMaxMin);
            txtboxminimo.Text = t.minimo.ToString("N0");
            txtboxmaximo.Text = t.maximo.ToString("N0");
            _minimo = t.minimo;
            _maximo = t.maximo;
            if (t.accion == "FINALIZADA")
            {
                MessageBox.Show("La terminal se encuentra fuera de servicio");
                MessageBox.Show("Se retira el monto completo");
                MessageBox.Show("Gracias por jugar");
                retirarTotal();
                this.Close();
            }
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            string jsonRequest = "";

            t.accion = "SESIONCERRADA";
            jsonRequest = JSONbuilder.serialize(t);
            caller.ExecuteClient(jsonRequest);
            mediaPlayer.Stop();
            loginForm.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double apuesta = 0;
            actualizarTerminal();
            ComboBoxItem typeItem = (ComboBoxItem)comboIngresar.SelectedItem;
            if (typeItem != null)
            {
                string value = typeItem.Content.ToString();
                double totalInt = 0;
                try
                {
                    totalInt = Convert.ToDouble(total.Text);
                }
                catch
                {
                }
                double apuestaInt = Convert.ToDouble(value);
                totalInt += apuestaInt;
                total.Text = totalInt.ToString("N0");
                apuesta = t.apuesta;
                t.accion = "deposito";
                t.apuesta = apuestaInt;
                var jsonRequest = JSONbuilder.serialize(t);
                caller.ExecuteClient(jsonRequest);
                t.apuesta = apuesta;
            }
        }

        private void btnRetirar_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(total.Text) > 0)
            {
                double retiro = retirarTotal();
                actualizarTerminal();
            }
        }

        private double retirarTotal()
        {
            double retiro = Convert.ToDouble(total.Text);
            total.Text = "0";
            t.accion = "retirarGanancias";
            t.apuesta = retiro;
            var jsonRequest = JSONbuilder.serialize(t);
            caller.ExecuteClient(jsonRequest);
            return retiro;
        }

        private double retirarGanancias(string ganancias)
        {
            double retiro = 0;
            double apuesta = 0;
            t.accion = "retirarGanancias";
            apuesta = t.apuesta;
            t.apuesta = Double.Parse(ganancias);
            var jsonRequest = JSONbuilder.serialize(t);
            caller.ExecuteClient(jsonRequest);
            t.apuesta = apuesta;
            retiro = Double.Parse(ganancias);
            return retiro;
        }

        private void btnPlay_Click_1(object sender, RoutedEventArgs e)
        {
            double creditoDisponible = 0;
            actualizarTerminal();
            ComboBoxItem typeItem = (ComboBoxItem)combo1.SelectedItem;
            if (typeItem != null)
            {
                totalInt = Convert.ToDouble(typeItem.Content);
            }
            else
            {
                totalInt = 0;
            }
            try
            {
                creditoDisponible = Double.Parse(total.Text);
            }
            catch
            {
                creditoDisponible = 0;
            }
            if (creditoDisponible > 0)
            {
                if (creditoDisponible >= totalInt)
                {
                    if (totalInt > 0)
                    {
                        if ((totalInt >= _minimo) && (totalInt <= _maximo))
                        {
                            temp = new DispatcherTimer();
                            temp.Interval = TimeSpan.FromMilliseconds(1600);
                            temp.Tick += Temp_Tick;
                            temp.Start();

                            Storyboard sb = (Storyboard)FindResource("Storyboard1");
                            sb.Begin();
                        }
                        else
                        {
                            MessageBox.Show("El valor a apostar no esta dentro del rango minimo/maximo. Por favor vuelva a intentarlo.");
                        }
                    }
                }
            }
        }

        private void Temp_Tick(object sender, EventArgs e)
        {
            bool distintos;
            string jsonRequest = "";
            Random r = new Random();
            BitmapImage[] pictures = new BitmapImage[6];
            pictures[0] = new BitmapImage(uriSource: new Uri(@"pack://application:,,,/Tragamonedas;component/1.jpg", UriKind.RelativeOrAbsolute));
            pictures[1] = new BitmapImage(uriSource: new Uri(@"pack://application:,,,/Tragamonedas;component/2.jpg", UriKind.RelativeOrAbsolute));
            pictures[2] = new BitmapImage(uriSource: new Uri(@"pack://application:,,,/Tragamonedas;component/3.jpg", UriKind.RelativeOrAbsolute));
            pictures[3] = new BitmapImage(uriSource: new Uri(@"pack://application:,,,/Tragamonedas;component/4.jpg", UriKind.RelativeOrAbsolute));
            pictures[4] = new BitmapImage(uriSource: new Uri(@"pack://application:,,,/Tragamonedas;component/5.jpg", UriKind.RelativeOrAbsolute));
            pictures[5] = new BitmapImage(uriSource: new Uri(@"pack://application:,,,/Tragamonedas;component/6.jpg", UriKind.RelativeOrAbsolute));

            t.accion = "jugada";
            t.apuesta = totalInt;
            jsonRequest = JSONbuilder.serialize(t);
            var response = caller.ExecuteClient(jsonRequest);
            if (response.Contains("true"))
            {
                ficha1 = r.Next(1, 6);
                ficha2 = ficha1;
                ficha3 = ficha2;
            }
            else
            {
                ficha1 = r.Next(1, 6);
                ficha2 = r.Next(1, 6);
                ficha3 = r.Next(1, 6);

                if ((ficha1 == ficha2) || (ficha1 == ficha3))
                {
                    distintos = false;
                }
                else
                {
                    distintos = true;
                }

                while (distintos == false)
                {
                    ficha1 = r.Next(1, 6);
                    if ((ficha1 != ficha2))
                    {
                        distintos = true;
                    }
                }
            }

            image1.Source = pictures[ficha1];
            image2.Source = pictures[ficha2];
            image3.Source = pictures[ficha3];

            if (ficha1 == ficha2 && ficha2 == ficha3)
            {
                Storyboard gn = (Storyboard)FindResource("Storyboard2");
                gn.Begin();
                ReproducirArchivoAudio(audioGanador);

                t.accion = "apuestaGanador";
                jsonRequest = JSONbuilder.serialize(t);
                var newTotal = caller.ExecuteClient(jsonRequest);
                Window1 popupRetirar = new Window1(Double.Parse(newTotal));
                popupRetirar.ShowDialog();
                if (popupRetirar.isRetirar())
                {
                    retirarGanancias(newTotal);
                }
                else
                {
                    total.Text = (Double.Parse(total.Text) + Double.Parse(newTotal)).ToString("N0");
                }
            }
            else
            {
                ReproducirArchivoAudio(audioPerdedor);
                Storyboard pd = (Storyboard)FindResource("Storyboard3");
                pd.Begin();
                t.accion = "apuestaPerdedor";
                jsonRequest = JSONbuilder.serialize(t);
                var newTotal = caller.ExecuteClient(jsonRequest);
                total.Text = (Double.Parse(total.Text) - totalInt).ToString("N0");
            }


            // Agregar un temporizador para esperar antes de reiniciar el audio ambiente
            var waitTimer = new DispatcherTimer();
            waitTimer.Interval = TimeSpan.FromSeconds(2); // Ajusta el tiempo de espera según sea necesario
            waitTimer.Tick += (s, args) =>
            {
                ReproducirArchivoAudio(audioAmbiente);
                waitTimer.Stop();
            };
            waitTimer.Start();

            temp.Stop();
        }

        // Método para reproducir archivos de audio
        private void ReproducirArchivoAudio(string rutaArchivo)
        {
            mediaPlayer.Stop();
            mediaPlayer.Open(new Uri(rutaArchivo, UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
        }
    }
}
