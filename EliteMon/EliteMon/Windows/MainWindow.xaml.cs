using System.Windows;
using NSW.EliteDangerous.API;

namespace NSW.EliteDangerous.Monitor.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEliteDangerousAPI _api;

        public MainWindow(IEliteDangerousAPI api)
        {
            InitializeComponent();
            _api = api;
            Title = App.Title;
        }
    }
}
