# How-to-highlight-selected-data-points-by-using-GetDataPoints-method-in-.NET-MAUI-Cartesian-Charts
The [.NET MAUI Cartesian Chart](https://www.syncfusion.com/maui-controls/maui-cartesian-charts) provides customization options for enhancing data visualization. One of its key features is the ability to interact with data through the [GetDataPoints](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.CartesianSeries.html#Syncfusion_Maui_Charts_CartesianSeries_GetDataPoints_Microsoft_Maui_Graphics_Rect_) method of the [Cartesian series](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.CartesianSeries.html). This method allows retrieval of data points that lie within a specified rectangular area or within defined X and Y coordinate ranges. This article aims to demonstrate how to highlight selected data points using the **GetDataPoints** method, detailing two effective methods:

### **Method 1: Custom Rectangular Area Selection**
 This way handling touch points to draw a custom rectangle and highlight data points that fall within it:

**Step 1:  Create the Extension Class**
Create an extension class inherited from the [ChartInteractiveBehavior](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartInteractiveBehavior.html) class to handle the custom touch interactions on the chart. Define fields for storing the starting and ending coordinates of the touch and a flag to indicate when the rectangle should be shown. Implement the IDrawable interface to draw the selection rectangle. Override the [OnTouchDown](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBehavior.html#Syncfusion_Maui_Charts_ChartBehavior_OnTouchDown_Syncfusion_Maui_Charts_ChartBase_System_Single_System_Single_), [OnTouchMove](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBehavior.html#Syncfusion_Maui_Charts_ChartBehavior_OnTouchMove_Syncfusion_Maui_Charts_ChartBase_System_Single_System_Single_), and [OnTouchUp](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBehavior.html#Syncfusion_Maui_Charts_ChartBehavior_OnTouchUp_Syncfusion_Maui_Charts_ChartBase_System_Single_System_Single_) methods to manage the drawing and selection process.

**OnTouchDown Method :**  Capture the initial touch point when the user touches down on the chart.
**OnTouchMove Method :**  Update the end coordinates of the selection rectangle dynamically.

**OnTouchDown Method :** 
Finalize the rectangle dimensions by utilizing the [SeriesBounds](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartBase.html#Syncfusion_Maui_Charts_ChartBase_SeriesBounds), which provides the actual rendering bounds of chart series. Then, get the data points that fall inside the rectangle using the `GetDataPoints` method. Update the indexes of these data points to the [SelectedIndexes](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartSelectionBehavior.html?tabs=tabid-5%2Ctabid-7%2Ctabid-3%2Ctabid-1#Syncfusion_Maui_Charts_ChartSelectionBehavior_SelectedIndexes) property of  [ChartSelectionBehavior](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartSelectionBehavior.html).

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
```html
<chart:SfCartesianChart>
    <chart:SfCartesianChart.PlotAreaBackgroundView>
        <GraphicsView Drawable="{x:Reference InteractionExt}" x:Name="graphicsView" InputTransparent="True" ZIndex="1"/>
    </chart:SfCartesianChart.PlotAreaBackgroundView>
    
    <chart:SfCartesianChart.InteractiveBehavior>
        <local:ChartInteractionExt x:Name="InteractionExt" Graphics="{x:Reference graphicsView}"/>
    </chart:SfCartesianChart.InteractiveBehavior>
    . . .
    <chart:SfCartesianChart.Series>
        <chart:ScatterSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" PointWidth="8" Opacity="0.8" Fill="#FE7A36" PointHeight="8" >
            <chart:ScatterSeries.SelectionBehavior>
                <chart:DataPointSelectionBehavior Type="Multiple"  SelectionBrush="#3652AD"/>
            </chart:ScatterSeries.SelectionBehavior>
        </chart:ScatterSeries>

    </chart:SfCartesianChart.Series>
</chart:SfCartesianChart>
```

**Output**
 ![GetDataPoints.gif](https://support.syncfusion.com/kb/agent/attachment/article/16174/inline?token=eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIzNzA2Iiwib3JnaWQiOiIzIiwiaXNzIjoic3VwcG9ydC5zeW5jZnVzaW9uLmNvbSJ9.XB4_s1DqiYHD1jFJFSKiOOTi8cVwvYFpRjffhWMXnGM)

### **Method 2: Selection Zoom Events**
This method involves handling selection zoom events to draw a custom rectangle and highlight data points that fall within it:
#### Zoom and Pan Events:
**[ZoomStart:](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_ZoomStart)** Triggered when the user initiates a zoom action. Can be canceled to interrupt the action.
**[ZoomDelta:](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_ZoomDelta)** Activated during the zooming process and can be canceled.
**[ZoomEnd:](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_ZoomEnd)** Triggered when the zooming action finishes.
**[SelectionZoomStart:](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_SelectionZoomStart)** Occurs when the user begins box selection zooming.
**[SelectionZoomDelta:](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_SelectionZoomDelta)** Activated during the process of selecting a region for zooming and can be canceled.
**[SelectionZoomEnd:](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_SelectionZoomEnd)** Triggered after the selection zooming ends.
**[Scroll:](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_Scroll)** Triggered during panning and can be canceled.

**Step 1: Initialize Selection Zoom Events**
Initialize the  [SelectionZoomDelta](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_SelectionZoomDelta) and [SelectionZoomEnd](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_SelectionZoomEnd) events in the [SfCartesianChart.](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html) Initialize the [ZoomPanBehavior](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_ZoomPanBehavior) and enable selection zoom using the [EnableSelectionZooming](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartZoomPanBehavior.html#Syncfusion_Maui_Charts_ChartZoomPanBehavior_EnableSelectionZooming) API available in [ChartZoomPanBehavior.](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartZoomPanBehavior.html)
 
 ```html
<chart:SfCartesianChart x:Name="cartesianChart"           
                        SelectionZoomDelta="Chart_SelectionZoomDelta"
                        SelectionZoomEnd="Chart_SelectionZoomEnd">

    <chart:SfCartesianChart.ZoomPanBehavior>
        <chart:ChartZoomPanBehavior EnableSelectionZooming="True" />
    </chart:SfCartesianChart.ZoomPanBehavior>
    
    <chart:SfCartesianChart.XAxes>
        <chart:NumericalAxis x:Name="primaryAxis" />
    </chart:SfCartesianChart.XAxes>
    
    <chart:SfCartesianChart.YAxes>
        <chart:NumericalAxis x:Name="secondaryAxis" />
    </chart:SfCartesianChart.YAxes>
    
    <chart:SfCartesianChart.Series>
        <chart:ScatterSeries ItemsSource="{Binding Data}"
                             XBindingPath="XValue" 
                             YBindingPath="YValue">
            <chart:ScatterSeries.SelectionBehavior>
                <chart:DataPointSelectionBehavio  
                                 Type="Multiple"  
                                 SelectionBrush="#3652AD"/>
            </chart:ScatterSeries.SelectionBehavior>
        </chart:ScatterSeries>
    </chart:SfCartesianChart.Series>
    
</chart:SfCartesianChart> 
 ```
**Step 2: Handle the interaction**
The [SelectionZoomDelta](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_SelectionZoomDelta) event is activated during the process of selecting a region in the chart area. Inside the event handler, retrieve the selecting area rectangle values. Then, use the [GetDataPoints](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.CartesianSeries.html#Syncfusion_Maui_Charts_CartesianSeries_GetDataPoints_Microsoft_Maui_Graphics_Rect_) method to retrieve the data points that fall inside the rectangle. Update the indexes of these data points in the [SelectedIndexes](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartSelectionBehavior.html#Syncfusion_Maui_Charts_ChartSelectionBehavior_SelectedIndex) property of [ChartSelectionBehavior.](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartSelectionBehavior.html)

 ```csharp
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
 ```
**Step 3: Cancel the zoom**
The [SelectionZoomEnd](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.SfCartesianChart.html#Syncfusion_Maui_Charts_SfCartesianChart_ZoomEnd) event is invoked when selecting a region from the chart area. Inside the event handler, set the [ZoomFactor](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartAxis.html#Syncfusion_Maui_Charts_ChartAxis_ZoomFactor) to 1 and the [ZoomPosition](https://help.syncfusion.com/cr/maui/Syncfusion.Maui.Charts.ChartAxis.html#Syncfusion_Maui_Charts_ChartAxis_ZoomPosition) to 0. This helps cancel the zoom.

 ```csharp
private void Chart_SelectionZoomEnd(object sender, ChartSelectionZoomEventArgs e)
{
     primaryAxis.ZoomFactor = 1;
     primaryAxis.ZoomPosition = 0;
     secondaryAxis.ZoomFactor = 1;
     secondaryAxis.ZoomPosition = 0;
 } 
 ```
 ![Selection_zoom.gif](https://support.syncfusion.com/kb/agent/attachment/article/16174/inline?token=eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjI0ODUzIiwib3JnaWQiOiIzIiwiaXNzIjoic3VwcG9ydC5zeW5jZnVzaW9uLmNvbSJ9.mcnMg6jPkXsxDZqv152Z1mzVSk-CI-62kO_T0Alylsk)
