using System;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using VorlagenOrdner.Klassen;
using VorlagenOrdner.Seiten;

namespace VorlagenOrdner
{
    public partial class MainWindow : Window
    {
        private readonly string _ExePfad = Directory.GetCurrentDirectory();
        private string _DateiBin => _ExePfad + @"\optionen.bin";

        public TRootItem _RootItem;

        public SeiteBereich _SeiteBereich;
        public SeiteThema _SeiteThema;
        public SeiteVorlage _SeiteVorlage;

        public MainWindow()
        {
            InitializeComponent();

            _RootItem = new TRootItem();
            LoadItems();
            InitSeiten();

            JgTreeView.ItemsSource = _RootItem.ListeBereiche;

            JgTreeView.SelectedItemChanged += (sender, e) =>
            {
                var obj = (JgItem)e.NewValue;
                SeiteEinstellen(obj);
            };
        }

        public void InitSeiten()
        {
            _SeiteBereich = new SeiteBereich(ClickNeu, _RootItem.Drucker);
            Grid.SetColumn(_SeiteBereich, 1);
            GridStamm.Children.Add(_SeiteBereich);

            _SeiteThema = new SeiteThema(ClickNeu, _RootItem.Drucker, _ExePfad)
            {
                Visibility = Visibility.Hidden
            };
            Grid.SetColumn(_SeiteThema, 1);
            Grid.SetColumnSpan(_SeiteThema, 2);
            GridStamm.Children.Add(_SeiteThema);

            _SeiteVorlage = new SeiteVorlage(ClickNeu, _RootItem.Drucker)
            {
                Visibility = Visibility.Hidden
            };
            Grid.SetColumn(_SeiteVorlage, 1);
            GridStamm.Children.Add(_SeiteVorlage);
        }

        private void SeiteEinstellen(JgItem item)
        {
            var typen = new Type[] { typeof(TBereichItem), typeof(TThemaItem), typeof(TVorlageItem) };
            var seiten = new InitSeite[] { _SeiteBereich, _SeiteThema, _SeiteVorlage };

            for (int i = 0; i < 3; i++)
            {
                if (!(item.GetType() == typen[i]) && (seiten[i].Visibility == Visibility.Visible))
                    seiten[i].Visibility = Visibility.Hidden;
            }

            var erg = GetSeiteAusItem(item);
            if (erg.Seite.Visibility == Visibility.Hidden)
                erg.Seite.Visibility = Visibility.Visible;

            erg.Seite.SetDaten(item);
        }

        public (JgItem.ItemAuswahl SeitenArt, InitSeite Seite) GetSeiteAusItem(JgItem item)
        {
            if (item is TBereichItem)
                return (JgItem.ItemAuswahl.Bereich, _SeiteBereich);
            if (item is TThemaItem)
                return (JgItem.ItemAuswahl.Thema, _SeiteThema);

            return (JgItem.ItemAuswahl.Vorlage, _SeiteVorlage);
        }

        public void ClickNeu(JgItem.ItemAuswahl auswahl)
        {
            var aktItem = (JgItem)JgTreeView.SelectedItem;

            switch (auswahl)
            {
                case JgItem.ItemAuswahl.Bereich:
                    var bereich = new TBereichItem(null);
                    _RootItem.ListeBereiche.Add(bereich);
                    aktItem = bereich;
                    break;
                case JgItem.ItemAuswahl.Thema:
                    if (aktItem is TThemaItem)
                        aktItem = aktItem.Parent;

                    (aktItem as TBereichItem).ListeThemen.Add(new TThemaItem(aktItem));
                    break;
                case JgItem.ItemAuswahl.Vorlage:
                    if (aktItem is TVorlageItem)
                        aktItem = aktItem.Parent;
                    (aktItem as TThemaItem).ListeVorlagen.Add(new TVorlageItem(aktItem));
                    break;
                case JgItem.ItemAuswahl.Loeschen:
                    if (aktItem is TBereichItem)
                        _RootItem.ListeBereiche.Remove((TBereichItem)aktItem);
                    else if (aktItem is TThemaItem)
                        (aktItem.Parent as TBereichItem).ListeThemen.Remove((TThemaItem)aktItem);
                    else if (aktItem is TVorlageItem)
                        (aktItem.Parent as TThemaItem).ListeVorlagen.Remove((TVorlageItem)aktItem);
                    break;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            pd.Print();
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {


            System.Drawing.Image img = System.Drawing.Image.FromFile("D:\\Foto.jpg");
            Point loc = new Point(100, 100);
            //e.Graphics.DrawImage(JgImage, loc);
        }

        public void LoadItems()
        {
            if (File.Exists(_DateiBin))
            {
                using (Stream file = File.Open(_DateiBin, FileMode.Open))
                {
                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        _RootItem = (TRootItem)bf.Deserialize(file);
                    }
                    catch { }
                }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (Stream file = File.Open(_DateiBin, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file, _RootItem);
            }
        }
    }
}
