using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTreeView
{
  public static class DirectoryStructure
  {
    public static List<DirectoryItem> GetLogicalDrives()
    {
      return Directory.GetLogicalDrives()
        .Select(drive => new DirectoryItem
        {
          FullPath = drive,
          Type = DirectoryItemType.Drive
        })
        .ToList();
    }

    public static List<DirectoryItem> GetDirectoryContents(string fullPath)
    {
      var items = new List<DirectoryItem>();

      try
      {
        var dirs = Directory.GetDirectories(fullPath);

        if (dirs.Length > 0)
        {
          items.AddRange(dirs.Select(d => new DirectoryItem
          {
            FullPath = d,
            Type = DirectoryItemType.Folder
          }));
        }
      }
      catch (Exception)
      {
        throw;
      }

      try
      {
        var files = Directory.GetFiles(fullPath);

        if (files.Length > 0)
        {
          items.AddRange(files.Select(d => new DirectoryItem
          {
            FullPath = d,
            Type = DirectoryItemType.File
          }));
        }
      }
      catch (Exception)
      {
        throw;
      }

      return items;
    }

    public static string GetFileFolderName(string directoryPath)
    {
      if (string.IsNullOrEmpty(directoryPath))
      {
        return string.Empty;
      }

      var normalizedPath = directoryPath.Replace('/', '\\');

      var lastIndex = normalizedPath.LastIndexOf('\\');

      if (lastIndex <= 0)
      {
        return directoryPath;
      }

      return directoryPath.Substring(lastIndex + 1);
    }
  }
}
