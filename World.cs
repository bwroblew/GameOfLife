using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    [Serializable]
    class World
    {
        private bool[,] cellLives;
        private int rows;
        private int cols;
        private Random randGen = new Random();
        
        public World()
        {
            rows = GameSettings.getInstance().BoardHeight;
            cols = GameSettings.getInstance().BoardWidth;
            cellLives = new bool[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    cellLives[row,col] = false;
                }
            }
        }

        public void AddRandomLivings()
        {
            var cellsNumber = cellLives.Length;
            var cellsToBeBorn = randGen.Next(1, Math.Max((int)(cellsNumber * GameSettings.randomDensity), 1));
            for (int cell = 0; cell < cellsToBeBorn; cell++)
            {
                var row = randGen.Next(0, rows);
                var col = randGen.Next(0, cols);
                cellLives[row, col] = true;
            }
        }

        public bool[,] GetCurrentState()
        {
            return cellLives;
        }

        public void SimulateGeneration()
        {
            bool[,] nextGenCells = new bool[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (cellLives[row, col])
                    {
                        if (IsOverpopulated(row, col) || IsUnderpopulated(row, col))
                            nextGenCells[row, col] = false;
                        else
                            nextGenCells[row, col] = true;
                    }
                    else
                    {
                        if (WillBeBorn(row, col))
                            nextGenCells[row, col] = true;
                        else
                            nextGenCells[row, col] = false;
                    }
                }
            }
            cellLives = nextGenCells;
        }

        public void ChangeCellState(int row, int col)
        {
            cellLives[row, col] = !cellLives[row, col];
        }

        private int GetNeighboursCount(int row, int col)
        {
            int neighbours = 0;
            for (int y = Math.Max(row - 1, 0); y < Math.Min(row + 2, rows); y++)
            {
                for (int x = Math.Max(col - 1, 0); x < Math.Min(col + 2, cols); x++)
                {
                    if (!(y == row && x == col) && cellLives[y, x])
                    {
                        neighbours++;
                    }
                }
            }
            return neighbours;
        }

        private bool IsUnderpopulated(int row, int col)
        {
            if (GetNeighboursCount(row, col) < GameSettings.getInstance().MinNeighboursToLive)
                return true;
            return false;
        }

        private bool IsOverpopulated(int row, int col)
        {
            if (GetNeighboursCount(row, col) > GameSettings.getInstance().MaxNeighboursToLive)
                return true;
            return false;
        }

        private bool WillBeBorn(int row, int col)
        {
            if (GetNeighboursCount(row, col) == GameSettings.getInstance().NeighboursToReproduce)
                return true;
            return false;
        }

        public void Resize(int ySize, int xSize)
        {
            bool[,] newCells = new bool[ySize, xSize];
            for (int row = 0; row < ySize; row++)
            {
                for (int col = 0; col < xSize; col++)
                {
                    if (row < rows && col < cols)
                    {
                        newCells[row, col] = cellLives[row, col];
                    } else
                    {
                        newCells[row, col] = false;
                    }
                }
            }
            cellLives = newCells;
        }
        
    }
}
