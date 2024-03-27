using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Components
{
    public partial class ProgressCircle
    {
        [Parameter]
        public double Value { get; set; }

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
        }

        private string GetDisplayValue()
        {
            return $"{Math.Round(Value)}%";
        }

        private double GetStrokeDashArray()
        {
            return 2 * Math.PI * 110;
        }

        private double GetStrokeDashOffset()
        {
            return GetStrokeDashArray() * ((100 - Value) / 100);
        }
    }
}