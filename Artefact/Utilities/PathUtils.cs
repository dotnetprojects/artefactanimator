/*
    Copyright © 2010 Jesse Graupmann
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
  
    Special Thanks:
    
        Dave Myron | http://www.artefactgroup.com
        Regular expression to parse PathGeometry from String.     
*/

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace Artefact.Utilities
{
    public static class PathUtils
    {
        private static readonly Regex PathGeometryExpression = new Regex("(?:[A-Z](?:(?:[0-9.-]+)[, ]?)*)(?=\\s|\\Z)");

        /// <summary>
        /// Converts a string of Path.Data into PathGeometry. 
        /// </summary>
        /// <param name="pathStr">Path.Data in string form.</param>
        /// <returns>PathGeometry from data string</returns>
        /// <example>
        /// pathStr = "M532,181 L532,219 L571,219 L570.54004,180.9397 z";
        /// var geometry = PathUtils.ParsePathGeometryString(pathStr);
        /// ArtefactAnimator.AddEase(myPath, Path.DataProperty, geometry, 3, AnimationTransitions.CubicEaseOut, 0);
        /// </example>
        public static PathGeometry ParsePathGeometryString ( string pathStr )
        {
            // string too short or doesn't exist
            if ( pathStr == null || pathStr.Length < 2 )return new PathGeometry();

            // groups representing segments
            var matches = PathGeometryExpression.Matches(pathStr);

            // single figure holding segements
            var figure = new PathFigure
            {
                Segments = new PathSegmentCollection(), 
                StartPoint = new Point(),
                IsClosed = pathStr.Substring(pathStr.Length-1, 1).ToLower() == "z"
            };

            // final path geometry
            var pathGeo = new PathGeometry { Figures = new PathFigureCollection{figure} };

            // parse matches and put in figure
            for (var i = 0; i < matches.Count; i++) ParseMatch(figure, matches[i]);

            return pathGeo;
        }

        private static Point[] GetPoints(IList<string> stringPts)
        {
            // final Point Array;
            var pts = new Point[stringPts.Count];

            for ( var i = 0; i < stringPts.Count; i++)
            {
                // convert groups of points
                var ptStr = stringPts[i].Split(',');
                pts[i] = new Point { X = double.Parse(ptStr[0]), Y = double.Parse(ptStr[1])};
            }

            return pts;
        }

        private static void ParseMatch(PathFigure figure, Capture match)
        {
            var val = match.Value;

            // type of path
            var t = val.Substring(0, 1).ToLower();

            // point data in string
            val = val.Substring(1, val.Length - 1);

            // seperate points
            var strPts = val.Split(' ');

            // parse points to doubles
            var pts = GetPoints(strPts);

            // create segment by type
            switch (t)
            {
                case "m":
                    // M 10,50
                    figure.StartPoint = pts[0];
                    break;

                case "c":
                    // C 100,0 200,200 300,100 = BezierSegment
                    figure.Segments.Add(new BezierSegment {Point1 = pts[0], Point2 = pts[1], Point3 = pts[2]});
                    break;

                case "l":
                    // L 200,70 = LineSegment
                    figure.Segments.Add(new LineSegment {Point = pts[0]});
                    break;

                case "q":
                    // Q 200,200 300,100 = QuadraticBezierSegment
                    figure.Segments.Add(new QuadraticBezierSegment {Point1 = pts[0], Point2 = pts[1]});
                    break;

                case "a":
                    // TODO Figure out how to parse ArcSegment -> http://msdn.microsoft.com/en-us/library/system.windows.media.arcsegment.aspx
                    // A 100,50 45 1 0 200,100 = ArcSegment
                    figure.Segments.Add(new ArcSegment {Point = pts[0]});
                    break;
            }
        }
    }
}
