using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Artefact.Animation;
using Artefact.Animation.Extensions;
using System.Diagnostics;
using System.Windows.Media.Effects;

namespace ArtefactAnimatorSilverlightTest
{
	public partial class MainPage
	{
	    private static readonly UIElement[] Items = new UIElement[25];

		public MainPage()
		{
			InitializeComponent();
            Loaded += _Loaded;
		}

        void _Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= _Loaded;
            KeyDown += _KeyDown;
            MouseMove += _MouseMove;

            Width = double.NaN;
            Height = double.NaN;

            AnimationBlurEffects.Init();
            Animation3DEffects.Init();

            for (var i = 0; i < Items.Length; i++)
            {
                var g = new Grid { Width = 30, Height = 30 };
                var ball = new Ellipse { Fill = new SolidColorBrush(Colors.Red), Opacity = .2 };
                var tb = new TextBlock { Text = "" + i, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                
                Canvas.SetLeft(g, 0D);
                Canvas.SetTop(g, 0D);
                
                Items[i] = g;
                
                g.Children.Add(ball);
                g.Children.Add(tb);
                g.RenderTransformOrigin = new Point(.5, .5);
                g.Effect = new DropShadowEffect();

                LayoutRoot.Children.Add(g);
            }

            CompositionTarget.Rendering += (s, args) =>
            {
                countTxt.Text = "EaseObjects in memory = " + EaseObject.EaseObjectRunningCount;
            };
        }

        void _KeyDown(object sender, KeyEventArgs e)
        {
            // REMOVE ALL EASING FOR ITEMS
            for (var i = 0; i < Items.Length; i++)
            {
                UIElement ball = Items[i];

                //// MOVEMENT

                if (e.Key == Key.R)
                {
                    double x = _rnd.NextDouble() * LayoutRoot.ActualWidth;
                    double y = _rnd.NextDouble() * LayoutRoot.ActualHeight;

                    double delay = i * .02;
                    double time = .6;
                    PercentHandler ease = AnimationTransitions.CubicEaseOut;

                    ball.SlideTo(x, y, time, ease, delay);
                }

                //// EFFECTS
                
                else if (e.Key == Key.D)
                {
                    var effect = (DropShadowEffect)ball.Effect;
                    if (effect == null)
                    {
                        effect = new DropShadowEffect();
                        ball.Effect = effect;
                    }
                    DropShadowEffect effectClone = effect.Copy();
                    effectClone.BlurRadius = _rnd.NextDouble() * 20;
                    effectClone.Opacity = .7 + (_rnd.NextDouble() * .3);
                    effectClone.ShadowDepth = _rnd.NextDouble() * 20;

                    ArtefactAnimator.AddEase(ball, UIElement.EffectProperty, effectClone, .8, AnimationTransitions.CubicEaseOut, 0);
                }
                else if (e.Key == Key.C)
                {
                    var effect = (DropShadowEffect)ball.Effect;
                    if (effect == null) continue;
                    ArtefactAnimator.AddEase(effect, DropShadowEffect.ColorProperty, new Color()
                    {
                        A = 1,
                        R = (byte)(_rnd.NextDouble() * 255),
                        G = (byte)(_rnd.NextDouble() * 255),
                        B = (byte)(_rnd.NextDouble() * 255)
                    }
                    , 1, AnimationTransitions.CubicEaseOut, 0);
                }

                //// REMOVE EFFECT
                
                else if (e.Key == Key.X)
                {
                    ball.Effect = null;
                }

                //// 3D
                
                else if (e.Key == Key.Z)
                {
                    Canvas.SetLeft(ball, Canvas.GetLeft(ball) + 2);

                    var pp = (PlaneProjection)ball.Projection ??
                             new PlaneProjection { RotationX = 0D, RotationY = 0D, RotationZ = 0D };

                    ball.Projection = pp;

                    EaseObject easeObject = ArtefactAnimator.AddEase(pp, 
                        new []{ 
                        Animation3DEffects.RotationX, 
                        Animation3DEffects.RotationY, 
                        Animation3DEffects.RotationZ},
                        new []{
                        RandomDouble(-75,75), 
                        RandomDouble(-75,75), 
                        RandomDouble(-75,75)
                    },
                    2, AnimationTransitions.ElasticEaseOut, 0);

                    easeObject.Update += (eo, per) => 
                    {
                        (eo.Data as UIElement).Projection = eo.Target as PlaneProjection;
                    };
                    easeObject.Data = ball;
                }
            }
        }

        public static Random Rnd = new Random();
        public static double RandomDouble(double min, double max)
        {
            return (Rnd.NextDouble() * (max - min)) + min;
        }

        void _MouseMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }

        private void UpdatePosition(MouseEventArgs e)
        {
            double x = e.GetPosition(this).X - (30 / 2);
            double y = e.GetPosition(this).Y - (30 / 2);

            for (int n = 0; n < Items.Length; n++)
            {
                UIElement ball = Items[n];

                double size = 20 + (_rnd.NextDouble() * 150);
                double delay = (n * .01) + 0;
                
                //  DEPENDENCY PROPERTY
                //  ArtefactAnimator.AddEase(ball, new[] { Canvas.LeftProperty, Canvas.TopProperty }, new[] { x, y }, 2, AnimationTransitions.ElasticEaseOut, delay);

                //  STRINGS
                ball.DimensionsTo(size, size, 3, AnimationTransitions.ElasticEaseOut, 0);
                EaseObject eo = ball.SlideTo(x + (size / 2), y + (size / 2), 1, AnimationTransitions.CubicEaseOut, delay);
                ArtefactAnimator.AddEase(ball, RenderTransformProperty, new CompositeTransform { Rotation = _rnd.NextDouble() * 360 }, 4, AnimationTransitions.ElasticEaseOut, 0);
            }
        }

	    readonly Random _rnd = new Random();
        private void BtnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            EaseObjectGroup eog = new EaseObjectGroup();
            
            for (int i = 0; i < 4; i++)
            {
                double star = .3 + (_rnd.NextDouble() * 2.7);

                EaseObject eo = ArtefactAnimator.AddEase(col, new[] { AnimationTypes.ColumWidthStar }, new object[] { star }, 1, AnimationTransitions.ElasticEaseOut, i * .5);
                
                eog.AddEaseObject(eo);
            }
        }
	}
}