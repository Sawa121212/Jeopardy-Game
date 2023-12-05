﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JeopardyGame.Views
{
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}