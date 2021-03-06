﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTreeView
{
  public class DirectoryStructureViewModel : BaseViewModel
  {
    public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

    public DirectoryStructureViewModel()
    {
      var logicalDrives = DirectoryStructure.GetLogicalDrives();
      this.Items = new ObservableCollection<DirectoryItemViewModel>(logicalDrives.Select(drive => new DirectoryItemViewModel(drive.FullPath, drive.Type)));
    }
  }
}
