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

        private TRootItem _RootItem;

        private SeiteBereich _SeiteBereich;
        private SeiteThema _SeiteThema;

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

        private void InitSeiten()
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
        }

        private void SeiteEinstellen(JgItem item)
        {
            var typen = new Type[] { typeof(TBereichItem), typeof(TThemaItem), typeof(TVorlageItem) };
            var seiten = new InitSeite[] { _SeiteBereich, _SeiteThema };

            for (int i = 0; i < 2; i++)
            {
                if (!(item.GetType() == typen[i]) && (seiten[i].Visibility == Visibility.Visible))
                    seiten[i].Visibility = Visibility.Hidden;
            }

            var erg = GetSeiteAusItem(item);
            if (erg.Seite.Visibility == Visibility.Hidden)
                erg.Seite.Visibility = Visibility.Visible;

            erg.Seite.SetDaten(item);
        }

        private (JgItem.ItemAuswahl SeitenArt, InitSeite Seite) GetSeiteAusItem(JgItem item)
        {
            if (item is TBereichItem)
                return (JgItem.ItemAuswahl.Bereich, _SeiteBereich);
            else
                return (JgItem.ItemAuswahl.Thema, _SeiteThema);
        }

        private void ClickNeu(JgItem.ItemAuswahl auswahl)
        {
            var aktItem = (JgItem)JgTreeView.SelectedItem;

            switch (auswahl)
            {
                case JgItem.ItemAuswahl.Bereich:
                    var bereich = new TBereichItem();
                    _RootItem.ListeBereiche.Add(bereich);
                    aktItem = bereich;
                    break;
                case JgItem.ItemAuswahl.Thema:
                    if (aktItem is TThemaItem)
                        aktItem = SucheParent(aktItem);

                    (aktItem as TBereichItem).ListeThemen.Add(new TThemaItem(aktItem));
                    break;
                case JgItem.ItemAuswahl.Vorlage:
                    if (aktItem is TVorlageItem)
                        aktItem = SucheParent(aktItem);
                    (aktItem as TThemaItem).ListeVorlagen.Add(new TVorlageItem(aktItem));
                    break;
                case JgItem.ItemAuswahl.Loeschen:
                    if (aktItem is TBereichItem)
                        _RootItem.ListeBereiche.Remove((TBereichItem)aktItem);
                    else if (aktItem is TThemaItem)
                    {
                        var parent = (TBereichItem)SucheParent(aktItem);
                        parent.ListeThemen.Remove((TThemaItem)aktItem);
                    }
                    else if (aktItem is TVorlageItem)
                    {
                        var parent = (TThemaItem)SucheParent(aktItem);
                        parent.ListeVorlagen.Remove((TVorlageItem)aktItem);
                    }
                    break;
            }
        }

        private JgItem SucheParent(JgItem item)
        {
            foreach(var bereich in _RootItem.ListeBereiche)
            {
                foreach(var thema in bereich.ListeThemen)
                {
                    if (item == thema)
                        return bereich;
                    
                    foreach(var vorlage in thema.ListeVorlagen)
                    {
                        if (vorlage == item)
                            return thema;
                    }
                }
            }

            return null;
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

        private void LoadItems()
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
