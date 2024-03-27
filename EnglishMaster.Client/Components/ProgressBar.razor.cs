using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Components
{
    public partial class ProgressBar
    {
        [Parameter]
        public double Value { get; set; }
        [Parameter]
        public bool HasDisplayValue { get; set; }
        private string _style = "width:0%;";

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (Value < 0)
            {
                throw new NotSupportedException($"Not support value:{Value}");
            }
            if (Value > 100)
            {
                throw new NotSupportedException($"Not support value:{Value}");
            }
            _style = $"width:{Value}%;";
        }

        private string GetDisplayValue()
        {
            return $"{Math.Round(Value, 1)}%";
        }
    }
}