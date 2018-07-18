using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathGame.Classes
{
    class AdjacencyList
    {
        public int where;
        public int weight;
        public AdjacencyList tail;

        public AdjacencyList(int where, int weight, AdjacencyList tail)
        {
            this.where = where;
            this.weight = weight;
            this.tail = tail;
        }
    }
}
