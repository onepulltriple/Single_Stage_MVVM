using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SingleStage.Infrastructure
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type _, object __, CultureInfo ___)
        {
            if (value is bool)
            {
                bool shouldBecomeCollapsed = (bool)value;

                if (shouldBecomeCollapsed)
                    return Visibility.Collapsed; // true means control should get collapsed

                return Visibility.Visible; // false means control should remain visible
            }

            return Visibility.Collapsed; // fallback is to collapse the control
        }

        public object ConvertBack(object value, Type _, object __, CultureInfo ___)
        {
            if (value is Visibility)
            {
                Visibility visibility = (Visibility)value;

                if (visibility == Visibility.Visible)
                    return false; // converts Visible to false

                return true; // converts any non-Visible state to true, e.g. Hidden, Collapsed --> true
            }

            return true; // fallback
        }
    }
}
