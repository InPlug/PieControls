using System.Collections.ObjectModel;

namespace NetEti.CustomControls
{
    /// <summary>
    /// Sammlung von Segmenten mit Daten für das aktuelle PieChart.
    /// Projekt von Kashif Imran auf Code Project - vielen Dank dafür.
    /// https://www.codeproject.com/Articles/442506/Simple-and-Easy-to-Use-Pie-Chart-Controls-in-WPF
    /// </summary>
    public class PieDataCollection : ObservableCollection<PieSegment>
    {
        /// <summary>
        /// Name der Collection.
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Holt oder setzt Informationen zum Zeichnen eines
        /// Radius (z.B. Anzeige für Maximal- Minimalwert).
        /// </summary>
        public PieRadialLine RadialLine { get; set; }
    }
}
