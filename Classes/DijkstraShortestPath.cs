using System;

namespace ShortestPathGame.Classes
{
    class DijkstraShortestPath
    {
        public static int Solve(AdjacencyList[] graph, int from, int where, int numberOfVertices, int[] parent)
        {
            int[] shortestDistances = new int[numberOfVertices];
            bool[] coveredVertices = new bool[numberOfVertices];

            MinHeap heap = new MinHeap(numberOfVertices);
            parent[from] = -1;

            for (int i = 0; i < numberOfVertices; i++)
            {
                shortestDistances[i] = Int32.MaxValue;
                coveredVertices[i] = false;
            }

            shortestDistances[from] = 0;
            heap.Add(from, 0);

            while (!coveredVertices[where])
            {
                while (coveredVertices[heap[0].index])
                {
                    heap.Pop();
                }

                int minimumIndex = heap[0].index;
                heap.Pop();

                coveredVertices[minimumIndex] = true;

                AdjacencyList tempList = graph[minimumIndex];

                while (tempList != null)
                {
                    if (!coveredVertices[tempList.where]
                     && shortestDistances[minimumIndex] + tempList.weight < shortestDistances[tempList.where])
                    {
                        parent[tempList.where] = minimumIndex;
                        shortestDistances[tempList.where] = shortestDistances[minimumIndex] + tempList.weight;
                        heap.Add(tempList.where, shortestDistances[tempList.where]);
                    }

                    tempList = tempList.tail;
                }
            }

             
            return shortestDistances[where];
        }
    }
}
