using System.Linq;
using System.Collections.ObjectModel;

namespace NetEti.CustomControls
{
    /// <summary>
    /// Enthält Hilfsroutinen für PieCharts:
    /// liefert eine Gesamtsumme für die in "collection" übergebenen PieChart-Segmente.
    /// Projekt von Kashif Imran auf Code Project - vielen Dank dafür.
    /// https://www.codeproject.com/Articles/442506/Simple-and-Easy-to-Use-Pie-Chart-Controls-in-WPF
    /// </summary>
    public static class PieCollectionHelper
    {
        /// <summary>
        /// Liefert eine Gesamtsumme für die in "collection" übergebenen PieChart-Segmente.
        /// </summary>
        /// <param name="collection">1-n PieChart Segmente.</param>
        /// <returns>Gesamtsumme für die übergebenen Segmente.</returns>
        public static double GetTotal(this ObservableCollection<PieSegment> collection)
        {
            return collection.Sum((a) => { return a.Value; });
        }
    }
}
