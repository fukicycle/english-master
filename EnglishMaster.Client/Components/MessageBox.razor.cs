namespace EnglishMaster.Client.Components
{
    public partial class MessageBox
    {
        private void CloseButtonOnClick()
        {
            StateContainer.DialogContent = null;
        }

        private void OkButtonOnClick()
        {
            StateContainer.DialogContent = null;
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }
}
