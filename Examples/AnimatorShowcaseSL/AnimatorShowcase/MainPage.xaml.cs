using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using Artefact.Animation;
using Artefact.Animation.Extensions;
using System.Windows.Media;

namespace AnimatorShowcase
{
    public static class Helper
    {
        #region MATH
        public static readonly Random Rnd = new Random();
        public static double RandomRange(double low, double high)
        {
            return (low + ((high - low) * Rnd.NextDouble()));
        }

        public static double GetApothem(double height, double sides)
        {
            var x0 = 360.0 / (2 * sides);
            var k = Math.Sin(Math.PI * x0 / 180.0);
            var r = (double)(height / 2) / k;
            return -Math.Sqrt((r * r) - Math.Pow((double)height / 2, (double)2));
        }
        #endregion

        #region 3D
        public static PlaneProjection Copy(PlaneProjection planeProjection)
        {
            return planeProjection == null ? new PlaneProjection() : new PlaneProjection
            {
                CenterOfRotationX = planeProjection.CenterOfRotationX,
                CenterOfRotationY = planeProjection.CenterOfRotationY,
                CenterOfRotationZ = planeProjection.CenterOfRotationZ,
                RotationX = planeProjection.RotationX,
                RotationY = planeProjection.RotationY,
                RotationZ = planeProjection.RotationZ,
                LocalOffsetX = planeProjection.LocalOffsetX,
                LocalOffsetY = planeProjection.LocalOffsetY,
                LocalOffsetZ = planeProjection.LocalOffsetZ,
                GlobalOffsetX = planeProjection.GlobalOffsetX,
                GlobalOffsetY = planeProjection.GlobalOffsetY,
                GlobalOffsetZ = planeProjection.GlobalOffsetZ
            };
        }

        public static EaseObject AnimateZProps(UIElement element,
            double rotationX, double rotationY, double rotationZ,
            double localOffsetX, double localOffsetY, double localOffsetZ,
            double globalOffsetX, double globalOffsetY, double globalOffsetZ,
            double centerOfRotationX, double centerOfRotationY, double centerOfRotationZ,
            double time, PercentHandler ease, double delay)
        { 
            return ArtefactAnimator.AddEase(element, UIElement.ProjectionProperty,
            new PlaneProjection
            {
                RotationX = rotationX,
                RotationY = rotationY,
                RotationZ = rotationZ,
                LocalOffsetX = localOffsetX,
                LocalOffsetY = localOffsetY,
                LocalOffsetZ = localOffsetZ,
                GlobalOffsetX = globalOffsetX,
                GlobalOffsetY = globalOffsetY,
                GlobalOffsetZ = globalOffsetZ,
                CenterOfRotationX = centerOfRotationX,
                CenterOfRotationY = centerOfRotationY,
                CenterOfRotationZ = centerOfRotationZ,
            },
            time, ease, delay);
        }
        #endregion

        #region UI
        public static bool HasChildrenAndAreVisible(Panel panelElement)
        {
            return panelElement.Children.Count >= 1 && panelElement.Children.Any(child => child.Visibility == Visibility.Visible);
        }
        #endregion

        #region DEBUG
        public static void SendToConsole(string message)
        {
            System.Windows.Browser.HtmlPage.Window.Eval("window.console.log(\"" + message + "\");");
        }
        #endregion

        #region LISTS
        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = new RNGCryptoServiceProvider();
            var n = list.Count;
            while (n > 1)
            {
                var b = new byte[1];
                do provider.GetBytes(b);
                while (!(b[0] < n * (Byte.MaxValue / n)));
                var k = (b[0] % n);
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        #endregion
    }

    public enum Views
    {
        None,
        Grid,
        Stack,
        Pile
    }

    public partial class MainPage
    {
        public List<RoundedSquare> Items;
        private Views _view;

        public MainPage()
        {
            // Required to initialize variables
            InitializeComponent();
 
            canvas.Children.Clear();
            versionTxt.Text = "version: " + ArtefactAnimator.Version;

            Loaded += MainPage_Loaded;
        }

        #region PROPS

        private double CenterX
        {
            get { return BoundsWidth / 2; }
        }

        private double CenterY
        {
            get { return BoundsHeight / 2; }
        }

        private double BoundsWidth
        {
            get { return bounds.Width; }
        }

