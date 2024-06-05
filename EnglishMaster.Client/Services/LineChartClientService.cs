using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.LineChart;

namespace EnglishMaster.Client.Services;
public sealed class LineChartClientService
{
    private const string FONT_COLOR = "#F2F2F2";

    private LineOptions GenerateDefaultOptions(string title)
    {
        return new LineOptions
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
            Scales = new Scales
            {
                XAxes = new List<CartesianAxis>
                    {
                        new CategoryAxis
                        {
                            Ticks = new ChartJs.Blazor.Common.Axes.Ticks.CategoryTicks
                            {
                                FontColor = FONT_COLOR
                            }
                        }
                    },
                YAxes = new List<CartesianAxis>
                    {
                        new LinearCartesianAxis
                        {
                            Ticks = new ChartJs.Blazor.Common.Axes.Ticks.LinearCartesianTicks
                            {
                                FontColor = FONT_COLOR,
                                BeginAtZero = true,
                                StepSize = 10,
                                Min = 0,
                                Max = 100
                            }
                        }
                    }
            }
        };
    }

    public LineConfig Create<TValue>(Dictionary<string, TValue> data, string title, string datasetLabel)
    {
        LineConfig lineConfig = new LineConfig
        {
            Options = GenerateDefaultOptions(title)
        };
        LineDataset<TValue> dataset = new LineDataset<TValue>();
        dataset.Label = datasetLabel;
        foreach (var singleData in data)
        {
            lineConfig.Data.Labels.Add(singleData.Key);
            dataset.Add(singleData.Value);
        }
        lineConfig.Data.Datasets.Add(dataset);
        return lineConfig;
    }

    public LineConfig Create<TValue>(Dictionary<DateTime, TValue> data, string title, string datasetLabel)
    {
        LineConfig lineConfig = new LineConfig
        {
            Options = GenerateDefaultOptions(title)
        };
        LineDataset<TValue> dataset = new LineDataset<TValue>();
        dataset.Label = datasetLabel;
        foreach (var singleData in data)
        {
            lineConfig.Data.Labels.Add(singleData.Key.ToString("MM/dd"));
            dataset.Add(singleData.Value);
        }
        lineConfig.Data.Datasets.Add(dataset);
        return lineConfig;
    }
}