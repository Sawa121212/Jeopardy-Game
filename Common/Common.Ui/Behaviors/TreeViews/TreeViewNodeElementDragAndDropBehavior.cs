/*
using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using Common.Core.Entities.DataBase;
using Common.Core.Interfaces;
using Common.Ui.Behaviors.DragAndDrop;
using Common.Ui.DragAndDrop;
using Common.Ui.Exceptions;
using Common.Ui.ScreenHelper;
using ReactiveUI;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewNodeElementDragAndDropBehavior : Behavior<TreeView>, IDragAndDropBehavior
    {
        private const double ScrollTolerance = 50;
        private const double ScrollOffset = 5;
        private const double DragStartOffset = 5;

        private volatile bool _isDragging;
        private Point _startPosition;
        private IDataModel _startElement;

        /// <summary>
        /// Разрешение копирования элементов дерева. По умолчанию true.
        /// </summary>
        public static readonly StyledProperty<bool> AllowCopyProperty =
            AvaloniaProperty.Register<TreeViewNodeElementDragAndDropBehavior, bool>(nameof(AllowCopy));

        /// <summary>
        /// Разрешение перемещения элементов дерева. По умолчанию true.
        /// </summary>
        // public static readonly DependencyProperty AllowMoveProperty = DependencyProperty.Register(
        //     nameof(AllowMove), typeof(bool), typeof(TreeViewNodeElementDragAndDropBehavior), new PropertyMetadata(default(bool)));

        public static readonly StyledProperty<bool> AllowMoveProperty =
            AvaloniaProperty.Register<TreeViewNodeElementDragAndDropBehavior, bool>(nameof(AllowMove));

        /// <summary>
        /// Фабрика стратегий перемещения элементов.
        /// </summary>
        // public static readonly DependencyProperty DragAndDropStrategyFactoryProperty = DependencyProperty.Register(
        //     nameof(DragAndDropStrategyFactory), typeof(IDataModelDragAndDropStrategyFactory), typeof(TreeViewNodeElementDragAndDropBehavior), new PropertyMetadata(default(IDataModelDragAndDropStrategyFactory)));

        // TODO: Не ясно что использовать в AvaloniaPropertyMetadata в качестве аргумента
        // public static readonly StyledProperty<IDataModelDragAndDropStrategyFactory>
        //     DragAndDropStrategyFactoryProperty =
        //         AvaloniaProperty
        //             .Register<TreeViewNodeElementDragAndDropBehavior, IDataModelDragAndDropStrategyFactory>(
        //                 nameof(DragAndDropStrategyFactory),
        //                 new AvaloniaPropertyMetadata(default(IDataModelDragAndDropStrategyFactory));

        /// <summary>
        /// Разрешить перетаскивание из других источников.
        /// </summary>
        public static readonly StyledProperty<bool> AllowOtherSourceProperty =
            AvaloniaProperty.Register<TreeViewNodeElementDragAndDropBehavior, bool>(nameof(AllowOtherSource));

        /// <summary>
        /// При перетаскивании из других источников использовать режим копирования вместо перемещения.
        /// </summary>
        public static readonly StyledProperty<bool> IsCopyOnlyFromOtherSourceProperty =
            AvaloniaProperty.Register<TreeViewNodeElementDragAndDropBehavior, bool>(nameof(IsCopyOnlyFromOtherSource));

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

        public IDataModelDragAndDropStrategyFactory DragAndDropStrategyFactory
        {
            get => (IDataModelDragAndDropStrategyFactory)GetValue(DragAndDropStrategyFactoryProperty);
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

        public TreeViewNodeElementDragAndDropBehavior()
        {
            AllowCopy = true;
            AllowMove = true;
            AllowOtherSource = true;
        }

        /// <inheritdoc />
        public bool IsAllowDrop { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PointerPressed += AssociatedObjectOnMouseLeftButtonDown;
            AssociatedObject.PointerMoved += AssociatedObjectOnMouseMove;
            // AssociatedObject.DragOver += AssociatedObjectOnDragOver;
            AssociatedObject.PointerEnter += AssociatedObjectOnDragEnter;
            AssociatedObject.PointerLeave += AssociatedObjectOnDragLeave;
            AssociatedObject.PointerReleased += AssociatedObjectOnDrop;
            // AssociatedObject.GiveFeedback += AssociatedObjectOnGiveFeedback;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PointerPressed -= AssociatedObjectOnMouseLeftButtonDown;
            AssociatedObject.PointerMoved -= AssociatedObjectOnMouseMove;
            // AssociatedObject.DragOver -= AssociatedObjectOnDragOver;
            AssociatedObject.PointerEnter -= AssociatedObjectOnDragEnter;
            AssociatedObject.PointerLeave -= AssociatedObjectOnDragLeave;
            AssociatedObject.PointerReleased -= AssociatedObjectOnDrop;
            // AssociatedObject.GiveFeedback -= AssociatedObjectOnGiveFeedback;
            base.OnDetaching();
        }
        
        
        
        private void AssociatedObjectOnDrop(object sender, PointerEventArgs e)
        {
            // var point = e.GetCurrentPoint(null).Properties.IsLeftButtonPressed;
            // var 

            if (e.Data.GetData(typeof(DataWithSource).ToString(), false))
            {
                if (e.Data.GetData(typeof(DataWithSource)) is DataWithSource dataWithSource)
                {
                    if (dataWithSource.DragSource.TryGetTarget(out var sourceTarget))
                    {
                        var sameDragSource = sourceTarget.Equals(AssociatedObject);
                        if (sameDragSource || AllowOtherSource)
                        {
                            var copyMode = e.KeyModifiers == KeyModifiers.Control || IsCopyOnlyFromOtherSource && !sameDragSource;

                            if (e.Source is Control element)
                            {
                                if (element.DataContext is IDataModel destinationElement)
                                {
                                    if (dataWithSource.Data.TryGetTarget(out var someObject) && someObject is IDataModel nodeElement)
                                    {
                                        var strategy = DragAndDropStrategyFactory?.CreateStrategy(destinationElement);
                                        if (strategy != null)
                                        {
                                            try
                                            {
                                                if (strategy.Execute(nodeElement, copyMode))
                                                {
                                                   /*var destinationItem = (e.OriginalSource as DependencyObject)?.TryFindParent<TreeViewItem>();
                                                    if (destinationItem != null)
                                                    {
                                                        var parentTreeViewItem = VisualTreeHelper.GetParent(destinationItem)?.TryFindParent<TreeViewItem>();
                                                        if (parentTreeViewItem != null)
                                                        {
                                                            TreeViewHelper.SelectChildElement(parentTreeViewItem, nodeElement);       
                                                        }
                                                    }#1#
                                                }
                                                else
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

        /*private void AssociatedObjectOnGiveFeedback(object sender, GiveFeedbackEventArgs e)
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
        }#1#
        
        /*private void AssociatedObjectOnDragOver(object sender, PointerEventArgs e)
        {
            var control = sender as ItemsControl;
            var scrollViewer = control?.FindVisualChildren<ScrollViewer>().FirstOrDefault(viewer => viewer.Name.IsEquals("ScrollViewer_Content"));
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
        }#1#

        private void AssociatedObjectOnDragEnter(object sender, PointerEventArgs e)
        {
            // if (e.Data.GetDataPresent(typeof(DataWithSource).ToString(), false))
            if (e.)
            {
                if (e.Data.GetData(typeof(DataWithSource)) is DataWithSource dataWithSource)
                {
                    if (dataWithSource.DragSource.TryGetTarget(out var sourceTarget))
                    {
                        var sourceBehavior = Interaction.GetBehaviors(sourceTarget).OfType<IDragAndDropBehavior>().FirstOrDefault();
                        if (sourceBehavior != null)
                        {
                            var sameDragSource = sourceTarget.Equals(AssociatedObject);
                            if (sameDragSource || AllowOtherSource)
                            {
                                var copyMode = e.KeyModifiers == KeyModifiers.Control || IsCopyOnlyFromOtherSource && !sameDragSource;

                                if (e.Source is Control element)
                                {
                                    if (element.DataContext is IDataModel destinationItem)
                                    {
                                        if (dataWithSource.Data.TryGetTarget(out var someObject) && someObject is IDataModel nodeElement)
                                        {
                                            var strategy = DragAndDropStrategyFactory?.CreateStrategy(destinationItem);
                                            if (strategy != null)
                                            {
                                                sourceBehavior.IsAllowDrop = strategy.CanExecute(nodeElement);
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

        private void AssociatedObjectOnDragLeave(object sender, PointerEventArgs e)
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

        private void AssociatedObjectOnMouseLeftButtonDown(object sender, PointerEventArgs e)
        {
            if (e.Source is Control frameworkElement)
            {
                _startElement = frameworkElement.DataContext as IDataModel;
                _startPosition = e.GetPosition(null);
            }
            else
            {
                _startElement = null;
            }
        }

        private void AssociatedObjectOnMouseMove(object sender, PointerEventArgs e)
        {
            if (e.Source is Control element)
            {
                // TODO: Нет аналогов TryFindParent и IsMouseOver
                // var item = element.TryFindParent<TreeViewItem>();
                if (element != null && /*element.IsMouseOver &&#1# element.DataContext != null)
                {
                    if (element.DataContext.Equals(AssociatedObject.SelectedItem))
                    {
                        MouseMove(sender, e);
                    }
                }
            }
        }

        private void MouseMove(object sender, PointerEventArgs e)
        {
            if (_startElement == null)
                return;
            
            //if (e.LeftButton == MouseButtonState.Pressed && !_isDragging && IsDragStart(e.GetPosition(null)))
            if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed == true
                 && !_isDragging 
                 && IsDragStart(e.GetPosition(null)))
            {
                if (e.Source is Control frameworkElement)
                {
                    if (frameworkElement.DataContext is IDataModel nodeElement)
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
        protected virtual void DragDropProccess(IDataModel element)
        {
            var data = new DataObject();
            var dataWithSource = new DataWithSource(AssociatedObject, element);
            data.Set(typeof(DataWithSource).ToString(), dataWithSource);

            var effects = DragDropEffects.None;
            if (AllowCopy) effects |= DragDropEffects.Copy;
            if (AllowMove) effects |= DragDropEffects.Move;

            IsAllowDrop = false;
            try
            {
                // TODO: нужен triger event
                // DragDrop.DoDragDrop(AssociatedObject, data, effects);
                DragDrop.DoDragDrop(e/*triger event#1#, data, effects);
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
            return Math.Abs(position.X - _startPosition.X) > Screen.PrimaryScreen.WorkingArea.X + DragStartOffset ||
                   Math.Abs(position.Y - _startPosition.Y) > Screen.PrimaryScreen.WorkingArea.Y + DragStartOffset;
            
            // return Math.Abs(position.X - _startPosition.X) > SystemParameters.MinimumHorizontalDragDistance + DragStartOffset ||
            //        Math.Abs(position.Y - _startPosition.Y) > SystemParameters.MinimumVerticalDragDistance + DragStartOffset;
        }
    }
}
*/


