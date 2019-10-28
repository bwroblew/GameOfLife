using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class GameSettings
    {
        private static GameSettings instance = null;
        public static GameSettings getInstance()
        {
            if (instance == null)
                instance = new GameSettings();
            return instance;
        }

        private GameSettings()
        {
            BoardHeight = defaultBoardHeight;
            BoardWidth = defaultBoardWidth;
            BlockSize = defaultBlockSize;
        }

        private int boardHeight;
        private int defaultBoardHeight = 10;

        private int boardWidth;
        private int defaultBoardWidth = 10;

        private int blockSize;
        private int defaultBlockSize = 50;

        public const float randomDensity = 0.1f;

        private int minNeighboursToLive = 2;
        private int maxNeighboursToLive = 3;
        private int neighboursToReproduce = 3;

        public int BoardHeight
        {
            get => boardHeight;
            set
            {
                boardHeight = value;
                AdjustBlockSize();
            }
        }
        public int BoardWidth
        {
            get => boardWidth;
            set
            {
                boardWidth = value;
                AdjustBlockSize();
            }
        }

        public int BlockSize { get => blockSize; set => blockSize = value; }
        public int MinNeighboursToLive { get => minNeighboursToLive; set => minNeighboursToLive = value; }
        public int MaxNeighboursToLive { get => maxNeighboursToLive; set => maxNeighboursToLive = value; }
        public int NeighboursToReproduce { get => neighboursToReproduce; set => neighboursToReproduce = value; }

        private void AdjustBlockSize()
        {
            float scaleRatio = Math.Max((float)BoardHeight / defaultBoardHeight,
                                        (float)BoardWidth / defaultBoardWidth);
            BlockSize = Math.Max((int)(defaultBlockSize / scaleRatio), 4);
        }

    }
}
