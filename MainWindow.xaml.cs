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
using System.Diagnostics;
using System.Threading;


namespace Solarni_Sistem
{

    public partial class MainWindow : Window
    {
        Random rdm = new Random();

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        private static Object obj = new Object();


        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            Rotiraj_Po_Orbiti(orbita_merkura, merkur, 88);
            Rotiraj_Po_Orbiti(orbita_venere, venera, 225);
            Rotiraj_Po_Orbiti(orbita_zemlje, zemlja, 365);
            Rotiraj_Po_Orbiti(orbita_marsa, mars, 687);
            Rotiraj_Po_Orbiti(orbita_jupitera, jupiter, 12 * 365);
            Rotiraj_Po_Orbiti(orbita_saturna, saturn, 29 * 365);
            Rotiraj_Po_Orbiti(orbita_urana, uran, 84 * 365);
            Rotiraj_Po_Orbiti(orbita_neptuna, neptun, 165 * 365);


        }

        public void Rotiraj_Po_Orbiti(Ellipse orbita, Rectangle planeta, double broj_dana_revolucije)
        {

            double x = Canvas.GetLeft(planeta) + (planeta.Width / 2f);
            double y = Canvas.GetTop(planeta) + (planeta.Height / 2f);


            double theta = Math.Asin(2 * Math.Abs(Canvas.GetTop(orbita) + (orbita.Height / 2) - y) / orbita.Width);

            double step = (2f * orbita.Width) / broj_dana_revolucije * Math.Sin(theta);


            if (y > (Canvas.GetTop(orbita) + (orbita.Height / 2f)))
            {
                x -= step;
                y = orbita.Height / 2;
                double kvadrat = Math.Pow(((x - Canvas.GetLeft(orbita) - (orbita.Width / 2)) / (orbita.Width / 2)), 2);
                if (kvadrat > 1)
                {
                    kvadrat = 1;
                    x = Canvas.GetLeft(orbita);
                    planeta.Tag = step;
                }
                y = y * Math.Sqrt(1 - kvadrat);
                y += Canvas.GetTop(orbita) + (orbita.Height / 2f);
            }
            else if (y < Canvas.GetTop(orbita) + (orbita.Height / 2f))
            {
                x += step;
                y = Convert.ToSingle(orbita.Height / 2f);
                double kvadrat = Math.Pow(((x - Canvas.GetLeft(orbita) - (orbita.Width / 2)) / (orbita.Width / 2)), 2);
                if (kvadrat > 1)
                {
                    x = Canvas.GetLeft(orbita) + orbita.Width;
                    kvadrat = 1;
                    planeta.Tag = step;
                }
                y = y * (-Math.Sqrt(1 - kvadrat));
                y += Canvas.GetTop(orbita) + (orbita.Height / 2f);
            }
            else if (x == Canvas.GetLeft(orbita))
            {

                x += Convert.ToSingle(planeta.Tag);
                y = Convert.ToSingle(orbita.Height / 2f);
                double kvadrat = Math.Pow(((x - Canvas.GetLeft(orbita) - (orbita.Width / 2)) / (orbita.Width / 2)), 2);
                y = y * (-Math.Sqrt(1 - kvadrat));
                y += Canvas.GetTop(orbita) + (orbita.Height / 2f);
            }
            else
            {
                x -= Convert.ToSingle(planeta.Tag);
                y = orbita.Height / 2;
                double kvadrat = Math.Pow(((x - Canvas.GetLeft(orbita) - (orbita.Width / 2)) / (orbita.Width / 2)), 2);
                y = y * Math.Sqrt(1 - kvadrat);
                y += Canvas.GetTop(orbita) + (orbita.Height / 2f);
            }


            Canvas.SetLeft(planeta, x - (planeta.Width / 2f));
            Canvas.SetTop(planeta, y - (planeta.Height / 2f));
            Osvjezi_Velicinu_Planete(x, y, orbita, planeta, theta, broj_dana_revolucije, step);

        }


