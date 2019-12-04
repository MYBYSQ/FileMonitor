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
using System.Windows.Forms;
using System.IO;
namespace FileMonitor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public delegate void AddChangeInfoListDel(string type, string changeDesc);
    public partial class MainWindow : Window 
    {
        private SourcePath Path  = new SourcePath();
        private ObservableCollection<ChangeInfo> ChangeInfoList = new ObservableCollection<ChangeInfo>();
        private ChangeMonitor _monitor = null;
        public MainWindow()
        {
            InitializeComponent();
            soucePathTB.DataContext = Path;
            infoListLV.ItemsSource = ChangeInfoList;
        }

        private void btn_broswer_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "please select folder";
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK || dialog.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                Path.Path = dialog.SelectedPath.ToString();
            }
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Path.Path))
            {
                System.Windows.MessageBox.Show("path can't empty");
                return;
            }
            _monitor = new ChangeMonitor(Path.Path);
            _monitor.AddInfoEvent += addInfo;
            infoListLV.ItemsSource = ChangeInfoList;
            _monitor.start();
            btn_start.IsEnabled = false;
            btn_stop.IsEnabled = true;

        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            if(_monitor != null)
            {
                _monitor.stop();
            }
        }


        private void addInfo(string type, string info)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ChangeInfo infoItem = new ChangeInfo(type, getCurrentTime(), info);

                ChangeInfoList.Add(infoItem);
            }));
           
        }

        private string getCurrentTime()
        {
            System.DateTime dt = System.DateTime.Now;
            string curTime = dt.ToString() + " " + dt.Millisecond.ToString();
            return curTime;
        }

        private void soucePathTB_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            //e.Effects = System.Windows.DragDropEffects.Copy;
           // e.Handled = true;
            string path = ((System.Array)e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)).GetValue(0).ToString();
            if (Directory.Exists(path))
            {
                Path.Path = path;
            }
        }
    }
}
