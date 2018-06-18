using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfTreeView
{
  public class DirectoryItemViewModel : BaseViewModel
  {
    public ICommand ExpandCommand { get; set; }

    public DirectoryItemType Type { get; set; }

    public string FullPath { get; set; }

    public string Name { get { return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFileFolderName(this.FullPath); } }

    public ObservableCollection<DirectoryItemViewModel> Children { get; set; }

    public bool CanExpand { get { return this.Type != DirectoryItemType.File; } }

    public bool IsExpanded
    {
      get
      {
        return this.Children?.Count(f => f != null) > 0;
      }
      set
      {
        if (value == true)
        {
          this.Expand();
        }
        else
        {
          this.ClearChildren();
        }
      }
    }

    public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
    {
      this.ExpandCommand = new RelayCommand(this.Expand);
      this.FullPath = fullPath;
      this.Type = type;

      this.ClearChildren();
    }

    private void ClearChildren()
    {
      this.Children = new ObservableCollection<DirectoryItemViewModel>();
      if (this.Type != DirectoryItemType.File)
      {
        this.Children.Add(null);
      }
    }

    private void Expand()
    {
      if (this.Type == DirectoryItemType.File)
      {
        return;
      }

      var children = DirectoryStructure.GetDirectoryContents(this.FullPath);
      this.Children = new ObservableCollection<DirectoryItemViewModel>(
                          children.Select(content => new DirectoryItemViewModel(content.FullPath, content.Type)));
    }
  }
}
