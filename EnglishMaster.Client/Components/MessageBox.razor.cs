using EnglishMaster.Client.Pages;
using System.Diagnostics;

namespace EnglishMaster.Client.Components
{
    public partial class MessageBox
    {
        private string _display = "display: none;";

        protected override void OnInitialized()
        {
            StateContainer.OnMessageChanged += StateContainer_OnMessageChanged;
        }

        private void StateContainer_OnMessageChanged()
        {
            if (StateContainer.Message == string.Empty)
            {
                _display = "display: none;";
            }
            else
            {
                _display = "display: block;";
            }
            StateHasChanged();
        }

        private void CloseButtonOnClick()
        {
            StateContainer.Message = string.Empty;
        }

        private void OkButtonOnClick()
        {
            StateContainer.Message = string.Empty;
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }
}
