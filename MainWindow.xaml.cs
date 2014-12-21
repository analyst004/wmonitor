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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MongoDB.Driver;
using MySql.Data.MySqlClient;
using System.Timers;
using System.Windows.Threading;
using MongoDB;
using MongoDB.Bson;

namespace wmonitor
{

    public class CommandParameter 
    {
        public bool CanEditBeExecuted {get; set;}
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged, ISynchronizeInvoke
    {

        private DispatcherTimer CheckTimer = new DispatcherTimer();
        private MySqlConnection dbConn = null;
        private MongoDatabase _mongodb;

        public CommandParameter parameter { get; set; }

        public MainWindow()
        {
            parameter = new CommandParameter();
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            parameter.CanEditBeExecuted = false;

            this.DataContext = this;

            if (!ConnectLogDatabase()) {
                return;
            }
            if (!ConnectUrlDatabase()) {
                return;
            }
            UpdateCrawlStatus();
            InitCheckTimer();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MonitorCommands.BindCommandsToWindow(this);
        }

        private void InitCheckTimer()
        {
            CheckTimer.Tick += new EventHandler(OnUpdateUrlDatabase);
            CheckTimer.Interval = new TimeSpan(0, 0, 15);
            CheckTimer.Start();

        }

        private bool ConnectLogDatabase()
        {
            this.Status = "正在连接日志数据库 ... ";
            const string strconn = @"server=10.1.1.2;port=3306;userid=root;password=;database=logdb";

            try
            {
                dbConn = new MySqlConnection(strconn);
                dbConn.Open();
                this.Status = "连接日志数据库成功。";
                return true;
            } catch (MySqlException e) {
                
                this.Status = "连接数据酷失败 " + e.ToString();
                return false;
            }
            
        }

        private bool ConnectUrlDatabase()
        {
            this.Status = "正在连接Url数据库...";
            //数据库连接字符串
            const string strconn = "mongodb://10.1.1.5:27017";
            //数据库名称
            const string dbName = "webdb";

            try {
                //创建数据库链接
                //MongoClient client = new MongoClient(strconn);
                MongoClient client = new MongoClient(strconn);
                MongoServer server = client.GetServer();
                //MongoServer server = MongoDB.Driver.MongoServer.Create(strconn);
                server.Connect();
                //获得数据库urldb
                _mongodb = server.GetDatabase(dbName);
                this.Status = "连接Url数据库成功";
                return true;
            } catch (Exception e) {
                this.Status = "连接Url数据库失败 " + e.ToString();
                return false;
            }
        }

        private void UpdateCrawlStatus()
        {
            UrlDatabase.Clear();
            MongoCollection collStatus = Mongo.GetCollection("crawl_status");
            MongoCursor<BsonDocument> cursor = collStatus.FindAllAs<BsonDocument>();
            foreach (BsonDocument doc in cursor)
            {
                CrawlStatus status = new CrawlStatus();
                MongoCollection coll = Mongo.GetCollection(doc["id"].AsString);
                if (coll != null)
                {
                    status.Count = coll.Count();
                }
                status.SucceedCount = doc["succcount"].AsInt64;
                status.FailedCount = doc["failcount"].AsInt64;
                status.Configuration = doc["conf"].AsString;
                status.Domain = doc["domain"].AsString;
                status.Name = doc["name"].AsString;
                status.Id = doc["id"].AsString;
                status.Status = doc["status"].AsString;
                status.LastRunTime = doc["lastruntime"].AsDateTime;
                UrlDatabase.Add(status);
            }

            Status = "更新成功 " + DateTime.Now.ToUniversalTime();
        }

        private void OnUpdateUrlDatabase(object sender, EventArgs e)
        {
            UpdateCrawlStatus();   
        }

        private String _status;
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

        
        public MongoDatabase Mongo
        {
            get
            {
                return _mongodb;
            }
        }

        public MySqlConnection LogDatabase
        {
            get
            {
                return dbConn;
            }
        }

        private ObservableCollection<CrawlStatus> _urldb;
        public ObservableCollection<CrawlStatus> UrlDatabase
        {
            get
            {
                if (_urldb == null)
                    _urldb = new ObservableCollection<CrawlStatus>();
                return _urldb;
            }

            set
            {
                this.OnPropertyChanged("UrlDatabase");
                return;
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

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {

            string caption = "退出";
            string message = "您确定要退出程序吗？";
            MessageBoxButton buttons = MessageBoxButton.YesNo;

            MessageBoxResult ret = MessageBox.Show(message, caption, buttons);
            if (ret == System.Windows.MessageBoxResult.Yes)
            {

            }
            else
            {
                e.Cancel = true;
            }

        }

        private void DataGridRow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

        }

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            throw new NotImplementedException();
        }

       public object EndInvoke(IAsyncResult result)
       {
          throw new NotImplementedException();
       }
       public object Invoke(Delegate method, object[] args)
       {
          throw new NotImplementedException();
       }
       public bool InvokeRequired
       {
           get
           {
               return false;
           }
          //throw new NotImplementedException();
       }
    }
}
