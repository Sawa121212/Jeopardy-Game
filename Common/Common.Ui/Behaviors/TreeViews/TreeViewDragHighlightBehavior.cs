/*using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using Common.Extensions;
using Common.Ui.Extensions;

namespace Common.Ui.Behaviors.TreeViews
{
    public class TreeViewDragHighlightBehavior : Behavior<TreeView>
    {
        private readonly IList<Border> _highlighteds;

        public TreeViewDragHighlightBehavior()
        {
            _highlighteds = new List<Border>();
        }
        
        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeave += OnMouseLeave;
            AssociatedObject.PreviewDragLeave += OnDragLeave;
            AssociatedObject.PreviewDrop += OnPreviewDrop;
            AssociatedObject.PreviewDragOver += OnDragOver;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeave -= OnMouseLeave;
            AssociatedObject.PreviewDragLeave -= OnDragLeave;
            AssociatedObject.PreviewDrop -= OnPreviewDrop;
            AssociatedObject.PreviewDragOver -= OnDragOver;
            base.OnDetaching();
        }
        
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            ClearHighlight();
        }

        private void OnPreviewDrop(object sender, DragEventArgs e)
        {
            ClearHighlight();
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if(e.OriginalSource is Border border && border.Name.IsEquals("Bd"))
            {
                Highlight(border, true);
            }
            else if (e.OriginalSource is FrameworkElement element)
            {
                var bd = element.TryFindParent<Border>("Bd");
                if (bd != null)
                {
                    Highlight(bd, true);
                }
            }
        }

        private void OnDragLeave(object sender, DragEventArgs e)
        {
            if(e.OriginalSource is Border border && border.Name.IsEquals("Bd"))
            {
                ClearHighlight();
            }
        }

        private void Highlight(Border border, bool highlight)
        {
            if (border == null)
                return;
            
            var mouseOverProperty = border.GetValue(DragExtension.IsMouseOverExProperty);
            if (mouseOverProperty is bool mouseOver)
            {
                if (!mouseOver.Equals(highlight))
                {
                    border.SetValue(DragExtension.IsMouseOverExProperty, highlight);
                    if(highlight)
                    {
                        ClearHighlight();
                        _highlighteds.Add(border);
                    }
                    else
                    {
                        ClearHighlight();
                    }
                }
            }
        }
        
        private void ClearHighlight()
        {
            foreach (var border in _highlighteds)
            {
                border?.SetValue(DragExtension.IsMouseOverExProperty, false);
            }
            _highlighteds.Clear();
        }
    }
}*/