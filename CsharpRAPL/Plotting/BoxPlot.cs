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
	public double UpperQuartile { get; }
	public double LowerQuartile { get; }

	public bool IsVisible { get; set; } = true;
	public int XAxisIndex { get; set; }
	public int YAxisIndex { get; set; }

	public PlotOptions PlotOptions;
	private double _errorBelow;
	private double _errorAbove;


	public BoxPlot(double position, double[] plotData, double errorBelow, double errorAbove) {
		PlotData = plotData.Length != 0
			? plotData
			: throw new ArgumentException("plotData must be an array that contains elements");
		Position = position;
		UpperQuartile = plotData.UpperQuartile();
		LowerQuartile = plotData.LowerQuartile();
		MaxValue = plotData.Max();
		MinValue = plotData.Min();
		_errorBelow = errorBelow;
		_errorAbove = errorAbove;
		PlotOptions = new PlotOptions();
	}

	public AxisLimits GetAxisLimits() {
		double minSize = Math.Min(_errorBelow, LowerQuartile);
		double maxSize = Math.Max(_errorAbove, UpperQuartile);

		if (PlotOptions.UseMinSize) {
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
		RenderBarVertical(dims, gfx, Position);
	}

	private void RenderBarVertical(PlotDimensions dims, Graphics gfx, double position) {
		// bar body
		float centerPx = dims.GetPixelX(position);
		double edge1 = position - PlotOptions.BarWidth / 2;
		double valueSpan = UpperQuartile - LowerQuartile;

		var rect = new RectangleF(dims.GetPixelX(edge1),
			dims.GetPixelY(UpperQuartile),
			(float)(PlotOptions.BarWidth * dims.PxPerUnitX),
			(float)(valueSpan * dims.PxPerUnitY));

		// errorbar
		float errorCapStartX = dims.GetPixelX(position - PlotOptions.ErrorCapSize * PlotOptions.BarWidth / 2);
		float errorCapEndX = dims.GetPixelX(position + PlotOptions.ErrorCapSize * PlotOptions.BarWidth / 2);
		float errorCapAboveY = dims.GetPixelY(_errorAbove);
		float errorCapBelowY = dims.GetPixelY(_errorBelow);

		RenderBarFromRect(rect, gfx);

		if (!(PlotOptions.ErrorLineWidth > 0) || !(_errorAbove > double.Epsilon) || !(_errorBelow > double.Epsilon)) {
			return;
		}

		using var errorPen = new Pen(PlotOptions.ErrorColor, PlotOptions.ErrorLineWidth);
		gfx.DrawLine(errorPen, centerPx, dims.GetPixelY(UpperQuartile), centerPx, errorCapAboveY);
		gfx.DrawLine(errorPen, centerPx, dims.GetPixelY(UpperQuartile), centerPx, errorCapBelowY);

		gfx.DrawLine(errorPen, errorCapStartX, errorCapAboveY, errorCapEndX, errorCapAboveY);
		gfx.DrawLine(errorPen, errorCapStartX, errorCapBelowY, errorCapEndX, errorCapBelowY);
	}

	private void RenderBarFromRect(RectangleF rect, Graphics gfx) {
		using var outlinePen = new Pen(PlotOptions.BorderColor, PlotOptions.BorderLineWidth);
		using Brush fillBrush = GDI.Brush(PlotOptions.FillColor, PlotOptions.HatchColor, PlotOptions.HatchStyle);
		gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);
		if (PlotOptions.BorderLineWidth > 0) {
			gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);
		}
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