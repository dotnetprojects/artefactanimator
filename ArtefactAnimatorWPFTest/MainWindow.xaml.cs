using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Artefact.Animation;
using System.Diagnostics;
using System.Windows.Media.Effects;
using Artefact.Animation.Extensions;

namespace ArtefactAnimatorWPFTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
        private static readonly UIElement[] Items = new UIElement[25];

		public MainWindow()
		{
			InitializeComponent();
            Loaded += _Loaded;
        }

        void _Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= _Loaded;
            PreviewKeyDown += _KeyDown;
            MouseMove += _MouseMove;

            Width = double.NaN;
            Height = double.NaN;

            AnimationBlurEffects.Init();

            for (var i = 0; i < Items.Length; i++)
            {
                var g = new Grid {Width = 30, Height = 30};
                var ball = new Ellipse { Fill=new SolidColorBrush(Colors.Red), Opacity=.2};
                var tb = new TextBlock { Text = "" + i, HorizontalAlignment=HorizontalAlignment.Center, VerticalAlignment=VerticalAlignment.Center };
                
                Canvas.SetLeft(g, 0);
                Canvas.SetTop(g, 0);
                
                Items[i] = g;
                
                g.Children.Add(ball);
                g.Children.Add(tb);
                g.NormalizeTransformGroup();
                g.RenderTransformOrigin = new Point(.5, .5);

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

                    ArtefactAnimator.AddEase(ball, UIElement.EffectProperty, effectClone, 2, AnimationTransitions.CubicEaseOut, 0);
                }
                else if (e.Key == Key.C)
                {
                    var effect = (DropShadowEffect)Items[i].Effect;
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
                    Items[i].Effect = null;
                }
            }
        }

        void _MouseMove(object sender, MouseEventArgs e)
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            double x = Mouse.GetPosition(this).X;
            double y = Mouse.GetPosition(this).Y;

            for (int n = 0; n < Items.Length; n++)
            {
                UIElement ball = Items[n];

                double size = 20 + (_rnd.NextDouble() * 150);
                double delay = (((double)n) * .01) + 0;

                //  STRINGS
                ball.DimensionsTo(size, size, 3, AnimationTransitions.ElasticEaseOut, 0);
                ball.SlideTo(x + (size / 2), y + (size / 2), 1, AnimationTransitions.CubicEaseOut, delay);
                ball.RotateTo(_rnd.NextDouble() * 360, 1, AnimationTransitions.ElasticEaseOut, 0); 
            }
        }

	    readonly Random _rnd = new Random();
        private void BtnClick(object sender, RoutedEventArgs e)
        {
            var eog = new EaseObjectGroup();
            eog.Complete += g => Debug.WriteLine("COMPLETE");
            
            for (var i = 0; i < 4; i++)
            {
                var width = .3 + (_rnd.NextDouble() * 2.7);
                var eo = ArtefactAnimator.AddEase(col, new[] { AnimationTypes.ColumWidthStar }, new object[] { width }, 1, AnimationTransitions.ElasticEaseOut, i * .5);
                
                eog.AddEaseObject(eo);
            }
        }

	}
}