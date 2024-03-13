using EnglishMaster.Client.Stores;
using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Pages
{
    public abstract class PageBase : ComponentBase, IDisposable
    {
        [Inject]
        public IStateContainer StateContainer { get; private set; } = null!;

        protected override void OnInitialized()
        {
            StateContainer.OnMessageChanged += StateHasChanged;
            StateContainer.OnLoadingStateChanged += StateHasChanged;
        }
        public void Dispose()
        {
            StateContainer.OnMessageChanged -= StateHasChanged;
            StateContainer.OnLoadingStateChanged -= StateHasChanged;
        }

        protected void RunActionWithLoading(Action action)
        {
            try
            {
                StateContainer.IsLoading = true;
                action();
            }
            catch (Exception ex)
            {
                StateContainer.Message = ex.Message;
            }
            finally
            {
                StateContainer.IsLoading = false;
            }
        }
    }
}
