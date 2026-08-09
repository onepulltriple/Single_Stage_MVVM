using System.Windows.Input;

namespace SingleStage.Infrastructure
{
    // this is one reusable class to handle all commands
    public class RelayCommand : ICommand  // WPF understands anything implementing ICommand
    {
        // what should happen?
        // a function call, requiring ONE parameter and returning no value
        // this is the code that runs when a button is clicked
        private readonly Action<object?> _execute;

        // am I allowed to do it right now?
        // like an action, but returns a value (bool in this case)
        // optional: should the button be enabled?
        private readonly Func<object?, bool>? _canExecute;
        //private readonly Predicate<object?> _canExecute;

        // WPF listens for something changing and will then re-check whether the button should be enabled
        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        #region Constructors
        public RelayCommand(Action execute)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = _ => execute();
        }

        public RelayCommand(Action<object?> execute) : this(execute, null) { }
        //public RelayCommand(Action<object?> execute, Predicate<object> canExecute)

        // takes in the code from the ViewModel
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        // answers whether or not the button can be clicked (defaults to true)
        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
            //return _canExecute == null || _canExecute(parameter);
        }

        // this runs when the user clicks a button
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        // tells WPF to check CanExecute() again
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
