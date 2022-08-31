using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkClasses
{
    partial class MainWindow
    {
        private Random Rand = new Random();

        private Network BuildGridNetwork(
            string filename,
            double width,
            double height,
            int numRows,
            int numCols)
        {
            const int MARGIN = 20; // pixels
            var network = new Network();

            width -= MARGIN * 2;
            height -= MARGIN * 2;
            int xSpacing = (int)(width / numCols);
            int ySpacing = (int)(height / numRows);
            int nodeName = 1;
            var nodes = new Node[numCols, numRows];

            for (int y = 0; y < numRows; y++)
            {
                for (int x = 0; x < numCols; x++)
                {
                    nodes[x, y] = new Node(network, new Point(MARGIN + x * xSpacing, MARGIN + y * ySpacing), nodeName.ToString());
                    nodeName++;
                    if (x != 0)
                    {
                        MakeRandomizedLink(network, nodes[x, y], nodes[x - 1, y]);
                        MakeRandomizedLink(network, nodes[x - 1, y], nodes[x, y]);
                    }
                    if (y != 0)
                    {
                        MakeRandomizedLink(network, nodes[x, y], nodes[x, y - 1]);
                        MakeRandomizedLink(network, nodes[x, y - 1], nodes[x, y]);
                    }

                }
            }

            network.SaveIntoFile(filename);

            return network;
        }

        private double Distance(Node from, Node to)
        {
            if (from != null && to != null)
            {
                var width = Math.Abs(from.Center.X - to.Center.X);
                var height = Math.Abs(from.Center.Y - to.Center.Y);
                var distance = Math.Sqrt(width * width + height * height);
                return distance;
            }

            throw new ArgumentNullException("From and/To To Node cannot be null");
        }

        private void MakeRandomizedLink(Network network, Node from, Node to)
        {
            if (from != null && to != null)
            {
                int cost = (int)(Distance(from, to) * (Rand.Next(100, 120) / 100.0)); // double between 1.0 and 1.2
                var link = new Link(network, from, to, cost);
                return;
            }

            throw new ArgumentNullException("From and/To To Node cannot be null");
        }
    }
}
