using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkClasses
{
    public class Network
    {
        public List<Node> Nodes { get; set; }
        public List<Link> Links { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Network()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Clear();
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

        public void SaveIntoFile(string filename)
        {
            var network = Serialization();
            File.WriteAllTest();
        }
    }
}
