namespace EnglishMaster.Client.Components
{
    public partial class Loader
    {
        private string _style = "display: none;";

        protected override void OnInitialized()
        {
            StateContainer.OnLoadingStateChanged += OnLoadingStateChanged;
        }

        private void DisplayLoader()
        {
            _style = "display: flex;";
        }

        private void HideLoader()
        {
            _style = "display: none;";
        }

        private void OnLoadingStateChanged()
        {
            if (StateContainer.IsLoading)
            {
                DisplayLoader();
            }
            else
            {
                HideLoader();
            }
            StateHasChanged();
        }
    }
}
