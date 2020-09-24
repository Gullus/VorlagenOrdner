using System.Windows;
using System.Windows.Controls;

namespace VorlagenOrdner.Klassen
{
    public partial class MarginUpDown : UserControl
    {
        public float Value
        {
            get => (float)GetValue(MyMargin);
            set {
                TbValue.Text = value.ToString();
                SetValue(MyMargin, value);
            }
        }

        public static readonly DependencyProperty MyMargin = DependencyProperty.Register("Value",
            typeof(float),
            typeof(MarginUpDown),
            new PropertyMetadata(0f, new PropertyChangedCallback((obj, e) =>
            {
                (obj as MarginUpDown).TbValue.Text = e.NewValue.ToString();
            })));

        public MarginUpDown()
        {
            InitializeComponent();
        }

        private void Click_Minus(object sender, RoutedEventArgs e)
        {
            Value--;
            RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
        }

        private void Click_Plus(object sender, RoutedEventArgs e)
        {
            Value++;
            RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
        }

        // Value changed
        private static readonly RoutedEvent ValueChangedEvent =
           EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
           typeof(RoutedEventHandler), typeof(MarginUpDown));

        /// <summary>The ValueChanged event is called when the TextBoxValue of the control changes.</summary>
        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        //Increase button clicked
        private static readonly RoutedEvent IncreaseClickedEvent =
            EventManager.RegisterRoutedEvent("IncreaseClicked", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(MarginUpDown));

        /// <summary>The IncreaseClicked event is called when the Increase button clicked</summary>
        public event RoutedEventHandler IncreaseClicked
        {
            add { AddHandler(IncreaseClickedEvent, value); }
            remove { RemoveHandler(IncreaseClickedEvent, value); }
        }

        //Increase button clicked
        private static readonly RoutedEvent DecreaseClickedEvent =
            EventManager.RegisterRoutedEvent("DecreaseClicked", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(MarginUpDown));

        /// <summary>The DecreaseClicked event is called when the Decrease button clicked</summary>
        public event RoutedEventHandler DecreaseClicked
        {
            add { AddHandler(DecreaseClickedEvent, value); }
            remove { RemoveHandler(DecreaseClickedEvent, value); }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse((sender as TextBox).Text,out var erg))
            {
                Value = erg;
                RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
            }
        }
    }
}
