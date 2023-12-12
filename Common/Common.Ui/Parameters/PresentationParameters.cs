using Common.Extensions.ValueTypes;
using ReactiveUI;

namespace Common.Ui.Parameters
{
    public class PresentationParameters : ReactiveObject, IPresentationParameters
    {
        private const double MIN_SCALE = 0.5;
        private const double MAX_SCALE = 3;
        private const double DEFAULT_SCALE = 1.0;

        private double _scaleFactor;

        public PresentationParameters()
        {
            //ToDo нормальное задание значения
            _scaleFactor = 1;
        }

        /// <inheritdoc />
        public double ScaleFactor
        {
            get => _scaleFactor;
            set
            {
                double newValue = value.NaNToDouble(DEFAULT_SCALE).Bound(MIN_SCALE, MAX_SCALE);
                this.RaiseAndSetIfChanged(ref _scaleFactor, newValue);
            }
        }
    }
}