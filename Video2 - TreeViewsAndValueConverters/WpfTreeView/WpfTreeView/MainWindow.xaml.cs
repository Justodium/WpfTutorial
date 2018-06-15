﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfTreeView
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      foreach (var drive in Directory.GetLogicalDrives())
      {
        var item = new TreeViewItem
        {
          Header = drive,
          Tag = drive
        };


        item.Items.Add(null);

        item.Expanded += this.Folder_Expanded;

        FolderView.Items.Add(item);
      }
    }

    private void Folder_Expanded(object sender, RoutedEventArgs e)
    {
      var item = (TreeViewItem)sender;

      if (item.Items.Count != 1 || item.Items[0] != null)
      {
        return;
      }

      item.Items.Clear();

      var fullPath = (string)item.Tag;

      var directories = new List<string>();

      try
      {
        var dirs = Directory.GetDirectories(fullPath);

        if (dirs.Length > 0)
        {
          directories.AddRange(dirs);
        }
      }
      catch (Exception)
      {
        throw;
      }

      directories.ForEach(directoryPath =>
      {
        var subItem = new TreeViewItem()
        {
          Header = GetFileFolderName(directoryPath),
          Tag = directoryPath
        };

        subItem.Items.Add(null);

        subItem.Expanded += Folder_Expanded;

        item.Items.Add(subItem);
      });
    }

    private static string GetFileFolderName(string directoryPath)
    {
      if (string.IsNullOrEmpty(directoryPath))
      {
        return string.Empty;
      }

      var normalizedPath = directoryPath.Replace('/', '\\');

      var lastIndex = normalizedPath.LastIndexOf('\\' );

      if (lastIndex <= 0)
      {
        return directoryPath;
      }

      return directoryPath.Substring(lastIndex + 1);
    }
  }
}
