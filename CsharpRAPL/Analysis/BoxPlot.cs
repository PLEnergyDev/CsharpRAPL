using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using Accord.Statistics;
using ScottPlot;
using ScottPlot.Config;
using ScottPlot.Drawing;
using HatchStyle = ScottPlot.Drawing.HatchStyle;

namespace CsharpRAPL.Analysis;

public class BoxPlot : Plottable, IPlottable {
	public readonly double Position;
	public readonly double[] PlotData;
	public readonly double MaxValue;
	public readonly double MinValue;
	public readonly double UpperQuartile;
	public readonly double LowerQuartile;

	public Color FillColor { get; set; } = Color.Green;
	public Color HatchColor { get; set; } = Color.Blue;
	public Color BorderColor { get; set; } = Color.Black;
	public Color ErrorColor { get; set; } = Color.Black;
	public float BorderLineWidth { get; set; } = 1f;
	public float ErrorLineWidth { get; set; } = 1f;
	public string LegendLabel { get; set; } = "";
	public double ErrorCapSize { get; set; } = 0.4;
	public double BarWidth { get; set; } = 0.8;
	public HatchStyle HatchStyle { get; set; } = HatchStyle.None;

	public bool UseMinSize { get; init; } = true;

	private readonly double _errorBelow;
	private readonly double _errorAbove;


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
	}

	public override AxisLimits2D GetLimits() {
		double minSize = Math.Min(_errorBelow, LowerQuartile);
		double maxSize = Math.Max(_errorAbove, UpperQuartile);

		if (UseMinSize) {
			minSize = 0.0;
		}

		double startPosition = Position - BarWidth / 2.0;
		double endPosition = Position + BarWidth / 2.0;
		return new AxisLimits2D(startPosition, endPosition, minSize, maxSize);
	}

	public string? ValidationErrorMessage { get; private set; }

	public bool IsValidData(bool deepValidation = false) {
		try {
			Validate.AssertHasElements("PlotData", PlotData);
			if (deepValidation) {
				Validate.AssertAllReal("PlotData", PlotData);
			}
		}
		catch (ArgumentException ex) {
			ValidationErrorMessage = ex.Message;
			return false;
		}

		ValidationErrorMessage = null;
		return true;
	}

	public override void Render(Settings settings) => throw new InvalidOperationException("Use new Render() method");

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
		double edge1 = position - BarWidth / 2;
		double valueSpan = UpperQuartile - LowerQuartile;

		var rect = new RectangleF(dims.GetPixelX(edge1),
			dims.GetPixelY(UpperQuartile),
			(float)(BarWidth * dims.PxPerUnitX),
			(float)(valueSpan * dims.PxPerUnitY));

		// errorbar
		float errorCapStartX = dims.GetPixelX(position - ErrorCapSize * BarWidth / 2);
		float errorCapEndX = dims.GetPixelX(position + ErrorCapSize * BarWidth / 2);
		float errorCapAboveY = dims.GetPixelY(_errorAbove);
		float errorCapBelowY = dims.GetPixelY(_errorBelow);

		RenderBarFromRect(rect, gfx);

		if (!(ErrorLineWidth > 0) || !(_errorAbove > double.Epsilon) || !(_errorBelow > double.Epsilon)) return;

		using var errorPen = new Pen(ErrorColor, ErrorLineWidth);
		gfx.DrawLine(errorPen, centerPx, dims.GetPixelY(UpperQuartile), centerPx, errorCapAboveY);
		gfx.DrawLine(errorPen, centerPx, dims.GetPixelY(UpperQuartile), centerPx, errorCapBelowY);

		gfx.DrawLine(errorPen, errorCapStartX, errorCapAboveY, errorCapEndX, errorCapAboveY);
		gfx.DrawLine(errorPen, errorCapStartX, errorCapBelowY, errorCapEndX, errorCapBelowY);
	}

	private void RenderBarFromRect(RectangleF rect, Graphics gfx) {
		using var outlinePen = new Pen(BorderColor, BorderLineWidth);
		using Brush fillBrush = GDI.Brush(FillColor, HatchColor, HatchStyle);
		gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);
		if (BorderLineWidth > 0) {
			gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);
		}
	}

	public override string ToString() =>
		$"BoxPlot{(string.IsNullOrWhiteSpace(LegendLabel) ? (object)"" : " (" + LegendLabel + ")")} with {GetPointCount()} points";

	public override int GetPointCount() => PlotData.Length;

	public override LegendItem[] GetLegendItems() => new LegendItem[] {
		new() {
			label = LegendLabel,
			color = FillColor,
			lineWidth = 10.0,
			markerShape = MarkerShape.none,
			hatchColor = HatchColor,
			hatchStyle = HatchStyle,
			borderColor = BorderColor,
			borderWith = BorderLineWidth
		}
	};
}