        private double BoundsHeight
        {
            get { return bounds.Height; }
        }
        #endregion

        #region INITIALIZE
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainPage_Loaded;

            CompositionTarget.Rendering += _Render;

            Items = new List<RoundedSquare>();
            for (var i = 0; i < 25; i++)
            {
                var item = new RoundedSquare
                {
                    CacheMode = new BitmapCache(),
                    Width = 80,
                    Height = 80
                };

                item.NormalizeTransformGroup();
                item.RenderTransformOrigin = new Point(.5, .5);
                item.MouseLeftButtonDown += ItemMouseLeftButtonDown;

                Items.Add(item);
                canvas.Children.Add(item);
            }

            // Hide the options
            stackOptions.AutoAlphaCollapsedTo(0);
            gridOptions.AutoAlphaCollapsedTo(0);
            pileOptions.AutoAlphaCollapsedTo(0);

            SetView(Views.Pile);
        }

        void _Render(object sender, EventArgs e)
        {
            countTxt.Text = EaseObject.EaseObjectRunningCount.ToString();
            frameTxt.Text = FPS.Update().ToString();
            fpsLineTrans.Rotation++;
        }

        #endregion

        #region UTILS
        private void SetOptionsVisible(Panel panelElement, bool isTrue)
        {
            if (!isTrue || !Helper.HasChildrenAndAreVisible(panelElement))
            {
                options.AutoAlphaCollapsedTo(0, .5, AnimationTransitions.CubicEaseOut, 0);
            }
            else if (Helper.HasChildrenAndAreVisible(panelElement))
            {
                options.AutoAlphaCollapsedTo(1, .5, AnimationTransitions.CubicEaseOut, 0);
            }
            panelElement.AutoAlphaCollapsedTo(isTrue ? 1 : 0, .5, AnimationTransitions.CubicEaseOut, 0);
        }
        #endregion

        #region INTERACTION
        #region NAV PANEL
        private void PileBtnClick(object sender, RoutedEventArgs e)
        {
            SetView(Views.Pile);
        }

        private void StackBtnClick(object sender, RoutedEventArgs e)
        {
            SetView(Views.Stack);
        }

        private void GridBtnClick(object sender, RoutedEventArgs e)
        {
            SetView(Views.Grid);
        }
        #endregion

