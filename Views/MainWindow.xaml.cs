using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        private GUIContext guic = null;
        private Button[,] boardCells;

        public MainWindow()
        {
            guic = new GUIContext
            {
                Height = GameSettings.getInstance().BoardHeight,
                Width = GameSettings.getInstance().BoardWidth,
                GenNumber = 1,
                MinNeighb = GameSettings.getInstance().MinNeighboursToLive,
                MaxNeighb = GameSettings.getInstance().MaxNeighboursToLive,
                ToReproduce = GameSettings.getInstance().NeighboursToReproduce,
            };

            this.InitializeComponent();
            InitializeGUI();
            GameManager.getInstance().InitializeGame();
        }

        private void InitializeGUI()
        {
            InitializeBoard();
            this.DataContext = guic;
        }

        private void InitializeBoard()
        {
            GridLengthConverter myGridLengthConverter = new GridLengthConverter();
            GridLength side = (GridLength)myGridLengthConverter.ConvertFromString("Auto");
            int rows = GameSettings.getInstance().BoardHeight;
            int cols = GameSettings.getInstance().BoardWidth;
            for (int row = 0; row < rows; row++)
            {
                LayoutRoot.RowDefinitions.Add(new RowDefinition());
                LayoutRoot.RowDefinitions[row].Height = side;
            }

            for (int col = 0; col < cols; col++)
            {
                LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
                LayoutRoot.ColumnDefinitions[col].Width = side;
            }

            boardCells = new Button[rows, cols];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                {
                    boardCells[row, col] = new Button();
                    boardCells[row, col].Height = GameSettings.getInstance().BlockSize;
                    boardCells[row, col].Width = GameSettings.getInstance().BlockSize;
                    boardCells[row, col].Click += CellButton_Click;
                    Grid.SetColumn(boardCells[row, col], col);
                    Grid.SetRow(boardCells[row, col], row);
                    boardCells[row, col].Style = Application.Current.FindResource("ButtonWhite") as Style;
                    LayoutRoot.Children.Add(boardCells[row, col]);
                }
        }

        private void UpdateBoard()
        {
            var livingCells = GameManager.getInstance().world.GetCurrentState();
            for (int row = 0; row < livingCells.GetLength(0); row++)
            {
                for (int col = 0; col < livingCells.GetLength(1); col++)
                {
                    if (livingCells[row, col])
                    {
                        boardCells[row, col].Style = Application.Current.FindResource("ButtonBlack") as Style;
                    }
                    else
                    {
                        boardCells[row, col].Style = Application.Current.FindResource("ButtonWhite") as Style;
                    }
                }
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            GameSettings.getInstance().BoardHeight = guic.Height;
            GameSettings.getInstance().BoardWidth = guic.Width;
            GameSettings.getInstance().MinNeighboursToLive = guic.MinNeighb;
            GameSettings.getInstance().MaxNeighboursToLive = guic.MaxNeighb;
            GameSettings.getInstance().NeighboursToReproduce = guic.ToReproduce;
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }

        private void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            GameManager.getInstance().world.AddRandomLivings();
            UpdateBoard();
        }

        private void NextGenButton_Click(object sender, RoutedEventArgs e)
        {
            GameManager.getInstance().world.SimulateGeneration();
            UpdateBoard();
        }

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int clickedRow = (int)button.GetValue(Grid.RowProperty);
            int clickedCol = (int)button.GetValue(Grid.ColumnProperty);
            GameManager.getInstance().world.ChangeCellState(clickedRow, clickedCol);
            UpdateBoard();
        }

        private void MulGenButton_Click(object sender, RoutedEventArgs e)
        {
            for (int gen = 0; gen < guic.GenNumber; gen++)
            {
                GameManager.getInstance().world.SimulateGeneration();
                UpdateBoard();
                Dispatcher.Invoke(new Action(() => Thread.Sleep(100)), DispatcherPriority.Background);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var res = GameManager.getInstance().Save();
            if (!res)
                MessageBox.Show("Problem with saving game...");
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var res = GameManager.getInstance().Load();
            if (!res)
                MessageBox.Show("Problem with loading game...");
            UpdateBoard();
        }
    }

    public class GUIContext
    {
        private int height;
        public int Height
        {
            get { return height; }
            set
            {
                if (value >= 0) height = value;
                else height = 0;
            }
        }

        private int width;
        public int Width
        {
            get { return width; }
            set
            {
                if (value >= 0) width = value;
                else width = 0;
            }
        }

        public int GenNumber { get => genNumber; set => genNumber = value; }
        public int MinNeighb { get => minNeighb; set => minNeighb = value; }
        public int MaxNeighb { get => maxNeighb; set => maxNeighb = value; }
        public int ToReproduce { get => toReproduce; set => toReproduce = value; }

        private int genNumber;
        private int minNeighb;
        private int maxNeighb;
        private int toReproduce;
    }

}
