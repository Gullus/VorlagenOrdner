using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VorlagenOrdner.Klassen;

namespace VorlagenOrdner.Seiten
{
    public partial class SeiteVorlage : InitSeite
    {
        public SeiteVorlage(ClickNeu clickAuswahl, TJgDrucker drucker)
            :base(clickAuswahl, drucker)
        {
            InitializeComponent();
        }

        private void Click_NeueVorlage(object sender, RoutedEventArgs e)
        {
            AuswahlClicked(JgItem.ItemAuswahl.Vorlage);
        }

        public override JgItem GetDaten() => (TVorlageItem)GridDaten.DataContext;

        public override void SetDaten(JgItem value)
        {
            GridDaten.DataContext = value;
        }
    }
}
