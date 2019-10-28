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

        private int boardHeight;
        private int defaultBoardHeight = 10;

        private int boardWidth;
        private int defaultBoardWidth = 10;

        private int blockSize;
        private int defaultBlockSize = 50;

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

        private GameSettings()
        {
            BoardHeight = defaultBoardHeight;
            BoardWidth = defaultBoardWidth;
            BlockSize = defaultBlockSize;
        }

        private void AdjustBlockSize()
        {
            float scaleRatio = Math.Max((float)BoardHeight / defaultBoardHeight,
                                        (float)BoardWidth / defaultBoardWidth);
            BlockSize = Math.Max((int)(defaultBlockSize / scaleRatio), 4);
        }

    }
}
