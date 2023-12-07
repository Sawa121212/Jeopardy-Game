/*
using System;
using System.Windows.Input;
using Avalonia.Xaml.Interactivity;
using AvaloniaUI.Ribbon;
using Common.Extensions;

namespace Common.Ui.Behaviors.RibbonTabs
{
    public class RibbonTabSelectedBehavior : Behavior<RibbonTab>
    {
        public static readonly DependencyProperty TabSelectedProperty = DependencyProperty.Register(
            "TabSelected", typeof(bool), typeof(RibbonTabSelectedBehavior), new PropertyMetadata(default(bool), TabSelectedChanged ));

        private static void TabSelectedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var b = dependencyObject as RibbonTabSelectedBehavior;
             b?.SelectionChangedEvent.Raise(b, new SelectionChangedEventArgs(b.AssociatedObject, b.TabSelected));
        }

        public bool TabSelected
        {
            get => (bool)GetValue(TabSelectedProperty);
            set => SetValue(TabSelectedProperty, value);
        }

        public static readonly DependencyProperty SelectedCommandProperty = DependencyProperty.Register(
            "SelectedCommand", typeof(ICommand), typeof(RibbonTabSelectedBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand SelectedCommand
        {
            get => (ICommand)GetValue(SelectedCommandProperty);
            set => SetValue(SelectedCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            SelectionChangedEvent += RibbonTabSelectionChangedEvent;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            SelectionChangedEvent -= RibbonTabSelectionChangedEvent;
        }

        private void RibbonTabSelectionChangedEvent(object sender, SelectionChangedEventArgs e)
        {
            if(e.IsSelected) SelectedCommand?.Execute(null);
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChangedEvent;

        public class SelectionChangedEventArgs : EventArgs
        {
            public SelectionChangedEventArgs(RibbonTab ribbonTab, bool isSelected)
            {
                Tab = ribbonTab;
                IsSelected = isSelected;
            }
            public RibbonTab Tab { get; }
            public bool IsSelected { get; }
        }
    }
}
*/
