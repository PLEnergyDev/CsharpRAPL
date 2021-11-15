using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using Accord.Statistics;
using ScottPlot;
using ScottPlot.Drawing;
using ScottPlot.Plottable;

namespace CsharpRAPL.Plotting;

public class BoxPlot : IPlottable {
	public double Position { get; }
	public double[] PlotData { get; }
	public double MaxValue { get; }
	public double MinValue { get; }
	public double UpperPValueQuantile { get; }
	public double LowerPValueQuantile { get; }
	public double Average { get; set; }
	public double Median { get; set; }
	public PlotOptions PlotOptions { get; }
	public bool IsVisible { get; set; } = true;
	public int XAxisIndex { get; set; }
	public int YAxisIndex { get; set; }

	private readonly double _errorBelow;
	private readonly double _errorAbove;


	public BoxPlot(double position, double[] plotData, double errorBelow, double errorAbove, PlotOptions plotOptions,
		double pValue = 0.05) {
		PlotData = plotData.Length != 0
			? plotData
			: throw new ArgumentException("plotData must be an array that contains elements");
		Position = position;
		UpperPValueQuantile = plotData.Quantile(1 - (pValue / 2));
		LowerPValueQuantile = plotData.Quantile(pValue / 2);
		MaxValue = plotData.Max();
		MinValue = plotData.Min();
		Average = plotData.Average();
		Median = plotData.Median();
		_errorBelow = errorBelow;
		_errorAbove = errorAbove;
		PlotOptions = plotOptions;
	}

	public AxisLimits GetAxisLimits() {
		double minSize = Math.Min(_errorBelow, LowerPValueQuantile);
		double maxSize = Math.Max(_errorAbove, UpperPValueQuantile);

		if (PlotOptions.StartFromZero) {
			minSize = 0.0;
		}

		double startPosition = Position - PlotOptions.BarWidth / 2.0;
		double endPosition = Position + PlotOptions.BarWidth / 2.0;
		return new AxisLimits(startPosition, endPosition, minSize, maxSize);
	}

	public void ValidateData(bool deep = false) {
		Validate.AssertHasElements("PlotData", PlotData);
		if (deep) {
			Validate.AssertAllReal("PlotData", PlotData);
		}
	}

	public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false) {
		using Graphics gfx = Graphics.FromImage(bmp);
		gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
		gfx.TextRenderingHint =
			lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;
		RenderBarVertical(dims, gfx);
	}

	private void RenderBarVertical(PlotDimensions dims, Graphics gfx) {
		// bar body
		float edge = dims.GetPixelX(Position - PlotOptions.BarWidth / 2);
		double valueSpan = UpperPValueQuantile - LowerPValueQuantile;

		var rect = new RectangleF(edge, dims.GetPixelY(UpperPValueQuantile),
			(float)(PlotOptions.BarWidth * dims.PxPerUnitX),
			(float)(valueSpan * dims.PxPerUnitY));

		RenderBarFromRect(rect, gfx);

		if (!(PlotOptions.ErrorLineWidth > 0) || !(_errorAbove > double.Epsilon) || !(_errorBelow > double.Epsilon)) {
			return;
		}

		RenderExtra(dims, gfx);
	}

	private void RenderBarFromRect(RectangleF rect, Graphics gfx) {
		using var outlinePen = new Pen(PlotOptions.BorderColor, PlotOptions.BorderLineWidth);
		using Brush fillBrush = GDI.Brush(PlotOptions.FillColor, PlotOptions.HatchColor,
			PlotOptions.HatchStyle);
		gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);
		if (PlotOptions.BorderLineWidth > 0) {
			gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);
		}
	}

	private void RenderExtra(PlotDimensions dims, Graphics gfx) {
		float centerBottom = dims.GetPixelX(Position);
		// Error Bar
		float errorCapStartX = dims.GetPixelX(Position - PlotOptions.ErrorCapSize * PlotOptions.BarWidth / 2);
		float errorCapEndX = dims.GetPixelX(Position + PlotOptions.ErrorCapSize * PlotOptions.BarWidth / 2);
		float errorCapAboveY = dims.GetPixelY(_errorAbove);
		float errorCapBelowY = dims.GetPixelY(_errorBelow);

		float startX = dims.GetPixelX(Position - PlotOptions.BarWidth / 2);
		float endX = dims.GetPixelX(Position + PlotOptions.BarWidth / 2);

		float averageStartY = dims.GetPixelY(Average);
		float averageEndY = dims.GetPixelY(Average);
		
		float medianStartY = dims.GetPixelY(Median);
		float medianEndY = dims.GetPixelY(Median);

		using var pen = new Pen(PlotOptions.ErrorColor, PlotOptions.ErrorLineWidth);
		gfx.DrawLine(pen, centerBottom, dims.GetPixelY(UpperPValueQuantile), centerBottom, errorCapAboveY);
		gfx.DrawLine(pen, centerBottom, dims.GetPixelY(UpperPValueQuantile), centerBottom, errorCapBelowY);

		gfx.DrawLine(pen, errorCapStartX, errorCapAboveY, errorCapEndX, errorCapAboveY);
		gfx.DrawLine(pen, errorCapStartX, errorCapBelowY, errorCapEndX, errorCapBelowY);

		pen.Width = 2;
		gfx.DrawLine(pen, startX - 5, medianStartY, endX + 5, medianEndY);

		pen.Width = 1;
		pen.DashStyle = DashStyle.Dash;
		gfx.DrawLine(pen, startX, averageStartY, endX, averageEndY);
	}

	public override string ToString() {
		return
			$"BoxPlot{(string.IsNullOrWhiteSpace(PlotOptions.LegendLabel) ? (object)"" : $" ({PlotOptions.LegendLabel})")} with {GetPointCount()} points";
	}

	public int GetPointCount() {
		return PlotData.Length;
	}

	public LegendItem[] GetLegendItems() {
		return new LegendItem[] {
			new() {
				label = PlotOptions.LegendLabel,
				color = PlotOptions.FillColor,
				lineWidth = 10.0,
				markerShape = MarkerShape.none,
				hatchColor = PlotOptions.HatchColor,
				hatchStyle = PlotOptions.HatchStyle,
				borderColor = PlotOptions.BorderColor,
				borderWith = PlotOptions.BorderLineWidth
			}
		};
	}
}