        #region OPTION INTERACTION
        private void GridRandomBtnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in Items)
            {
                var nScale = Helper.RandomRange(.5, 1.3);
                item.ScaleTo(nScale, nScale, 2, AnimationTransitions.ElasticEaseOut, 0);
            }
        }

        private void GridResortBtnClick(object sender, RoutedEventArgs e)
        {
            var eog = new EaseObjectGroup { UseStoppedEvent = false };
            var len = Items.Count;
            for (var i = 0; i < len; i++)
            {
                var item = Items[i];

                // BOUNCE OFF THE BOTTOM - VIA CUSTOM EASE POINTS
                var eo = ArtefactAnimator.AddEase(Items[i], RoundedSquare.PositionProperty, new Point(Canvas.GetLeft(item), bounds.Height - ((item.Height * item.ScaleY) / 2) - (item.Height / 2)),
                    1.3, CustomAnimationTransitions.CreateCustomBezierEase(CustomAnimationTransitions.CustomEasePointsBounce), (len - 1 - i) * .05);

                eog.AddEaseObject(eo);
            }

            // WHEN GROUP ANIMATION IS COMPLETE -> RESORT
            eog.Complete += group =>
            {
                // SORT BY SCALE
                var sorted =
                    from item in Items
                    orderby item.ScaleX, item.ScaleY descending
                    select item;

                // RESET LIST
                Items = sorted.ToList();

                // RE-GRID
                OrganizeGrid();
            };
        }

        private void PileRandomizeBtnClick(object sender, RoutedEventArgs e)
        {
            //...
        }

        private void CircleBtnClick(object sender, RoutedEventArgs e)
        {
            var len = Items.Count;
            var center = Helper.GetApothem(80, len);

            var eog = new EaseObjectGroup { UseStoppedEvent = false };

            for (var i = 0; i < len; i++)
            {
                var item = Items[i];

                ArtefactAnimator.StopEase(item,
                                                ProjectionProperty,
                                                AnimationTypes.X,
                                                AnimationTypes.Y,
                                                RoundedSquare.PositionProperty);

                // movement
                double x = CenterX - (item.Width / 2);
                double y = CenterY - (item.Height / 2);
                var eo = ArtefactAnimator.AddEase(item, RoundedSquare.PositionProperty,
                                                new Point { X = x, Y = y },
                                                .6, AnimationTransitions.SineEaseOut, 0);

                Helper.AnimateZProps(item, 0, i * (180 / len), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, .6, AnimationTransitions.CubicEaseOut, 0);

                eog.AddEaseObject(eo);
            }

            eog.Complete += group =>
            {
                for (var i = 0; i < len; i++)
                {
                    var item = Items[i];

                    // 3d
                    Helper.AnimateZProps(item, 0, ((double)i / len) * 360, 45, 0, 0, 0, 0, 0, 0, 0, 0, center,
                    2, AnimationTransitions.ElasticEaseOut, (len - i) * .1).OnComplete((o, p) => ((RoundedSquare)o.Target).Flash());
                }
            };
        }

        private void StackStackBtnClick(object sender, RoutedEventArgs e)
        {
            OrganizeStack();
        }
        #endregion

        #region ITEM INTERACTION
        void ItemMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (RoundedSquare)sender;
            switch (_view)
            {
                case Views.Grid:
                    break;
                case Views.Pile:
                    PileNav(item);
                    break;
                case Views.Stack:
                    break;
            }
        }
        #endregion
        #endregion

        #region Navigation
        private void PileNav(RoundedSquare selectedItem)
        {
            var selectedProjection = selectedItem.Projection as PlaneProjection;
            var selectedPos = selectedItem.Position;

            if (selectedProjection == null) return;


            foreach (var item in Items)
            {
                var currentProjection = Helper.Copy(item.Projection as PlaneProjection);
                currentProjection.LocalOffsetZ -= selectedProjection.LocalOffsetZ;
                currentProjection.LocalOffsetX -= selectedProjection.LocalOffsetX;
                currentProjection.LocalOffsetY -= selectedProjection.LocalOffsetY;

                var newPos = new Point(item.Position.X - selectedPos.X + CenterX, item.Position.Y - selectedPos.Y + CenterY);

                ArtefactAnimator.AddEase(item, RoundedSquare.PositionProperty, newPos, .6, AnimationTransitions.CubicEaseOut, 0);
                ArtefactAnimator.AddEase(item, ProjectionProperty, currentProjection, 1, AnimationTransitions.CubicEaseIn, 0).Update += (eo, p) =>
                {
                    var target = (FrameworkElement)eo.Target;
                    var pp = target.Projection as PlaneProjection;

                    // set blur based on z-depth
                    if (pp == null) return;

                    target.Effect = new BlurEffect { Radius = -pp.LocalOffsetZ / 50 };
                    target.Opacity = pp.LocalOffsetZ < 200 ? 1 : 1 - (Math.Min(pp.LocalOffsetZ - 200, 500) / 500);
                };
            }

        }
        #endregion

        #region VIEWS

        private void SetView(Views newView)
        {
            if (_view == newView) return;
            var oldView = _view;
            _view = newView;

            // OLD VIEW

            switch (oldView)
            {
                case Views.Stack:

                    stackBtn.IsEnabled = true;
                    SetOptionsVisible(stackOptions, false);
                    break;

                case Views.Grid:

                    gridBtn.IsEnabled = true;
                    SetOptionsVisible(gridOptions, false);
                    foreach (var item in Items)
                    {
                        item.ScaleTo(1, 1, .4, null, 0);
                    }
                    break;

                case Views.Pile:

                    pileBtn.IsEnabled = true;
                    SetOptionsVisible(pileOptions, false);

                    foreach (var item in Items)
                    {
                        item.AlphaTo(1, .6, AnimationTransitions.CubicEaseOut, 0);
                        item.Effect = null;
                    }
                    break;
            }

            // NEW VIEW

            switch (newView)
            {
                case Views.Stack:

                    SetOptionsVisible(stackOptions, true);
                    stackBtn.IsEnabled = false;
                    OrganizeStack();
                    break;

                case Views.Grid:

                    SetOptionsVisible(gridOptions, true);
                    gridBtn.IsEnabled = false;
                    OrganizeGrid();
                    break;

                case Views.Pile:

                    SetOptionsVisible(pileOptions, true);
                    pileBtn.IsEnabled = false;
                    OrganizePile();
                    break;
            }
        }
        #endregion

        #region STACK

        private void OrganizeStack()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];


                ArtefactAnimator.StopEase(item,
                                                Animation3DEffects.Projection,
                                                AnimationTypes.X,
                                                AnimationTypes.Y,
                                                RoundedSquare.PositionProperty);

                // movement
                double x = CenterX - (item.Width / 2);
                double y = CenterY - (item.Height / 2) + (CenterY / 3);

                /* 
                item.SlideTo(   x, y,
                                .8, AnimationTransitions.CubicEaseOut, i * .1);
                */

                // Instead of using a shortcut - you could also animate a position point.
                ArtefactAnimator.AddEase(item, RoundedSquare.PositionProperty,
                                                new Point { X = x, Y = y },
                                                .8, AnimationTransitions.CubicEaseOut, i * .1);

                // 3d
                Helper.AnimateZProps(item, 114, -22, 0, 0, 0, 10 * -i, 0, 0, 0, 0, 0, 0,
                .6, AnimationTransitions.CubicEaseOut, i * .1).OnUpdate((eo, p) =>
                {
                    var target = (FrameworkElement)eo.Target;
                    var pp = target.Projection as PlaneProjection;

                    if (pp != null) target.Effect = new BlurEffect { Radius = -pp.LocalOffsetZ / 50 };

                }).OnComplete((eo, p) => ((RoundedSquare)eo.Target).Flash());
            }
        }
        #endregion

        #region PILE

        private void OrganizePile()
        {
            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                ArtefactAnimator.StopEase(item,
                                                ProjectionProperty,
                                                AnimationTypes.X,
                                                AnimationTypes.Y,
                                                RoundedSquare.PositionProperty);
                // movement
                double x = Helper.RandomRange(20, BoundsWidth - item.Width - 20);
                double y = Helper.RandomRange(60, BoundsHeight - item.Height - 60);

                ArtefactAnimator.AddEase(item, RoundedSquare.PositionProperty,
                                                new Point { X = x, Y = y },
                                                .8, AnimationTransitions.CubicEaseOut, i * .05);

                // 3d
                double depth = (Helper.Rnd.NextDouble() * -2000.0) + 400;

                Helper.AnimateZProps(item, 10, 0, 0, 0, 0, depth, 0, 0, 0, 0, 0, 0,
                .6, AnimationTransitions.CubicEaseOut, i * .05).OnUpdate((eo, p) =>
                {
                    var target = (FrameworkElement)eo.Target;
                    var pp = target.Projection as PlaneProjection;

                    if (pp != null) target.Effect = new BlurEffect { Radius = -pp.LocalOffsetZ / 50 };

                }).OnComplete((eo, p) => ((RoundedSquare)eo.Target).Flash());
            }
        }
        #endregion

        #region GRID

        private void OrganizeGrid()
        {
            const double xSpace = 10;
            const double ySpace = 10;
            double w = Items[0].Width;
            double h = Items[0].Height;

            double cols = Math.Floor(BoundsWidth / w);

            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                double row = Math.Floor(i / cols);
                double col = i % cols;

                ArtefactAnimator.StopEase(item, ProjectionProperty,
                                                AnimationTypes.X,
                                                AnimationTypes.Y,
                                                RoundedSquare.PositionProperty);

                // movement
                double x = col * (w + xSpace);
                double y = row * (h + ySpace);

                ArtefactAnimator.AddEase(item, RoundedSquare.PositionProperty,
                                                new Point { X = x, Y = y },
                                                .8, AnimationTransitions.CubicEaseOut, 0);

                // 3d
                Helper.AnimateZProps(item, 0, 0, 0, 0, 0, -i, 0, 0, 0, 0, 0, 0,
                 .6, AnimationTransitions.CubicEaseOut, 0).OnUpdate((eo, p) =>
                 {
                     var target = (FrameworkElement)eo.Target;
                     var pp = target.Projection as PlaneProjection;
                     if (pp != null) target.Effect = new BlurEffect { Radius = -pp.LocalOffsetZ / 50 };
                 }).OnComplete((eo, p) => ((RoundedSquare)eo.Target).Flash());
            }
        }


        #endregion
    }
}