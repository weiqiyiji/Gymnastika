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

        event EventHandler CloseRequest;
    }

    public class SportViewModel : NotificationObject, ISportViewModel
    {
        public SportViewModel()
            : this(new Sport())
        {

        }

        public SportViewModel(Sport sport)
        {
            Sport = sport;
        }

        #region Properties

        private Sport _sport;
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

        public event EventHandler CloseRequest = delegate { };

        #region CloseCommnad

        ICommand _closeCommnad;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommnad == null)
                {
                    _closeCommnad = new DelegateCommand(Close, CanCloseDelegate);
                }
                return _closeCommnad;
            }
        }

        void Close()
        {
            CloseRequest(this, EventArgs.Empty);
        }

        bool CanClose()
        {
            return CloseRequest != null;
        }

        Func<bool> _canCloseDelegate;
        public Func<bool> CanCloseDelegate
        {
            get
            {
                if (_canCloseDelegate == null)
                {
                    _canCloseDelegate = CanClose;
                }
                return _canCloseDelegate;
            }
        }

        #endregion

    }
}
