using System.Drawing;
using ScottPlot.Drawing;

namespace CsharpRAPL.Plotting;

public class PlotOptions {
	public PlotOptions() { }

	public PlotOptions(PlotOptions plotOptions) {
		Name = plotOptions.Name;
		Height = plotOptions.Height;
		Width = plotOptions.Width;
		FillColor = plotOptions.FillColor;
		HatchColor = plotOptions.HatchColor;
		BorderColor = plotOptions.BorderColor;
		ErrorColor = plotOptions.ErrorColor;
		BorderLineWidth = plotOptions.BorderLineWidth;
		ErrorLineWidth = plotOptions.ErrorLineWidth;
		LegendLabel = plotOptions.LegendLabel;
		ErrorCapSize = plotOptions.ErrorCapSize;
		BarWidth = plotOptions.BarWidth;
		HatchStyle = plotOptions.HatchStyle;
		RotateText = plotOptions.RotateText;
		StartFromZero = plotOptions.StartFromZero;
	}

	public string Name { get; set; } = "";
	public int Width { get; set; } = 600;
	public int Height { get; set; } = 450;
	public Color? FillColor { get; set; }
	public Color HatchColor { get; set; } = Color.Blue;
	public Color BorderColor { get; set; } = Color.Black;
	public Color ErrorColor { get; set; } = Color.Black;
	public float BorderLineWidth { get; set; } = 1f;
	public float ErrorLineWidth { get; set; } = 1f;
	public string LegendLabel { get; set; } = "";
	public double ErrorCapSize { get; set; } = 0.4;
	public double BarWidth { get; set; } = 0.8;
	public HatchStyle HatchStyle { get; set; } = HatchStyle.None;
	public bool RotateText { get; set; } = true;
	public bool StartFromZero { get; set; }
}