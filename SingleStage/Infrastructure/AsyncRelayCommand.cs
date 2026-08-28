using System.Windows.Input;

namespace SingleStage.Infrastructure
{
    public class AsyncRelayCommand : ICommand
    {
        // What should happen?
        // An asynchronous function requiring ONE parameter
        // and returning a Task.
        private readonly Func<object?, Task> _execute;

        // Am I allowed to do it right now?
        // Optional: determines whether the command can execute.
        private readonly Func<object?, bool>? _canExecute;

        // Prevents multiple executions of the same command simultaneously.
        private bool _isExecuting;

        // WPF listens for this and re-checks CanExecute().
        public event EventHandler? CanExecuteChanged;

        #region Constructors

        // Allows:
        //
        // new AsyncRelayCommand(() => DoSomethingAsync())
        //
        public AsyncRelayCommand(Func<Task> execute)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = _ => execute();
        }

        // Allows:
        //
        // new AsyncRelayCommand(
        //     parameter => DoSomethingAsync(parameter));
        //
        public AsyncRelayCommand(Func<object?, Task> execute)
            : this(execute, null)
        {
        }

        // Allows:
        //
        // new AsyncRelayCommand(
        //     parameter => DoSomethingAsync(parameter),
        //     parameter => CanDoSomething(parameter));
        //
        public AsyncRelayCommand(
            Func<object?, Task> execute,
            Func<object?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        public bool CanExecute(object? parameter)
        {
            return !_isExecuting &&
                   (_canExecute?.Invoke(parameter) ?? true);
        }

        public void Execute(object? parameter)
        {
            _ = ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object? parameter)
        {
            if (!CanExecute(parameter))
                return;

            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();

                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
