using System.Windows;
using Azure.NLog.Query.UI.Controllers;

namespace Azure.NLog.Query.UI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      this.DataContext = new QueryController();
      this.InitializeComponent();
    }
  }
}