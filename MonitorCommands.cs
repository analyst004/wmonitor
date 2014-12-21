using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MySql.Data.MySqlClient;

namespace wmonitor
{
    public class MonitorCommands
    {
        private static RoutedUICommand _ViewConfig;
        private static RoutedUICommand _ViewLog;

        static MonitorCommands() 
        {
            _ViewConfig = new RoutedUICommand("View Configuration", "ViewConfig", typeof(MonitorCommands));
            _ViewLog = new RoutedUICommand("View Log", "ViewLog", typeof(MonitorCommands));
        }

        public static RoutedUICommand ViewConfig
        {
            get { return _ViewConfig; }
        }

        public static void ViewConfig_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            String conf = e.Parameter as String;
            ConfWindow wndConf = new ConfWindow(conf);
            wndConf.ShowDialog();
        }

        public static void ViewConfig_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static RoutedUICommand ViewLog
        {
            get { return _ViewLog;  }
        }

        public static void ViewLog_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            String id = e.Parameter as String;
            MainWindow mainwnd = App.Current.MainWindow as MainWindow;
            MySqlConnection logdb = mainwnd.LogDatabase;
            LogWindow wndLog = new LogWindow(id, logdb);
            wndLog.Show();
        }

        public static void ViewLog_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        public static void BindCommandsToWindow(Window window)
        {
            window.CommandBindings.Add(
                new CommandBinding(ViewConfig, ViewConfig_Executed, ViewConfig_CanExecute));
            window.CommandBindings.Add(
                new CommandBinding(ViewLog, ViewLog_Executed, ViewLog_CanExecute));
        }
    }
}
