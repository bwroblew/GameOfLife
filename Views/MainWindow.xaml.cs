using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        private GUIContext guic = null;
        public MainWindow()
        {
            guic = new GUIContext
            {
                Height = GameSettings.getInstance().BoardHeight,
                Width = GameSettings.getInstance().BoardWidth
            };

            this.InitializeComponent();
            InitializeGUI();
        }

        private void InitializeGUI()
        {
            InitializeChessboard();
            this.DataContext = guic;
        }

        private void InitializeChessboard()
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

            Button[,] square = new Button[rows, cols];
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                {
                    square[row, col] = new Button();
                    square[row, col].Height = GameSettings.getInstance().BlockSize;
                    square[row, col].Width = GameSettings.getInstance().BlockSize;
                    Grid.SetColumn(square[row, col], col);
                    Grid.SetRow(square[row, col], row);
                    if ((row + col) % 2 == 0)
                    {
                        square[row, col].Style = Application.Current.FindResource("ButtonGreen") as Style;
                    }
                    else
                    {
                        square[row, col].Style = Application.Current.FindResource("ButtonRed") as Style;
                    }
                    LayoutRoot.Children.Add(square[row, col]);
                }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            GameSettings.getInstance().BoardHeight = guic.Height;
            GameSettings.getInstance().BoardWidth = guic.Width;
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
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
    }

}
