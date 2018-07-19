using ShortestPathGame.Classes;
using System;
using System.Windows;

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
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Check.IsEnabled = true;
            Next.IsEnabled = false;
            AnserChecker.Text = "";
            Result.Text = "";
            Answer.Text = "";

            myCanvas.Children.Clear();

            if(++level > 5)
            {
                level = 1;
            }

            util = new GraphDrawUtil(myCanvas, level);
            result = util.result;
            Points.Text = $"Points: {points}";
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            if(Answer.Text != "" && result == Convert.ToInt32(Answer.Text))
            {
                AnserChecker.Text = ":)";
                Result.Text = result.ToString();
                Answer.Text = "";
                points += 10 * level;
                Points.Text = $"Points: {points}";
                Check.IsEnabled = false;
                Next.IsEnabled = true;
            }
            else
            {
                points -= 10;
                AnserChecker.Text = ":(";
                Points.Text = $"Points: {points}";
            }
        }
    }
}
