using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Music.Other
{
    public class AnotherCommandImplementation : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public AnotherCommandImplementation(Action<object> execute)
            : this(execute, null) { }

        public AnotherCommandImplementation(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute ?? (x => true);    //оператор ?? - левый операнд, если он не null, правый - в другом случае
        }

        public bool CanExecute(object parameter) => canExecute(parameter);
        public void Execute(object parameter) => execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
