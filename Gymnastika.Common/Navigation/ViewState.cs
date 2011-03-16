using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Common.Navigation
{
    public class StateChangingEventArgs : EventArgs
    {
        public ViewState CurrentState { get; set; }
        public ViewState TargetState { get; set; }

        public StateChangingEventArgs(ViewState currentState, ViewState targetState)
        {
            CurrentState = currentState;
            TargetState = targetState;
        }
    }

    public class StateChangedEventArgs : EventArgs
    {
        public ViewState PreviousState { get; set; }
        public ViewState CurrentState { get; set; }

        public StateChangedEventArgs(ViewState previousState, ViewState currentState)
        {
            PreviousState = previousState;
            CurrentState = currentState;
        }
    }

    public delegate void StateChangingHandler(ViewState targetState);

    public class ViewState
    {
        public string Header { get; set; }
        public string Name { get; set; }
    }
}
