using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;

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


