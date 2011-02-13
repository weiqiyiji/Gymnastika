using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;
using Gymnastika.Common.Configuration;
using Gymnastika.Data.Models;
using Gymnastika.Data;
using Gymnastika.Data.Migration;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Gymnastika.MigrationGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isOpenFilePathSelected = true;
        private bool _isSaveFilePathSelected = false;
        private string _dbName;
        private string _dbFolder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TableName.Text))
            {
                MessageBox.Show("表名不能为空");
                return;
            }

            if (!_isOpenFilePathSelected)
            {
                MessageBox.Show("请选择文件");
                return;
            }

            if (!_isSaveFilePathSelected)
            {
                MessageBox.Show("请选择文件输出路径");
                return;
            }

            string version = DateTime.Now.ToString("yyyyMMddHHmmss");

            XmlDocument doc = new XmlDocument();
            doc.Load("MigrationClassTemplate.xml");

            string template = doc.SelectSingleNode("/template").InnerText;
            template = template.Replace("{version}", version)
                               .Replace("{table_name}", TableName.Text);

            Stream stream = File.Create(
                System.IO.Path.Combine(SavePath.Text, GetMigrationFileName(TableName.Text, version)));

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(template);

            writer.Close();

            MessageBox.Show("保存成功");
        }

        //private void OpenFile_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.DefaultExt = "sdf";
        //    openFileDialog.Title = "选择sdf文件";
            
        //    if (openFileDialog.ShowDialog(this) == true)
        //    {
        //        _isOpenFilePathSelected = true;
        //        SdfPath.Text = openFileDialog.FileName;
        //    }
        //}

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.OverwritePrompt = true;
            //saveFileDialog.DefaultExt = "cs";
            //saveFileDialog.CreatePrompt = true;
            //saveFileDialog.CheckPathExists = true;
            //saveFileDialog.Title = "输出Migration文件";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _isSaveFilePathSelected = true;
                SavePath.Text = dialog.SelectedPath;
            }
        }

        private string GetMigrationFileName(string tableName, string version)
        {
            return string.Format("Migration_{0}_{1}.cs", tableName, version);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
