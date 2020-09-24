using System.Windows;
using VorlagenOrdner.Klassen;

namespace VorlagenOrdner.Seiten
{
    public partial class SeiteBereich : InitSeite
    {
        public SeiteBereich(ClickNeu clickAuswahl, TJgDrucker drucker)
            : base(clickAuswahl, drucker)
        {
            InitializeComponent();
        }

        private void Click_NeuerBereich(object sender, RoutedEventArgs e)
        {
            AuswahlClicked(JgItem.ItemAuswahl.Bereich);
        }

        private void Click_NeuesThema(object sender, RoutedEventArgs e)
        {
            AuswahlClicked(JgItem.ItemAuswahl.Thema);
        }

        public override JgItem GetDaten() => (TBereichItem)GridDaten.DataContext;

        public override void SetDaten(JgItem value)
        {
            GridDaten.DataContext = value;
        }
    }
}
