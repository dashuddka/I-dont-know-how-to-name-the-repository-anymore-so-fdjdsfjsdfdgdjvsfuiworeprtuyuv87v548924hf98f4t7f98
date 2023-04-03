using Avalonia.Controls.Shapes;
using Avalonia.Media;
using DynamicData;
using GraphicsEditor.Models;
using GraphicsEditor.ViewModels.Pages;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace GraphicsEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int figureIndex;
        private int figureListIndex;
        private object[] figureViews;
        private object content;
        private ObservableCollection<Figures> figureList;
        private ObservableCollection<Shape> shapes;
        private Figures curFigure;
        private bool replace;

        public MainWindowViewModel()
        {
            replace = false;
            figureViews = new object[6];
            FigureListIndex = -1;
            FigureList = new ObservableCollection<Figures>();
            Shapes = new ObservableCollection<Shape>();
            figureViews[0] = new MenuLineViewModel();
            figureViews[1] = new MenuPolylineViewModel();
            figureViews[2] = new MenuPolygonViewModel();
            figureViews[3] = new MenuRectangleViewModel();
            figureViews[4] = new MenuEllipseViewModel();
            figureViews[5] = new MenuPathViewModel();
            FigureIndex = 0;
            ClearParam = ReactiveCommand.Create(() =>  {
                FigureListIndex = -1;
                if (Content is MenuLineViewModel newObject)
                {
                    newObject.Name = "";
                    newObject.StartPoint = "";
                    newObject.EndPoint = "";
                    newObject.StrokeNum = 0;
                    newObject.ThicknessLine = 1;
                    newObject.Rotate = "";
                    newObject.Scale = "";
                    newObject.Skew = "";
                    newObject.Center = "";
                }
                if (Content is MenuPolylineViewModel polyline)
                {
                    polyline.Name = "";
                    polyline.Points = "";
                    polyline.StrokeNum = 0;
                    polyline.ThicknessLine = 1;
                    polyline.Rotate = "";
                    polyline.Scale = "";
                    polyline.Skew = "";
                    polyline.Center = "";
                }
                if (Content is MenuPolygonViewModel polygon) 
                {
                    polygon.Name = "";
                    polygon.Points = "";
                    polygon.StrokeNum = 0;
                    polygon.FillNum = 0;
                    polygon.ThicknessLine = 1;
                    polygon.Rotate = "";
                    polygon.Scale = "";
                    polygon.Skew = "";
                    polygon.Center = "";
                }
                if (Content is MenuRectangleViewModel rectangle)
                {
                    rectangle.Name = "";
                    rectangle.StartPoint = "";
                    rectangle.Width = "";
                    rectangle.Height = "";
                    rectangle.FillNum = 0;
                    rectangle.StrokeNum = 0;
                    rectangle.ThicknessLine = 1;
                    rectangle.Rotate = "";
                    rectangle.Scale = "";
                    rectangle.Skew = "";
                    rectangle.Center = "";
                }
                if (Content is MenuEllipseViewModel ellipse)
                {
                    ellipse.Name = "";
                    ellipse.StartPoint = "";
                    ellipse.Width = "";
                    ellipse.Height = "";
                    ellipse.FillNum = 0;
                    ellipse.StrokeNum = 0;
                    ellipse.ThicknessLine = 1;
                    ellipse.Rotate = "";
                    ellipse.Scale = "";
                    ellipse.Skew = "";
                    ellipse.Center = "";
                }
                if (Content is MenuPathViewModel path)
                {
                    path.Name = "";
                    path.Commands = "";
                    path.StrokeNum = 0;
                    path.FillNum = 0;
                    path.ThicknessLine = 1;
                    path.Rotate = "";
                    path.Scale = "";
                    path.Skew = "";
                    path.Center = "";
                }
                FigureIndex = figureIndex;
            });
            AddFigure = ReactiveCommand.Create(() =>
            {
                if (Content is MenuLineViewModel newObject)
                {
                    if (newObject.Name == "" || newObject.EndPoint == "" || newObject.StartPoint == "") return;
                    curFigure = new LineElement { Name = newObject.Name, EndPoint = newObject.EndPoint, StartPoint = newObject.StartPoint, StrokeColor = newObject.Colors[newObject.StrokeNum].ToString(), StrokeThickness = newObject.ThicknessLine, Center = newObject.Center, Rotate = newObject.Rotate, Scale = newObject.Scale, Skew = newObject.Skew };
                }
                if (Content is MenuPolylineViewModel newObjectPolyline)
                {
                    if (newObjectPolyline.Name == "" || newObjectPolyline.Points == "") return;
                    curFigure = new BrokenLine { Name = newObjectPolyline.Name, Points = newObjectPolyline.Points, StrokeColor = newObjectPolyline.Colors[newObjectPolyline.StrokeNum].ToString(), StrokeThickness = newObjectPolyline.ThicknessLine, Center = newObjectPolyline.Center, Rotate = newObjectPolyline.Rotate, Scale = newObjectPolyline.Scale, Skew = newObjectPolyline.Skew };
                }
                if (Content is MenuPolygonViewModel newObjectPolygon)
                {
                    if (newObjectPolygon.Name == "" || newObjectPolygon.Points == "") return;
                    curFigure = new PolygonElement { Name = newObjectPolygon.Name, Points = newObjectPolygon.Points, StrokeColor = newObjectPolygon.Colors[newObjectPolygon.StrokeNum].ToString(), FillColor = newObjectPolygon.Colors[newObjectPolygon.FillNum].ToString(), StrokeThickness = newObjectPolygon.ThicknessLine, Center = newObjectPolygon.Center, Rotate = newObjectPolygon.Rotate, Scale = newObjectPolygon.Scale, Skew = newObjectPolygon.Skew };
                }
                if (Content is MenuRectangleViewModel newObjectRectangle)
                {
                    if (newObjectRectangle.Name == "" || newObjectRectangle.StartPoint == "" || newObjectRectangle.Width == "" || newObjectRectangle.Height == "") return;
                    if (double.TryParse(newObjectRectangle.Width, out _) == false)
                    {
                        ClearParam.Execute().Subscribe(); 
                        return;
                    }
                    if (double.TryParse(newObjectRectangle.Height, out _) == false)
                    {
                        ClearParam.Execute().Subscribe();
                        return;
                    }
                    curFigure = new RectangleElement { Name = newObjectRectangle.Name, StartPoint = newObjectRectangle.StartPoint, Width = double.Parse(newObjectRectangle.Width), Height = double.Parse(newObjectRectangle.Height), StrokeColor = newObjectRectangle.Colors[newObjectRectangle.StrokeNum].ToString(), FillColor = newObjectRectangle.Colors[newObjectRectangle.FillNum].ToString(), StrokeThickness = newObjectRectangle.ThicknessLine, Center = newObjectRectangle.Center, Rotate = newObjectRectangle.Rotate, Scale = newObjectRectangle.Scale, Skew = newObjectRectangle.Skew };
                }
                if (Content is MenuEllipseViewModel newObjectEllipse)
                {
                    if (newObjectEllipse.Name == "" || newObjectEllipse.StartPoint == "" || newObjectEllipse.Width == "" || newObjectEllipse.Height == "") return;
                    if (double.TryParse(newObjectEllipse.Width, out _) == false)
                    {
                        ClearParam.Execute().Subscribe();
                        return;
                    }
                    if (double.TryParse(newObjectEllipse.Height, out _) == false)
                    {
                        ClearParam.Execute().Subscribe();
                        return;
                    }
                    curFigure = new EllipseElement { Name = newObjectEllipse.Name, StartPoint = newObjectEllipse.StartPoint, Width = double.Parse(newObjectEllipse.Width), Height = double.Parse(newObjectEllipse.Height), StrokeColor = newObjectEllipse.Colors[newObjectEllipse.StrokeNum].ToString(), FillColor = newObjectEllipse.Colors[newObjectEllipse.FillNum].ToString(), StrokeThickness = newObjectEllipse.ThicknessLine, Center = newObjectEllipse.Center, Rotate = newObjectEllipse.Rotate, Scale = newObjectEllipse.Scale, Skew = newObjectEllipse.Skew };
                }
                if (Content is MenuPathViewModel newObjectPath)
                {
                    if (newObjectPath.Name == "" || newObjectPath.Commands == "") return;
                    curFigure = new CompFig { Name = newObjectPath.Name, Commands = newObjectPath.Commands, StrokeColor = newObjectPath.Colors[newObjectPath.StrokeNum].ToString(), FillColor = newObjectPath.Colors[newObjectPath.FillNum].ToString(), StrokeThickness = newObjectPath.ThicknessLine, Center = newObjectPath.Center, Rotate = newObjectPath.Rotate, Scale = newObjectPath.Scale, Skew = newObjectPath.Skew };
                }
                AddShape(curFigure);
                ClearParam.Execute().Subscribe();
            });
        }

        public Shape ElementToShape(Figures obj)
        {
            if (obj is LineElement line)
            {
                return new Line
                {
                    Name = line.Name,
                    StartPoint = Converters.StringToPoint(line.StartPoint),
                    EndPoint = Converters.StringToPoint(line.EndPoint),
                    Stroke = Converters.StringToBrush(line.StrokeColor),
                    StrokeThickness = line.StrokeThickness,
                    RenderTransform = Converters.MakeTransform(line)
                };
            }
            if (obj is BrokenLine polyline)
            {
                return new Polyline
                {
                    Name = polyline.Name,
                    Points = Converters.StringToPoints(polyline.Points),
                    Stroke = Converters.StringToBrush(polyline.StrokeColor),
                    StrokeThickness = polyline.StrokeThickness,
                    RenderTransform = Converters.MakeTransform(polyline)
                };
            }
            if (obj is PolygonElement polygon)
            {
                return new Polygon
                {
                    Name = polygon.Name,
                    Points = Converters.StringToPoints(polygon.Points),
                    Fill = Converters.StringToBrush(polygon.FillColor),
                    Stroke = Converters.StringToBrush(polygon.StrokeColor),
                    StrokeThickness = polygon.StrokeThickness,
                    RenderTransform = Converters.MakeTransform(polygon)
                };
            }
            if (obj is RectangleElement rectangle)
            {
                return new Avalonia.Controls.Shapes.Rectangle
                {
                    Name = rectangle.Name,
                    Margin = Converters.StringToMargin(rectangle.StartPoint),
                    Width = rectangle.Width,
                    Height = rectangle.Height,
                    Fill = Converters.StringToBrush(rectangle.FillColor),
                    Stroke = Converters.StringToBrush(rectangle.StrokeColor),
                    StrokeThickness = rectangle.StrokeThickness,
                    RenderTransform = Converters.MakeTransform(rectangle)
                };
            }
            if (obj is EllipseElement ellipse)
            {
                return new Avalonia.Controls.Shapes.Ellipse
                {
                    Name = ellipse.Name,
                    Margin = Converters.StringToMargin(ellipse.StartPoint),
                    Width = ellipse.Width,
                    Height = ellipse.Height,
                    Fill = Converters.StringToBrush(ellipse.FillColor),
                    Stroke = Converters.StringToBrush(ellipse.StrokeColor),
                    StrokeThickness = ellipse.StrokeThickness,
                    RenderTransform = Converters.MakeTransform(ellipse)

                };
            }
            if (obj is CompFig path)
            {
                return new Avalonia.Controls.Shapes.Path
                {
                    Name = path.Name,
                    Data = Geometry.Parse(path.Commands),
                    Fill = Converters.StringToBrush(path.FillColor),
                    Stroke = Converters.StringToBrush(path.StrokeColor),
                    StrokeThickness = path.StrokeThickness,
                    RenderTransform = Converters.MakeTransform(path)
                };
            }
            return null;
        }
        private bool CheckTrans(Figures obj)
        {
            if (obj.Center != "")
            {
                string[] trans = obj.Center.Split(" ");
                if (trans.Count() != 2) return false;
                foreach (string el in trans)
                {
                    if (double.TryParse(el, out _) == false) return false;
                }
            }
            if (obj.Rotate != "")
            {
                string[] trans = obj.Rotate.Split(" ");
                if (trans.Count() != 1) return false;
                if (double.TryParse(obj.Rotate, out _) == false) return false;
            }
            if (obj.Scale != "")
            {
                string[] trans = obj.Scale.Split(" ");
                if (trans.Count() != 2) return false;
                foreach (string el in trans)
                {
                    if (double.TryParse(el, out _) == false) return false;
                }
            }
            if (obj.Skew != "")
            {
                string[] trans = obj.Skew.Split(" ");
                if (trans.Count() != 2) return false;
                foreach (string el in trans)
                {
                    if (double.TryParse(el, out _) == false) return false;
                }
            }
            return true;
        }
        private void AddShape(Figures obj)
        {
            if (obj is LineElement line)
            {
                if (line.Name == "") return;
                string[] st = line.StartPoint.Split(",");
                string[] en = line.EndPoint.Split(",");
                if (CheckTrans(line) == false) return;
                if (st.Count() != 2 || en.Count() != 2) return;
                foreach (string el in st)
                {
                    if (double.TryParse(el, out _) == false) return;
                }
                foreach (string el in en)
                {
                    if (double.TryParse(el, out _) == false) return;
                }
                if (!(FigureList.Any(n => n.Name == line.Name)))
                {
                    FigureList.Add(line);
                    Shapes.Add(ElementToShape(line));
                }
                else
                {
                    FigureList.Replace(FigureList.First(n => n.Name == line.Name), line);
                    Shapes.Replace(Shapes.First(n => n.Name == line.Name), ElementToShape(line));
                }
            }
            if (obj is BrokenLine polyline)
            {
                if (polyline.Name == "") return;
                if (CheckTrans(polyline) == false) return;
                string[] st = polyline.Points.Replace(",", " ").Split(" ");
                if (st.Count()%2==1 || st.Count()<4) return;
                foreach (string el in st)
                {
                    if (double.TryParse(el, out _) == false) return;
                }
                if (!(FigureList.Any(n => n.Name == polyline.Name)))
                {
                    FigureList.Add(polyline);
                    Shapes.Add(ElementToShape(polyline));
                }
                else
                {
                    FigureList.Replace(FigureList.First(n => n.Name == polyline.Name), polyline);
                    Shapes.Replace(Shapes.First(n => n.Name == polyline.Name), ElementToShape(polyline));
                }
            }
            if (obj is PolygonElement polygon)
            {
                if (polygon.Name == "") return;
                if (CheckTrans(polygon) == false) return;
                string[] st = polygon.Points.Replace(",", " ").Split(" ");
                if (st.Count() % 2 == 1 || st.Count() < 6) return;
                foreach (string el in st)
                {
                    if (double.TryParse(el, out _) == false) return;
                }
                if (!(FigureList.Any(n => n.Name == polygon.Name)))
                {
                    FigureList.Add(polygon);
                    Shapes.Add(ElementToShape(polygon));
                }
                else
                {
                    FigureList.Replace(FigureList.First(n => n.Name == polygon.Name), polygon);
                    Shapes.Replace(Shapes.First(n => n.Name == polygon.Name), ElementToShape(polygon));
                }
            }
            if (obj is RectangleElement rectangle)
            {
                if (rectangle.Name == "" || rectangle.Width<1 || rectangle.Height<1) return;
                if (CheckTrans(rectangle) == false) return;
                string[] st = rectangle.StartPoint.Split(",");
                if (st.Count() != 2) return;
                foreach (string el in st)
                {
                    if (double.TryParse(el, out _) == false) return;
                }
                if (!(FigureList.Any(n => n.Name == rectangle.Name)))
                {
                    FigureList.Add(rectangle);
                    Shapes.Add(ElementToShape(rectangle));
                }
                else
                {
                    FigureList.Replace(FigureList.First(n => n.Name == rectangle.Name), rectangle);
                    Shapes.Replace(Shapes.First(n => n.Name == rectangle.Name), ElementToShape(rectangle));
                }
            }
            if (obj is EllipseElement ellipse)
            {
                if (ellipse.Name == "" || ellipse.Width < 1 || ellipse.Height < 1) return;
                if (CheckTrans(ellipse) == false) return;
                string[] st = ellipse.StartPoint.Split(",");
                if (st.Count() != 2) return;
                foreach (string el in st)
                {
                    if (double.TryParse(el, out _) == false) return;
                }
                if (!(FigureList.Any(n => n.Name == ellipse.Name)))
                {
                    FigureList.Add(ellipse);
                    Shapes.Add(ElementToShape(ellipse));
                }
                else
                {
                    FigureList.Replace(FigureList.First(n => n.Name == ellipse.Name), ellipse);
                    Shapes.Replace(Shapes.First(n => n.Name == ellipse.Name), ElementToShape(ellipse));
                }
            }
            if (obj is CompFig path)
            {
                try
                {
                    var temp = Geometry.Parse(path.Commands);
                }
                catch
                {
                    return;
                }
                if (CheckTrans(path) == false) return;
                if (!(FigureList.Any(n => n.Name == path.Name)))
                {
                    FigureList.Add(path);
                    Shapes.Add(ElementToShape(path));
                }
                else
                {
                    FigureList.Replace(FigureList.First(n => n.Name == path.Name), path);
                    Shapes.Replace(Shapes.First(n => n.Name == path.Name), ElementToShape(path));
                }
            }
        }

        public object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public int FigureIndex
        {
            get => figureIndex;
            set {
                this.RaiseAndSetIfChanged(ref figureIndex, value);
                Content = figureViews[figureIndex];
                if (replace == true) FigureListIndex = -1;
            }
        }
        public int FigureListIndex
        {
            get => figureListIndex;
            set 
            {
                replace = false;
                this.RaiseAndSetIfChanged(ref figureListIndex, value);
                if (figureListIndex != -1)
                {
                    if (FigureList[figureListIndex] is LineElement line)
                    {
                        FigureIndex = 0;
                        if (Content is MenuLineViewModel cont)
                        {
                            replace = true;
                            cont.Name = line.Name;
                            cont.StartPoint = line.StartPoint;
                            cont.EndPoint = line.EndPoint;
                            cont.SetIndexOfColor(Converters.StringToBrush(line.StrokeColor));
                            cont.ThicknessLine = line.StrokeThickness;
                            cont.Center = line.Center;
                            cont.Rotate = line.Rotate;
                            cont.Scale = line.Scale;
                            cont.Skew = line.Skew;
                        }
                    }
                    if (FigureList[figureListIndex] is BrokenLine polyline)
                    {
                        FigureIndex = 1;
                        if (Content is MenuPolylineViewModel cont)
                        {
                            replace = true;
                            cont.Name = polyline.Name;
                            cont.Points = polyline.Points;
                            cont.SetIndexOfColor(Converters.StringToBrush(polyline.StrokeColor));
                            cont.ThicknessLine = polyline.StrokeThickness;
                            cont.Center = polyline.Center;
                            cont.Rotate = polyline.Rotate;
                            cont.Scale = polyline.Scale;
                            cont.Skew = polyline.Skew;
                        }
                    }
                    if (FigureList[figureListIndex] is PolygonElement polygon)
                    {
                        FigureIndex = 2;
                        if (Content is MenuPolygonViewModel cont)
                        {
                            replace = true;
                            cont.Name = polygon.Name;
                            cont.Points = polygon.Points;
                            cont.SetIndexOfColor(Converters.StringToBrush(polygon.StrokeColor));
                            cont.SetIndexOfColorFill (Converters.StringToBrush(polygon.FillColor));
                            cont.ThicknessLine = polygon.StrokeThickness;
                            cont.Center = polygon.Center;
                            cont.Rotate = polygon.Rotate;
                            cont.Scale = polygon.Scale;
                            cont.Skew = polygon.Skew;
                        }
                    }
                    if (FigureList[figureListIndex] is RectangleElement rectangle)
                    {
                        FigureIndex = 3;
                        if (Content is MenuRectangleViewModel cont)
                        {
                            replace = true;
                            cont.Name = rectangle.Name;
                            cont.StartPoint = rectangle.StartPoint;
                            cont.Width = rectangle.Width.ToString();
                            cont.Height = rectangle.Height.ToString();
                            cont.SetIndexOfColor(Converters.StringToBrush(rectangle.StrokeColor));
                            cont.SetIndexOfColorFill(Converters.StringToBrush(rectangle.FillColor));
                            cont.ThicknessLine = rectangle.StrokeThickness;
                            cont.Center = rectangle.Center;
                            cont.Rotate = rectangle.Rotate;
                            cont.Scale = rectangle.Scale;
                            cont.Skew = rectangle.Skew;
                        }
                    }
                    if (FigureList[figureListIndex] is EllipseElement ellipse)
                    {
                        FigureIndex = 4;
                        if (Content is MenuEllipseViewModel cont)
                        {
                            replace = true;
                            cont.Name = ellipse.Name;
                            cont.StartPoint = ellipse.StartPoint;
                            cont.Width = ellipse.Width.ToString();
                            cont.Height = ellipse.Height.ToString();
                            cont.SetIndexOfColor(Converters.StringToBrush(ellipse.StrokeColor));
                            cont.SetIndexOfColorFill(Converters.StringToBrush(ellipse.FillColor));
                            cont.ThicknessLine = ellipse.StrokeThickness;
                            cont.Center = ellipse.Center;
                            cont.Rotate = ellipse.Rotate;
                            cont.Scale = ellipse.Scale;
                            cont.Skew = ellipse.Skew;
                        }
                    }
                    if (FigureList[figureListIndex] is CompFig path)
                    {
                        FigureIndex = 5;
                        if (Content is MenuPathViewModel cont)
                        {
                            replace = true;
                            cont.Name = path.Name;
                            cont.Commands = path.Commands;
                            cont.SetIndexOfColor(Converters.StringToBrush(path.StrokeColor));
                            cont.SetIndexOfColorFill(Converters.StringToBrush(path.FillColor));
                            cont.ThicknessLine = path.StrokeThickness;
                            cont.Center = path.Center;
                            cont.Rotate = path.Rotate;
                            cont.Scale = path.Scale;
                            cont.Skew = path.Skew;
                        }
                    }
                }
            }
        }

        public ObservableCollection<Figures> FigureList
        {
            get => figureList;
            set
            {
                this.RaiseAndSetIfChanged(ref figureList, value);
            }
        }

        public ObservableCollection<Shape> Shapes
        {
            get => shapes;
            set => this.RaiseAndSetIfChanged(ref shapes, value);
        }
        public Figures CurFigure
        {
            get => curFigure;
            set => this.RaiseAndSetIfChanged(ref curFigure, value);
        }
        public ReactiveCommand<Unit, Unit> AddFigure { get; }
        public ReactiveCommand<Unit, Unit> ClearParam { get; }
    }
}
