using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NetworkClasses
{
    public class Node
    {
        public int Index { get; set; }
        public Network Network { get; set; }
        public Point Center { get; set; }
        public string Text { get; set; }
        public List<Link> Links { get; set; }
        public Ellipse? MyEllipse { get; set; }
        public Label? MyLabel { get; set; }
        public Link? ShortestPathLink { get; set; }
        private bool isStartNode;

        public bool IsStartNode
        {
            get { return isStartNode; }
            set
            {
                isStartNode = value;
                SetNodeAppearance();
            }
        }
        private bool isEndNode;

        public bool IsEndNode
        {
            get { return isEndNode; }
            set
            {
                isEndNode = value;
                SetNodeAppearance();
            }
        }

        public int TotalCost { get; internal set; }

        private void SetNodeAppearance()
        {
            if (MyEllipse == null)
            {
                return;
            }
            else if (IsStartNode)
            {
                MyEllipse.Fill = Brushes.Pink;
                MyEllipse.Stroke = Brushes.Red;
                MyEllipse.StrokeThickness = 2;
            }
            else if (IsEndNode)
            {
                MyEllipse.Fill = Brushes.LightGreen;
                MyEllipse.Stroke = Brushes.Green;
                MyEllipse.StrokeThickness = 2;
            }
            else
            {
                MyEllipse.Fill = Brushes.White;
                MyEllipse.Stroke = Brushes.Black;
                MyEllipse.StrokeThickness = 1;
            }
        }

        public Node(Network network, Point center, string text)
        {
            Network = network;
            Center = center;
            Text = text;
            Links = new List<Link>();
            Index = -1;
            network.AddNode(this);
            IsEndNode = false;
            IsStartNode = false;
        }

        public override string ToString()
        {
            return $"[{Text}]";
        }

        public void AddLink(Link link)
        {
            Links.Add(link);
        }

        public void Draw(Canvas mainCanvas, bool drawLabels)
        {
            const int LARGE_RADIUS = 10;
            const int SMALL_RADIUS = 3;
            var radius = drawLabels ? LARGE_RADIUS : SMALL_RADIUS;
            Rect bounds = new Rect(Center.X - radius, Center.Y - radius, 2 * radius, 2 * radius);
            MyEllipse = mainCanvas.DrawEllipse(bounds, Brushes.White, Brushes.Black, 1);
            MyEllipse.Tag = this;
            MyEllipse.MouseDown += Network.ellipse_MouseDown;
            MyLabel = mainCanvas.DrawString(Text, radius, radius, Center, 0, radius, Brushes.Black);
            MyLabel.Tag = this;
            MyLabel.MouseDown += Network.label_MouseDown;
        }
    }
}