        public void Osvjezi_Velicinu_Planete(double x_osa, double y_osa, Ellipse orbita, Rectangle planeta, double theta, double broj_dana_revolucije, double step)
        {
            if (x_osa > Canvas.GetLeft(orbita) + (orbita.Width / 2) && x_osa != Canvas.GetLeft(orbita) && x_osa != Canvas.GetLeft(orbita) + orbita.Width && step > planeta.Width / broj_dana_revolucije)
            {
                planeta.Width += planeta.Width / broj_dana_revolucije;
                planeta.Height += planeta.Width / broj_dana_revolucije;
            }
            else if (x_osa < Canvas.GetLeft(orbita) + (orbita.Width / 2) && x_osa != Canvas.GetLeft(orbita) && x_osa != Canvas.GetLeft(orbita) + orbita.Width && step > planeta.Width / broj_dana_revolucije)
            {
                planeta.Width -= planeta.Width / broj_dana_revolucije;
                planeta.Height -= planeta.Width / broj_dana_revolucije;
            }
            if (theta > 0.66 && y_osa > (Canvas.GetTop(orbita) + (orbita.Height / 2f)))
            {
                planeta.Width = Convert.ToSingle(orbita.Tag);
                planeta.Height = Convert.ToSingle(orbita.Tag);
            }
        }

        private void btnSekvencijalno_Click(object sender, RoutedEventArgs e)
        {

            Stopwatch stoperica = new Stopwatch();
            stoperica.Reset();
            stoperica.Start();
            generisiZvjezdice(0.5, 0.5, 100);
            generisiZvjezdice(0.5, 0.5, 10000);
            generisiZvjezdice(1, 1, 700);
            generisiZvjezdice(1.3, 1.3, 600);
            generisiZvjezdice(1.5, 1.5, 400);
            generisiZvjezdice(1.7, 1.7, 300);
            stoperica.Stop();

            txtSekvencijalno.Text = Math.Round(Convert.ToSingle(stoperica.Elapsed.TotalMilliseconds),2).ToString()+" [ms]";
        }

        private void btnParalelno_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stoperica = new Stopwatch();
            stoperica.Reset();
            stoperica.Start();

            Thread t_najmanje = new Thread(() => generisiZvjezdice(0.5, 0.5, 1000));
            t_najmanje.Start();
            Thread t_malo = new Thread(() => generisiZvjezdice(1, 1, 700));
            t_malo.Start();
            Thread t_srednje = new Thread(() => generisiZvjezdice(1.3, 1.3, 600));
            t_srednje.Start();
            Thread t_velike = new Thread(() => generisiZvjezdice(1.5, 1.5, 400));
            t_velike.Start();
            Thread t_najvece = new Thread(() => generisiZvjezdice(1.7, 1.7, 300));
            t_najvece.Start();

            stoperica.Stop();
            txtParalelno.Text = Math.Round(Convert.ToSingle(stoperica.Elapsed.TotalMilliseconds), 2).ToString() + " [ms]";

        }
        public void generisiZvjezdice(double sirina, double visina, int broj_zvijezda)
        {


            System.Windows.Shapes.Rectangle[] rect = new System.Windows.Shapes.Rectangle[300000];


            if (!this.Dispatcher.CheckAccess())
            {
                this.Dispatcher.Invoke(
                new Action(delegate ()

                {

                    for (int i = 0; i < broj_zvijezda; i++)
                    {
                        rect[i] = new System.Windows.Shapes.Rectangle();
                        rect[i].Width = sirina;
                        rect[i].Height = visina;
                        rect[i].Fill = System.Windows.Media.Brushes.LightYellow;
                        rect[i].RadiusX = 90;
                        rect[i].RadiusY = 90;
                        lock (obj)
                        {
                            Canvas.SetLeft(rect[i], rdm.Next(0, 1600));
                            Canvas.SetTop(rect[i], rdm.Next(0, 800));
                            canvasWIndow.Children.Add(rect[i]);
                        }
                        Thread.Sleep(1);
                    }

                }));
            }
            else
            {


                for (int i = 0; i < broj_zvijezda; i++)
                {
                    rect[i] = new System.Windows.Shapes.Rectangle();
                    rect[i].Width = sirina;
                    rect[i].Height = visina;
                    rect[i].Fill = System.Windows.Media.Brushes.LightYellow;
                    rect[i].RadiusX = 90;
                    rect[i].RadiusY = 90;

                    Canvas.SetLeft(rect[i], rdm.Next(0, 1600));
                    Canvas.SetTop(rect[i], rdm.Next(0, 800));
                    canvasWIndow.Children.Add(rect[i]);
                }


            }

        }

    }


}
