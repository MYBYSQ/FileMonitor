using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FileMonitor
{
    public class ChangeInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _changeType;
        private string _changeTime;
        private string _changeInfo;

        public string ChangeType
        {
            get
            {
                return _changeType;
            }
            set
            {
                _changeType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChangeType"));
            }
        }

        public string ChangeTime
        {
            get
            {
                return _changeTime;
            }
            set
            {
                _changeTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChangeTime"));
            }
        }

        public string ChangeDes
        {
            get
            {
                return _changeInfo;
            }
            set
            {
                _changeInfo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChangeDes"));
            }
        }

        public ChangeInfo(string type,string time,string info)
        {
            _changeType = type;
            _changeTime = time;
            _changeInfo = info;
        }
    }
}
