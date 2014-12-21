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
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;

namespace wmonitor
{
    /// <summary>
    /// Interaction logic for Log.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        private String crawlId;
        private MySqlConnection database;

        public LogWindow(String CrawlId, MySqlConnection database)
        {
            InitializeComponent();
            this.DataContext = this;
            crawlId = CrawlId;
            this.database = database;
            QueryLog(mnuInfo.IsChecked, mnuWarn.IsChecked, mnuError.IsChecked);
        }

        private void QueryLog(bool includeInfo, bool includeWarn, bool includeError)
        {
            String filter = " ";
            if (!includeInfo)
            {
                filter += "AND level!='INFO' ";
            }
            if (!includeWarn)
            {
                filter += "AND level!='WARN' ";
            }
            if (!includeError)
            {
                filter += "AND level!='ERROR' ";
            }

            LogDatabase.Clear();
            MySqlDataReader reader = null;
            try
            {
                String sql = "SELECT date,millisecond,level,message,location FROM loginfo WHERE id='"+ crawlId + "'" + filter + " ORDER BY date DESC LIMIT 1024;";
                MySqlCommand cmd = new MySqlCommand(sql, database);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Log log = new Log();
                    log.Date = reader.GetDateTime(0);
                    log.Millisecond = reader.GetInt16(1);
                    log.Level = reader.GetString(2);
                    log.Message = reader.GetString(3);
                    log.Location = reader.GetString(4);
                    LogDatabase.Add(log);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());    
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }

        private ObservableCollection<Log> _logdb;
        public ObservableCollection<Log> LogDatabase
        {
            get
            {
                if (_logdb == null)
                    _logdb = new ObservableCollection<Log>();
                return _logdb;
            }

        }

        private void mnuFilter_Click(object sender, RoutedEventArgs e)
        {
            QueryLog(mnuInfo.IsChecked, mnuWarn.IsChecked, mnuError.IsChecked);
        }

        private void mnuRefresh_Click(object sender, RoutedEventArgs e)
        {
            QueryLog(mnuInfo.IsChecked, mnuWarn.IsChecked, mnuError.IsChecked);
        }

    }
}
