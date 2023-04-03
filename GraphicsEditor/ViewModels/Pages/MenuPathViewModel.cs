using Avalonia.Media;
using GraphicsEditor.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEditor.ViewModels.Pages
{
    public class MenuPathViewModel : ViewModelBase
    {
        private string name;
        private string commands;
        private int strokeNum;
        private int fillNum;
        private ObservableCollection<SolidColorBrush> colors;
        private double thicknessLine;
        private string rotate = "";
        private string scale = "";
        private string skew = "";
        private string center = "";

        public MenuPathViewModel()
        {
            Name = "";
            Commands = "";
            StrokeNum = 0;
            FillNum = 0;
            ThicknessLine = 1;
            Colors = new ObservableCollection<SolidColorBrush>();
            var brushes = typeof(Brushes).GetProperties().Select(brush => brush.GetValue(brush));
            foreach (object? el in brushes)
            {
                Colors.Add(Converters.StringToBrush(el.ToString()));
            }
        }

        public void SetIndexOfColor(SolidColorBrush color)
        {
            StrokeNum = Colors.IndexOf(color);
        }
        public void SetIndexOfColorFill(SolidColorBrush color)
        {
            FillNum = Colors.IndexOf(color);
        }

        public string Commands
        {
            get => commands;
            set => this.RaiseAndSetIfChanged(ref commands, value);
        }
        public double ThicknessLine
        {
            get => thicknessLine;
            set => this.RaiseAndSetIfChanged(ref thicknessLine, value);
        }
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }
        public int StrokeNum
        {
            get => strokeNum;
            set => this.RaiseAndSetIfChanged(ref strokeNum, value);
        }

        public int FillNum
        {
            get => fillNum;
            set => this.RaiseAndSetIfChanged(ref fillNum, value);
        }
        public ObservableCollection<SolidColorBrush> Colors
        {
            get => colors;
            set => this.RaiseAndSetIfChanged(ref colors, value);
        }
        public string Rotate
        {
            get => rotate;
            set => this.RaiseAndSetIfChanged(ref rotate, value);
        }
        public string Scale
        {
            get => scale;
            set => this.RaiseAndSetIfChanged(ref scale, value);
        }
        public string Skew
        {
            get => skew;
            set => this.RaiseAndSetIfChanged(ref skew, value);
        }
        public string Center
        {
            get => center;
            set => this.RaiseAndSetIfChanged(ref center, value);
        }
    }
}
