using SingleStage.ViewModels;
using System.Windows;

namespace SingleStage.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeLoginWindow.xaml
    /// </summary>
    public partial class EmployeeLoginWindow01 : Window
    {
        private readonly EmployeeLoginViewModel _viewModel;

        public EmployeeLoginWindow01(EmployeeLoginViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel
                ?? throw new ArgumentNullException(nameof(viewModel));

            this.DataContext = _viewModel;

            _viewModel.LoginSucceeded += ViewModel_LoginSucceeded;

            this.Loaded += (_, _) =>
            {
                MinWidth = ActualWidth;
                MinHeight = ActualHeight;
            };
        }

        private void ViewModel_LoginSucceeded(object? sender, EventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.LoginSucceeded -= ViewModel_LoginSucceeded;
            base.OnClosed(e);
        }
    }
}
