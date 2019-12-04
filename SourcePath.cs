using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace FileMonitor
{
    public class SourcePath : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _sourcePath;
        public string Path
        {
            get
            {
                return _sourcePath;
            }
            set
            {
                _sourcePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Path"));
            }
        }
    }
}
