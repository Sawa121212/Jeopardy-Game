﻿using Avalonia.Controls;
using Common.Core.Interfaces;
using Common.Ui.Views;
using Confirmation.Domain.Enums;

namespace Confirmation.Ui.Views
{
    public partial class ConfirmationView : Window, IViewWithResult<ConfirmationResultEnum>
    {
        public ConfirmationView()
        {
            InitializeComponent();
        }

        public ConfirmationResultEnum Result =>
            DataContext is IResult<ConfirmationResultEnum> result
                ? result.GetResult()
                : ConfirmationResultEnum.Cancel;
    }
}