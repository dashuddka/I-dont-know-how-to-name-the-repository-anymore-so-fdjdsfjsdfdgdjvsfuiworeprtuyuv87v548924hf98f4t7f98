using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using DynamicData;
using GraphicsEditor.Models;
using GraphicsEditor.ViewModels;
using GraphicsEditor.ViewModels.Pages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphicsEditor.Views
{
    public partial class MainWindow : Window
    {
        private Point pointerPositionIntoShape;
        private int shapeNumber;
        public MainWindow()
        {
            InitializeComponent();
            AddHandler(DragDrop.DropEvent, DropShapes);
        }
        private async void OpenFileDialogMenuXmlClick(object sender, RoutedEventArgs routedEventArgs)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            List<string> formates = new List<string>
            {
                "xml"
            };
            openFileDialog.Filters.Add(new FileDialogFilter { Extensions = formates });
            openFileDialog.AllowMultiple = false;
            string[]? result = await openFileDialog.ShowAsync(this);
            if (DataContext is MainWindowViewModel dataContext)
            {
                if (result != null)
                {
                    dataContext.FigureList = new ObservableCollection<Figures>();
                    dataContext.Shapes = new ObservableCollection<Shape>();
                    dataContext.FigureList = Serializer<ObservableCollection<Figures>>.Load(result[0]);
                    foreach (Figures f in dataContext.FigureList)
                    {
                        dataContext.Shapes.Add(dataContext.ElementToShape(f));
                    }
                }
            }

        }
        private async void OpenFileDialogMenuJsonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            List<string> formates = new List<string>
            {
                "json"
            };
            openFileDialog.Filters.Add(new FileDialogFilter { Extensions = formates, Name = "Json files" });
            openFileDialog.AllowMultiple = false;
            string[]? result = await openFileDialog.ShowAsync(this);
            if (DataContext is MainWindowViewModel dataContext)
            {
                if (result != null)
                {
                    dataContext.FigureList = new ObservableCollection<Figures>();
                    dataContext.Shapes = new ObservableCollection<Shape>();
                    dataContext.FigureList = JsonSerializer<ObservableCollection<Figures>>.Load(result[0]);
                    foreach (Figures f in dataContext.FigureList)
                    {
                        dataContext.Shapes.Add(dataContext.ElementToShape(f));
                    }
                }
            }

        }
        private async void SaveFileDialogMenuXmlClick(object sender, RoutedEventArgs routedEventArgs)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            List<string> formates = new List<string>
            {
                "xml"
            };
            saveFileDialog.Filters.Add(new FileDialogFilter { Extensions = formates, Name = "Xml files" });
            string? result = await saveFileDialog.ShowAsync(this);
            if (DataContext is MainWindowViewModel dataContext)
            {
                if (result != null)
                {
                    Serializer<ObservableCollection<Figures>>.Save(result, dataContext.FigureList);
                }
            }
        }
        private async void SaveFileDialogMenuJsonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            List<string> formates = new List<string>
            {
                "json"
            };
            saveFileDialog.Filters.Add(new FileDialogFilter { Extensions = formates, Name = "Json files" });
            string? result = await saveFileDialog.ShowAsync(this);
            if (DataContext is MainWindowViewModel dataContext)
            {
                if (result != null)
                {
                    JsonSerializer<ObservableCollection<Figures>>.Save(result, dataContext.FigureList);
                }
            }
        }
        private async void SaveFileDialogMenuPngClick(object sender, RoutedEventArgs routedEventArgs)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            List<string> formates = new List<string>
            {
                "png"
            };
            saveFileDialog.Filters.Add(new FileDialogFilter { Extensions = formates, Name = "Png files" });
            string? result = await saveFileDialog.ShowAsync(this);
            var canvas = this.GetVisualDescendants().OfType<Canvas>().Where(canvas => canvas.Name.Equals("canvas")).FirstOrDefault();
            if (canvas != null)
            {
                var pixelSize = new PixelSize((int)canvas.Bounds.Width, (int)canvas.Bounds.Height);
                var size = new Size(canvas.Bounds.Width, canvas.Bounds.Height);
                using (RenderTargetBitmap bitmap = new RenderTargetBitmap(pixelSize, new Avalonia.Vector(96, 96)))
                {
                    canvas.Measure(size);
                    canvas.Arrange(new Rect(size));
                    bitmap.Render(canvas);
                    bitmap.Save(result);
                }
            }
        }
        private void DeleteShape(object sender, RoutedEventArgs routedEventArgs)
        {
            Grid grid = ((Button)sender).Parent as Grid;
            TextBlock textblock = grid.Children[0] as TextBlock;
            string name = textblock.Text;
            if (DataContext is MainWindowViewModel window)
            {
                window.FigureList.Remove(window.FigureList.First(x => x.Name == name));
                window.Shapes.Remove(window.Shapes.First(x => x.Name == name));
            }
        }
        private double startX, startY;
        private void PointerPressedOnCanvas(object? sender, PointerPressedEventArgs pointerPressedEventArgs)
        {
            if (pointerPressedEventArgs.Source is Shape shape)
            {
                if (this.DataContext is MainWindowViewModel mainWindow)
                {
                    for (int i = 0; i < mainWindow.Shapes.Count; ++i)
                    {
                        if (mainWindow.Shapes[i].Name == shape.Name)
                        {
                            shapeNumber = i;
                            break;
                        }
                    }
                }
                if (shape is Line line) { startX = line.StartPoint.X; startY = line.StartPoint.Y; }
                pointerPositionIntoShape = pointerPressedEventArgs.GetPosition(shape);
                this.PointerMoved += PointerMoveDragShape;
                this.PointerReleased += PointerPressedReleasedDragShape;
            }
        }
        private void PointerMoveDragShape(object? sender, PointerEventArgs pointerEventArgs)
        {
            if (DataContext is MainWindowViewModel mainWindow)
            {
                if (pointerEventArgs.Source is Line line)
                {
                    Point currentPointerPosition = pointerEventArgs
                        .GetPosition(
                        this.GetVisualDescendants()
                        .OfType<Canvas>()
                        .FirstOrDefault());
                    double rasPoX = currentPointerPosition.X - line.StartPoint.X;
                    double rasPoY = currentPointerPosition.Y - line.StartPoint.Y;
                    double endDx = line.EndPoint.X - line.StartPoint.X;
                    double endDy = line.EndPoint.Y - line.StartPoint.Y;
                    line.StartPoint = new Point(
                        startX + currentPointerPosition.X - pointerPositionIntoShape.X,
                        startY + currentPointerPosition.Y - pointerPositionIntoShape.Y);
                    line.EndPoint = new Point(
                        endDx + line.StartPoint.X,
                        endDy + line.StartPoint.Y);
                }
                if (pointerEventArgs.Source is Polyline polyline)
                {
                    Point currentPointerPosition = pointerEventArgs
                        .GetPosition(
                        this.GetVisualDescendants()
                        .OfType<Canvas>()
                        .FirstOrDefault());

                    polyline.Margin = new Thickness(
                        currentPointerPosition.X - pointerPositionIntoShape.X,
                        currentPointerPosition.Y - pointerPositionIntoShape.Y);
                }
                if (pointerEventArgs.Source is Polygon polygon)
                {
                    Point currentPointerPosition = pointerEventArgs
                        .GetPosition(
                        this.GetVisualDescendants()
                        .OfType<Canvas>()
                        .FirstOrDefault());

                    polygon.Margin = new Thickness(
                        currentPointerPosition.X - pointerPositionIntoShape.X,
                        currentPointerPosition.Y - pointerPositionIntoShape.Y);
                }
                if (pointerEventArgs.Source is Rectangle rectangle)
                {
                    Point currentPointerPosition = pointerEventArgs
                        .GetPosition(
                        this.GetVisualDescendants()
                        .OfType<Canvas>()
                        .FirstOrDefault());

                    rectangle.Margin = new Thickness(
                        currentPointerPosition.X - pointerPositionIntoShape.X,
                        currentPointerPosition.Y - pointerPositionIntoShape.Y);
                }
                if (pointerEventArgs.Source is Ellipse ellipse)
                {
                    Point currentPointerPosition = pointerEventArgs
                        .GetPosition(
                        this.GetVisualDescendants()
                        .OfType<Canvas>()
                        .FirstOrDefault());

                    ellipse.Margin = new Thickness(
                        currentPointerPosition.X - pointerPositionIntoShape.X,
                        currentPointerPosition.Y - pointerPositionIntoShape.Y);
                }
                if (pointerEventArgs.Source is Path path)
                {
                    Point currentPointerPosition = pointerEventArgs
                        .GetPosition(
                        this.GetVisualDescendants()
                        .OfType<Canvas>()
                        .FirstOrDefault());

                    path.Margin = new Thickness(
                        currentPointerPosition.X - pointerPositionIntoShape.X,
                        currentPointerPosition.Y - pointerPositionIntoShape.Y);
                }
            }
        }
        private void PointerPressedReleasedDragShape(object? sender, PointerReleasedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindow)
            {
                if (mainWindow.FigureList[shapeNumber] is LineElement lin)
                {
                    if (mainWindow.Shapes[shapeNumber] is Line line)
                    {
                        lin.StartPoint = line.StartPoint.X.ToString();
                        lin.StartPoint += ", ";
                        lin.StartPoint += line.StartPoint.Y.ToString();
                        lin.EndPoint = line.EndPoint.X.ToString();
                        lin.EndPoint += ", ";
                        lin.EndPoint += line.EndPoint.Y.ToString();
                    }
                }
                if (mainWindow.FigureList[shapeNumber] is BrokenLine pol)
                {
                    if (mainWindow.Shapes[shapeNumber] is Polyline polyline)
                    {
                        pol.Points = "";
                        foreach (Point point in polyline.Points)
                        {
                            pol.Points += (point.X + polyline.Margin.Left).ToString();
                            pol.Points += ",";
                            pol.Points += (point.Y + polyline.Margin.Top).ToString();
                            pol.Points += " ";
                        }
                        pol.Points = pol.Points[..^1];
                        polyline.Points = Converters.StringToPoints(pol.Points);
                        polyline.Margin = new Thickness(0, 0);
                    }
                }
                if (mainWindow.FigureList[shapeNumber] is PolygonElement gon)
                {
                    if (mainWindow.Shapes[shapeNumber] is Polygon polygon)
                    {
                        gon.Points = "";
                        foreach (Point point in polygon.Points)
                        {
                            gon.Points += (point.X + polygon.Margin.Left).ToString();
                            gon.Points += ",";
                            gon.Points += (point.Y + polygon.Margin.Top).ToString();
                            gon.Points += " ";
                        }
                        gon.Points = gon.Points[..^1];
                        polygon.Points = Converters.StringToPoints(gon.Points);
                        polygon.Margin = new Thickness(0, 0);
                    }
                }
                if (mainWindow.FigureList[shapeNumber] is RectangleElement rec)
                {
                    rec.StartPoint = mainWindow.Shapes[shapeNumber].Margin.Left.ToString();
                    rec.StartPoint += ", ";
                    rec.StartPoint += mainWindow.Shapes[shapeNumber].Margin.Top.ToString();
                }
                if (mainWindow.FigureList[shapeNumber] is EllipseElement el)
                {
                    el.StartPoint = mainWindow.Shapes[shapeNumber].Margin.Left.ToString();
                    el.StartPoint += ", ";
                    el.StartPoint += mainWindow.Shapes[shapeNumber].Margin.Top.ToString();
                }
                if (mainWindow.FigureList[shapeNumber] is CompFig pathEl)
                {
                    if (mainWindow.Shapes[shapeNumber] is Path path)
                    {
                        string numberX = "", numberY = "";
                        int i = 2;
                        while (i < pathEl.Commands.Length)
                        {
                            if (pathEl.Commands[i] != ',') { numberX += pathEl.Commands[i]; pathEl.Commands = pathEl.Commands.Remove(i, 1); }
                            else break;
                        }
                        ++i;
                        while (i < pathEl.Commands.Length)
                        {
                            if (pathEl.Commands[i] != ' ') { numberY += pathEl.Commands[i]; pathEl.Commands = pathEl.Commands.Remove(i, 1); }
                            else break;
                        }
                        numberX = (double.Parse(numberX) + path.Margin.Left).ToString();
                        numberY = (double.Parse(numberY) + path.Margin.Top).ToString();
                        pathEl.Commands = pathEl.Commands.Insert(2, numberX);
                        pathEl.Commands = pathEl.Commands.Insert(3 + numberX.Length, numberY);
                        path.Data = Geometry.Parse(pathEl.Commands);
                        path.Margin = new Thickness(0, 0);
                    }
                }
                if (mainWindow.FigureListIndex > -1)
                {
                    if (mainWindow.FigureList[mainWindow.FigureListIndex] is LineElement lineEl)
                    {
                        if (mainWindow.Content is MenuLineViewModel cont) { cont.StartPoint = lineEl.StartPoint; cont.EndPoint = lineEl.EndPoint; }
                    }
                    if (mainWindow.FigureList[mainWindow.FigureListIndex] is BrokenLine polylineEl)
                    {
                        if (mainWindow.Content is MenuPolylineViewModel cont) { cont.Points = polylineEl.Points; }
                    }
                    if (mainWindow.FigureList[mainWindow.FigureListIndex] is PolygonElement polygonEl)
                    {
                        if (mainWindow.Content is MenuPolygonViewModel cont) { cont.Points = polygonEl.Points; }
                    }
                    if (mainWindow.FigureList[mainWindow.FigureListIndex] is RectangleElement rectangle)
                    {
                        if (mainWindow.Content is MenuRectangleViewModel cont) cont.StartPoint = rectangle.StartPoint;
                    }
                    if (mainWindow.FigureList[mainWindow.FigureListIndex] is EllipseElement elip)
                    {
                        if (mainWindow.Content is MenuEllipseViewModel cont) cont.StartPoint = elip.StartPoint;
                    }
                    if (mainWindow.FigureList[mainWindow.FigureListIndex] is CompFig path)
                    {
                        if (mainWindow.Content is MenuPathViewModel cont) cont.Commands = path.Commands;
                    }
                }
                this.PointerMoved -= PointerMoveDragShape;
                this.PointerReleased -= PointerPressedReleasedDragShape;
            }
        }
        private void DropShapes(object? sender, DragEventArgs dragEventArgs)
        {
            if (dragEventArgs.Data.Contains(DataFormats.FileNames) == true)
            {
                string? fileName = dragEventArgs.Data.GetFileNames()?.FirstOrDefault();

                if (fileName != null)
                {
                    if (this.DataContext is MainWindowViewModel mainWindow)
                    {
                        if (fileName.EndsWith(".xml"))
                        {
                            mainWindow.FigureList = new ObservableCollection<Figures>();
                            mainWindow.Shapes = new ObservableCollection<Shape>();
                            mainWindow.FigureList = Serializer<ObservableCollection<Figures>>.Load(fileName);
                            foreach (Figures f in mainWindow.FigureList)
                            {
                                mainWindow.Shapes.Add(mainWindow.ElementToShape(f));
                            }
                        }
                        if (fileName.EndsWith(".json"))
                        {
                            mainWindow.FigureList = new ObservableCollection<Figures>();
                            mainWindow.Shapes = new ObservableCollection<Shape>();
                            mainWindow.FigureList = JsonSerializer<ObservableCollection<Figures>>.Load(fileName);
                            foreach (Figures f in mainWindow.FigureList)
                            {
                                mainWindow.Shapes.Add(mainWindow.ElementToShape(f));
                            }
                        }
                    }
                }
            }
        }
    }
}
