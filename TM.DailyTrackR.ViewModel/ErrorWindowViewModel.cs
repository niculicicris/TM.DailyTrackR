using Prism.Commands;
using Prism.Mvvm;
using TM.DailyTrackR.Common;

namespace TM.DailyTrackR.ViewModel;

public class ErrorWindowViewModel : BindableBase
{
    public ErrorWindowViewModel(string errorMessage)
    {
        ErrorMessage = errorMessage;
        CloseCommand = new DelegateCommand(Close);
    }

    public string ErrorMessage { get; }

    public DelegateCommand CloseCommand { get; }

    private void Close()
    {
        ViewService.Instance.CloseWindow(this);
    }
}