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
using EquationInterpreter;

namespace GrapherWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        double GridStep = 1d;

        double StartGraphX = -10f;
        double EndGraphX = 10f;
        double XPercentStep = 0.1f; //Percent off range we step -> 0.1% = 1000 x plots

        public string[] CleanInput()
        {
            string DataToClean = InputBox.Text;
            string UnspacedData = DataToClean.Replace(" ","");
            string SpacedData = "";

            for (int i = 0; i < UnspacedData.Length - 1; i++)
            {
                SpacedData += UnspacedData[i];
                if (RPN.AllowedSymbols.Contains(UnspacedData[i+1].ToString()))
                {
                    SpacedData += ' ';
                }
                else if (RPN.AllowedSymbols.Contains(UnspacedData[i].ToString()))
                {
                    SpacedData += ' ';
                }
            }
            SpacedData += UnspacedData[UnspacedData.Length - 1];

            return SpacedData.Split(' ');
        }

        public void Graph()
        {            
            List<string> Formula = RPN.RPNGenerator(CleanInput());
          //  if (Formula[Formula.Count-1] == "-") Formula.Insert(0, "0");

            double PreviousX = StartGraphX;
            double PreviousY = RPN.FormulaCalculator(Formula, PreviousX);

            double Scale = GraphCanvas.ActualWidth / (EndGraphX - StartGraphX);
            double XOffset = 0 - StartGraphX;

            //redo for y
            /*
            for (double i = StartGraphX; i < EndGraphX; i += GridStep)
            {                
                Line NewLine = new Line();
                NewLine.X1 = (i + XOffset) * Scale;
                NewLine.Y1 = GraphCanvas.ActualHeight;
                NewLine.X2 = (i + XOffset) * Scale;
                NewLine.Y2 = 0;
                NewLine.Stroke = new SolidColorBrush(Colors.Black);

                GraphCanvas.Children.Add(NewLine);
            }
            */

            for (double i = StartGraphX; i < EndGraphX; i += GridStep)
            {
                Line NewLine = new Line();
                NewLine.X1 = (i + XOffset) * Scale;
                NewLine.Y1 = GraphCanvas.ActualHeight;
                NewLine.X2 = (i + XOffset) * Scale;
                NewLine.Y2 = 0;
                NewLine.Stroke = new SolidColorBrush(Colors.Black);

                GraphCanvas.Children.Add(NewLine);
            }

            for (double i = 0; i < 100; i += XPercentStep)
            {
                double X = StartGraphX + (EndGraphX - StartGraphX) * (i/100d);
                double Y = RPN.FormulaCalculator(Formula, X);

                Line NewLine = new Line();
                NewLine.X1 = (PreviousX + XOffset) * Scale;
                NewLine.Y1 = GraphCanvas.ActualHeight/2d - (PreviousY * Scale);
                NewLine.X2 = (X + XOffset) * Scale;
                NewLine.Y2 = GraphCanvas.ActualHeight/2d - (Y * Scale);
                NewLine.Stroke = new SolidColorBrush(Colors.Black);

                GraphCanvas.Children.Add(NewLine);

                PreviousX = X;
                PreviousY = Y;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GraphCanvas.Children.Clear();
            Graph();
        }
    }
}
