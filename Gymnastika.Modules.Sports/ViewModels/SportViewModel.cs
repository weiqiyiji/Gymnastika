using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;
using Microsoft.Practices.Prism.ViewModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ISportViewModel : INotifyPropertyChanged
    {
        Sport Sport { set; get; }

        string ImageUri { get; }

        string Brief { get; }

        string Name { get; }

        double Calories { get; }

        int Minutes { get; }

        string IntroductionUri { get; }

        ICommand CloseCommand { get; }
        ICommand ExpandCommand { get; }
        ICommand MinimizeCommand { get; }

        
        event EventHandler RequestCloseEvent;
        event EventHandler RequestExpandCommandEvent;
        event EventHandler RequestMinimizeEvent;
    }

    public class SportViewModel : NotificationObject, ISportViewModel
    {

        public SportViewModel()
        {
        }

        #region Properties

        private Sport _sport = new Sport();
        public Sport Sport
        {
            get { return _sport; }
            set
            {
                if (_sport != value)
                {
                    _sport = value;
                    RaisePropertyChanged("");
                }
            }
        }

        public string ImageUri
        {
            get { return Sport.ImageUri; }
        }

        public string Brief
        {
            get { return Sport.Brief; }
        }


        public string Name
        {
            get { return Sport.Name; }
        }

        public double Calories
        {
            get { return Sport.Calories; }
        }

        public int Minutes
        {
            get { return Sport.Minutes; }
        }

        public string IntroductionUri
        {
            get { return Sport.IntroductionUri; }
        }

        #endregion

        public event EventHandler RequestCloseEvent = delegate { };


        ICommand _closeCommnad;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommnad == null)
                {
                    _closeCommnad = new DelegateCommand(Close);
                }
                return _closeCommnad;
            }
        }

        void Close()
        {
            RequestCloseEvent(this, EventArgs.Empty);
        }



        ICommand _expandCommand;
        public ICommand ExpandCommand
        {
            get 
            {
                if (_expandCommand == null)
                    _expandCommand = new DelegateCommand(Expand);
                return _expandCommand;
            }
        }

        void Expand()
        {
            if (RequestExpandCommandEvent != null)
                RequestExpandCommandEvent(this,EventArgs.Empty);
        }

        ICommand _minimizeCommand;
        public ICommand MinimizeCommand
        {
            get 
            {
                if (_minimizeCommand == null)
                    _minimizeCommand = new DelegateCommand(Minimize);
                return _minimizeCommand;
            }
        }

        void Minimize()
        {
            if (RequestMinimizeEvent != null)
                RequestMinimizeEvent(this, EventArgs.Empty);
        }

        public event EventHandler RequestExpandCommandEvent = delegate { };

        public event EventHandler RequestMinimizeEvent = delegate { };
    }
}
