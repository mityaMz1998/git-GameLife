using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameLife
{
    public partial class MainWindow : Window
    {
        int k = 0;
        bool finish; // flag designed to stop the game
        Button[,] system = new Button[20, 20]; // a two-dimensional array consisting of squares (buttons)
        bool[,,] mass = new bool[20, 20, 2]; // a two-dimensional array that shows which squares are green (the last parameter contains a copy of the array)
        public MainWindow()
        {
            InitializeComponent();
            BuildGrid(20);
        }
        void BuildGrid(int n) // method for building a grid
        {
            var grid = new Grid();
            grid1.Children.Add(grid);
            for (int i = 0; i < n; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());

                for (int j = 0; j < n; j++)
                {
                    var button = new Button();
                    button.BorderBrush = Brushes.Gray;
                    button.Background = Brushes.Black;
                    button.BorderThickness = new Thickness(1);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    system[i, j] = button;
                    grid.Children.Add(button);
                    button.Click += Button_Click;
                }
            }
        }
        int GreenSosediCount(int i, int j) // a method for counting the number of adjacent green squares
        {
            int sosedi = 0;
            if (i > 0 && mass[i - 1, j, k] == true) { sosedi++; }
            if (i > 0 && j < 19 && mass[i - 1, j + 1, k] == true) { sosedi++; }
            if (j < 19 && mass[i, j + 1, k] == true) { sosedi++; };
            if (i < 19 && j < 19 && mass[i + 1, j + 1, k] == true) { sosedi++; }
            if (i < 19 && mass[i + 1, j, k] == true) { sosedi++; }
            if (i < 19 && j > 0 && mass[i + 1, j - 1, k] == true) { sosedi++; }
            if (j > 0 && mass[i, j - 1, k] == true) { sosedi++; }
            if (i > 0 && j > 0 && mass[i - 1, j - 1, k] == true) { sosedi++; }
            return sosedi;
        }
        private void Button_Click(object sender, RoutedEventArgs e) // method that allows the user to select green squares
        {
            if ((sender as Button).Background != Brushes.LightGreen)
                (sender as Button).Background = Brushes.LightGreen;
            else
                (sender as Button).Background = Brushes.Black;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (sender == system[i, j])
                        mass[i, j, k] = true;
                }
            }
        }
        void Step() // the method that determines the color of the squares in the next step
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (GreenSosediCount(i, j) == 3 && !mass[i, j, k])
                    {
                        mass[i, j, 1 - k] = true;
                        system[i, j].Background = Brushes.LightGreen;
                    }
                    else if (GreenSosediCount(i, j) < 2 && mass[i, j, k])
                    {
                        mass[i, j, 1 - k] = false;
                        system[i, j].Background = Brushes.Black;
                    }
                    else if (GreenSosediCount(i, j) > 3 && mass[i, j, k])
                    {
                        mass[i, j, 1 - k] = false;
                        system[i, j].Background = Brushes.Black;
                    }
                    else
                        mass[i, j, 1 - k] = mass[i, j, k];
                }
            }
        }
        private async void btnStart_Click(object sender, RoutedEventArgs e) // method for starting or continuing the game
        {
            finish = false;
            while (!finish)
            {
                Step();
                k = 1 - k;
                await Task.Delay(200);
            }
        }
        private void btnStop_Click(object sender, RoutedEventArgs e) // method for stopping the game
        {
            finish = true;
        }
    }
}
