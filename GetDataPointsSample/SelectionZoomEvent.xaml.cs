using Syncfusion.Maui.Charts;

namespace GetDataPointsSample;

public partial class SelectionZoomEvent : ContentPage
{
    public SelectionZoomEvent()
    {
        InitializeComponent();
    }

    private void Chart_SelectionZoomDelta(object sender, ChartSelectionZoomDeltaEventArgs e)
    {
        var selectedIndexes = new List<int>();

        foreach (var series in cartesianChart.Series)
        {
            if (series is ScatterSeries scatterSeries)
            {
                var rect = new Rect(e.ZoomRect.X - cartesianChart.SeriesBounds.Left, e.ZoomRect.Y, e.ZoomRect.Width, e.ZoomRect.Height);
                var dataPoints = scatterSeries.GetDataPoints(rect);

                if (dataPoints != null && viewModel != null)
                {
                    for (int i = 0; i < viewModel.Data.Count; i++)
                    {
                        if (dataPoints.Contains(viewModel.Data[i]))
                            selectedIndexes.Add(i);
                    }
                    scatterSeries.SelectionBehavior.SelectedIndexes = selectedIndexes;
                }
            }
        }
    }

    private void Chart_SelectionZoomEnd(object sender, ChartSelectionZoomEventArgs e)
    {
        primaryAxis.ZoomFactor = 1;
        primaryAxis.ZoomPosition = 0;
        secondaryAxis.ZoomFactor = 1;
        secondaryAxis.ZoomPosition = 0;
    }
}