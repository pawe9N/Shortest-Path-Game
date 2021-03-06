﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShortestPathGame.Classes
{
    class GraphDrawUtil
    {
        public int result;
        private Canvas myCanvas;
        
        private int MAX_VERTICES;
        private int VERTICES_IN_ROW;
        private int ROWS_OF_VERTICES;
        private int MAX_LINES;
        private int MARGIN_LEFT;
        private int MARGIN_TOP;
        private int PADDING_LEFT;

        private Point[] points;
        private Border[] vertices;
        private Line[] lines;
        private AdjacencyList[] graph;
        private int[] weights;
        private Stack<int> path;
        private int[] parent;
        private int from, where;

        private int amountOfVertices;
        private int amountOfLines;


        public GraphDrawUtil(Canvas myCanvas, int level)
        {
            this.myCanvas = myCanvas;

            switch (level)
            {
                case 1: Initialize(3, 3, 120, 40, 160); break;
                case 2: Initialize(4, 3, 25, 30, 170); break;
                case 3: Initialize(5, 3, 5, 50, 138); break;
                case 4: Initialize(4, 4, 100, 20, 120); break;
                case 5: Initialize(5, 4, 40, 20, 120); break;
            }

            MakeAndDrawGraph();
        }

        private void Initialize(int VERTICES_IN_ROW, int ROWS_OF_VERTICES, int MARGIN_LEFT, int MARGIN_TOP, int PADDING_LEFT)
        {         
            this.VERTICES_IN_ROW = VERTICES_IN_ROW;
            this.ROWS_OF_VERTICES = ROWS_OF_VERTICES;
            this.MARGIN_LEFT = MARGIN_LEFT;
            this.MARGIN_TOP = MARGIN_TOP;
            this.PADDING_LEFT = PADDING_LEFT;
            MAX_VERTICES = VERTICES_IN_ROW* ROWS_OF_VERTICES;
            MAX_LINES = MAX_VERTICES - ROWS_OF_VERTICES + MAX_VERTICES - VERTICES_IN_ROW;

            points = new Point[MAX_VERTICES];
            vertices = new Border[MAX_VERTICES];
            lines = new Line[MAX_LINES];
            graph = new AdjacencyList[MAX_VERTICES];
            weights = new int[MAX_LINES];
            path = new Stack<int>();
            parent = new int[MAX_VERTICES];

            amountOfVertices = 0;
            amountOfLines = 0;
        }

        private void MakeAndDrawGraph()
        {
            Random rnd = new Random();
            for (int i = 0, x = MARGIN_LEFT, y = MARGIN_TOP; i < MAX_VERTICES; i++, x += PADDING_LEFT)
            {
                if (x > MARGIN_LEFT + (VERTICES_IN_ROW - 1) * PADDING_LEFT)
                {
                    y += PADDING_LEFT;
                    x = MARGIN_LEFT;
                }

                points[i] = new Point(x, y);

                int rand = rnd.Next(1, 11);
                if (i > 0 && i % VERTICES_IN_ROW != 0)
                {
                    CreateLine(points[i], points[i - 1]);
                    GraphCreator.Push(ref graph[i - 1], i, rand);
                    GraphCreator.Push(ref graph[i], i - 1, rand);
                    weights[amountOfLines - 1] = rand;
                }

                rand = rnd.Next(1, 11);
                if (i >= VERTICES_IN_ROW)
                {
                    CreateLine(points[i], points[i - VERTICES_IN_ROW]);
                    GraphCreator.Push(ref graph[i - VERTICES_IN_ROW], i, rand);
                    GraphCreator.Push(ref graph[i], i - VERTICES_IN_ROW, rand);
                    weights[amountOfLines - 1] = rand;
                }
            }

            char symbol = 'A';
            for (int i = 0; i < MAX_VERTICES; i++)
            {
                AddText(points[i], symbol.ToString());
                symbol++;
            }

            InitializeDjikstraAlgorithmWithRandomVerticles(graph);

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

        private void ChangeColor(int c1, int c2)
        {
            foreach(Line line in lines)
            {
                if((points[c1].X+15 == line.X1 && points[c1].Y+15 == line.Y1 && points[c2].X + 15 == line.X2 && points[c2].Y + 15 == line.Y2)
                    || (points[c1].X + 15 == line.X2 && points[c1].Y + 15 == line.Y2 && points[c2].X + 15 == line.X1 && points[c2].Y + 15 == line.Y1))
                {
                    line.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
                }
            }

            vertices[c1].Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            vertices[c2].Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
        }

        private void InitializeDjikstraAlgorithmWithRandomVerticles(AdjacencyList[] graph)
        {
            Random rnd = new Random();
            from = rnd.Next(0, MAX_VERTICES - 1);
            vertices[from].Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            do
            {
                where = rnd.Next(0, MAX_VERTICES - 1);
            } while (where == from);
            
            vertices[where].Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));

            result = DijkstraShortestPath.Solve(graph, from, where, MAX_VERTICES, parent);
        }

        public void ColorPath()
        {
            if (parent[from] == -1)
            {
                GetPath(parent, where, path);
                ColorVertices(path, path.Pop());
            }
        }

        private void GetPath(int [] parent, int j, Stack<int> path)
        {
            if (parent[j] == -1)
            {
                path.Push(j);
                return;
            }

            GetPath(parent, parent[j], path);

            path.Push(j);
        }

        private void ColorVertices(Stack<int> path, int i)
        {
            while(path.Count != 0)
            {
                ChangeColor(i, path.Peek());
                ColorVertices(path, path.Pop());
            }
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
