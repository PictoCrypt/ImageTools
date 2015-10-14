using System;
using System.Threading;
using System.Windows.Input;

namespace ImageToolApp
{
	public delegate void IdeskCommandExecuteDelegate(object parameter);

	public delegate bool IdeskCommandCanExecuteDelegate(object parameter);

	public delegate void IdeskCommandNoParamExecuteDelegate();

	public delegate bool IdeskCommandNoParamCanExecuteDelegate();

	public class UICommand : ICommand
	{
		private UICommand
			(IdeskCommandExecuteDelegate actionExecute, IdeskCommandCanExecuteDelegate actionCanExecute = null, bool startInThread = false)
		{
			mActionExecute = actionExecute;
			mActionCanExecute = actionCanExecute;
			mStartInThread = startInThread;
		}

		private UICommand
			(IdeskCommandNoParamExecuteDelegate actionExecute,
			IdeskCommandNoParamCanExecuteDelegate actionCanExecute = null,
			bool startInThread = false)
		{
			mActionNoParamExecute = actionExecute;
			mActionNoParamCanExecute = actionCanExecute;
			mStartInThread = startInThread;
		}

		public event EventHandler CanExecuteChanged;

		public static UICommand Regular
			(IdeskCommandNoParamExecuteDelegate actionExecute, IdeskCommandNoParamCanExecuteDelegate actionCanExecute = null)
		{
			return new UICommand(actionExecute, actionCanExecute, false);
		}

		public static UICommand Regular(IdeskCommandExecuteDelegate actionExecute, IdeskCommandCanExecuteDelegate actionCanExecute = null)
		{
			return new UICommand(actionExecute, actionCanExecute, false);
		}

		public static UICommand Thread
			(IdeskCommandNoParamExecuteDelegate actionExecute, IdeskCommandNoParamCanExecuteDelegate actionCanExecute = null)
		{
			return new UICommand(actionExecute, actionCanExecute, true);
		}

		public void Execute(object parameter)
		{
			if (mActionExecute != null)
			{
				if (mStartInThread)
				{
					new Thread(() => mActionExecute(parameter)) { IsBackground = true }.Start();
					return;
				}
				mActionExecute(parameter);
			}
			else if (mActionNoParamExecute != null)
			{
				if (mStartInThread)
				{
					new Thread(() => mActionNoParamExecute()) { IsBackground = true }.Start();
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

		private readonly IdeskCommandCanExecuteDelegate mActionCanExecute;
		private readonly IdeskCommandExecuteDelegate mActionExecute;
		private readonly IdeskCommandNoParamCanExecuteDelegate mActionNoParamCanExecute;
		private readonly IdeskCommandNoParamExecuteDelegate mActionNoParamExecute;
		private readonly bool mStartInThread;
	}
}