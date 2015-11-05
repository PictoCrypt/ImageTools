using System;
using System.Threading;
using System.Windows.Input;

namespace UserControlClassLibrary
{
    public delegate void CommandExecuteDelegate(object parameter);

    public delegate bool CommandCanExecuteDelegate(object parameter);

    public delegate void CommandNoParamExecuteDelegate();

    public delegate bool CommandNoParamCanExecuteDelegate();

    public class UICommand : ICommand
    {
        private readonly CommandCanExecuteDelegate mActionCanExecute;
        private readonly CommandExecuteDelegate mActionExecute;
        private readonly CommandNoParamCanExecuteDelegate mActionNoParamCanExecute;
        private readonly CommandNoParamExecuteDelegate mActionNoParamExecute;
        private readonly bool mStartInThread;

        private UICommand
            (CommandExecuteDelegate actionExecute, CommandCanExecuteDelegate actionCanExecute = null,
                bool startInThread = false)
        {
            mActionExecute = actionExecute;
            mActionCanExecute = actionCanExecute;
            mStartInThread = startInThread;
        }

        private UICommand
            (CommandNoParamExecuteDelegate actionExecute,
                CommandNoParamCanExecuteDelegate actionCanExecute = null,
                bool startInThread = false)
        {
            mActionNoParamExecute = actionExecute;
            mActionNoParamCanExecute = actionCanExecute;
            mStartInThread = startInThread;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (mActionExecute != null)
            {
                if (mStartInThread)
                {
                    new Thread(() => mActionExecute(parameter)) {IsBackground = true}.Start();
                    return;
                }
                mActionExecute(parameter);
            }
            else if (mActionNoParamExecute != null)
            {
                if (mStartInThread)
                {
                    new Thread(() => mActionNoParamExecute()) {IsBackground = true}.Start();
                    return;
                }
                mActionNoParamExecute();
            }
        }

        public bool CanExecute(object parameter)
        {
            if (mActionCanExecute != null)
            {
                return mActionCanExecute(parameter);
            }
            return mActionNoParamCanExecute == null || mActionNoParamCanExecute();
        }

        public static UICommand Regular
            (CommandNoParamExecuteDelegate actionExecute,
                CommandNoParamCanExecuteDelegate actionCanExecute = null)
        {
            return new UICommand(actionExecute, actionCanExecute, false);
        }

        public static UICommand Regular(CommandExecuteDelegate actionExecute,
            CommandCanExecuteDelegate actionCanExecute = null)
        {
            return new UICommand(actionExecute, actionCanExecute, false);
        }

        public static UICommand Thread
            (CommandNoParamExecuteDelegate actionExecute,
                CommandNoParamCanExecuteDelegate actionCanExecute = null)
        {
            return new UICommand(actionExecute, actionCanExecute, true);
        }
    }
}