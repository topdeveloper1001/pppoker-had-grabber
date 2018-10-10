//-----------------------------------------------------------------------
// <copyright file="PMWindow.cs" company="Ace Poker Solutions">
// Copyright © 2017 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PPPokerHandGrabber.Common.Wpf.Controls
{
    [TemplatePart(Name = "PART_TOP_BORDER", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_LEFT_BORDER", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_RIGHT_BORDER", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_BOTTOM_BORDER", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_RESIZE_BORDER", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_MINIMIZE_BUTTON", Type = typeof(Button))]
    [TemplatePart(Name = "PART_MAXIMIZE_BUTTON", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CLOSE_BUTTON", Type = typeof(Button))]
    [TemplatePart(Name = "PART_TITLE", Type = typeof(Border))]
    public class PHGWindow : Window
    {
        #region Part names

        private const string TopBorderPartName = "PART_TOP_BORDER";
        private const string LeftBorderPartName = "PART_LEFT_BORDER";
        private const string RightBorderPartName = "PART_RIGHT_BORDER";
        private const string BottomBorderPartName = "PART_BOTTOM_BORDER";
        private const string ResizeBorderPartName = "PART_RESIZE_BORDER";
        private const string MinimizeButtonPartName = "PART_MINIMIZE_BUTTON";
        private const string MaximizeButtonPartName = "PART_MAXIMIZE_BUTTON";
        private const string CloseButtonPartName = "PART_CLOSE_BUTTON";
        private const string TitlePartName = "PART_TITLE";

        #endregion

        #region Part elements

        /// <summary>
        /// Resize element
        /// </summary>
        private Thumb ResizeBorder { get; set; }

        /// <summary>
        /// Top border
        /// </summary>
        private Thumb TopBorder { get; set; }

        /// <summary>
        /// Bottom border
        /// </summary>
        private Thumb BottomBorder { get; set; }

        /// <summary>
        /// Left border
        /// </summary>
        private Thumb LeftBorder { get; set; }

        /// <summary>
        /// Right border
        /// </summary>
        private Thumb RightBorder { get; set; }

        /// <summary>
        /// Close button
        /// </summary>
        private Button CloseButton { get; set; }

        /// <summary>
        /// Minimize button
        /// </summary>
        private Button MinimizeButton { get; set; }

        /// <summary>
        /// Maximize button
        /// </summary>
        private Button MaximizeButton { get; set; }

        /// <summary>
        /// Title border
        /// </summary>
        private Border TitleBorder { get; set; }

        /// <summary>
        /// Minimize Command
        /// </summary>
        private readonly RoutedUICommand MinimizeCommand = new RoutedUICommand("Minimize", "Minimize", typeof(PHGWindow));

        /// <summary>
        /// Maximize / Restore command
        /// </summary>
        private readonly RoutedUICommand MaximizeRestoreCommand = new RoutedUICommand("MaximizeRestore", "MaximizeRestore", typeof(PHGWindow));

        #endregion

        #region DependencyProperties

        public static readonly DependencyProperty VersionTitleProperty = DependencyProperty.Register("VersionTitle", typeof(string), typeof(PHGWindow));

        public string VersionTitle
        {
            get
            {
                return (string)GetValue(VersionTitleProperty);
            }
            set
            {
                SetValue(VersionTitleProperty, value);
            }
        }

        public static readonly DependencyProperty StatusBarTemplateProperty = DependencyProperty.Register("StatusBarTemplate", typeof(DataTemplate), typeof(PHGWindow));

        public DataTemplate StatusBarTemplate
        {
            get
            {
                return (DataTemplate)GetValue(StatusBarTemplateProperty);
            }
            set
            {
                SetValue(StatusBarTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty CanMinimizeProperty = DependencyProperty.Register("CanMinimize", typeof(bool), typeof(PHGWindow),
            new PropertyMetadata(true));

        public bool CanMinimize
        {
            get
            {
                return (bool)GetValue(CanMinimizeProperty);
            }
            set
            {
                SetValue(CanMinimizeProperty, value);
            }
        }

        public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register("CanMaximize", typeof(bool), typeof(PHGWindow),
            new PropertyMetadata(true));

        public bool CanMaximize
        {
            get
            {
                return (bool)GetValue(CanMaximizeProperty);
            }
            set
            {
                SetValue(CanMaximizeProperty, value);
            }
        }

        public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register("CanClose", typeof(bool), typeof(PHGWindow),
            new PropertyMetadata(true));

        public bool CanClose
        {
            get
            {
                return (bool)GetValue(CanCloseProperty);
            }
            set
            {
                SetValue(CanCloseProperty, value);
            }
        }

        public static readonly DependencyProperty CanResizeProperty = DependencyProperty.Register("CanResize", typeof(bool), typeof(PHGWindow),
            new FrameworkPropertyMetadata(true));

        public bool CanResize
        {
            get
            {
                return (bool)GetValue(CanResizeProperty);
            }
            set
            {
                SetValue(CanResizeProperty, value);
            }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AttachBorders();
            AttachButtons();
            AttachTitleBorder();
        }

        private void AttachTitleBorder()
        {
            if (TitleBorder != null)
            {
                TitleBorder.MouseLeftButtonDown -= OnTitleMouseLeftButtonDown;
            }

            var titleBorder = GetChildControl<Border>(TitlePartName);

            if (titleBorder != null)
            {
                titleBorder.MouseLeftButtonDown += OnTitleMouseLeftButtonDown;
                TitleBorder = titleBorder;
            }
        }

        private void AttachButtons()
        {
            AttachButton(CloseButton, CloseButtonPartName, OnCloseButtonClick);
            AttachButton(MinimizeButton, MinimizeButtonPartName, OnMinimizeButtonClick);
            AttachButton(MaximizeButton, MaximizeButtonPartName, OnMaximizeButtonClick);
        }

        private void AttachButton(Button button, string partName, RoutedEventHandler onButtonClick)
        {
            if (button != null)
            {
                button.Click -= onButtonClick;
            }

            var newButton = GetChildControl<Button>(partName);

            if (newButton != null)
            {
                newButton.Click += onButtonClick;
                button = newButton;
            }
        }

        private void AttachBorders()
        {
            AttachBorder(LeftBorder, LeftBorderPartName, OnLeftBorderDragDelta);
            AttachBorder(RightBorder, RightBorderPartName, OnRightBorderDragDelta);
            AttachBorder(BottomBorder, BottomBorderPartName, OnBottomBorderDragDelta);
            AttachBorder(TopBorder, TopBorderPartName, OnTopBorderDragDelta);
            AttachBorder(ResizeBorder, ResizeBorderPartName, OnResizeBorderDragDelta);
        }

        private void AttachBorder(Thumb border, string partName, DragDeltaEventHandler onDragDelta)
        {
            if (border != null)
            {
                border.DragDelta -= onDragDelta;
            }

            var newBorder = GetChildControl<Thumb>(partName);

            if (newBorder != null)
            {
                newBorder.DragDelta += onDragDelta;
                border = newBorder;
            }
        }

        #region On Border Drag Delta

        private void OnRightBorderDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width >= MinWidth && (Width + e.HorizontalChange) > 1)
            {
                Width += e.HorizontalChange;
            }
            else
            {
                Width = MinWidth + 1;
                ResizeBorder?.ReleaseMouseCapture();
            }
        }

        private void OnLeftBorderDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width >= MinWidth)
            {
                Width -= e.HorizontalChange;
                Left += e.HorizontalChange;
            }
            else
            {
                Width = MinWidth + 1;
                ResizeBorder?.ReleaseMouseCapture();
            }
        }

        private void OnBottomBorderDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height >= MinHeight && (Height + e.VerticalChange) > 1)
            {
                Height += e.VerticalChange;
            }
            else
            {
                Height = MinHeight + 1;
                ResizeBorder?.ReleaseMouseCapture();
            }
        }

        private void OnTopBorderDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height >= MinHeight)
            {
                Height -= e.VerticalChange;
                Top += e.VerticalChange;
            }
            else
            {
                Height = MinHeight + 1;
                ResizeBorder?.ReleaseMouseCapture();
            }
        }

        private void OnResizeBorderDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width >= MinWidth && (Width + e.HorizontalChange) > 1)
            {
                Width += e.HorizontalChange;
            }
            else
            {
                Width = MinWidth + 1;
                ResizeBorder?.ReleaseMouseCapture();
            }

            if (Height >= MinHeight && (Height + e.VerticalChange) > 1)
            {
                Height += e.VerticalChange;
            }
            else
            {
                Height = MinHeight + 1;
                ResizeBorder?.ReleaseMouseCapture();
            }
        }

        #endregion

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnMaximizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaximizeButton.Style = (Style)FindResource("MaximizeButton");
            }
            else
            {
                WindowState = WindowState.Maximized;
                MaximizeButton.Style = (Style)FindResource("MaximizeButtonSel");
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnTitleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// Gets the child control from the template
        /// </summary>
        /// <typeparam name="T">Type of control requested</typeparam>
        /// <param name="childName">Name of the child control</param>
        /// <returns>Control instance if there is one with the specified name; null otherwise</returns>
        private T GetChildControl<T>(string childName) where T : DependencyObject
        {
            var templateChild = GetTemplateChild(childName) as T;
            return templateChild;
        }
    }
}