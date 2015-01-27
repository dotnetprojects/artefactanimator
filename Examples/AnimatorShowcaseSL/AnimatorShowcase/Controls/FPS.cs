using System;

namespace AnimatorShowcase
{
    public static class FPS
    {
        private static double _fps;
        private static int _frames;
        private static DateTime _last = DateTime.Now;

        public static double Update()
        {
            _frames++;
            var dif = DateTime.Now.Subtract(_last).TotalMilliseconds;

            if ( dif > 1000)
            {
                _fps = Math.Round(((_frames*(dif/1000.0))*100))/100;
                _last = DateTime.Now;
                _frames = 0;
            }
            return _fps;
        }
    }
}
