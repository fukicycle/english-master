using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.RadarChart;

namespace EnglishMaster.Client.Services
{
    public class RadarChartClientService
    {
        private const string FONT_COLOR = "#F2F2F2";

        private RadarOptions GenerateDefaultOptions(string title)
        {
            return new RadarOptions
            {
                Responsive = true,
                AspectRatio = 1.3,
                Title = new OptionsTitle
                {
                    Display = true,
                    Text = title,
                    FontColor = FONT_COLOR
                },
                Legend = new Legend
                {
                    Display = true,
                    Labels = new LegendLabels
                    {
                        FontColor = FONT_COLOR,
                    }
                },
                Scale = new LinearRadialAxis
                {
                    Ticks = new ChartJs.Blazor.Common.Axes.Ticks.LinearRadialTicks
                    {
                        Min = 0,
                        Max = 100
                    },
                    GridLines = new GridLines
                    {
                        Color = FONT_COLOR
                    },
                    PointLabels = new PointLabels
                    {
                        FontColor = FONT_COLOR
                    }
                }
            };
        }

        public RadarConfig Create<TValue>(Dictionary<string, TValue> data, string title, string datasetLabel)
        {
            RadarConfig radarConfig = new RadarConfig
            {
                Options = GenerateDefaultOptions(title)
            };
            RadarDataset<TValue> dataset = new RadarDataset<TValue>
            {
                Fill = true
            };
            dataset.Label = datasetLabel;
            foreach (var singleData in data)
            {
                radarConfig.Data.Labels.Add(singleData.Key);
                dataset.Add(singleData.Value);
            }
            radarConfig.Data.Datasets.Add(dataset);
            return radarConfig;
        }
    }
}
