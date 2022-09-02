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

        internal enum AlgorithmTypes
        {
            LabelSetting,
            LabelCorrecting,
        }

        private AlgorithmTypes algorithmType = AlgorithmTypes.LabelSetting;
        internal AlgorithmTypes AlgorithmType
        {
            get
            {
                return algorithmType;
            }
            set
            {
                // Save the new value.
                algorithmType = value;

                // Use the newly selected algorithm to
                // check for a tree and path.
                CheckForPath();
            }
        }

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
            StartNode = null;
            EndNode = null;
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
            if (node == null) return;

            // Update the start/end node.
            if (e.ChangedButton == MouseButton.Left)
            {
                // Update the start node.
                // Deselect the previous start node.
                if (StartNode != null) StartNode.IsStartNode = false;

                // Select the new start node.
                StartNode = node;
                StartNode.IsStartNode = true;
            }
            else
            {
                // Update the end node.
                // Deselect the previous end node.
                if (EndNode != null) EndNode.IsEndNode = false;

                // Select the new end node.
                EndNode = node;
                EndNode.IsEndNode = true;
            }

            // Check for shortest paths.
            CheckForPath();
        }

        internal void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            var node = label!.Tag as Node;
            NodeClicked(node, e);
        }

        private void CheckForPath()
        {
            if (StartNode != null)
            {
                switch (AlgorithmType)
                {
                    case AlgorithmTypes.LabelSetting:
                        FindPathTreeLabelSetting();
                        break;
                    case AlgorithmTypes.LabelCorrecting:
                        FindPathTreeLabelCorrecting();
                        break;
                    default:
                        break;
                }

                if (EndNode != null)
                {
                    FindPath();
                }
            }
        }

        // Build a shortest path tree rooted at the start node.
        private void FindPathTreeLabelCorrecting()
        {
            if (StartNode == null) return;

            // Reset all nodes and links.
            foreach (Node node in Nodes)
            {
                node.TotalCost = double.PositiveInfinity;
                node.IsInPath = false;
                node.ShortestPathLink = null;
            }
            foreach (Link link in Links)
            {
                link.IsInTree = false;
                link.IsInPath = false;
            }

            // Place the start node on the candidate list.
            StartNode.TotalCost = 0;
            List<Node> candidateList = new List<Node>();
            candidateList.Add(StartNode);

            // Process the candidate list until it is empty.
            // See https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm.
            int numPops = 0;
            while (candidateList.Count > 0)
            {
                // Process the first item in the candidate list.
                Node bestCandidate = candidateList[0];
                candidateList.RemoveAt(0);
                numPops++;

                // Check this node's links.
                foreach (Link link in bestCandidate.Links)
                {
                    // Get the node at the other end of this link.
                    Node otherNode = link.ToNode;

                    // See if we can improve the other node's total cost.
                    double newTotalCost = bestCandidate.TotalCost + link.Cost;
                    if (newTotalCost < otherNode.TotalCost)
                    {
                        otherNode.TotalCost = newTotalCost;
                        otherNode.ShortestPathLink = link;

                        // Add the other node to the candidate list.
                        candidateList.Add(otherNode);
                    }
                }
            }

            // Print stats.
            Console.WriteLine(string.Format("Pops: {0}", numPops));

            // Set IsInTree for links in the shortest path tree.
            foreach (Node node in Nodes)
            {
                if (node.ShortestPathLink != null)
                    node.ShortestPathLink.IsInTree = true;
            }
        }

        // Build a shortest path tree rooted at the start node.
        private void FindPathTreeLabelSetting()
        {
            if (StartNode == null) return;

            // Reset all nodes and links.
            foreach (Node node in Nodes)
            {
                node.TotalCost = double.PositiveInfinity;
                node.IsInPath = false;
                node.ShortestPathLink = null;
                node.Visited = false;
            }
            foreach (Link link in Links)
            {
                link.IsInTree = false;
                link.IsInPath = false;
            }

            // Place the start node on the candidate list.
            StartNode.TotalCost = 0;
            List<Node> candidateList = new List<Node>();
            candidateList.Add(StartNode);

            // Process the candidate list until it is empty.
            // See https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm.
            int numPops = 0;
            int numChecks = 0;
            while (candidateList.Count > 0)
            {
                // Find the candidate with the smallest totalCost.
                double bestTotalCost = double.PositiveInfinity;
                int bestIndex = -1;
                for (int index = 0; index < candidateList.Count; index++)
                {
                    numChecks++;
                    if (bestTotalCost > candidateList[index].TotalCost)
                    {
                        bestTotalCost = candidateList[index].TotalCost;
                        bestIndex = index;
                    }
                }

                // Process the best candidate.
                Node bestCandidate = candidateList[bestIndex];
                candidateList.RemoveAt(bestIndex);
                bestCandidate.Visited = true;
                numPops++;

                // Check this node's links.
                foreach (Link link in bestCandidate.Links)
                {
                    // Get the node at the other end of this link.
                    Node otherNode = link.ToNode;
                    if (otherNode.Visited) continue;

                    // See if we can improve the other node's totalCost.
                    double newTotalCost = bestCandidate.TotalCost + link.Cost;
                    if (newTotalCost < otherNode.TotalCost)
                    {
                        otherNode.TotalCost = newTotalCost;
                        otherNode.ShortestPathLink = link;

                        // Add the other node to the candidate list.
                        candidateList.Add(otherNode);
                    }
                }
            }

            // Print stats.
            Console.WriteLine(string.Format("Checks: {0}", numChecks));
            Console.WriteLine(string.Format("Pops:   {0}", numPops));

            // Set IsInTree for links in the shortest path tree.
            foreach (Node node in Nodes)
            {
                if (node.ShortestPathLink != null)
                    node.ShortestPathLink.IsInTree = true;
            }
        }
        private void FindPath()
        {
            if (StartNode == null || EndNode == null) return;

            // If there is no path between the start and end nodes, return.
            if (EndNode.ShortestPathLink == null) return;

            // Follow the path backwards from the end node to the start node.
            Node node = EndNode;
            while (node != StartNode && node != null)
            {
                // Mark this node's shortest path link.
                if (node.ShortestPathLink != null)
                {
                    node.ShortestPathLink.IsInPath = true;
                    node = node.ShortestPathLink.FromNode;
                }
            }
            Console.WriteLine(string.Format("Total cost: {0}", EndNode.TotalCost));
        }
    }
}
