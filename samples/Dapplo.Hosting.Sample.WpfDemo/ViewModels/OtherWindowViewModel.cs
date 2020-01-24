using System;
using System.Collections.Generic;
using System.Text;
using Dapplo.Microsoft.Extensions.Hosting.Wpf;

namespace Dapplo.Hosting.Sample.WpfDemo.ViewModels {
  public class OtherWindowViewModel : IWpfViewModel {
    public OtherWindowViewModel() {
      Message = "Hello again";
    }

    public string Message { get; }
  }
}
