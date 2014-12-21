using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace wmonitor
{
    public class CrawlStatus : INotifyPropertyChanged
    {
        private String  _id;
        private String  _domain;
        private String  _name;
        private long     _count;
        private long _succ_count;
        private long _fail_count;
        private String  _conf;
        private String  _status;
        private DateTime _lastruntime;



        public String Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public String Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                _domain = value;
                this.OnPropertyChanged("Domain");
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public long Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                this.OnPropertyChanged("Count");
            }
        }

        public long SucceedCount
        {
            get
            {
                return _succ_count;
            }
            set
            {
                _succ_count = value;
                OnPropertyChanged("SucceedCount");
            }
        }

        public long FailedCount
        {
            get
            {
                return _fail_count;
            }
            set
            {
                _fail_count = value;
                OnPropertyChanged("FailedCount");
            }
        }

        public String Configuration
        {
            get
            {
                return _conf;
            }
            set
            {
                _conf = value;
                OnPropertyChanged("Configuration");
            }
        }

        public DateTime LastRunTime
        {
            get
            {
                return _lastruntime;
            }
            set
            {
                _lastruntime = value;
                OnPropertyChanged("LastRunTime");
            }
        }

        public String Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                this.OnPropertyChanged("Status");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(
                    this, new PropertyChangedEventArgs(propName));
        }
    }
}
