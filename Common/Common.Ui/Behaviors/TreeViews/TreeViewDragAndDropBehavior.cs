/*
using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Behaviors.DragAndDrop;
using Common.Ui.DragAndDrop;
using Common.Ui.Exceptions;
using ReactiveUI;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewDragAndDropBehavior : Behavior<TreeView>, IDragAndDropBehavior
    {
        private const double ScrollTolerance = 50;
        private const double ScrollOffset = 5;
        private const double DragStartOffset = 5;

        private volatile bool _isDragging;
        private Point _startPosition;
        private NodeElement _startElement;

        /// <summary>
        /// Разрешение копирования элементов дерева. По умолчанию true.
        /// </summary>
        public static readonly DependencyProperty AllowCopyProperty = DependencyProperty.Register(
            "AllowCopy", typeof(bool), typeof(TreeViewDragAndDropBehavior), new PropertyMetadata(default(bool)));

        /// <summary>
        /// Разрешение перемещения элементов дерева. По умолчанию true.
        /// </summary>
        public static readonly DependencyProperty AllowMoveProperty = DependencyProperty.Register(
            "AllowMove", typeof(bool), typeof(TreeViewDragAndDropBehavior), new PropertyMetadata(default(bool)));

        /// <summary>
        /// <see cref="IDragAndDropStrategy"/>
        /// </summary>
        public static readonly DependencyProperty DragAndDropStrategyFactoryProperty = DependencyProperty.Register(
            "DragAndDropStrategyFactory", typeof(IDragAndDropStrategyFactory), typeof(TreeViewDragAndDropBehavior), new PropertyMetadata(default(IDragAndDropStrategyFactory)));

        /// <summary>
        /// Разрешить перетаскивание из других источников.
        /// </summary>
        public static readonly DependencyProperty AllowOtherSourceProperty = DependencyProperty.Register(
            "AllowOtherSource", typeof(bool), typeof(TreeViewDragAndDropBehavior), new PropertyMetadata(default(bool)));

        /// <summary>
        /// При перетаскивании из других источников использовать режим копирования вместо перемещения.
        /// </summary>
        public static readonly DependencyProperty IsCopyOnlyFromOtherSourceProperty = DependencyProperty.Register(
            "IsCopyOnlyFromOtherSource", typeof(bool), typeof(TreeViewDragAndDropBehavior), new PropertyMetadata(default(bool)));


        /// <summary>
        /// Разрешение копирования элементов дерева. По умолчанию true.
        /// </summary>
        public bool AllowCopy
        {
            get => (bool)GetValue(AllowCopyProperty);
            set => SetValue(AllowCopyProperty, value);
        }

        /// <summary>
        /// Разрешение перемещения элементов дерева. По умолчанию true.
        /// </summary>
        public bool AllowMove
        {
            get => (bool)GetValue(AllowMoveProperty);
            set => SetValue(AllowMoveProperty, value);
        }

        /// <summary>
        /// <see cref="IDragAndDropStrategy"/>
        /// </summary>
        public IDragAndDropStrategyFactory DragAndDropStrategyFactory
        {
            get => (IDragAndDropStrategyFactory)GetValue(DragAndDropStrategyFactoryProperty);
            set => SetValue(DragAndDropStrategyFactoryProperty, value);
        }

        /// <summary>
        /// Разрешить перетаскивание из других источников.
        /// </summary>
        public bool AllowOtherSource
        {
            get => (bool)GetValue(AllowOtherSourceProperty);
            set => SetValue(AllowOtherSourceProperty, value);
        }

        /// <summary>
        /// При перетаскивании из других источников использовать режим копирования вместо перемещения.
        /// </summary>
        public bool IsCopyOnlyFromOtherSource
        {
            get => (bool)GetValue(IsCopyOnlyFromOtherSourceProperty);
            set => SetValue(IsCopyOnlyFromOtherSourceProperty, value);
        }

        public TreeViewDragAndDropBehavior()
        {
            AllowCopy = true;
            AllowMove = true;
            AllowOtherSource = true;
        }

        /// <inheritdoc />
        public bool IsAllowDrop { get; set; }

        protected override void OnSetup()
        {
            base.OnSetup();
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObjectOnMouseLeftButtonDown;
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
            AssociatedObject.DragOver += AssociatedObjectOnDragOver;
            AssociatedObject.DragEnter += AssociatedObjectOnDragEnter;
            AssociatedObject.DragLeave += AssociatedObjectOnDragLeave;
            AssociatedObject.Drop += AssociatedObjectOnDrop;
            AssociatedObject.GiveFeedback += AssociatedObjectOnGiveFeedback;
        }
        
        protected override void OnCleanup()
        {
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObjectOnMouseLeftButtonDown;
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
            AssociatedObject.DragOver -= AssociatedObjectOnDragOver;
            AssociatedObject.DragEnter -= AssociatedObjectOnDragEnter;
            AssociatedObject.DragLeave -= AssociatedObjectOnDragLeave;
            AssociatedObject.Drop -= AssociatedObjectOnDrop;
            AssociatedObject.GiveFeedback -= AssociatedObjectOnGiveFeedback;
            base.OnCleanup();
        }
        
        private void AssociatedObjectOnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataWithSource).ToString(), false))
            {
                if (e.Data.GetData(typeof(DataWithSource)) is DataWithSource dataWithSource)
                {
                    if (dataWithSource.DragSource.TryGetTarget(out var sourceTarget))
                    {
                        var sameDragSource = sourceTarget.Equals(AssociatedObject);
                        if (sameDragSource || AllowOtherSource)
                        {
                            var copyMode = e.KeyStates == DragDropKeyStates.ControlKey || IsCopyOnlyFromOtherSource && !sameDragSource;

                            if (e.OriginalSource is FrameworkElement element)
                            {
                                if (element.DataContext is NodeElement destinationItem)
                                {
                                    if (dataWithSource.Data.TryGetTarget(out var someObject) && someObject is NodeElement nodeElement)
                                    {
                                        var strategy = DragAndDropStrategyFactory?.CreateStrategy(destinationItem, nodeElement, copyMode);
                                        if (strategy != null)
                                        {
                                            try
                                            {
                                                if (!strategy.ExecuteAlgoritm())
                                                {
                                                    //ToDo: Если понадобится логировать отмену или пустую операцию по перемещению
                                                    //throw new DragAndDropCancelException();
                                                }
                                            }
                                            finally
                                            {
                                                if (strategy is IClearable clearable)
                                                {
                                                    clearable.Clear();
                                                }
                                            }
                                        }
                                    }
                                    else
                                        throw new Exception("Отсутствует Dropped элемент.");
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Отсутствует DragSource.");
                    }
                }
            }
        }

        private void AssociatedObjectOnGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (!IsAllowDrop)
            {
                e.UseDefaultCursors = false;
                Mouse.SetCursor(Cursors.No);
            }
            else
            {
                e.UseDefaultCursors = true;
            }
            e.Handled = true;
        }

        private void AssociatedObjectOnDragOver(object sender, DragEventArgs e)
        {
            var control = sender as ItemsControl;
            var scrollViewer = control?.FindVisualChild<ScrollViewer>();
            if (scrollViewer != null)
            {
                var verticalPos = e.GetPosition(control).Y;
                if (verticalPos < ScrollTolerance)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - ScrollOffset);
                }
                if (verticalPos > control.ActualHeight - ScrollTolerance)
                {
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + ScrollOffset);
                }
            }
        }

        private void AssociatedObjectOnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataWithSource).ToString(), false))
            {
                if (e.Data.GetData(typeof(DataWithSource)) is DataWithSource dataWithSource)
                {
                    if (dataWithSource.DragSource.TryGetTarget(out var sourceTarget))
                    {
                        var sourceBehavior = Interaction<,>.GetBehaviors(sourceTarget).OfType<IDragAndDropBehavior>().FirstOrDefault();
                        if (sourceBehavior != null)
                        {
                            var sameDragSource = sourceTarget.Equals(AssociatedObject);
                            if (sameDragSource || AllowOtherSource)
                            {
                                var copyMode = e.KeyStates == DragDropKeyStates.ControlKey || IsCopyOnlyFromOtherSource && !sameDragSource;

                                if (e.OriginalSource is FrameworkElement element)
                                {
                                    if (element.DataContext is NodeElement destinationItem)
                                    {
                                        if (dataWithSource.Data.TryGetTarget(out var someObject) && someObject is NodeElement nodeElement)
                                        {
                                            var strategy = DragAndDropStrategyFactory?.CreateStrategy(destinationItem, nodeElement, copyMode);
                                            if (strategy != null)
                                            {
                                                sourceBehavior.IsAllowDrop = strategy.CanExecute();
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                            sourceBehavior.IsAllowDrop = false;
                        }
                    }
                }
            }
        }

        private void AssociatedObjectOnDragLeave(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataWithSource).ToString(), false))
            {
                if (e.Data.GetData(typeof(DataWithSource)) is DataWithSource dataWithSource)
                {
                    if (dataWithSource.DragSource.TryGetTarget(out var sourceTarget))
                    {
                        var sourceBehavior = Interaction.GetBehaviors(sourceTarget).OfType<IDragAndDropBehavior>().FirstOrDefault();
                        if (sourceBehavior != null)
                        {
                            sourceBehavior.IsAllowDrop = false;
                        }
                    }
                }
            }
        }

        private void AssociatedObjectOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement frameworkElement)
            {
                _startElement = frameworkElement.DataContext as NodeElement;
                _startPosition = e.GetPosition(null);
            }
            else
            {
                _startElement = null;
            }
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource is UIElement element)
            {
                var item = element.TryFindParent<TreeViewItem>();
                if (item != null && item.IsMouseOver && item.DataContext != null)
                {
                    if (item.DataContext.Equals(AssociatedObject.SelectedItem))
                    {
                        MouseMove(sender, e);
                    }
                }
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (_startElement == null)
                return;
            if (e.LeftButton == MouseButtonState.Pressed && !_isDragging && IsDragStart(e.GetPosition(null)))
            {
                if (e.OriginalSource is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.DataContext is NodeElement nodeElement)
                    {
                        if (nodeElement.Equals(_startElement))
                        {
                            _isDragging = true;
                            DragDropProccess(nodeElement);
                            _isDragging = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Процедура перетаскивания элемента.
        /// </summary>
        /// <param name="element"></param>
        protected virtual void DragDropProccess(NodeElement element)
        {
            var data = new DataObject();
            var dataWithSource = new DataWithSource(AssociatedObject, element);
            data.SetData(typeof(DataWithSource), dataWithSource);

            var effects = DragDropEffects.None;
            if (AllowCopy) effects |= DragDropEffects.Copy;
            if (AllowMove) effects |= DragDropEffects.Move;

            IsAllowDrop = false;
            try
            {
                DragDrop.DoDragDrop(AssociatedObject, data, effects);
            }
            catch (DragAndDropCancelException)
            {
                //операция отменена или не была выполнена.
            }
            catch (Exception)
            {
                //Если бросить элемент в другое приложение.
            }
            finally
            {
                dataWithSource.Clear();
            }
        }

        private bool IsDragStart(Point position)
        {
            return Math.Abs(position.X - _startPosition.X) > SystemParameters.MinimumHorizontalDragDistance + DragStartOffset ||
                   Math.Abs(position.Y - _startPosition.Y) > SystemParameters.MinimumVerticalDragDistance + DragStartOffset;
        }
    }
}
*/
