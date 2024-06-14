# How-to-highlight-selected-data-points-by-using-GetDataPoints-method-in-.NET-MAUI-Cartesian-Charts
This article in the Syncfusion Knowledge Base explains how to highlight selected data points by using GetDataPoints method in .NET MAUI Cartesian Charts

The [.NET MAUI Cartesian Chart](https://www.syncfusion.com/maui-controls/maui-cartesian-charts) offers various customization options for improved data visualization. The [Cartesian series](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.CartesianSeries.html) provides the [GetDataPoints]() method to retrieve the data points that fall  within a specified rectangular area or within defined X and Y coordinate ranges. This article will guide you through the process of highlighting selected data points using the `GetDataPoints` method.

**Step 1:  Create the Extension Class**
Create an extension class inherited from the [ChartInteractiveBehavior](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartInteractiveBehavior.html) class to handle the custom touch interactions on the chart. Define fields for storing the starting and ending coordinates of the touch and a flag to indicate when the rectangle should be shown. Implement the IDrawable interface to draw the selection rectangle. Override the [OnTouchDown](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBehavior.html#Syncfusion_Maui_Charts_ChartBehavior_OnTouchDown_Syncfusion_Maui_Charts_ChartBase_System_Single_System_Single_), [OnTouchMove](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBehavior.html#Syncfusion_Maui_Charts_ChartBehavior_OnTouchMove_Syncfusion_Maui_Charts_ChartBase_System_Single_System_Single_), and [OnTouchUp](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBehavior.html#Syncfusion_Maui_Charts_ChartBehavior_OnTouchUp_Syncfusion_Maui_Charts_ChartBase_System_Single_System_Single_) methods to manage the drawing and selection process.

**OnTouchDown Method :** 
Capture the initial touch point when the user touches down on the chart.

**OnTouchMove Method :** 
Update the end coordinates of the selection rectangle dynamically.

**OnTouchDown Method :** 
Finalize the rectangle dimensions by utilizing the [SeriesBounds](), which provides the actual rendering bounds of chart series. Then, get the data points that fall inside the rectangle using the `GetDataPoints` method. Update the indexes of these data points to the [SelectedIndexes](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartSelectionBehavior.html?tabs=tabid-5%2Ctabid-7%2Ctabid-3%2Ctabid-1#Syncfusion_Maui_Charts_ChartSelectionBehavior_SelectedIndexes) property of  [ChartSelectionBehavior](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartSelectionBehavior.html).

Here's the complete code for the custom interaction class:

**[C#]**
```
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
        if (showRect)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;
            canvas.DrawRectangle(startX, startY, endX, endY);
        }
    }

    protected override void OnTouchDown(ChartBase chart, float pointX, float pointY)
    {
        var seriesBounds = chart.SeriesBounds;
        startX = pointX - seriesBounds.Left;
        startY = pointY - seriesBounds.Top;
        showRect = true;
        Graphics.Invalidate();
    }

    protected override void OnTouchMove(ChartBase chart, float pointX, float pointY)
    {
        if (showRect)
        {
            var seriesBounds = chart.SeriesBounds;
            endX = pointX - startX - seriesBounds.Left;
            endY = pointY - startY - seriesBounds.Top;
            Graphics.Invalidate();
        }
    }

    protected override void OnTouchUp(ChartBase chart, float pointX, float pointY)
    {
        if (showRect && chart is SfCartesianChart cartesianChart)
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
            showRect = false;
            Graphics.Invalidate();
        }
    }
}
```
**Step 2: Assign the Extension Class to the Chart**
Assign the ChartInteractionExt class to the [InteractiveBehavior](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBase.html#Syncfusion_Maui_Charts_ChartBase_InteractiveBehavior) property of the [SfCartesianChart](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html). Define the GraphicsView in the [PlotAreaBackgroundView](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBase.html#Syncfusion_Maui_Charts_ChartBase_PlotAreaBackgroundView) to handle custom drawing.
**[XAML]**
```
<chart:SfCartesianChart>
    <chart:SfCartesianChart.PlotAreaBackgroundView>
        <GraphicsView Drawable="{x:Reference InteractionExt}" x:Name="graphicsView" InputTransparent="True" ZIndex="1"/>
    </chart:SfCartesianChart.PlotAreaBackgroundView>
    
    <chart:SfCartesianChart.InteractiveBehavior>
        <local:ChartInteractionExt x:Name="InteractionExt" Graphics="{x:Reference graphicsView}"/>
    </chart:SfCartesianChart.InteractiveBehavior>
    . . .
    <chart:SfCartesianChart.Series>
        <chart:ScatterSeries ItemsSource="{Binding Data}" XBindingPath="Value" YBindingPath="Size" PointWidth="8" Opacity="0.8" Fill="#FE7A36" PointHeight="8" >
            <chart:ScatterSeries.SelectionBehavior>
                <chart:DataPointSelectionBehavior Type="Multiple"  SelectionBrush="#3652AD"/>
            </chart:ScatterSeries.SelectionBehavior>
        </chart:ScatterSeries>

    </chart:SfCartesianChart.Series>
</chart:SfCartesianChart>
```
**Output**
 ![GetDataPoints.gif](https://support.syncfusion.com/kb/agent/attachment/article/16174/inline?token=eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIzNzA2Iiwib3JnaWQiOiIzIiwiaXNzIjoic3VwcG9ydC5zeW5jZnVzaW9uLmNvbSJ9.XB4_s1DqiYHD1jFJFSKiOOTi8cVwvYFpRjffhWMXnGM)