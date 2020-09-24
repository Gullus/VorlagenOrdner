using System.Windows;
using System.Windows.Controls;
using VorlagenOrdner.Klassen;

namespace VorlagenOrdner.Seiten
{
    public delegate void ClickNeu(JgItem.ItemAuswahl auswahl);

    public abstract partial class InitSeite : UserControl
    {
        internal TJgDrucker JgDrucker;

        public ClickNeu AuswahlClicked { get; set; }

        public InitSeite(ClickNeu clickAuswahl, TJgDrucker drucker)
        {
            AuswahlClicked = clickAuswahl;
            JgDrucker = drucker;
        }

        public abstract JgItem GetDaten();
        public abstract void SetDaten(JgItem value);

        internal void Click_Loeschen(object sender, RoutedEventArgs e)
        {
            AuswahlClicked(JgItem.ItemAuswahl.Loeschen);
        }
    }
}
