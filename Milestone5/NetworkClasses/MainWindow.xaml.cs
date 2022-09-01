using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;
using Microsoft.Win32;
using static NetworkClasses.Network;

namespace NetworkClasses
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

        private Network MyNetwork = new Network();

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();

                dialog.DefaultExt = ".net";
                dialog.Filter = "Network Files|*.net|All Files|*.*";

                // Display the dialog.
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    // Open the network.
                    MyNetwork = new Network(dialog.FileName);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                MyNetwork = new Network();
            }

            // Display the network.
            DrawNetwork();
        }



        private void makeTestNetworks_Click(object sender, RoutedEventArgs e)

        {
            BuildGridNetwork("3x3_grid.net", 300, 300, 3, 3);

            BuildGridNetwork("4x4_grid.net", 300, 300, 4, 4);

            BuildGridNetwork("5x8_grid.net", 600, 400, 5, 8);

            BuildGridNetwork("6x10_grid.net", 600, 400, 6, 10);

            BuildGridNetwork("10x15_grid.net", 600, 400, 10, 15);

            BuildGridNetwork("20x30_grid.net", 600, 400, 20, 30);

            MessageBox.Show("Done");
        }

        private void algorithmComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = comboBox!.SelectedValue as ComboBoxItem;
            switch (selectedItem!.Content)
            {
                case "Label Setting":
                    MyNetwork.AlgorithmType = AlgorithmTypes.LabelSetting;
                    break;
                case "Label Correcting":
                    MyNetwork.AlgorithmType = AlgorithmTypes.LabelCorrecting;
                    break;
                default:
                    throw new ArgumentException($"Unknown algorithm value: {selectedItem.Content}");
            }
        }


        private void DrawNetwork()
        {
            // Remove any previous drawing.
            mainCanvas.Children.Clear();

            // Make the network draw itself.
            MyNetwork.Draw(mainCanvas);
        }

        private void ExitCommand_Executed(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
