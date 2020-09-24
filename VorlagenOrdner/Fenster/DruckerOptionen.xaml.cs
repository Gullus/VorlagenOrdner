using System.Windows;
using System.Windows.Controls;
using VorlagenOrdner.Klassen;

namespace VorlagenOrdner.Fenster
{
    public partial class DruckerOptionen : Window
    {
        public string Drucker => lbPrinterSelection.SelectedItem.ToString();

        public DruckerOptionen(TJgDrucker drucker)
        {
            InitializeComponent();
            lbPrinterSelection.SelectedItem = drucker.Drucker;
        }

        private void cmbPrinterSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Click_Ok(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Click_Abbrechen(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
