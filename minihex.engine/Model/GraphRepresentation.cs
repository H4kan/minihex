using minihex.engine.Model.Enums;
using QuikGraph;
using QuikGraph.Algorithms;

namespace minihex.engine.Model
{
    public class GraphRepresentation
    {
        private byte[,] Edges { get; set; }

        private PlayerColor[] Vertices { get; set; }


        // 0001 is left side
        // 0010 is right side
        // 0100 is up side
        // 1000 is down side
        private byte[] SideVertices { get; set; }

        // this is used to speed up things a little bit
        private bool[] DeadVertice { get; set; }

        private int Size { get; set; }

        private PlayerColor TargerColor { get; set; }
        private byte TargetMask { get; set; }
        private byte LeftMask { get; set; }
        private byte RightMask { get; set; }

        private List<int>? WinningPath { get; set; }

        public GraphRepresentation(int size, PlayerColor targerColor)
        {
            this.Edges = new byte[size * size, size * size];
            this.DeadVertice = new bool[size * size];
            this.Vertices = new PlayerColor[size * size];
            this.SideVertices = new byte[size * size];
            this.Size = size;

            this.SetupEmptyBoard();
            TargerColor = targerColor;

            if (this.TargerColor == PlayerColor.White)
            {
                TargetMask |= 0b0011;
                LeftMask |= 0b0001;
                RightMask |= 0b0010;
            }
            else
            {
                TargetMask |= 0b1100;
                LeftMask |= 0b0100;
                RightMask |= 0b1000;
            }
        }


        private void AddEdge(int i, int j)
        {
            this.Edges[i, j] = 1;
            this.Edges[j, i] = 1;
        }

        private void RemoveEdge(int i, int j)
        {
            this.Edges[i, j] = 0;
            this.Edges[j, i] = 0;
        }


        private void SetupEmptyBoard()
        {
            for (int i = 0; i < Size * Size; i++)
            {
                if (i >= Size)
                {
                    this.AddEdge(i, i - Size);
                }
                if (i % Size > 0)
                {
                    this.AddEdge(i, i - 1);

                    if (i / Size < Size - 1)
                    {
                        this.AddEdge(i, i + Size - 1);
                    }
                }

                // left side
                if (i % Size == 0)
                {
                    this.SideVertices[i] |= 0b0001;
                }
                // right side
                else if (i % Size == Size - 1)
                {
                    this.SideVertices[i] |= 0b0010;
                }

                // up side
                if (i < Size)
                {
                    this.SideVertices[i] |= 0b0100;
                }
                // down side
                else if (i / Size == Size - 1)
                {
                    this.SideVertices[i] |= 0b1000;
                }
            }
        }

        public void ColorVertice(int i, PlayerColor color)
        {
            this.Vertices[i] = color;
            if (color != TargerColor)
            {
                this.KillVertice(i);
            }
            this.WinningPath = null;
        }

        // move all edges from neighbouring colored to new colored and "delete" all colored neighbours
        public void ColorVerticeAndReduce(int i, PlayerColor color)
        {
            this.ColorVertice(i, color);

            // isolate diff color vertice and kill it

            if (color == TargerColor)
            {
                // iterate over neighbours of i
                for (int j = 0; j < Size * Size; j++)
                {
                    if (!DeadVertice[j] && Edges[i, j] == 1 && Vertices[j] == color && i != j)
                    {
                        for (int k = 0; k < Size * Size; k++)
                        {
                            if (!DeadVertice[k] && Edges[j, k] == 1 && k != i)
                            {
                                this.AddEdge(i, k);
                                this.RemoveEdge(j, k);
                            }
                        }
                        this.SideVertices[i] |= this.SideVertices[j];
                        this.DeadVertice[j] = true;
                    }
                }
            }
        }

        public bool IsGameFinished()
        {
            for (int i = 0; i < Size * Size; i++)
            {
                if (!this.DeadVertice[i] && ((this.SideVertices[i] & TargetMask) == TargetMask))
                {
                    return true;
                }
            }

            return false;
        }

