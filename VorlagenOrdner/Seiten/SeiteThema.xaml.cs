using System;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using VorlagenOrdner.Fenster;
using VorlagenOrdner.Klassen;

namespace VorlagenOrdner.Seiten
{
    public partial class SeiteThema : InitSeite
    {
        private string _ExePfad;

        public SeiteThema(ClickNeu clickAuswahl, TJgDrucker drucker, string exePfad)
            : base(clickAuswahl, drucker)
        {
            InitializeComponent();
            _ExePfad = exePfad;
            BilderNamenLaden();
            GridDrucker.DataContext = drucker;
        }

        private void Click_NeuesThema(object sender, RoutedEventArgs e)
        {
            AuswahlClicked(JgItem.ItemAuswahl.Thema);
        }

        private void Click_NeueVorlage(object sender, RoutedEventArgs e)
        {
            AuswahlClicked(JgItem.ItemAuswahl.Vorlage);
        }

        public override JgItem GetDaten() => (TBereichItem)GridDaten.DataContext;

        public override void SetDaten(JgItem value)
        {
            GridDaten.DataContext = value;
            Helper.ComboBilderLaden(JgImage, _ExePfad, (value as TDruckItem).BildName);

            var istSeiteThema = value is TThemaItem;
            BtThema.Visibility = istSeiteThema ? Visibility.Visible : Visibility.Hidden;
            TbUeberschrift.Text = istSeiteThema ? "Thema bearbeiten" : "Vorlage bearbeiten";
            BtLoeschen.Content = istSeiteThema ? "Thema löschen" : "Vorlage löschen";
        }

        private void Click_DropDownOpen(object sender, EventArgs e) => BilderNamenLaden();

        private void BilderNamenLaden()
        {
            var dir = _ExePfad + @"\Bilder\";
            ComboBildNamen.ItemsSource = Directory.GetFiles(dir).Select(s => System.IO.Path.GetFileName(s));
        }

        private void Click_Drucker(object sender, RoutedEventArgs e)
        {
            var form = new DruckerOptionen(JgDrucker);
            if (form.ShowDialog() ?? false)
            {
                JgDrucker.Drucker = form.Drucker;
            }
        }

        private void Click_SelectChange(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            if (cb.SelectedItem != null)
                Helper.ComboBilderLaden(JgImage, _ExePfad, (sender as ComboBox).SelectedItem.ToString());
        }

        private void AnzeigeFehler(string fehlerText, Exception fehler = null)
        {
            if (fehler != null)
                fehlerText += "/nGrund: " + fehler.Message;

            MessageBox.Show(fehlerText, "Warnung!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Click_Drucken(object sender, RoutedEventArgs e)
        {
            var ds = (TDruckItem)GridDaten.DataContext;
            System.Drawing.Image img;

            if (string.IsNullOrWhiteSpace(ds.BildName))
            {
                AnzeigeFehler("Es wurde kein Bild ausgewählt!");
                return;
            }

            if (string.IsNullOrWhiteSpace(JgDrucker.Drucker))
            {
                AnzeigeFehler("Es wurde kein Drucker ausgewählt!");
                return;
            }

            try
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                MemoryStream ms = new MemoryStream();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)JgImage.Source));
                encoder.Save(ms);
                img = System.Drawing.Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                AnzeigeFehler("Fehler bei der Bildkonvertierung", ex);
                return;
            }

            try
            {
                var printerSettings = new PrinterSettings()
                {
                    PrinterName = JgDrucker.Drucker,
                    Copies = ds.AnzahlKopien
                };
                var pageSettings = new PageSettings(printerSettings)
                {
                    Color = false,
                    Landscape = ds.IstQuerformat
                };

                PrintDocument pd = new PrintDocument()
                {
                    DefaultPageSettings = pageSettings,
                    PrinterSettings = printerSettings,
                    DocumentName = "Jg Print",
                };


                pd.PrintPage += (obj, e) =>
                {
                    var loc = new System.Drawing.Point(Convert.ToInt32(ds.RandLinks * 0.254), Convert.ToInt32(ds.RandOben * 0.254));
                    e.Graphics.DrawImage(img, loc);
                };


                pd.Print();
            }
            catch (Exception ex)
            {
                AnzeigeFehler("Fehler beim drucken der Vorlage", ex);
                return;
            }

        }
    }
}