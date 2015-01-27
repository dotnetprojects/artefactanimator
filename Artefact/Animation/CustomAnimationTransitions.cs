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




namespace Artefact.Animation
{
    public struct BezierEasePoint
    {
        public BezierEasePoint(double x, double y, double x2, double y2)
        {
            X = x;
            Y = y;
            X2 = x2;
            Y2 = y2;
        }

        public double X;
        public double Y;
        public double X2;
        public double Y2;
    }

    public static class CustomAnimationTransitions
    {
        #region CUSTOM BEZIER EQUATIONS
        public static PercentHandler CreateCustomBezierEase(BezierEasePoint[] pts)
        {
            return p => CustomBezierEase(p, pts);
        }
 
        public static double CustomBezierEase(double percent, BezierEasePoint[] pts)
        {
            int i;
            var ttl = pts[pts.Length - 1].X;
            var cur = ttl * percent;
            for (i = 0; cur > pts[i + 1].X; i++) { }
            var o = pts[i];
            var o2 = pts[i + 1];
            percent = (cur - o.X) / (o2.X - o.X);
            return QuadBezEase(percent, o.Y, o2.Y, o.Y2) / ttl;
        }

        #region EASE POINTS
        public static BezierEasePoint[] CustomEasePointsBounce =
        {
	        new BezierEasePoint ( 0, 0, 20, 20 ),
	        new BezierEasePoint ( 62, 195, 75, 40 ),
	        new BezierEasePoint ( 107, 197, 129, 134 ),
	        new BezierEasePoint ( 151, 196, 165, 161 ),
	        new BezierEasePoint ( 178, 198, 186, 186 ),
	        new BezierEasePoint ( 200, 200, 0, 0 )
        };
        #endregion

        #endregion

        #region BEZIER EQUATIONS
        public static double QuadBezEquation(double p, double i)
        {
            return 2 * p * (1 - p) * i + (p * p);
        }

        public static double QuadBezEase(double per, double p1, double p2, double p3)
        {
            return (QuadBezEquation(per, (p3 - p1) / (p2 - p1)) * (p2 - p1)) + p1;
        }
        #endregion
    }
}