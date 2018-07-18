using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShortestPathGame.Classes
{
    class GraphDrawUtil
    {
        private const int MAX_VERTICES = 15;
        private Canvas myCanvas;
        private Point[] points = new Point[MAX_VERTICES];
        private Border[] vertices = new Border[MAX_VERTICES];
        private Line[] lines = new Line[22];
        private string[] lineText = new string[22];
        private int amountOfVertices = 0;
        private int amountOfLines = 0;

        public int result;

        AdjacencyList[] graph = new AdjacencyList[MAX_VERTICES];
        int[] weights = new int[22];

        public GraphDrawUtil(Canvas myCanvas)
        {
            this.myCanvas = myCanvas;

            Random rnd = new Random();
            for (int i = 0, x = 50, y = 20; i < MAX_VERTICES; i++, x+=150)
            {
                if (x > 650)
                {
                    y += 150;
                    x = 50;
                }

                points[i] = new Point(x, y);

                int rand = rnd.Next(1, 11);
                if (i > 0 && i%5 != 0)
                {
                    CreateLine(points[i], points[i - 1]);
                    GraphCreator.Push(ref graph[i-1], i, rand);
                    GraphCreator.Push(ref graph[i], i-1, rand);
                    weights[amountOfLines - 1] = rand; 
                }

                if (i >= 5)
                {
                    CreateLine(points[i], points[i-5]);
                    GraphCreator.Push(ref graph[i-5], i, rand);
                    GraphCreator.Push(ref graph[i], i - 5, rand);
                    weights[amountOfLines - 1] = rand;
                }
            } 
 
            char symbol = 'A';
            for (int i = 0; i < MAX_VERTICES; i++)
            {
                AddText(points[i], symbol.ToString());
                symbol++;
            }

            ChangeColorOfTwoRandomVertices(graph);

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (lines[i] != null)
                {
                    AddText(lines[i], weights[i].ToString());
                }
            }
        }

        private void CreateLine(Point p1, Point p2)
        {
            lines[amountOfLines] = new Line
            {
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2.0,
                X1 = p1.X + 15,
                X2 = p2.X + 15,
                Y1 = p1.Y + 15,
                Y2 = p2.Y + 15
            };
            myCanvas.Children.Add(lines[amountOfLines]);
            amountOfLines++;
        }

        private void ChangeColor(Line l, int c1, int c2)
        {
            l.Stroke = new SolidColorBrush(Colors.LightGreen);
            vertices[c1].Background = Brushes.LightGreen;
            TextBlock tb = (TextBlock)vertices[c1].Child;
            tb.Foreground = Brushes.Black;
            vertices[c2].Background = Brushes.LightGreen;
        }

        private void ChangeColorOfTwoRandomVertices(AdjacencyList[] graph)
        {
            Random rnd = new Random();
            int randomVertex1 = rnd.Next(0, MAX_VERTICES - 1);
            vertices[randomVertex1].Background = Brushes.LightGreen;
            int randomVertex2;
            do
            {
                randomVertex2 = rnd.Next(0, MAX_VERTICES - 1);
            } while (randomVertex2 == randomVertex1);
            
            vertices[randomVertex2].Background = Brushes.LightGreen;

            result = DjikstraShortestPath.Solve(graph, randomVertex1, randomVertex2, MAX_VERTICES);
        }

        private void AddText(Line l, string text)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Black)
            };


            Border myBorder1 = new Border
            {
                BorderBrush = Brushes.Black,
                Background = Brushes.White,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(2),
                Padding = new Thickness(6, 3, 6, 3),
                Child = textBlock
            };
            Canvas.SetLeft(myBorder1, (l.X1 + l.X2) / 2 - 15);
            Canvas.SetTop(myBorder1, (l.Y1 + l.Y2) / 2 - 15);

            myCanvas.Children.Add(myBorder1);
        }

        private void AddText(Point point, string text)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White)
            };


            Border myBorder1 = new Border
            {
                BorderBrush = Brushes.Black,
                Background = Brushes.Black,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(90),
                Padding = new Thickness(10, 5, 10, 5),
                Child = textBlock
            };
            Canvas.SetLeft(myBorder1, point.X);
            Canvas.SetTop(myBorder1, point.Y);

            myCanvas.Children.Add(myBorder1);
            vertices[amountOfVertices] = myBorder1;
            amountOfVertices++;
        }
    }
}
