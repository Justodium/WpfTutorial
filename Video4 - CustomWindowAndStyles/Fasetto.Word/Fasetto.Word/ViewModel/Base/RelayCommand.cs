﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fasetto.Word
{
  public class RelayCommand : ICommand
  {
    private Action action;

    public event EventHandler CanExecuteChanged = (sender, e) => { };

    public RelayCommand(Action action)
    {
      this.action = action;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      this.action();
    }
  }
}
