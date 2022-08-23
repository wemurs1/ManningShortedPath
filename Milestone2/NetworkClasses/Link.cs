using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkClasses
{
    public class Link
    {
        public Network Network { get; set; }
        public Node FromNode { get; set; }
        public Node ToNode { get; set; }
        public int Cost { get; set; }

        public Link(Network network, Node fromNode, Node toNode, int cost)
        {
            Network = network;
            FromNode = fromNode;
            ToNode = toNode;
            Cost = cost;
            Network.AddLink(this);
            FromNode.AddLink(this);
        }

        public override string ToString()
        {
            return $"[{FromNode}] --> [{ToNode}] ({Cost})";
        }

        internal void Draw(Canvas mainCanvas)
        {
            mainCanvas.DrawLine(FromNode.Center, ToNode.Center, Brushes.Black, 1);
        }

        internal void DrawLabel(Canvas mainCanvas)
        {
            // throw new NotImplementedException();
        }
    }
}
