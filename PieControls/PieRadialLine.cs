using System.ComponentModel;
using System.Windows.Media;

namespace NetEti.CustomControls
{
    /// <summary>
    /// Fügt der PieChart-Klasse (Tortendiagramm) einen farblich abgesetzten Radius hinzu.
    /// Dient zur Veranschaulichung von Grenzwerten innerhalb des Tortendiagramms.
    /// </summary>
    /// <remarks>
    /// File: PieRadialLine.cs<br></br>
    /// Autor: Erik Nagel, NetEti<br></br>
    ///<br></br>
    /// 17.01.2015 Erik Nagel: erstellt<br></br>
    /// </remarks>
    public class PieRadialLine : INotifyPropertyChanged
    {
        /// <summary>
        /// WPF-Event für die UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Holt oder setzt den Wert für den Radius - bestimmt die Ausrichtung im PieChart.
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
        /// Holt oder setzt die Farbe für den Radius.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                solidBrush = new SolidColorBrush(color);
                solidBrush.Freeze();
                onPropertyChanged(this, "Color");
            }
        }

        /// <summary>
        /// Holt die Zeicheninformationen für den Radius.
        /// </summary>
        public Brush SolidBrush
        {
            get { return solidBrush; }
        }

        /// <summary>
        /// Holt oder setzt die Breite für den Radius.
        /// </summary>
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                onPropertyChanged(this, "Width");
            }
        }

        private double value;
        private double width;
        private Brush solidBrush;
        private Color color;

        private void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
