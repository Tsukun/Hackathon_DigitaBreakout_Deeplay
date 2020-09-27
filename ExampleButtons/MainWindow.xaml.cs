using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ExampleButtons
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

        private void OnClick1(object sender, RoutedEventArgs e) => OnClick(1);
        private void OnClick2(object sender, RoutedEventArgs e) => OnClick(2);
        private void OnClick3(object sender, RoutedEventArgs e) => OnClick(3);

        private void OnClick(int i)
        {
            LastButton.Text = i.ToString();
            History.Add(i);
        }

        public List<int> History { get; } = new List<int>();
    }
}
