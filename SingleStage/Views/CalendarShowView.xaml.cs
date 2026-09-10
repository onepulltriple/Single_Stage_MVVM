using SingleStage.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SingleStage.Views
{
    /// <summary>
    /// Interaction logic for CalendarShowView.xaml
    /// </summary>
    public partial class CalendarShowView : UserControl
    {
        public CalendarShowView()
        {
            InitializeComponent();
        }

        private void ShowBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not CalendarShowViewModel showViewModel)
                return;

            var calendarWeekView = FindParent<CalendarWeekView>(this);

            if (calendarWeekView?.DataContext is not CalendarWeekViewModel weekViewModel)
                return;

            weekViewModel.SelectShowCommand.Execute(showViewModel);

            e.Handled = true;
        }

        private static T? FindParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            DependencyObject? parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is T typedParent)
                    return typedParent;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}
