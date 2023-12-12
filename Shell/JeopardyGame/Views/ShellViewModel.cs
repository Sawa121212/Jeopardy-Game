using System.Windows.Input;
using Common.Core.Localization;
using Prism.Commands;
using Prism.Mvvm;

namespace JeopardyGame.Views
{
    public class ShellViewModel : BindableBase
    {
        private readonly ILocalizer _localizer;
        private bool _drawDirtyRects;
        private bool _drawFps = true;
        private bool _drawLayoutTimeGraph;
        private bool _drawRenderTimeGraph;

        public ShellViewModel(ILocalizer localizer)
        {
            _localizer = localizer;

            ToggleDrawDirtyRects = new DelegateCommand(() => DrawDirtyRects = !DrawDirtyRects);
            ToggleDrawFps = new DelegateCommand(() => DrawFps = !DrawFps);
            ToggleDrawLayoutTimeGraph = new DelegateCommand(() => DrawLayoutTimeGraph = !DrawLayoutTimeGraph);
            ToggleDrawRenderTimeGraph = new DelegateCommand(() => DrawRenderTimeGraph = !DrawRenderTimeGraph);
        }

        public bool DrawDirtyRects
        {
            get => _drawDirtyRects;
            set => SetProperty(ref _drawDirtyRects, value);
        }

        public bool DrawFps
        {
            get => _drawFps;
            set => SetProperty(ref _drawFps, value);
        }

        public bool DrawLayoutTimeGraph
        {
            get => _drawLayoutTimeGraph;
            set => SetProperty(ref _drawLayoutTimeGraph, value);
        }

        public bool DrawRenderTimeGraph
        {
            get => _drawRenderTimeGraph;
            set => SetProperty(ref _drawRenderTimeGraph, value);
        }

        public ICommand ToggleDrawDirtyRects { get; }
        public ICommand ToggleDrawFps { get; }
        public ICommand ToggleDrawLayoutTimeGraph { get; }
        public ICommand ToggleDrawRenderTimeGraph { get; }
    }
}