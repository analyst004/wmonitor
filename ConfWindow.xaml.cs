using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace wmonitor
{
    /// <summary>
    /// Interaction logic for ConfWindow.xaml
    /// </summary>
    public partial class ConfWindow : Window, INotifyPropertyChanged
    {
        private String _conf;


        public ConfWindow(String conf)
        {
            InitializeComponent();
            this._conf = conf;
            this.DataContext = this;
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
