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
        private Network BuildGridNetwork(
            string filename,
            double width,
            double height,
            int numRows,
            int numCols)
        {
            const int MARGIN = 5; // pixels
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
                        var link = new Link(network, nodes[x, y], nodes[x - 1, y], 1);
                        link = new Link(network, nodes[x - 1, y], nodes[x, y], 1);
                    }
                    if (y != 0)
                    {
                        var link = new Link(network, nodes[x, y], nodes[x, y - 1], 1);
                        link = new Link(network, nodes[x, y - 1], nodes[x, y], 1);
                    }

                }
            }


            return network;
        }
    }
}
