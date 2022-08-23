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
using Point = System.Drawing.Point;

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



        // Build small test networks.

        private void net1Button_Click(object sender, RoutedEventArgs e)

        {

            Network network = new Network();



            Node a = new Node(network, new Point(20, 20), "A");

            Node b = new Node(network, new Point(120, 120), "B");



            Link link_a_b = new Link(network, a, b, 10);



            // Validate the network.

            ValidateNetwork(network);

        }



        private void net2Button_Click(object sender, RoutedEventArgs e)

        {

            Network network = new Network();



            Node a = new Node(network, new Point(20, 20), "A");

            Node b = new Node(network, new Point(120, 20), "B");

            Node c = new Node(network, new Point(20, 120), "C");

            Node d = new Node(network, new Point(120, 120), "D");



            Link link_a_b = new Link(network, a, b, 10);

            Link link_b_d = new Link(network, b, d, 15);

            Link link_a_c = new Link(network, a, c, 20);

            Link link_c_d = new Link(network, c, d, 25);



            // Validate the network.

            ValidateNetwork(network);

        }



        private void net3Button_Click(object sender, RoutedEventArgs e)

        {

            Network network = new Network();



            Node a = new Node(network, new Point(20, 20), "A");

            Node b = new Node(network, new Point(120, 20), "B");

            Node c = new Node(network, new Point(20, 120), "C");

            Node d = new Node(network, new Point(120, 120), "D");



            Link link_a_b = new Link(network, a, b, 10);

            Link link_b_d = new Link(network, b, d, 15);

            Link link_a_c = new Link(network, a, c, 20);

            Link link_c_d = new Link(network, c, d, 25);



            Link link_b_a = new Link(network, b, a, 11);

            Link link_d_b = new Link(network, d, b, 16);

            Link link_c_a = new Link(network, c, a, 21);

            Link link_d_c = new Link(network, d, c, 26);



            // Validate the network.

            ValidateNetwork(network);

        }



        // Serialize the network, save it into a file,

        // read it back from the file, reserialize it,

        // and compare the two serializations.

        private void ValidateNetwork(Network network)

        {

            // Serialize.

            string serialization1 = network.Serialization();



            // Save into a file,

            network.SaveIntoFile("test_network.net");



            // Read it back from the file.

            network.ReadFromFile("test_network.net");



            // Reserialize.

            string serialization2 = network.Serialization();



            // Display the serialization.

            netTextBox.Text = network.Serialization();



            // Compare the two serializations.

            if (serialization1 == serialization2)

                statusLabel.Content = "OK";

            else

                statusLabel.Content = "Serializations do not match";

        }
    }
}
