using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace NetworkClasses
{
    public class Network
    {
        public List<Node> Nodes { get; set; }
        public List<Link> Links { get; set; }
        public Node? StartNode { get; set; }
        public Node? EndNode { get; set; }
        private const int CANVAS_EXTRA = 10;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Network()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Clear();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Network(string filename) : base()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            ReadFromFile(filename);
        }

        public void Clear()
        {
            Nodes = new List<Node>();
            Links = new List<Link>();
        }

        public void AddNode(Node node)
        {
            node.Index = Nodes.Count();
            Nodes.Add(node);
        }

        public void AddLink(Link link)
        {
            Links.Add(link);
        }

        public string Serialization()
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine($"{Nodes.Count} # Num nodes.");
            output.AppendLine($"{Links.Count} # Num links");
            output.AppendLine("# Nodes.");
            foreach (var node in Nodes)
            {
                output.AppendLine($"{node.Center.X},{node.Center.Y},{node.Text}");
            }
            output.AppendLine("# Links.");
            foreach (var link in Links)
            {
                output.AppendLine($"{link.FromNode.Index},{link.ToNode.Index},{link.Cost}");
            }

            return output.ToString();
        }

        public void Deserialize(string network)
        {
            Clear();
            using (var reader = new StringReader(network))
            {
                int nodes = 0;
                int links = 0;

                var nodesString = ReadNextLine(reader);
                if (nodesString != null) nodes = int.Parse(nodesString);
                var linksString = ReadNextLine(reader);
                if (linksString != null) links = int.Parse(linksString);

                for (int i = 0; i < nodes; i++)
                {
                    string[] nodeEntryArray = new string[0];
                    var nodeEntryString = ReadNextLine(reader);
                    if (nodeEntryString != null) nodeEntryArray = nodeEntryString.Split(',');
                    if (nodeEntryArray.Length == 3)
                    {
                        var point = new Point(int.Parse(nodeEntryArray[0]), int.Parse(nodeEntryArray[1]));
                        var node = new Node(this, point, nodeEntryArray[2]);
                    }
                }

                for (int i = 0; i < links; i++)
                {
                    string[] linkEntryArray = new string[0];
                    var linkEntryString = ReadNextLine(reader);
                    if (linkEntryString != null) linkEntryArray = linkEntryString.Split(',');
                    if (linkEntryArray.Length == 3)
                    {
                        var nodeIndex = int.Parse(linkEntryArray[0]);
                        Node? fromNode = null;
                        Node? toNode = null;
                        Link? link = null;
                        if (nodeIndex <= Nodes.Count + 1) fromNode = Nodes[nodeIndex];
                        nodeIndex = int.Parse(linkEntryArray[1]);
                        if (nodeIndex <= Nodes.Count) toNode = Nodes[nodeIndex];
                        if (fromNode != null && toNode != null) link = new Link(this, fromNode, toNode, int.Parse(linkEntryArray[2]));
                    }
                }
            }
        }

        public string? ReadNextLine(StringReader reader)
        {
            var line = reader.ReadLine();
            while (line != null)
            {
                var index = line.IndexOf('#');
                if (index != -1) line = line.Substring(0, index);
                line = line.Trim();
                if (line.Length > 0) return line;
                line = reader.ReadLine();
            }
            return null;
        }

        public void SaveIntoFile(string filename)
        {
            var network = Serialization();
            File.WriteAllText(filename, network);
        }

        public void ReadFromFile(string filename)
        {
            var fileContent = File.ReadAllText(filename);
            if (fileContent != null) Deserialize(fileContent);
        }

        public void Draw(Canvas mainCanvas)
        {
            var bounds = GetBounds();
            if (bounds == null) return;
            mainCanvas.Height = bounds.Value.Height + CANVAS_EXTRA;
            mainCanvas.Width = bounds.Value.Width + CANVAS_EXTRA;
            bool drawLabels = (Nodes?.Count < 100) ? true : false;

            foreach (var link in Links)
            {
                link.Draw(mainCanvas);
            }

            if (drawLabels)
                foreach (var link in Links)
                {
                    link.DrawLabel(mainCanvas);
                }

            foreach (var node in Nodes!)
            {
                node.Draw(mainCanvas, drawLabels);
            }
        }

        public Rect? GetBounds()
        {
            double minX, minY, maxX, maxY;

            if (Nodes.Count == 0) return null;
            minX = maxX = Nodes[0].Center.X;
            minY = maxY = Nodes[0].Center.Y;

            foreach (var node in Nodes)
            {
                if (node.Center.X < minX) minX = node.Center.X;
                if (node.Center.X > maxX) maxX = node.Center.X;
                if (node.Center.Y < minY) minY = node.Center.Y;
                if (node.Center.Y > maxY) maxY = node.Center.Y;
            }

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        internal void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ellipse = sender as Ellipse;
            var node = ellipse!.Tag as Node;
            NodeClicked(node, e);
        }

        private void NodeClicked(Node? node, MouseButtonEventArgs e)
        {
            Control? src = e.Source as Control;
            Node? oldNode = null;
            bool left = false;
            bool right = false;
            if (src == null || node == null) return;

            if (e.ChangedButton == MouseButton.Left)
            {
                left = true;
                oldNode = StartNode;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                right = true;
                oldNode = EndNode;
            }

            if (oldNode != null)
            {
                foreach (var link in oldNode.Links)
                {
                    if (left) link.IsInPath = false;
                    else if (right) link.IsInTree = false;
                }

                if (left) oldNode.IsStartNode = false;
                else if (right) oldNode.IsEndNode = false;
            }

            foreach (var link in node.Links)
            {
                if (left) link.IsInPath = true;
                else if (right) link.IsInTree = true;
            }

            if (left)
            {
                node.IsStartNode = true;
                StartNode = node;
            }
            else if (right)
            {
                node.IsEndNode = true;
                EndNode = node;
            }
        }

        internal void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            var node = label!.Tag as Node;
            NodeClicked(node, e);
        }
    }
}
