/*
    Copyright © 2009 Jesse Graupmann
    All rights reserved.

    Redistribution and use in source and binary forms, with or without 
    modification, are permitted provided that the following conditions 
    are met:

        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright 
          notice, this list of conditions and the following disclaimer 
          in the documentation and/or other materials provided with the 
          distribution.
        * Neither the name of the author nor the names of contributors 
          may be used to endorse or promote products derived from this 
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
    "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
    LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
    FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
    COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
    INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
    BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
    CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
    LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
    ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
    POSSIBILITY OF SUCH DAMAGE.
*/


#define IsDotNet4OrGreater

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace Artefact.Animation
{
    public static class EaseHelper
    {
        #region NUMBERS
        /// <summary>
        /// Returns new double by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static double EaseValue(double startValue, double endValue, double percent)
        {
            return startValue == endValue ? endValue : (startValue + ((endValue - startValue) * percent));
        }

        /// <summary>
        /// Returns new int by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static int EaseValue(int startValue, int endValue, double percent)
        {
            return startValue == endValue ? endValue : (int)(startValue + ((endValue - startValue) * percent));
        }

        /// <summary>
        /// Returns new decimal by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static decimal EaseValue(decimal startValue, decimal endValue, double percent)
        {
            return startValue == endValue ? endValue : (decimal)(((double)startValue) + (((double)(endValue - startValue) * percent)));
        }

        /// <summary>
        /// Returns new byte by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static byte EaseValue(byte startValue, byte endValue, double percent)
        {
            return startValue == endValue ? endValue : (byte)(startValue + (((endValue - startValue) * percent)));
        }
        #endregion

        #region COLORS
        /// <summary>
        /// Returns new Color with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Color EaseValue(Color startValue, Color endValue, double percent)
        {
            return new Color
            {
                A = EaseValue(startValue.A, endValue.A, percent),
                R = EaseValue(startValue.R, endValue.R, percent),
                G = EaseValue(startValue.G, endValue.G, percent),
                B = EaseValue(startValue.B, endValue.B, percent)
            };
        }
        #endregion

        #region BRUSHES
        /// <summary>
        /// Returns new SolidColorBrush with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static SolidColorBrush EaseValue(SolidColorBrush startValue, SolidColorBrush endValue, double percent)
        {
            return new SolidColorBrush
            {
                Opacity = EaseValue(startValue.Opacity, endValue.Opacity, percent),
                Color = EaseValue(startValue.Color, endValue.Color, percent),
            };
        }

        /// <summary>
        /// Returns new LinearGradientBrush with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static LinearGradientBrush EaseValue(LinearGradientBrush startValue, LinearGradientBrush endValue, double percent)
        {
            return new LinearGradientBrush
            {
                SpreadMethod = endValue.SpreadMethod,
                MappingMode = endValue.MappingMode,
                ColorInterpolationMode = endValue.ColorInterpolationMode,
                Opacity = EaseValue(startValue.Opacity, endValue.Opacity, percent),
                GradientStops = EaseValue(startValue.GradientStops, endValue.GradientStops, percent),
                EndPoint = EaseValue(startValue.EndPoint, endValue.EndPoint, percent),
                StartPoint = EaseValue(startValue.StartPoint, endValue.StartPoint, percent),
            };
        }

        /// <summary>
        /// Returns new RadialGradientBrush with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static RadialGradientBrush EaseValue(RadialGradientBrush startValue, RadialGradientBrush endValue, double percent)
        {
            return new RadialGradientBrush
            {
                SpreadMethod = endValue.SpreadMethod,
                MappingMode = endValue.MappingMode,
                ColorInterpolationMode = endValue.ColorInterpolationMode,
                Opacity = EaseValue(startValue.Opacity, endValue.Opacity, percent),
                RadiusX = EaseValue(startValue.RadiusX, endValue.RadiusX, percent),
                RadiusY = EaseValue(startValue.RadiusY, endValue.RadiusY, percent),
                Center = EaseValue(startValue.Center, endValue.Center, percent),
                GradientOrigin = EaseValue(startValue.GradientOrigin, endValue.GradientOrigin, percent),
                GradientStops = EaseValue(startValue.GradientStops, endValue.GradientStops, percent)
            };
        }

        /// <summary>
        /// Returns new GradientStopCollection with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static GradientStopCollection EaseValue(GradientStopCollection startValue, GradientStopCollection endValue, double percent)
        {
            if (endValue == null) return endValue;

            var collection = new GradientStopCollection();
            for (var i = 0; i < endValue.Count; i++)
            {
                var stop = new GradientStop();
                if (i < startValue.Count)
                {
                    stop.Offset = EaseValue(startValue[i].Offset, endValue[i].Offset, percent);
                    stop.Color = EaseValue(startValue[i].Color, endValue[i].Color, percent);
                }
                else
                {
                    stop.Offset = endValue[i].Offset;
                    stop.Color = endValue[i].Color;
                }
                collection.Add(stop);
            }
            return collection;
        }


        #endregion

        #region ARRAYS
        /// <summary>
        /// Returns new double[] with all values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static double[] EaseValue(double[] startValue, double[] endValue, double percent)
        {
            var a = new double[endValue.Length];
            for (var i = 0; i < endValue.Length; i++) a[i] = i < startValue.Length ? EaseValue(startValue[i], endValue[i], percent) : endValue[i];
            return a;
        }

        /// <summary>
        /// Returns new Point[] with all values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Point[] EaseValue(Point[] startValue, Point[] endValue, double percent)
        {
            var a = new Point[endValue.Length];
            for (var i = 0; i < endValue.Length; i++) a[i] = i < startValue.Length ? EaseValue(startValue[i], endValue[i], percent) : endValue[i];
            return a;
        }
        #endregion

        #region POINTS
        /// <summary>
        /// Returns new Point with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Point EaseValue(Point startValue, Point endValue, double percent)
        {
            return new Point(EaseValue(startValue.X, endValue.X, percent), EaseValue(startValue.Y, endValue.Y, percent));
        }

        /// <summary>
        /// Returns new PointCollection with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static PointCollection EaseValue(PointCollection startValue, PointCollection endValue, double percent)
        {
            var collection = new PointCollection();
            for (var i = 0; i < endValue.Count; i++) collection[i] = i < startValue.Count ? EaseValue(startValue[i], endValue[i], percent) : endValue[i];
            return collection;
        }
   

        /// <summary>
        /// Returns new Rect with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Rect EaseValue(Rect startValue, Rect endValue, double percent)
        {
            return new Rect(    EaseValue(startValue.X, endValue.X, percent), 
                                EaseValue(startValue.Y, endValue.Y, percent),
                                EaseValue(startValue.Width, endValue.Width, percent),
                                EaseValue(startValue.Height, endValue.Height, percent));
        }

        /// <summary>
        /// Returns new Matrix with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Matrix EaseValue(Matrix startValue, Matrix endValue, double percent)
        {
            return new Matrix(  EaseValue(startValue.M11, endValue.M11, percent),
                                EaseValue(startValue.M12, endValue.M12, percent),
                                EaseValue(startValue.M21, endValue.M21, percent),
                                EaseValue(startValue.M22, endValue.M22, percent),
                                EaseValue(startValue.OffsetX, endValue.OffsetX, percent),
                                EaseValue(startValue.OffsetY, endValue.OffsetY, percent));
        }

        /// <summary>
        /// Returns new Matrix3D with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Matrix3D EaseValue(Matrix3D startValue, Matrix3D endValue, double percent)
        {
            return new Matrix3D(EaseValue(startValue.M11, endValue.M11, percent),
                                EaseValue(startValue.M12, endValue.M12, percent),
                                EaseValue(startValue.M13, endValue.M13, percent),
                                EaseValue(startValue.M14, endValue.M14, percent),
                                EaseValue(startValue.M21, endValue.M21, percent),
                                EaseValue(startValue.M22, endValue.M22, percent),
                                EaseValue(startValue.M23, endValue.M23, percent),
                                EaseValue(startValue.M24, endValue.M24, percent),
                                EaseValue(startValue.M31, endValue.M31, percent),
                                EaseValue(startValue.M32, endValue.M32, percent),
                                EaseValue(startValue.M33, endValue.M33, percent),
                                EaseValue(startValue.M34, endValue.M34, percent),
                                EaseValue(startValue.OffsetX, endValue.OffsetX, percent),
                                EaseValue(startValue.OffsetY, endValue.OffsetY, percent),
                                EaseValue(startValue.OffsetZ, endValue.OffsetZ, percent),
                                EaseValue(startValue.M44, endValue.M44, percent));
        }
        #endregion

        #region BORDERS
        /// <summary>
        /// Returns new CornerRadius with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static CornerRadius EaseValue(CornerRadius startValue, CornerRadius endValue, double percent)
        {
            return new CornerRadius(    EaseValue(startValue.TopLeft, endValue.TopLeft, percent), 
                                        EaseValue(startValue.TopRight, endValue.TopRight, percent), 
                                        EaseValue(startValue.BottomLeft, endValue.BottomLeft, percent), 
                                        EaseValue(startValue.BottomRight, endValue.BottomRight, percent) );
        }
        #endregion

        #region THICKNESS / LENGTH
        /// <summary>
        /// Returns new Thickness with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Thickness EaseValue(Thickness startValue, Thickness endValue, double percent)
        {
            return new Thickness(       EaseValue(startValue.Left, endValue.Left, percent),
                                        EaseValue(startValue.Right, endValue.Right, percent),
                                        EaseValue(startValue.Top, endValue.Top, percent),
                                        EaseValue(startValue.Bottom, endValue.Bottom, percent));
        }

        /// <summary>
        /// Returns new GridLength with positive values eased from startValue to endValue using a time percentage 0 -> 1. GridUnitType is determined by the endValue.
        /// </summary>
        public static GridLength EaseValue(GridLength startValue, GridLength endValue, double percent)
        {
            return new GridLength(Math.Max(0,EaseValue(startValue.Value, endValue.Value, percent)), endValue.GridUnitType);
        }
        
        #endregion

        #region EFFECTS
        /// <summary>
        /// Returns new BlurEffect with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static BlurEffect EaseValue(BlurEffect startValue, BlurEffect endValue, double percent)
        {
            if (startValue == null) startValue = new BlurEffect { Radius = 0 };
            if (endValue == null) endValue = new BlurEffect { Radius = 0 };
           
            return new BlurEffect {  Radius = EaseValue(startValue.Radius,endValue.Radius,percent)   };
        }

        /// <summary>
        /// Returns new DropShadowEffect with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static DropShadowEffect EaseValue(DropShadowEffect startValue, DropShadowEffect endValue, double percent)
        {
            if (startValue == null) startValue = new DropShadowEffect { Color = Color.FromArgb(0, 0, 0, 0), Opacity = 0 };
            if (endValue == null) endValue = new DropShadowEffect { Color = Color.FromArgb(0, 0, 0, 0), Opacity = 0 };

            return new DropShadowEffect {   BlurRadius = EaseValue(startValue.BlurRadius, endValue.BlurRadius, percent), 
                                            Color = EaseValue(startValue.Color, endValue.Color, percent),
                                            Direction = EaseValue(startValue.Direction, endValue.Direction, percent),
                                            Opacity = EaseValue(startValue.Opacity, endValue.Opacity, percent),
                                            ShadowDepth = EaseValue(startValue.ShadowDepth, endValue.ShadowDepth, percent)
            };
        }
        #endregion

        #region PATHS
        /// <summary>
        /// Returns new Size by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static Size EaseValue(Size startValue, Size endValue, double percent)
        {
            return new Size(EaseValue(startValue.Width, endValue.Width, percent), EaseValue(startValue.Height, endValue.Height, percent));
        }

        /// <summary>
        /// Returns new PathGeometry by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static PathGeometry EaseValue(PathGeometry startValue, PathGeometry endValue, double percent)
        {
            return new PathGeometry
            {
               FillRule = endValue.FillRule, 
               Figures = EaseValue(startValue.Figures, endValue.Figures, percent)
            };
        }

        /// <summary>
        /// Returns new PathFigureCollection by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static PathFigureCollection EaseValue(PathFigureCollection startValue, PathFigureCollection endValue, double percent)
        {
            var collection = new PathFigureCollection();
            for (var i = 0; i < endValue.Count; i++ )
            {
                collection.Add(EaseValue(startValue[i], endValue[i], percent));
            }
            return collection; 
        }

        /// <summary>
        /// Returns new PathSegmentCollection by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static PathSegmentCollection EaseValue(PathSegmentCollection startValue, PathSegmentCollection endValue, double percent)
        {
            var collection = new PathSegmentCollection();
            for (var i = 0; i < endValue.Count; i++)
            {
                collection.Add(EaseValue(startValue[i], endValue[i], percent));
            }
            return collection;
        }

        /// <summary>
        /// Returns new PathFigure by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static PathFigure EaseValue(PathFigure startValue, PathFigure endValue, double percent)
        {
            return new PathFigure
            {
               IsClosed = endValue.IsClosed, 
               IsFilled = endValue.IsFilled, 
               StartPoint = EaseValue(startValue.StartPoint, endValue.StartPoint, percent),
               Segments = EaseValue(startValue.Segments, endValue.Segments, percent)
            };
        }

        /// <summary>
        /// Returns new PathSegment by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static PathSegment EaseValue(PathSegment startValue, PathSegment endValue, double percent)
        {
            var startType = startValue.GetType();
            var endType = endValue.GetType();
            if ( startType != endType ) return endValue; // can't ease different types... returning expected end type

            if ( endValue is LineSegment) return EaseValue((LineSegment)startValue, (LineSegment)endValue, percent);
            if ( endValue is BezierSegment) return EaseValue((BezierSegment)startValue, (BezierSegment)endValue, percent);
            if ( endValue is ArcSegment) return EaseValue((ArcSegment)startValue, (ArcSegment)endValue, percent);
            if ( endValue is PolyLineSegment) return EaseValue((PolyLineSegment)startValue, (PolyLineSegment)endValue, percent);
            if ( endValue is PolyBezierSegment) return EaseValue((PolyBezierSegment)startValue, (PolyBezierSegment)endValue, percent);
            if ( endValue is PolyQuadraticBezierSegment) return EaseValue((PolyQuadraticBezierSegment)startValue, (PolyQuadraticBezierSegment)endValue, percent);
            if ( endValue is QuadraticBezierSegment) return EaseValue((QuadraticBezierSegment)startValue, (QuadraticBezierSegment)endValue, percent);

            return endValue;
        }
         

        /// <summary>
        /// Returns new LineSegment by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        /// <example>XAML: Data="M 10,50 L 200,70"</example>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.windows.media.linesegment.aspx"/>
        public static LineSegment EaseValue(LineSegment startValue, LineSegment endValue, double percent)
        {
            return new LineSegment
            {
                Point = EaseValue(startValue.Point, endValue.Point, percent)
            };
        }

        /// <summary>
        /// Returns new PolyLineSegment by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        /// <example>XAML: Points="50,100 50,150"</example>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.windows.media.polylinesegment.aspx"/>
        public static PolyLineSegment EaseValue(PolyLineSegment startValue, PolyLineSegment endValue, double percent)
        {
            return new PolyLineSegment
            {
                Points = EaseValue(startValue.Points, endValue.Points, percent),
            };
        }

        /// <summary>
        /// Returns new PolyQuadraticBezierSegment by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        /// <example>XAML: Points="200,200 300,100 0,200 30,400"</example>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.windows.media.polyquadraticbeziersegment.aspx"/>
        public static PolyQuadraticBezierSegment EaseValue(PolyQuadraticBezierSegment startValue, PolyQuadraticBezierSegment endValue, double percent)
        {
            return new PolyQuadraticBezierSegment
            {
                Points = EaseValue(startValue.Points, endValue.Points, percent),
            };
        }

        /// <summary>
        /// Returns new PolyBezierSegment by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        /// <example>XAML: Points="0,0 200,0 300,100 300,0 400,0 600,100"</example>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.windows.media.polybeziersegment.aspx"/>
        public static PolyBezierSegment EaseValue(PolyBezierSegment startValue, PolyBezierSegment endValue, double percent)
        {
            return new PolyBezierSegment
            {
                Points = EaseValue(startValue.Points, endValue.Points, percent),
            };
        }

        /// <summary>
        /// Returns new QuadraticBezierSegment by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        /// <example>XAML: Data="M 10,100 Q 200,200 300,100"</example>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.windows.media.quadraticbeziersegment.aspx"/>
        public static QuadraticBezierSegment EaseValue(QuadraticBezierSegment startValue, QuadraticBezierSegment endValue, double percent)
        {
            return new QuadraticBezierSegment
            {
               Point1 = EaseValue(startValue.Point1, endValue.Point1, percent),
               Point2 = EaseValue(startValue.Point2, endValue.Point2, percent)
            };
        }

        /// <summary>
        /// Returns new BezierSegment  by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        /// <example>XAML: Data="M 10,100 C 100,0 200,200 300,100"</example>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.windows.media.beziersegment.aspx"/>
        public static BezierSegment EaseValue(BezierSegment startValue, BezierSegment endValue, double percent)
        {
            return new BezierSegment 
            {
                Point1 = EaseValue(startValue.Point1, endValue.Point1, percent),
                Point2 = EaseValue(startValue.Point2, endValue.Point2, percent),
                Point3 = EaseValue(startValue.Point3, endValue.Point3, percent)
            };
        }

        /// <summary>
        /// Returns new ArcSegment by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        /// <example>XAML: Data="M 10,100 A 100,50 45 1 0 200,100"</example>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.windows.media.arcsegment.aspx"/>
        public static ArcSegment EaseValue(ArcSegment startValue, ArcSegment endValue, double percent)
        {
            return new ArcSegment
            {
               IsLargeArc = endValue.IsLargeArc,
               Point = EaseValue(startValue.Point, endValue.Point, percent),
               RotationAngle = EaseValue(startValue.RotationAngle, endValue.RotationAngle, percent),
               Size = EaseValue(startValue.Size, endValue.Size, percent),
               SweepDirection = endValue.SweepDirection,
            };
        }


        #endregion

        #region TRANSFORMS
        #if SILVERLIGHT
        #if IsDotNet4OrGreater
        /// <summary>
        /// Returns new CompositeTransform by easing startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static CompositeTransform EaseValue(CompositeTransform startValue, CompositeTransform endValue, double percent)
        {
            return new CompositeTransform
            {
                CenterX =
                    (startValue.CenterX == endValue.CenterX)
                        ? endValue.CenterX
                        : EaseValue(startValue.CenterX, endValue.CenterX, percent),
                CenterY =
                    (startValue.CenterY == endValue.CenterY)
                        ? endValue.CenterY
                        : EaseValue(startValue.CenterY, endValue.CenterY, percent),
                Rotation =
                    (startValue.Rotation == endValue.Rotation)
                        ? endValue.Rotation
                        : EaseValue(startValue.Rotation, endValue.Rotation, percent),
                ScaleX =
                    (startValue.ScaleX == endValue.ScaleX)
                        ? endValue.ScaleX
                        : EaseValue(startValue.ScaleX, endValue.ScaleX, percent),
                ScaleY =
                    (startValue.ScaleY == endValue.ScaleY)
                        ? endValue.ScaleY
                        : EaseValue(startValue.ScaleY, endValue.ScaleY, percent),
                SkewX =
                    (startValue.SkewX == endValue.SkewX)
                        ? endValue.SkewX
                        : EaseValue(startValue.SkewX, endValue.SkewX, percent),
                SkewY =
                    (startValue.SkewY == endValue.SkewY)
                        ? endValue.SkewY
                        : EaseValue(startValue.SkewY, endValue.SkewY, percent),
                TranslateX =
                    (startValue.TranslateX == endValue.TranslateX)
                        ? endValue.TranslateX
                        : EaseValue(startValue.TranslateX, endValue.TranslateX, percent),
                TranslateY =
                    (startValue.TranslateY == endValue.TranslateY)
                        ? endValue.TranslateY
                        : EaseValue(startValue.TranslateY, endValue.TranslateY, percent),
            };
        }

        #endif
        #endif
        #endregion

        #region 3D
#if SILVERLIGHT
        /// <summary>
        /// Ease PlaneProjection with values eased from startValue to endValue using a time percentage 0 -> 1.
        /// </summary>
        public static void EaseValue(PlaneProjection current, PlaneProjection startValue, PlaneProjection endValue, double percent)
        {
            if (current.RotationX != endValue.RotationX) current.RotationX = EaseValue(startValue.RotationX, endValue.RotationX, percent);
            if (current.RotationY != endValue.RotationY) current.RotationY = EaseValue(startValue.RotationY, endValue.RotationY, percent);
            if (current.RotationZ != endValue.RotationZ) current.RotationZ = EaseValue(startValue.RotationZ, endValue.RotationZ, percent);
            if (current.LocalOffsetX != endValue.LocalOffsetX) current.LocalOffsetX = EaseValue(startValue.LocalOffsetX, endValue.LocalOffsetX, percent);
            if (current.LocalOffsetY != endValue.LocalOffsetY) current.LocalOffsetY = EaseValue(startValue.LocalOffsetY, endValue.LocalOffsetY, percent);
            if (current.LocalOffsetZ != endValue.LocalOffsetZ) current.LocalOffsetZ = EaseValue(startValue.LocalOffsetZ, endValue.LocalOffsetZ, percent);
            if (current.GlobalOffsetX != endValue.GlobalOffsetX) current.GlobalOffsetX = EaseValue(startValue.GlobalOffsetX, endValue.GlobalOffsetX, percent);
            if (current.GlobalOffsetY != endValue.GlobalOffsetY) current.GlobalOffsetY = EaseValue(startValue.GlobalOffsetY, endValue.GlobalOffsetY, percent);
            if (current.GlobalOffsetZ != endValue.GlobalOffsetZ) current.GlobalOffsetZ = EaseValue(startValue.GlobalOffsetZ, endValue.GlobalOffsetZ, percent);
            if (current.CenterOfRotationX != endValue.CenterOfRotationX) current.CenterOfRotationX = EaseValue(startValue.CenterOfRotationX, endValue.CenterOfRotationX, percent);
            if (current.CenterOfRotationY != endValue.CenterOfRotationY) current.CenterOfRotationY = EaseValue(startValue.CenterOfRotationY, endValue.CenterOfRotationY, percent);
            if (current.CenterOfRotationZ != endValue.CenterOfRotationZ) current.CenterOfRotationZ = EaseValue(startValue.CenterOfRotationZ, endValue.CenterOfRotationZ, percent);

            // TO INCLUDE?
            //if (current.ProjectionMatrix != endValue.ProjectionMatrix) EaseValue(current.ProjectionMatrix, startValue.ProjectionMatrix, endValue.ProjectionMatrix, percent);
        }
        #endif
        #endregion
    }
}
