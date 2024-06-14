using Syncfusion.Maui.Charts;
using System.ComponentModel;

namespace GetDataPointsSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }

    public class ChartInteractionExt : ChartInteractiveBehavior, IDrawable, INotifyPropertyChanged
    {
        float startX;
        float startY;
        float endX;
        float endY;

        bool showRect = false;
        public GraphicsView Graphics { get; set; }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;
            canvas.DrawRectangle(startX, startY, endX, endY);
        }

        protected override void OnTouchDown(ChartBase chart, float pointX, float pointY)
        {
            endY = 0; endX = 0;
            showRect = true;
            var seriesBounds = chart.SeriesBounds;
            startX = (float)(pointX - seriesBounds.Left);
            startY = (float)(pointY - seriesBounds.Top);           
        }


        protected override void OnTouchUp(ChartBase chart, float pointX, float pointY)
        {
            if (chart is SfCartesianChart cartesianChart)
            {
                var viewModel = chart.BindingContext as ScatterSeriesViewModel;

                var rect = new Rect(startX, startY, endX, endY);

                var selectedIndexes = new List<int>();

                foreach (var series in cartesianChart.Series)
                {
                    if (series is ScatterSeries scatterSeries)
                    {
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

            showRect = false;
            Graphics.Invalidate();
        }

        protected override void OnTouchMove(ChartBase chart, float pointX, float pointY)
        {
            var seriesBounds = chart.SeriesBounds;

            if (showRect)
            {
                float width = (float)(pointX - startX - seriesBounds.Left);
                float height = (float)(pointY - startY - seriesBounds.Top);
                endX = width;
                endY = height;
            }
            Graphics.Invalidate();
        }
    }

}
