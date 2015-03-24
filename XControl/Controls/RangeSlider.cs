namespace XControl.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:XControl.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:XControl.Controls;assembly=XControl.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:RangeSlider/>
    ///
    /// </summary>
    [DefaultEvent("RangeSelectionChanged"),
    TemplatePart(Name = "PART_Track", Type = typeof(Border)),
    TemplatePart(Name = "PART_Indicator", Type = typeof(Border)),
    TemplatePart(Name = "PART_LeftEdge", Type = typeof(RepeatButton)),
    TemplatePart(Name = "PART_ProgressThumb", Type = typeof(Thumb)),
    TemplatePart(Name = "PART_RightEdge", Type = typeof(RepeatButton)),
    TemplatePart(Name = "PART2_LeftThumb", Type = typeof(Thumb)),
    TemplatePart(Name = "PART2_RightThumb", Type = typeof(Thumb)),
    TemplatePart(Name = "PART2_LeftEdge", Type = typeof(RepeatButton)),
    TemplatePart(Name = "PART2_CenterEdge", Type = typeof(RepeatButton)),
    TemplatePart(Name = "PART2_RightEdge", Type = typeof(RepeatButton))]
    public class RangeSlider : Control
    {
        #region Data Members
        const double RepeatButtonMoveRatio = 0.1; // used to move the selection by x ratio when click the repeat buttons
        Border trackBorder;
        Border indicatorBorder;
        RepeatButton leftButton;
        RepeatButton rightButton;
        Thumb progressThumb;
        Thumb leftThumb;
        Thumb rightThumb;
        RepeatButton leftEdge;
        RepeatButton centerEdge;
        RepeatButton rightEdge;
        #endregion

        #region DependencyProperties
        public static readonly DependencyProperty StartProperty =
            DependencyProperty.Register("Start", typeof(int), typeof(RangeSlider), new UIPropertyMetadata(0,
                delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    RangeSlider slider = sender as RangeSlider;
                    slider.RecalculateWidth();
                }));

        public int Start
        {
            get { return (int)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        public static readonly DependencyProperty EndProperty =
            DependencyProperty.Register("End", typeof(int), typeof(RangeSlider), new UIPropertyMetadata(1,
                delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    RangeSlider slider = sender as RangeSlider;
                    slider.RecalculateWidth();
                }));
        
        public int End
        {
            get { return (int)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Min", typeof(int), typeof(RangeSlider), new UIPropertyMetadata(0));

        public int Min
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(RangeSlider), new UIPropertyMetadata(100));

        public int Max
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
            
        public static readonly DependencyProperty ValueProperty
            = DependencyProperty.Register("Value", typeof(int), typeof(RangeSlider), new UIPropertyMetadata(0,
                delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    RangeSlider slider = sender as RangeSlider;
                    slider.RecalculateProgress();
                }));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region Constructor
        static RangeSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(typeof(RangeSlider)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            trackBorder = EnforceInstance<Border>("PART_Track");
            indicatorBorder = EnforceInstance<Border>("PART_Indicator");
            leftButton = EnforceInstance<RepeatButton>("PART_LeftEdge");
            rightButton = EnforceInstance<RepeatButton>("PART_RightEdge");
            progressThumb = EnforceInstance<Thumb>("PART_ProgressThumb");
            leftThumb = EnforceInstance<Thumb>("PART2_LeftThumb");
            rightThumb = EnforceInstance<Thumb>("PART2_RightThumb");
            leftEdge = EnforceInstance<RepeatButton>("PART2_LeftEdge");
            centerEdge = EnforceInstance<RepeatButton>("PART2_CenterEdge");
            rightEdge = EnforceInstance<RepeatButton>("PART2_RightEdge");

            InitializeVisualElementsContainer();
            RecalculateWidth();
        }

        T EnforceInstance<T>(string partName)
            where T : FrameworkElement, new()
        {
            T element = GetTemplateChild(partName) as T;
            if (element == null)
                element = new T();
            return element;
        }

        //adds all visual element to the conatiner
        private void InitializeVisualElementsContainer()
        {
            // handle the drag delta
            progressThumb.DragDelta += ProgressThumbDragDelta;
            leftThumb.DragDelta += LeftThumbDragDelta;
            rightThumb.DragDelta += RightThumbDragDelta;
        }
        #endregion

        #region Event handlers for visual elements to drag the range
        private void ProgressThumbDragDelta(object sender, DragDeltaEventArgs e) 
        {
            MoveThumb(leftButton, rightButton, e.HorizontalChange);
            RecalculateProgress();
        }

        private void LeftThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            MoveThumb(leftEdge, centerEdge, e.HorizontalChange);
            RecalculateWidth();
        }

        private void RightThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            MoveThumb(centerEdge, rightEdge, e.HorizontalChange);
            RecalculateWidth();
        }
        #endregion

        #region Logic to resize range

        private static void MoveThumb(FrameworkElement x, FrameworkElement y, double horizontalChange)
        {
            double change = 0;
            if (horizontalChange < 0) // slider went left
                change = GetChangeKeepPositive(x.Width, horizontalChange);
            else if(horizontalChange > 0) // slider went right
                change = -GetChangeKeepPositive(y.Width, -horizontalChange);

            x.Width += change;
            y.Width -= change;
        }

        //ensures that the new value (newValue param) is a valid value. returns false if not
        private static double GetChangeKeepPositive(double width, double increment)
        {
            return Math.Max(width + increment, 0) - width;
        }

        private void RecalculateProgress() 
        {
            if (leftButton != null && rightButton != null)
            { 
                leftButton.Width = Math.Max(leftButton.Width, 0);
                rightButton.Width = Math.Max(rightButton.Width, 0);
                Value = (int)(leftButton.Width * 1.0 / 440 * (Max - Min));
            }
        }

        private void RecalculateWidth()
        {
            if (leftEdge != null && centerEdge != null && rightEdge != null)
            {
                leftEdge.Width = Math.Max(leftEdge.Width, 0);
                centerEdge.Width = Math.Max(centerEdge.Width, 0);
                rightEdge.Width = Math.Max(rightEdge.Width, 0);

                Start = (int)(leftEdge.Width * 1.0 / 440 * (Max - Min));
                End = (int)((leftEdge.Width + leftThumb.Width + centerEdge.Width) * 1.0 / 440 * (Max - Min));

                indicatorBorder.Width = (End - Start) * 1.0 / (Max - Min) * 440;
                indicatorBorder.Margin = new Thickness((Start * 1.0) / (Max - Min) * 440 + 1, 0, 0, 0);
            }
        }
        #endregion
    }
}
