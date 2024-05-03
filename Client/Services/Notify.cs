using MudBlazor;

namespace Aon_PMS.Services

{
    public class Notify
    {
        private readonly ISnackbar _snack;
        private readonly IDialogService _dialog;

        public Notify(ISnackbar snack, IDialogService dialog)
        {
            _snack = snack;
            _dialog = dialog;
        }

        public async Task<bool> Confirm(string msg)
        {
            bool cnf = await _dialog.ShowMessageBox("Confirm", msg, "OK", "NO") ?? false;
            return cnf;
        }

        public async Task<bool> Message(string? msg)
        {
            if (!(msg == null || msg == ""))
            {
                if (msg.Contains("Error"))
                {
                    await _dialog.ShowMessageBox("Error", msg.Split("-")[1], yesText: "OK");
                }
                else if (msg.Contains("Warning"))
                {
                    _snack.Add(msg.Split("-")[1], Severity.Warning);
                }
                else
                {
                    _snack.Add(msg, Severity.Success);
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> MessageN(string? msg)
        {
            await _dialog.ShowMessageBox("Error", msg, yesText: "OK");
            return false;
        }
        public async Task<bool> Dialog(string? msg)
        {
            await _dialog.ShowMessageBox("", msg, yesText: "OK");
            return false;
        }
    }
}