using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VorlagenOrdner.Klassen
{
    public static class Helper
    {
        public static string GetBildDatei(string pfad, string bildName)
        {
            return pfad + @"\Bilder\" + bildName;
        }

        public static void ComboBilderLaden(Image bild, string pfad, string bildName)
        {
            var dat = GetBildDatei(pfad, bildName);
            if (!string.IsNullOrWhiteSpace(bildName) && (File.Exists(dat)))
                bild.Source = new BitmapImage(new Uri(dat));
            else
                bild.Source = null;
        }
    }
}


