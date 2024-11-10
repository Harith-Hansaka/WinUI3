using UNDAI.SERVICES;

namespace UNDAI.COMMANDS.BASE
{
    public class NavigateCommand : CommandBase
    {
        public readonly NavigationService _navigationService;
        string _navigateTo;

        public NavigateCommand(NavigationService navigationService, string navigateTo)
        {
            _navigationService = navigationService;
            _navigateTo = navigateTo;
        }
        public override void Execute(object parameter)
        {
            _navigationService.NavigateTo(_navigateTo);
        }
    }
}
