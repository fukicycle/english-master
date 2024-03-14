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

        protected async Task RunActionWithLoading(Func<Task> func)
        {
            try
            {
                await InvokeAsync(() => StateContainer.IsLoading = true);
                await func();
            }
            catch (Exception ex)
            {
                StateContainer.Message = ex.Message;
            }
            finally
            {
                await InvokeAsync(() => StateContainer.IsLoading = false);
            }
        }
    }
}
