namespace EnglishMaster.Client.Stores
{
    public class StateContainer : IStateContainer
    {
        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    NotifyLoadingStateChanged();
                }
            }
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyMessageChanged();
                }
            }
        }

        public event Action OnLoadingStateChanged = null!;
        public event Action OnMessageChanged = null!;


        private void NotifyLoadingStateChanged()
        {
            if (OnLoadingStateChanged == null) throw new ArgumentNullException(nameof(OnLoadingStateChanged));
            OnLoadingStateChanged.Invoke();
        }
        private void NotifyMessageChanged()
        {
            if (OnMessageChanged == null) throw new ArgumentNullException(nameof(OnMessageChanged));
            OnMessageChanged.Invoke();
        }
    }
}
