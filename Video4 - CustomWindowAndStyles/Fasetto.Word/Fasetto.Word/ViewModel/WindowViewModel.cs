using Microsoft.Windows.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fasetto.Word
{
  public class WindowViewModel : BaseViewModel
  {
    private readonly Window window;

    private int outerMarginSize = 10;
    private int windowRadius = 10;

    public ICommand MinimizeCommand { get; set; }

    public ICommand MaximizeCommand { get; set; }

    public ICommand CloseCommand { get; set; }

    public ICommand MenuCommand { get; set; }

    public double WindowMinimumWidth { get; set; } = 400;

    public double WindowMinimumHeight { get; set; } = 400;

    public int ResizeBorder { get; set; } = 6;

    public Thickness ResizeBorderThickness { get { return new Thickness(this.ResizeBorder + this.OuterMarginSize); } }

    public Thickness InnerContentPadding { get { return new Thickness(this.ResizeBorder); } }

    public Thickness OuterMarginSizeThickness { get { return new Thickness(this.OuterMarginSize); } }

    public int OuterMarginSize
    {
      get
      {
        return this.window.WindowState == WindowState.Maximized ? 0 : this.outerMarginSize;
      }
      set
      {
        this.outerMarginSize = value;
      }
    }

    public int WindowRadius
    {
      get
      {
        return this.window.WindowState == WindowState.Maximized ? 0 : this.windowRadius;
      }
      set
      {
        this.windowRadius = value;
      }
    }

    public CornerRadius WindowCornerRadius { get { return new CornerRadius(this.WindowRadius); } }

    public int TitleHeight { get; set; } = 42;

    public GridLength TitleHeightLength { get { return new GridLength(this.TitleHeight + ResizeBorder); } }

    public WindowViewModel(Window window)
    {
      this.window = window;

      this.window.StateChanged += (sender, e) =>
      {
        base.OnPropertyChanged(nameof(ResizeBorderThickness));
        base.OnPropertyChanged(nameof(OuterMarginSize));
        base.OnPropertyChanged(nameof(OuterMarginSizeThickness));
        base.OnPropertyChanged(nameof(WindowRadius));
        base.OnPropertyChanged(nameof(WindowCornerRadius));
      };

      this.MinimizeCommand = new RelayCommand(() => this.window.WindowState = WindowState.Minimized);
      this.MaximizeCommand = new RelayCommand(() => this.window.WindowState ^= WindowState.Maximized);
      this.CloseCommand= new RelayCommand(() => this.window.Close());
      this.MenuCommand = new RelayCommand(() => System.Windows.SystemCommands.ShowSystemMenu(this.window, GetMousePosition()));

      //Fix window resize issue
      var resizer = new WindowResizer(this.window);
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetCursorPos(ref Win32Point pt);

    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Point
    {
      public Int32 X;
      public Int32 Y;
    };
    private static Point GetMousePosition()
    {
      Win32Point w32Mouse = new Win32Point();
      GetCursorPos(ref w32Mouse);
      return new Point(w32Mouse.X, w32Mouse.Y);
    }
  }
}
