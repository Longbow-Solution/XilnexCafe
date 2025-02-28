﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CFSSK.View
{
    /// <summary>
    /// Interaction logic for MenuDetails.xaml
    /// </summary>
    public partial class MenuDetails : UserControl
    {
        public MenuDetails()
        {
            InitializeComponent();

            #region Slider 1
            verticalOffset = 0;
            scrollStoryboard = this.FindResource("scrollStoryboard") as Storyboard;
            scrollAnimation = scrollStoryboard.Children[0] as DoubleAnimation;
            #endregion
        }

        double scrollableHeight;
        double verticalOffset;

        Storyboard scrollStoryboard;
        DoubleAnimation scrollAnimation;

        DispatcherTimer timer = new DispatcherTimer();

        #region Slider 1

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (verticalOffset == e.NewValue)
                return;

            verticalOffset = e.NewValue;
            Storyboard.SetTarget(scrollStoryboard, SvMenuDetails);
            Storyboard.SetTargetProperty(scrollStoryboard, new PropertyPath(ScrollViewerUtilities.VerticalOffsetProperty));
            scrollAnimation.From = ScrollViewerUtilities.GetVerticalOffset(SvMenuDetails);
            scrollAnimation.To = e.NewValue;

            scrollStoryboard.Begin();
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            ScrollViewerUtilities.SetVerticalOffset(SvMenuDetails, verticalOffset);
        }

        public void InitializeSlider()
        {
            if (scrollableHeight > 0)
            {
                verticalSlider.Maximum = scrollableHeight;
                verticalSliderVisible.Value = 0;
                ScrollViewerUtilities.SetVerticalOffset(SvMenuDetails, 0);
                verticalSliderVisible.Visibility = System.Windows.Visibility.Visible;
                verticalSlider.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                verticalSliderVisible.Visibility = System.Windows.Visibility.Collapsed;
                verticalSlider.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            verticalOffset = SvMenuDetails.VerticalOffset;
            //verticalSlider.Value = scroll.VerticalOffset;

            ScrollViewerUtilities.SetVerticalOffset(SvMenuDetails, verticalOffset);
        }

        private void scroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (SvMenuDetails.ScrollableHeight != scrollableHeight)
            {
                scrollableHeight = SvMenuDetails.ScrollableHeight;
                InitializeSlider();
            }

            verticalSliderVisible.Value = e.VerticalOffset;
            if (timer.IsEnabled) timer.Stop();
            timer.Start();
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            #region Slider 1

            if (timer == null)
                timer = new DispatcherTimer();

            SvMenuDetails.ScrollChanged += scroll_ScrollChanged;

            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += timer_Tick;

            scrollableHeight = SvMenuDetails.ScrollableHeight;
            InitializeSlider();
            #endregion
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            SvMenuDetails.ScrollChanged -= scroll_ScrollChanged;
            timer.Stop();
            timer.Tick -= timer_Tick;
            timer = null;

        }
    }
}
