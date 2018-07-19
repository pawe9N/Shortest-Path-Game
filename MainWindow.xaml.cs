using ShortestPathGame.Classes;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ShortestPathGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphDrawUtil util;
        private int level = 1;
        private int points = 0;
        private int result;
        public MainWindow()
        {
            InitializeComponent();

            util = new GraphDrawUtil(myCanvas, level);
            
            result = util.result;
            Points.Text = $"Points: {points}";
            Next.IsEnabled = false;
            Answer.Focus();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Answer.Focus();
            Check.IsEnabled = true;
            Next.IsEnabled = false;
            AnserChecker.Text = "";
            Result.Text = "_____";
            Answer.Text = "";

            myCanvas.Children.Clear();

            CheckPointsToNextLevel();

            util = new GraphDrawUtil(myCanvas, level);
            result = util.result;
            Points.Text = $"Points: {points}";
        }

        private void CheckPointsToNextLevel()
        {
            if(points < 50)
            {
                level = 1;
            }
            else if(points < 100)
            {
                level = 2;
            }
            else if (points < 200)
            {
                level = 3;
            }
            else if (points < 400)
            {
                level = 4;
            }
            else if (points >= 400)
            {
                level = 5;
            }
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            string answerString = Regex.Match(Answer.Text, @"\d+").Value;
            if (answerString != "" && result == Int32.Parse(answerString))
            {
                util.ColorPath();
                AnserChecker.Text = ":)";
                Result.Text = result.ToString();
                Answer.Text = "";
                points += 10 * level;
                Points.Text = $"Points: {points}";
                Check.IsEnabled = false;
                Next.IsEnabled = true;
                Next.Focus();
            }
            else if(answerString != "")
            {
                Answer.Text = "";
                if(points > 0)
                {
                    points -= 10;
                }
                AnserChecker.Text = ":(";
                Points.Text = $"Points: {points}";
            }
        }

        private void Check_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Return && Check.IsEnabled)
            {
                Check_Click(sender, e);
            }
        }
    }
}