        public List<int> FindPathDestructive(bool includeNonColor)
        {
            if (this.WinningPath != null)
            {
                return this.WinningPath;
            }

            Func<Edge<int>, double> wFunc;

            if (!includeNonColor)
            {
                this.KillNonColor();

                wFunc = (e) =>
                {
                    return (this.SideVertices[e.Source] & this.SideVertices[e.Target] & TargetMask) > 0 ? 0 : 1;
                };
            }
            else
            {
                wFunc = (e) =>
                {
                    return GetWeight(e.Source, e.Target);
                };
            }

            var g = this.MakeWeightedGraph();
            var lr = this.GetLRUndead();

            var func = g.ShortestPathsDijkstra(wFunc, lr.Item1);

            func(lr.Item2, out IEnumerable<Edge<int>>? path);

            if (path == null)
            {
                throw new Exception("Path not found");
            }

            var result = new List<int>();

            bool skipFirst = true;
            int target = lr.Item2, source = lr.Item1;
            foreach (Edge<int> e in path)
            {
                target = source == e.Source ? e.Target : e.Source;

                if (skipFirst && (this.SideVertices[target] & LeftMask) == 0)
                {
                    skipFirst = false;
                }
                if (!skipFirst)
                {
                    result.Add(source);

                    if ((this.SideVertices[target] & RightMask) > 0)
                    {
                        break;
                    }
                }

                source = target;
            }

            result.Add(target);

            this.WinningPath = result;

            return this.WinningPath;
        }

        private (int, int) GetLRUndead()
        {
            var result = (0, 0);

            for (int i = 0; i < Size * Size; i++)
            {
                if (!this.DeadVertice[i])
                {
                    if ((this.SideVertices[i] & LeftMask) > 0)
                    {
                        result.Item1 = i;
                    }
                }
            }

            for (int i = 0; i < Size * Size; i++)
            {
                if (!this.DeadVertice[i])
                {
                    if ((this.SideVertices[i] & RightMask) > 0)
                    {
                        result.Item2 = i;
                    }
                }
            }

            return result;
        }

        private UndirectedGraph<int, Edge<int>> MakeWeightedGraph()
        {
            var graph = new UndirectedGraph<int, Edge<int>>(false);

            graph.AddVertexRange(Enumerable.Range(0, Size * Size));


            for (int i = 0; i < Size * Size; i++)
            {
                if (this.DeadVertice[i]) continue;
                for (int j = 0; j < Size * Size; j++)
                {
                    if (!this.DeadVertice[j] && (
                        this.Edges[i, j] == 1 || (this.SideVertices[i] & this.SideVertices[j] & TargetMask) > 0))
                    {
                        graph.AddEdge(new Edge<int>(i, j));
                    }
                }
            }

            return graph;
        }

        private byte GetWeight(int i, int j)
        {
            byte result;
            if (this.Vertices[i] == PlayerColor.None
                && this.Vertices[j] == PlayerColor.None)
                result = 2;
            else
            {
                result = 1;
            }

            if ((this.SideVertices[i] & this.TargetMask) > 0 && this.Vertices[i] == PlayerColor.None
                || (this.SideVertices[j] & this.TargetMask) > 0 && this.Vertices[j] == PlayerColor.None)
            {
                result++;
            }

            if ((this.SideVertices[i] & this.SideVertices[j] & this.TargetMask) > 0)
            {
                result = 0;
            }


            return result;
        }


        private void KillNonColor()
        {
            for (int i = 0; i < Size * Size; i++)
            {
                if (this.Vertices[i] == PlayerColor.None)
                {
                    this.KillVertice(i);
                }
            }
        }

        private void KillVertice(int vertIdx)
        {
            for (int j = 0; j < Size * Size; j++)
            {
                if (!DeadVertice[j] && Edges[vertIdx, j] == 1)
                {
                    this.RemoveEdge(vertIdx, j);
                }
            }
            this.DeadVertice[vertIdx] = true;
        }

        public void CopyIn(byte[,] edges, PlayerColor[] vertices, byte[] sideVertices, bool[] deadVertice)
        {
            for (int i = 0; i < Size * Size; i++)
            {
                for (int j = 0; j < Size * Size; j++)
                {
                    this.Edges[i, j] = edges[i, j];
                }

                this.Vertices[i] = vertices[i];
                this.SideVertices[i] = sideVertices[i];
                this.DeadVertice[i] = deadVertice[i];
            }
        }

        public GraphRepresentation CopyOut()
        {
            var gr = new GraphRepresentation(this.Size, this.TargerColor);

            gr.CopyIn(this.Edges, this.Vertices, this.SideVertices, this.DeadVertice);

            return gr;
        }
    }
}
