using System.Windows;
using System.Windows.Controls;

namespace SingleStage.Infrastructure
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnBoundPasswordChanged));

        public static string? GetBoundPassword(DependencyObject obj)
        {
            return (string?)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string? value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        private static readonly DependencyProperty UpdatingPasswordProperty =
            DependencyProperty.RegisterAttached(
                "UpdatingPassword",
                typeof(bool),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(false));

        private static bool GetUpdatingPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(UpdatingPasswordProperty);
        }

        private static void SetUpdatingPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(UpdatingPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(
            DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (sender is not PasswordBox passwordBox)
                return;

            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;

            if (!GetUpdatingPassword(passwordBox))
            {
                string newPassword = e.NewValue as string ?? string.Empty;

                if (passwordBox.Password != newPassword)
                {
                    passwordBox.Password = newPassword;
                }
            }
        }

        private static void PasswordBox_PasswordChanged(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is not PasswordBox passwordBox)
                return;

            if (GetUpdatingPassword(passwordBox))
                return;

            SetUpdatingPassword(passwordBox, true);

            SetBoundPassword(passwordBox, passwordBox.Password);

            SetUpdatingPassword(passwordBox, false);
        }
    }
}
