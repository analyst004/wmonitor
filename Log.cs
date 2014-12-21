using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using MySql.Data.MySqlClient;

namespace wmonitor
{

    public class Log : INotifyPropertyChanged
    {
        private DateTime _date;
        private String _level;
        private int _millisecond;
        private String _message;
        private String _location;

        public DateTime Date 
        {
            get {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
            
        }

        public String Level 
        {
            get {
                return _level;
            }
            set
            {
                _level = value;
                OnPropertyChanged("Level");
            }
        }

        public int Millisecond
        {
            get
            {
                return _millisecond;
            }
            set
            {
                _millisecond = value;
                OnPropertyChanged("Millisecond");
            }
        }

        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }

        }

        public String Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged("Location");
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
