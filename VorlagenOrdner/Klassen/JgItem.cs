using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;

namespace VorlagenOrdner.Klassen
{
    [Serializable]
    public class JgItem : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public enum ItemAuswahl
        {
            Bereich,
            Thema,
            Vorlage,
            Loeschen
        }

        [NonSerialized]
        private JgItem _Parent;
        public JgItem Parent => _Parent;

        public JgItem(JgItem parent)
        {
            _Parent = parent;
        }

        private string _Bezeichnung = "";
        public string Bezeichnung
        {
            get => _Bezeichnung;
            set {
                _Bezeichnung = value;
                NotifyPropertyChanged();
            }
        }
    }

    [Serializable]
    public class TJgDrucker : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _Drucker = "";
        public string Drucker
        {
            get => _Drucker;
            set {
                _Drucker = value;
                NotifyPropertyChanged();
            }
        }
    }

    [Serializable]
    public class TRootItem
    {
        public ObservableCollection<TBereichItem> ListeBereiche { get; set; } = new ObservableCollection<TBereichItem>();

        public TJgDrucker Drucker { get; set; } = new TJgDrucker();
    }

    [Serializable]
    public class TBereichItem : JgItem
    {
        public ObservableCollection<TThemaItem> ListeThemen { get; set; } = new ObservableCollection<TThemaItem>();

        public TBereichItem(JgItem parent)
            : base(parent)
        {
            Bezeichnung = "> Neue Bezeichnung <";
        }
    }

    [Serializable]
    public abstract class TDruckItem : JgItem
    {
        private string _BildName = null;
        public string BildName
        {
            get => _BildName;
            set {
                _BildName = value;
                NotifyPropertyChanged();
            }
        }

        public Thickness RandBild => new Thickness(_RandLinks, _RandOben, _RandRechts, _RandUnten);

        private int _RandLinks = 25;
        public int RandLinks
        {
            get => _RandLinks;
            set {
                _RandLinks = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("RandBild");
            }
        }

        private int _RandOben = 20;
        public int RandOben
        {
            get => _RandOben;
            set {
                _RandOben = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("RandBild");
            }
        }

        private int _RandRechts = 20;
        public int RandRechts
        {
            get => _RandRechts;
            set {
                _RandRechts = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("RandBild");
            }
        }

        private int _RandUnten = 20;
        public int RandUnten
        {
            get => _RandUnten;
            set {
                _RandUnten = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("RandBild");
            }
        }

        private short _AnzahlKopien = 1;
        public short AnzahlKopien
        {
            get => _AnzahlKopien;
            set {
                _AnzahlKopien = value;
                NotifyPropertyChanged();
            }
        }

        private bool _IstQuerformat = false;
        public bool IstQuerformat
        {
            get => _IstQuerformat;
            set {
                _IstQuerformat = value;
                NotifyPropertyChanged();
            }
        }

        public TDruckItem(JgItem parent)
            : base(parent)
        { }
    }

    [Serializable]
    public class TThemaItem : TDruckItem
    {
        public ObservableCollection<TVorlageItem> ListeVorlagen { get; set; } = new ObservableCollection<TVorlageItem>();

        public TThemaItem(JgItem parent)
            : base(parent)
        {
            Bezeichnung = "> Neues Thema <";
        }
    }

    [Serializable]
    public class TVorlageItem : TDruckItem
    {
        public TVorlageItem(JgItem parent)
            : base(parent)
        {
            Bezeichnung = "Vorlage";
        }
    }
}
