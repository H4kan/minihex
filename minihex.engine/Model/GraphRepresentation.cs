using minihex.engine.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
            else
            {
                TargetMask |= 0b1100;
            }
        }


        private void AddEdge(int i, int j)
        {
            this.Edges[i,j] = 1;
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
                if (i >= Size) {
                    this.AddEdge(i, i - Size);
                }
                if (i % Size > 0) {
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
                else if (i % Size == Size -1)
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

        private void ColorVertice(int i, PlayerColor color)
        {
            this.Vertices[i] = color;
        }

        // move all edges from neighbouring colored to new colored and "delete" all colored neighbours
        public void ColorVerticeAndReduce(int i, PlayerColor color)
        { 
            this.ColorVertice(i, color);
            
            // isolate diff color vertice and kill it
            if (color != TargerColor)
            {
                for (int j = 0; j < Size * Size; j++)
                {
                    if (!DeadVertice[j] && Edges[i, j] == 1)
                    {
                        this.RemoveEdge(i, j);
                    }
                }
                this.DeadVertice[i] = true;
            }
            else
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

    }
}
