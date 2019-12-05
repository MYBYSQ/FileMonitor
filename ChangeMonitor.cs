using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
namespace FileMonitor
{
    class ChangeMonitor
    {
        public delegate void AddInfoEventHandler(string type, string changeDesc);
        public event AddInfoEventHandler AddInfoEvent;
        private string _path = string.Empty;
        private FileSystemWatcher _watcher = null;
        public ChangeMonitor(string path)
        {
            _path = path;
        }

        public void start()
        {
            if (!Directory.Exists(_path))
            {
                Console.WriteLine("path not exit:{0}", _path);
            }
            _watcher = new FileSystemWatcher(_path);
            _watcher.NotifyFilter = NotifyFilters.LastWrite |
                NotifyFilters.FileName |
                NotifyFilters.DirectoryName;

            _watcher.Changed += FileChangeEvent;
            _watcher.Renamed += FileRenameEvent;
            _watcher.Deleted += FileDeleteEvent;
            _watcher.Created += FileCreateEvent;

            _watcher.EnableRaisingEvents = true;
            _watcher.IncludeSubdirectories = true;
        }

        private void FileCreateEvent(object sender, FileSystemEventArgs e)
        {
            try
            {
                AddInfoEvent("create", e.Name);
                Console.WriteLine("create:{0}", e.Name);
            }
            catch
            {
                stop();
            }
        }

        private void FileDeleteEvent(object sender, FileSystemEventArgs e)
        {
            try
            {
                AddInfoEvent("delete", e.Name);
                Console.WriteLine("delete:{0}", e.Name);
            }
            catch
            {
                stop();
            }
        }

        private void FileRenameEvent(object sender, RenamedEventArgs e)
        {
            try
            {
                string info = string.Format("{0} to {1}", e.OldName, e.Name);

                AddInfoEvent("rename", info);
                Console.WriteLine("renamed:{0} to {1}", e.OldName, e.Name);
            }
            catch
            {
                stop();
            }
        }

        private void FileChangeEvent(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (File.Exists(e.FullPath))
                {
                    AddInfoEvent("edit", e.Name);
                    Console.WriteLine("edit:{0}", e.Name);
                }

            }
            catch
            {
                stop();
            }
        }

        public void stop()
        {
            _watcher.Changed -= FileChangeEvent;
            _watcher.Created -= FileCreateEvent;
            _watcher.Deleted -= FileDeleteEvent;
            _watcher.Renamed -= FileRenameEvent;
            _watcher.EnableRaisingEvents = false;
        }

        public void Test()
        {
            AddInfoEvent("edit", "test");
        }
    }
}
