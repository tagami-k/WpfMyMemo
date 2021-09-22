﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfMyMemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //OpenFileDialog openFileDialog1 = new OpenFileDialog();
        
        private string AppFileName; 

        private string FileNameValue;
        private string FileName
        {
            get { return FileNameValue; }
            set
            {
                FileNameValue = value;
                Edited = false;
            }
        }

        private bool EditedValue;
        private bool Edited
        {
            get { return EditedValue; }
            set
            {
                EditedValue = value;
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            string s = AppFileName;
            if (FileName != "")
                s += " - " + FileName;
            if (Edited)
                s += "(変更あり)";
            this.Title = s;

            //int len = textBoxMain.Text.Length;
            //MessageBox.Show(len.ToString());

            if (FileName == "" || !Edited || textBoxMain.Text.Length == 0)
                MenuItemFileSave.IsEnabled = false;
            else
                MenuItemFileSave.IsEnabled = true;

            if(!Edited || textBoxMain.Text.Length == 0)
                MenuItemFileSaveAs.IsEnabled = false;
            else
                MenuItemFileSaveAs.IsEnabled = true;
        }

        public MainWindow()
        {
            InitializeComponent();

            //AppFileName =  this.Title;
            //FileName = "";
        }

        private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("終了します");
            this.Close();   // Application.Exit(); と同じ。 En。vironment.Exit(1); は異なる。
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppFileName = this.Title;
            FileName = "";

        }

        private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Filter = "テキスト文書(.txt)|*.txt|HTML File(*.html, *.htm)|*.html;*.htm|すべてのファイル(*.*)|*.*";            
            openFileDialog1.FileName = "";

            bool? result = openFileDialog1.ShowDialog();
            if (result == true)
            {
                LoadFile(openFileDialog1.FileName);
                
                /*
                using (Stream fileStream = openFileDialog1.OpenFile())
                {
                    StreamReader sr = new StreamReader(fileStream, true);
                    textBoxMain.Text = sr.ReadToEnd();
                }
                */
            }
            else
            {
                MessageBox.Show("ファイルが選択されていません。");
            }
        }

        private void LoadFile(string value)
        {
            textBoxMain.Text = File.ReadAllText(value);
            FileName = value;
        }

        private void MenuItemFileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Filter = "テキスト文書(.txt)|*.txt|HTML File(*.html, *.htm)|*.html;*.htm|すべてのファイル(*.*)|*.*";
            saveFileDialog1.FileName = System.IO.Path.GetFileName(FileName);

            bool? result = saveFileDialog1.ShowDialog();
            if (result == true)
            {
                SaveFile(saveFileDialog1.FileName);      //SafeFileNameは、ファイル名のみ。

                /*
                using (Stream fileStream = saveFileDialog1.OpenFile())
                using(StreamWriter sw = new StreamWriter(fileStream))
                {
                    sw.Write(textBoxMain.Text);
                }
                */
            }
            else
            {
                MessageBox.Show("ファイル名が設定されていません。");
            }
        }

        private void SaveFile(string value)
        {
            File.WriteAllText(value, textBoxMain.Text);
            FileName = value;
        }

        private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(FileName);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!AskGiveUpText()) e.Cancel = true;
        }

        private bool AskGiveUpText()
        {
            if (!Edited || textBoxMain.Text.Length == 0)
                return true;
            //MessageBox.Show("編集内容を破棄しますか？", AppFileName, MessageBoxButton.YesNo, MessageBoxImage.Warning)
            if (MessageBox.Show("編集内容を破棄しますか？", AppFileName, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                return true;
            else 
                return false;
        }

        private void MenuItemFileNew_Click(object sender, RoutedEventArgs e)
        {
            if (!AskGiveUpText()) return;
            textBoxMain.Clear();
            FileName = "";
        }

        private void textBoxMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            Edited = true;
        }
    }
}
