using SingleStage.Entities;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace SingleStage.Infrastructure
{
    public class ArtistPerformanceCollectionConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is not IEnumerable artistPerformances)
                return string.Empty;

            return string.Join(
                ", ",
                artistPerformances
                    .Cast<ArtistPerformance>()
                    .Where(ap => ap.Artist is not null)
                    .Select(ap => ap.Artist.Name));
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
