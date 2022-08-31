using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace NetworkClasses
{
    public class Link
    {
        public Network Network { get; set; }
        public Node FromNode { get; set; }
        public Node ToNode { get; set; }
        public int Cost { get; set; }
        public Line? MyLine { get; set; }
        private bool isInTree;

        public bool IsInTree
        {
            get { return isInTree; }
            set
            {
                isInTree = value;
                SetLinkAppearance();
            }
        }

        private bool isInPath;

        public bool IsInPath
        {
            get { return isInPath; }
            set
            {
                isInPath = value;
                SetLinkAppearance();
            }
        }

        private void SetLinkAppearance()
        {
            if (MyLine == null)
            { 
                return; 
            }
            else if (IsInPath)
            {
                MyLine.Stroke = Brushes.Lime;
                MyLine.StrokeThickness = 6;
            }
            else if (IsInTree)
            {
                MyLine.Stroke = Brushes.Red;
                MyLine.StrokeThickness = 6;
            }
            else
            {
                MyLine.Stroke = Brushes.Black;
                MyLine.StrokeThickness = 1;
            }
        }

        public Link(Network network, Node fromNode, Node toNode, int cost)
        {
            Network = network;
            FromNode = fromNode;
            ToNode = toNode;
            Cost = cost;
            Network.AddLink(this);
            FromNode.AddLink(this);
            IsInTree = false;
            isInPath = false;
        }

        public override string ToString()
        {
            return $"[{FromNode}] --> [{ToNode}] ({Cost})";
        }

        internal void Draw(Canvas mainCanvas)
        {
            MyLine = mainCanvas.DrawLine(FromNode.Center, ToNode.Center, Brushes.Black, 1);
        }

        internal void DrawLabel(Canvas mainCanvas)
        {
            const int RADIUS = 10;
            var angleDegrees = FromNode.Center.GetAngleOfLine(ToNode.Center);
            var labelPoint = FromNode.Center.GetPointFromStart(ToNode.Center, 0.33);
            Rect bounds = new Rect(labelPoint.X - RADIUS, labelPoint.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
            mainCanvas.DrawEllipse(bounds, Brushes.White, Brushes.White, 1);
            mainCanvas.DrawString(Cost.ToString(), RADIUS, RADIUS, labelPoint, angleDegrees, RADIUS, Brushes.Black);
        }
    }
}
