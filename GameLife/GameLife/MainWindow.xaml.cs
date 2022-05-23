using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameLife
{
    public partial class MainWindow : Window
    {
        int k = 0;
        bool finish; // флаг, предназначенный для прекращения игры
        Button[,] system = new Button[20, 20]; // двухмерный массив, состоящий из квадратиков (кнопок)
        bool[,,] mass = new bool[20, 20, 2]; // двухмерный массив, который показывает, какие квадратики являются зелеными (последний параметр содержит копию массива)
        public MainWindow()
        {
            InitializeComponent();
            BuildGrid(20);
        }
        void BuildGrid(int n) // метод для построения сетки
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
        int GreenSosediCount(int i, int j) // метод для подсчета количества соседних зеленых квадратиков
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
        private void Button_Click(object sender, RoutedEventArgs e) // метод, позволяющий пользователю выбрать зеленые квадратики
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
        void Step() // метод, определяющий цвет квадратиков в следующем шаге
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
        private async void btnStart_Click(object sender, RoutedEventArgs e) // метод для начала или продолжения игры
        {
            finish = false;
            while (!finish)
            {
                Step();
                k = 1 - k;
                await Task.Delay(200);
            }
        }
        private void btnStop_Click(object sender, RoutedEventArgs e) // метод для остановки игры
        {
            finish = true;
        }
    }
}
