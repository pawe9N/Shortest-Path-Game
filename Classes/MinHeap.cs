using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathGame.Classes
{
    class MinHeap
    {
        public class Vertex
        {
            public int index;
            public int shortestPath;

            public Vertex(int index, int shortestPath)
            {
                this.index = index;
                this.shortestPath = shortestPath;
            }
        }

        public Vertex[] heap;
        public int amountOfElements;

        public MinHeap(int size)
        {
            heap = new Vertex[size];
            amountOfElements = 0;
        }

        public Vertex this[int i]
        {
            get
            {
                return this.heap[i];
            }
        }

        public void Add(int index, int shortestPath)
        {
            Vertex v = new Vertex(index, shortestPath);

            heap[amountOfElements] = v;

            amountOfElements++;

            int i = amountOfElements - 1;

            while (i != 0 && heap[(i - 1) / 2].shortestPath > heap[i].shortestPath)
            {
                Vertex temp = heap[i];
                heap[i] = heap[(i - 1) / 2];
                heap[(i - 1) / 2] = temp;

                i = (i - 1) / 2;
            }
        }

        private void MinHeapify(int i)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left < amountOfElements && heap[left].shortestPath < heap[i].shortestPath)
                smallest = left;
            if (right < amountOfElements && heap[right].shortestPath < heap[smallest].shortestPath)
                smallest = right;

            if (smallest != i)
            {
                Vertex temp = heap[i];
                heap[i] = heap[smallest];
                heap[smallest] = temp;

                MinHeapify(smallest);
            }
        }

        public Vertex Pop()
        {
            Vertex minimum = heap[0];
            heap[0] = heap[amountOfElements-1];
            amountOfElements--;

            MinHeapify(0);

            return minimum;
        }

    }
}
