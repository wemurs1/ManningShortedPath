﻿using System;
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
using Point = System.Drawing.Point;
using Microsoft.Win32;

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
