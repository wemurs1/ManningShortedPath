using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

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

        public void Draw(Canvas mainCanvas)
        {
            const int RADIUS = 10;
            Rect bounds = new Rect(Center.X - RADIUS, Center.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
            mainCanvas.DrawEllipse(bounds, Brushes.White, Brushes.Black, 1);
            mainCanvas.DrawString(Text, RADIUS, RADIUS, Center, 0,RADIUS, Brushes.Black);
        }
    }
}
