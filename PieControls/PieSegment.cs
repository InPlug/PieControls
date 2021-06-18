using System.Windows.Media;
using System.ComponentModel;
using System.Collections.Generic;

namespace NetEti.CustomControls
{
    /// <summary>
    /// Stellt ein Segment eines WPF-Tortendiagramms plus Zusatzinfos zur Verfügung.
    /// Projekt von Kashif Imran auf Code Project - vielen Dank dafür.
    /// https://www.codeproject.com/Articles/442506/Simple-and-Easy-to-Use-Pie-Chart-Controls-in-WPF
    /// </summary>
    public class PieSegment : INotifyPropertyChanged
    {
        /// <summary>
        /// WPF-Event für die UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        private double value;
        private Brush _gradientBrush;
        private Brush _solidBrush;
        private Color color;
        private Dictionary<Color, Brush[]> _colorsBrushes;

        /// <summary>
        /// Holt oder setzt den (Prozent-)Wert für dieses Segment.
        /// </summary>
        public double Value
        {
            get 
            {
                return value;
            }
            set 
            {
                this.value = value;
                onPropertyChanged(this, "Value");
            }
        }

        /// <summary>
        /// Holt oder setzt die Farbe für dieses Segment.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                if (value != color)
                {
                    color = value;
                    if (!this._colorsBrushes.ContainsKey(color))
                    {
                        this._gradientBrush = new LinearGradientBrush(MakeSecondColor(color, 50), color, 45);
                        this._solidBrush = new SolidColorBrush(color);
                        this._gradientBrush.Freeze();
                        this._solidBrush.Freeze();
                        // Das nachfolgende Cashing ist notwendig. Ansonsten folgen nach längerem Betrieb
                        // Speicherfehler: "Zur Verarbeitung dieses Befehls sind nicht genügend Speicherressourcen verfügbar..."
                        this._colorsBrushes.Add(color, new Brush[] { this._solidBrush, this._gradientBrush} );
                    }
                    else
                    {
                        this._solidBrush = this._colorsBrushes[color][0];
                        this._gradientBrush = this._colorsBrushes[color][1];
                    }
                    onPropertyChanged(this, "Color");
                }
            }
        }

        /// <summary>
        /// Konstructor - initialisiert den Cache für die Color-Brushes.
        /// </summary>
        public PieSegment()
        {
            this._colorsBrushes = new Dictionary<Color, Brush[]>();
        }

        //difference should be a maximum value of 100
        private Color MakeSecondColor(Color color, uint difference)
        {
            difference = difference > 100 ? 100 : difference;
            byte r = GetNewColorByte(color.R, difference);
            byte g = GetNewColorByte(color.G, difference);
            byte b = GetNewColorByte(color.B, difference);
            return Color.FromRgb(r, g, b);
        }

        //This method ensures that bytes never overflow to avoid drastic change in color
        private byte GetNewColorByte(byte oldByte, uint difference)
        {
            if (oldByte + difference > 255)
            {
                return (byte)(oldByte - difference);
            }
            else
            {
                return (byte)(oldByte + difference);
            }
        }

        /// <summary>
        /// Liefert einen Farbverlauf für das aktuelle Segment.
        /// </summary>
        public Brush GradientBrush
        {
            get { return this._gradientBrush; }
        }

        /// <summary>
        /// Liefert eine Farbe für das aktuelle Segment.
        /// </summary>
        public Brush SolidBrush
        {
            get { return this._solidBrush; }
        }

        /// <summary>
        /// Holt oder setzt den Namen für dieses Segment.
        /// </summary>
        public string Name 
        { 
            get 
            { 
                return name; 
            } 
            set 
            { 
                name = value; 
                onPropertyChanged(this, "Name"); 
            } 
        }

        private void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
