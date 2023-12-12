/*using System.Windows.Input;
using Avalonia.Xaml.Interactivity;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.FrameworkElements
{
    public class MousePositionBehavior<T> : Behavior<T> where T : FrameworkElement
    {
        public static readonly DependencyProperty MouseLeftButtonDownCommandProperty = DependencyProperty.Register(
            "MouseLeftButtonDownCommand", typeof(ICommand), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MouseLeftButtonUpCommandProperty = DependencyProperty.Register(
            "MouseLeftButtonUpCommand", typeof(ICommand), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MouseRightButtonDownCommandProperty = DependencyProperty.Register(
            "MouseRightButtonDownCommand", typeof(ICommand), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MouseRightButtonUpCommandProperty = DependencyProperty.Register(
            "MouseRightButtonUpCommand", typeof(ICommand), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MouseMoveCommandProperty = DependencyProperty.Register(
            "MouseMoveCommand", typeof(ICommand), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(MousePositionBehavior<T>), new PropertyMetadata(1.0));

        public static readonly DependencyProperty MouseEnterCommandProperty = DependencyProperty.Register(
            "MouseEnterCommand", typeof(ICommand), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty MouseEnterParamProperty = DependencyProperty.Register(
            "MouseEnterParam", typeof(object), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty PreviewMouseLeftButtonDownCommandProperty = DependencyProperty.Register(
            "PreviewMouseLeftButtonDownCommand", typeof(ICommand), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty PreviewMouseLeftButtonUpCommandProperty = DependencyProperty.Register(
            "PreviewMouseLeftButtonUpCommand", typeof(ICommand), typeof(MousePositionBehavior<T>), new PropertyMetadata(default(ICommand)));

        public ICommand PreviewMouseLeftButtonUpCommand
        {
            get => (ICommand) GetValue(PreviewMouseLeftButtonUpCommandProperty);
            set => SetValue(PreviewMouseLeftButtonUpCommandProperty, value);
        }

        public ICommand PreviewMouseLeftButtonDownCommand
        {
            get => (ICommand) GetValue(PreviewMouseLeftButtonDownCommandProperty);
            set => SetValue(PreviewMouseLeftButtonDownCommandProperty, value);
        }

        public object MouseEnterParam
        {
            get => GetValue(MouseEnterParamProperty);
            set => SetValue(MouseEnterParamProperty, value);
        }

        public ICommand MouseEnterCommand
        {
            get => (ICommand) GetValue(MouseEnterCommandProperty);
            set => SetValue(MouseEnterCommandProperty, value);
        }

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public ICommand MouseMoveCommand
        {
            get => (ICommand) GetValue(MouseMoveCommandProperty);
            set => SetValue(MouseMoveCommandProperty, value);
        }

        public ICommand MouseRightButtonUpCommand
        {
            get => (ICommand) GetValue(MouseRightButtonUpCommandProperty);
            set => SetValue(MouseRightButtonUpCommandProperty, value);
        }

        public ICommand MouseRightButtonDownCommand
        {
            get => (ICommand) GetValue(MouseRightButtonDownCommandProperty);
            set => SetValue(MouseRightButtonDownCommandProperty, value);
        }

        public ICommand MouseLeftButtonUpCommand
        {
            get => (ICommand) GetValue(MouseLeftButtonUpCommandProperty);
            set => SetValue(MouseLeftButtonUpCommandProperty, value);
        }

        public ICommand MouseLeftButtonDownCommand
        {
            get => (ICommand) GetValue(MouseLeftButtonDownCommandProperty);
            set => SetValue(MouseLeftButtonDownCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            AssociatedObject.MouseRightButtonDown += OnMouseRightButtonDown;
            AssociatedObject.MouseRightButtonUp += OnMouseRightButtonUp;
            AssociatedObject.MouseMove += OnMouseMove;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseRightButtonUp -= OnMouseRightButtonUp;
            AssociatedObject.MouseRightButtonDown -= OnMouseRightButtonDown;
            AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            AssociatedObject.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
            AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseEnter -= OnMouseEnter;
            base.OnDetaching();
        }

        protected virtual void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PreviewMouseLeftButtonDownCommand?.Invoke(MouseEnterParam ?? DivPoint(Mouse.GetPosition(AssociatedObject), Scale));
            e.Handled = Handled;
        }

        protected virtual void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PreviewMouseLeftButtonUpCommand?.Invoke(MouseEnterParam ?? DivPoint(Mouse.GetPosition(AssociatedObject), Scale));
            e.Handled = Handled;
        }

        protected virtual void OnMouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnterCommand?.Invoke(MouseEnterParam ?? DivPoint(Mouse.GetPosition(AssociatedObject), Scale));
            e.Handled = Handled;
        }

        protected bool Handled = false;

        protected Point DivPoint(Point point, double diver)
        {
            return new Point(point.X / diver, point.Y / diver);
        }

        protected virtual void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseLeftButtonDownCommand?.Invoke(DivPoint(Mouse.GetPosition(AssociatedObject),Scale));
            e.Handled = Handled;
        }

        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            MouseMoveCommand?.Invoke(DivPoint(Mouse.GetPosition(AssociatedObject), Scale));
            e.Handled = Handled;
        }

        protected virtual void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseRightButtonUpCommand?.Invoke(DivPoint(Mouse.GetPosition(AssociatedObject), Scale));
            e.Handled = Handled;
        }

        protected virtual void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseRightButtonDownCommand?.Invoke(DivPoint(Mouse.GetPosition(AssociatedObject), Scale));
            e.Handled = Handled;
        }

        protected virtual void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseLeftButtonUpCommand?.Invoke(DivPoint(Mouse.GetPosition(AssociatedObject), Scale));
            e.Handled = Handled;
        }
    }
}*/