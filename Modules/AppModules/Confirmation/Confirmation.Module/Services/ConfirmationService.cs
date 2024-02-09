using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Common.Extensions;
using Confirmation.Module.Enums;
using Material.Dialog;
using Material.Dialog.Icons;

namespace Confirmation.Module.Services
{
    public class ConfirmationService : IConfirmationService
    {
        public ConfirmationService()
        {
        }

        /// <inheritdoc />
        public async Task<ConfirmationResultEnum> ShowInfoAsync(
            string title,
            string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok)
        {
            return await ReturnConfirmationResult(title, message, DialogIconKind.Info, buttons).ConfigureAwait(true);
        }

        /// <inheritdoc />
        public async Task<ConfirmationResultEnum> ShowWarningAsync(
            string title, string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok)
        {
            return await ReturnConfirmationResult(title, message, DialogIconKind.Warning, buttons).ConfigureAwait(true);
        }

        /// <inheritdoc />
        public async Task<ConfirmationResultEnum> ShowErrorAsync(
            string title, string message,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok)
        {
            return await ReturnConfirmationResult(title, message, DialogIconKind.Error, buttons).ConfigureAwait(true);
        }

        private async Task<ConfirmationResultEnum> ReturnConfirmationResult(
            string title,
            string message,
            DialogIconKind iconKind,
            ConfirmationResultEnum buttons = ConfirmationResultEnum.Ok)
        {
            List<DialogButton> dialogButtons = GetButtons(buttons);
            DialogResult result = await DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
            {
                ContentHeader = title,
                SupportingText = message,
                StartupLocation = WindowStartupLocation.CenterOwner,
                NegativeResult = new DialogResult(DialogHelper.DIALOG_RESULT_CANCEL),
                DialogHeaderIcon = iconKind,
                DialogButtons = dialogButtons.ToArray()
            }).ShowDialog(GetWindow());

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return result.GetResult switch
            {
                DialogHelper.DIALOG_RESULT_ABORT => ConfirmationResultEnum.None,
                DialogHelper.DIALOG_RESULT_OK => ConfirmationResultEnum.Ok,
                DialogHelper.DIALOG_RESULT_YES => ConfirmationResultEnum.Yes,
                DialogHelper.DIALOG_RESULT_NO => ConfirmationResultEnum.No,
                DialogHelper.DIALOG_RESULT_CANCEL => ConfirmationResultEnum.Cancel,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private List<DialogButton> GetButtons(ConfirmationResultEnum resultButtons)
        {
            List<DialogButton> buttons = new();

            if (resultButtons.ContainsAny(ConfirmationResultEnum.None))
            {
                buttons.Add(
                    new DialogButton
                    {
                        Content = "None",
                        Result = DialogHelper.DIALOG_RESULT_ABORT
                    }
                );
            }

            if (resultButtons.ContainsAny(ConfirmationResultEnum.Ok))
            {
                buttons.Add(
                    new DialogButton
                    {
                        // ToDo: localization
                        Content = "Ок",
                        Result = DialogHelper.DIALOG_RESULT_OK
                    }
                );
            }

            if (resultButtons.ContainsAny(ConfirmationResultEnum.Yes))
            {
                buttons.Add(
                    new DialogButton
                    {
                        Content = "Да",
                        Result = DialogHelper.DIALOG_RESULT_YES
                    }
                );
            }

            if (resultButtons.ContainsAny(ConfirmationResultEnum.No))
            {
                buttons.Add(
                    new DialogButton
                    {
                        Content = "Нет",
                        Result = DialogHelper.DIALOG_RESULT_NO
                    }
                );
            }

            if (resultButtons.ContainsAny(ConfirmationResultEnum.Cancel))
            {
                buttons.Add(
                    new DialogButton
                    {
                        Content = "Отмена",
                        Result = DialogHelper.DIALOG_RESULT_CANCEL
                    }
                );
            }

            return buttons;
        }

        private Window GetWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app)
            {
                return app.MainWindow;
            }

            return null;
        }
    }
}