/*
using System;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Common.Ui.Behaviors.GridSplitters
{
    /// <summary>
    /// Направление сворачивания 
    /// </summary>
    public enum CollapseDirection
    {
        Forward,
        Backward
    }

    public class CollapseGridSplitterBehavior : Behavior<GridSplitter>
    {
        private double _collapseCriteria = 5;
        private double _storedHeigth;
        private double _storedWidth;
        private readonly SimpleMonitor _monitor;

        /// <summary>
        /// Направление сворачивания
        /// </summary>
        public static readonly DependencyProperty CollapseDirectionProperty = DependencyProperty.Register(
            "CollapseDirection", typeof(CollapseDirection), typeof(CollapseGridSplitterBehavior), new PropertyMetadata(default(CollapseDirection)));

        /// <summary>
        /// Состояние и управление 
        /// </summary>
        public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.Register(
            "IsCollapsed", typeof(bool), typeof(CollapseGridSplitterBehavior), new PropertyMetadata(default(bool), IsCollapsedChanged));

        /// <summary>
        /// Указывает необходимость запоминания положения до сворачивания
        /// </summary>
        public static readonly DependencyProperty RestorePositionProperty = DependencyProperty.Register(
            "RestorePosition", typeof(bool), typeof(CollapseGridSplitterBehavior), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Минимальный размер сворачиваемой области
        /// </summary>
        public static readonly DependencyProperty MinCollapseSizeProperty = DependencyProperty.Register(
            "MinCollapseSize", typeof(int), typeof(CollapseGridSplitterBehavior), new PropertyMetadata(default(int)));

        /// <summary>
        /// Активировать состояние <see cref="IsCollapsed" /> при загрузке.
        /// </summary>
        public static readonly DependencyProperty ActivateOnSetupProperty = DependencyProperty.Register(
            "ActivateOnSetup", typeof(bool), typeof(CollapseGridSplitterBehavior), new PropertyMetadata(default(bool)));

        public bool ActivateOnSetup
        {
            get => (bool)GetValue(ActivateOnSetupProperty);
            set => SetValue(ActivateOnSetupProperty, value);
        }

        public CollapseDirection CollapseDirection
        {
            get => (CollapseDirection)GetValue(CollapseDirectionProperty);
            set => SetValue(CollapseDirectionProperty, value);
        }

        public int MinCollapseSize
        {
            get => (int)GetValue(MinCollapseSizeProperty);
            set => SetValue(MinCollapseSizeProperty, value);
        }

        public bool RestorePosition
        {
            get => (bool)GetValue(RestorePositionProperty);
            set => SetValue(RestorePositionProperty, value);
        }

        public bool IsCollapsed
        {
            get => (bool)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }

        public CollapseGridSplitterBehavior()
        {
            _monitor = new SimpleMonitor();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseDoubleClick += AssociatedObject_PreviewMouseDoubleClick;
            //AssociatedObject.DragDelta += OnDragDelta;
            if (ActivateOnSetup)
                Proccess(IsCollapsed);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseDoubleClick -= AssociatedObject_PreviewMouseDoubleClick;
           // AssociatedObject.DragDelta -= OnDragDelta;
            base.OnDetaching();
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs dragDeltaEventArgs)
        {
            if (!_monitor.Busy)
            {
                try
                {
                    _monitor.Enter();
                    var splitter = sender as GridSplitter;
                    if (splitter?.Parent is Grid parent)
                    {
                        var splitterWidth = (double)splitter.GetValue(FrameworkElement.WidthProperty);
                        if (double.IsNaN(splitterWidth))
                        {
                            // Горизонтальный
                            var splitterRowIndex = (int)splitter.GetValue(Grid.RowProperty);
                            var collapsedRowIndex = CollapseDirection == CollapseDirection.Forward
                                ? splitterRowIndex + 1
                                : splitterRowIndex - 1;
                            if (parent.RowDefinitions.Count > collapsedRowIndex)
                            {
                                var threshold = CollapseDirection == CollapseDirection.Forward ? -dragDeltaEventArgs.VerticalChange : dragDeltaEventArgs.VerticalChange;
                                var heigth = parent.RowDefinitions[collapsedRowIndex].ActualHeight + threshold;
                                IsCollapsed = heigth < _collapseCriteria;
                            }
                        }
                        else
                        {
                            //Вертикальный
                            var splitterColumnIndex = (int)splitter.GetValue(Grid.ColumnProperty);
                            var collapsedColumnIndex = CollapseDirection == CollapseDirection.Forward
                                ? splitterColumnIndex + 1
                                : splitterColumnIndex - 1;
                            if (parent.ColumnDefinitions.Count > collapsedColumnIndex)
                            {
                                var threshold = CollapseDirection == CollapseDirection.Forward ? -dragDeltaEventArgs.HorizontalChange : dragDeltaEventArgs.HorizontalChange;
                                var width = parent.ColumnDefinitions[collapsedColumnIndex].ActualWidth + threshold;
                                IsCollapsed = width < _collapseCriteria;
                            }
                        }
                    }
                }
                finally
                {
                    _monitor.Dispose();
                }
            }
        }
        
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="splitter"></param>
        private void Proccess(GridSplitter splitter)
        {
            _monitor.Enter();
            if (splitter?.Parent is Grid parent)
            {
                //var splitterHeight = (double)splitter.GetValue(FrameworkElement.HeightProperty);
                var splitterWidth = (double)splitter.GetValue(FrameworkElement.WidthProperty);

                if (double.IsNaN(splitterWidth))
                {
                    // Горизонтальный
                    var splitterRowIndex = (int)splitter.GetValue(Grid.RowProperty);
                    var collapsedRowIndex = CollapseDirection == CollapseDirection.Forward
                        ? splitterRowIndex + 1
                        : splitterRowIndex - 1;
                    if (parent.RowDefinitions.Count > collapsedRowIndex)
                    {
                        if (Math.Abs(0 - parent.RowDefinitions[collapsedRowIndex].ActualHeight) < _collapseCriteria)
                        {
                            //Развернуть
                            if (RestorePosition)
                            {
                                parent.RowDefinitions[collapsedRowIndex].SetValue(RowDefinition.HeightProperty,
                                    _storedHeigth > _collapseCriteria ? new GridLength(_storedHeigth) : GridLength.Auto);
                            }
                            else
                            {
                                parent.RowDefinitions[collapsedRowIndex].SetValue(RowDefinition.HeightProperty, GridLength.Auto);
                            }
                            IsCollapsed = false;
                        }
                        else
                        {
                            //Свернуть
                            _storedHeigth = parent.RowDefinitions[collapsedRowIndex].ActualHeight;
                            parent.RowDefinitions[collapsedRowIndex].SetValue
                                (RowDefinition.HeightProperty, new GridLength(MinCollapseSize));
                            IsCollapsed = true;
                        }
                    }
                }
                else
                {
                    //Вертикальный
                    var splitterColumnIndex = (int)splitter.GetValue(Grid.ColumnProperty);
                    var collapsedColumnIndex = CollapseDirection == CollapseDirection.Forward
                        ? splitterColumnIndex + 1
                        : splitterColumnIndex - 1;
                    if (parent.ColumnDefinitions.Count > collapsedColumnIndex)
                    {
                        if (Math.Abs(0 - parent.ColumnDefinitions[collapsedColumnIndex].ActualWidth) < _collapseCriteria)
                        {
                            //Развернуть
                            if (RestorePosition)
                            {
                                parent.ColumnDefinitions[collapsedColumnIndex].SetValue(ColumnDefinition.WidthProperty,
                                    _storedWidth > _collapseCriteria ? new GridLength(_storedWidth) : GridLength.Auto);
                            }
                            else
                            {
                                parent.ColumnDefinitions[collapsedColumnIndex].SetValue(ColumnDefinition.WidthProperty, GridLength.Auto);
                            }
                            IsCollapsed = false;
                        }
                        else
                        {
                            //Свернуть
                            _storedWidth = parent.ColumnDefinitions[collapsedColumnIndex].ActualWidth;
                            parent.ColumnDefinitions[collapsedColumnIndex].SetValue
                                (ColumnDefinition.WidthProperty, new GridLength(MinCollapseSize));
                            IsCollapsed = true;
                        }
                    }
                }
            }
            _monitor.Dispose();
        }

        private void Proccess(bool collapse)
        {
            if (!_monitor.Busy)
            {
                try
                {
                    _monitor.Enter();
                    if (AssociatedObject != null)
                    {
                        if (AssociatedObject.Parent is Grid parent)
                        {
                            var splitterHeight = (double)AssociatedObject.GetValue(FrameworkElement.HeightProperty);
                            var splitterWidth = (double)AssociatedObject.GetValue(FrameworkElement.WidthProperty);

                            if (double.IsNaN(splitterWidth))
                            {
                                // Горизонтальный
                                var splitterRowIndex = (int)AssociatedObject.GetValue(Grid.RowProperty);
                                var collapsedRowIndex = CollapseDirection == CollapseDirection.Forward
                                    ? splitterRowIndex + 1
                                    : splitterRowIndex - 1;
                                if (parent.RowDefinitions.Count > collapsedRowIndex)
                                {
                                    if (!collapse)
                                    {
                                        //Развернуть
                                        if (RestorePosition)
                                        {
                                            parent.RowDefinitions[collapsedRowIndex].SetValue(RowDefinition.HeightProperty,
                                                _storedHeigth > _collapseCriteria ? new GridLength(_storedHeigth) : GridLength.Auto);
                                        }
                                        else
                                        {
                                            parent.RowDefinitions[collapsedRowIndex].SetValue(RowDefinition.HeightProperty, GridLength.Auto);
                                        }
                                        IsCollapsed = false;
                                    }
                                    else
                                    {
                                        //Свернуть
                                        _storedHeigth = parent.RowDefinitions[collapsedRowIndex].ActualHeight;
                                        parent.RowDefinitions[collapsedRowIndex].SetValue
                                            (RowDefinition.HeightProperty, new GridLength(MinCollapseSize));
                                        IsCollapsed = true;
                                    }
                                }
                            }
                            else
                            {
                                //Вертикальный
                                var splitterColumnIndex = (int)AssociatedObject.GetValue(Grid.ColumnProperty);
                                var collapsedColumnIndex = CollapseDirection == CollapseDirection.Forward
                                    ? splitterColumnIndex + 1
                                    : splitterColumnIndex - 1;
                                if (parent.ColumnDefinitions.Count > collapsedColumnIndex)
                                {
                                    if (!collapse)
                                    {
                                        //Развернуть
                                        if (RestorePosition)
                                        {
                                            parent.ColumnDefinitions[collapsedColumnIndex].SetValue(ColumnDefinition.WidthProperty,
                                                _storedWidth > _collapseCriteria ? new GridLength(_storedWidth) : GridLength.Auto);
                                        }
                                        else
                                        {
                                            parent.ColumnDefinitions[collapsedColumnIndex].SetValue(ColumnDefinition.WidthProperty, GridLength.Auto);
                                        }
                                        IsCollapsed = false;
                                    }
                                    else
                                    {
                                        //Свернуть
                                        _storedWidth = parent.ColumnDefinitions[collapsedColumnIndex].ActualWidth;
                                        parent.ColumnDefinitions[collapsedColumnIndex].SetValue
                                            (ColumnDefinition.WidthProperty, new GridLength(MinCollapseSize));
                                        IsCollapsed = true;
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    _monitor.Dispose();
                }
            }
        }

        void AssociatedObject_PreviewMouseDoubleClick
            (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Proccess(!IsCollapsed);
        }

        private static void IsCollapsedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var behavior = sender as CollapseGridSplitterBehavior;
            if (behavior?._monitor != null)
            {
                if (e.NewValue is bool collapsed)
                    behavior.Proccess(collapsed);
            }
        }
    }
}
*/
