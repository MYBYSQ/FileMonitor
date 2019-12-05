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
        public ObservableCollection<ChangeInfo> ChangeInfoList { get; set; } = new ObservableCollection<ChangeInfo>();
        private ChangeMonitor _monitor = null;
        public MainWindow()
        {
            InitializeComponent();
            soucePathTB.DataContext = Path;
            infoListDG.ItemsSource = ChangeInfoList;
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
            _monitor.start();
            btn_start.IsEnabled = false;
            btn_stop.IsEnabled = true;
            btn_save.IsEnabled = false;
            btn_clear.IsEnabled = false;

        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            if(_monitor != null)
            {
                _monitor.stop();
            }
            btn_start.IsEnabled = true;
            btn_stop.IsEnabled = false;
            btn_save.IsEnabled = true;
            btn_clear.IsEnabled = true;
        }


        private void addInfo(string type, string info)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ChangeInfo infoItem = new ChangeInfo(type, getCurrentTime(), info);

                ChangeInfoList.Add(infoItem);
                infoListDG.ScrollIntoView(infoListDG.Items[infoListDG.Items.Count-1]);
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
            string path = ((System.Array)e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)).GetValue(0).ToString();
            if (Directory.Exists(path))
            {
                Path.Path = path;
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "text files(*.txt)|*.txt";
            sfd.RestoreDirectory = true;
            if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK || sfd.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                string filepath = sfd.FileName;
                
                using (FileStream fs = File.OpenWrite(filepath))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        string info = "monitor path:" + Path.Path;
                        writer.WriteLine(info);
                        foreach(ChangeInfo ci in ChangeInfoList)
                        {
                            info = string.Format("{0} \t {1} \t {2}", ci.ChangeType, ci.ChangeTime, ci.ChangeDes);
                            writer.WriteLine(info);
                        }
                        writer.Close();
                    }
                }
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ChangeInfoList.Clear();
        }
    }
}
