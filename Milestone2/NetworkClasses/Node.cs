using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkClasses
{
    public class Node
    {
        public int Index { get; set; }
        public Network Network { get; set; }
        public Point Center { get; set; }
        public string Text { get; set; }
        public List<Link> Links { get; set; }

        public Node(Network network, Point center, string text)
        {
            Network = network;
            Center = center;
            Text = text;
            Links = new List<Link>();
            Index = -1;
            network.AddNode(this);
        }

        public override string ToString()
        {
            return $"[{Text}]";
        }

        public void AddLink(Link link)
        {
            Links.Add(link);
        }
    }
}
