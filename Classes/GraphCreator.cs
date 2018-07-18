using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathGame.Classes
{
    class GraphCreator
    {
        public static void Push(ref AdjacencyList head, int where, int weight)
        {
            AdjacencyList newNode = new AdjacencyList(where, weight, head);
            head = newNode;
        }

        public static void Pop(ref AdjacencyList head)
        {
            AdjacencyList tempNode = head;
            head = tempNode.tail;
        }
    }
}
