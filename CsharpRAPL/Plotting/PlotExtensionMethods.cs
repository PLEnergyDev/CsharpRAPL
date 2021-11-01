using System.Drawing;
using ScottPlot;
using ScottPlot.Drawing;

namespace CsharpRAPL.Plotting;

public static class PlotExtensionMethods {
	public static BoxPlot PlotBoxPlot(this Plot plot,
		double position,
		double[] data,
		double errorBelow,
		double errorAbove,
		string label = "",
		double barWidth = 0.8,
		Color? fillColor = null,
		double outlineWidth = 1.0,
		Color? outlineColor = null,
		double errorLineWidth = 1.0,
		double errorCapSize = 0.38,
		Color? errorColor = null,
		bool autoAxis = true,
		HatchStyle hatchStyle = HatchStyle.None,
		Color? hatchColor = null,
		bool useMinSize = true,
		bool rotateText = true) {
		var boxPlot = new BoxPlot(position, data, errorBelow, errorAbove) {
			PlotOptions = new PlotOptions {
				LegendLabel = label,
				BarWidth = barWidth,
				FillColor = fillColor ?? plot.GetSettings().GetNextColor(),
				ErrorLineWidth = (float)errorLineWidth,
				ErrorCapSize = errorCapSize,
				ErrorColor = errorColor ?? Color.Black,
				BorderLineWidth = (float)outlineWidth,
				BorderColor = outlineColor ?? Color.Black,
				HatchStyle = hatchStyle,
				HatchColor = hatchColor ?? Color.Gray,
				UseMinSize = useMinSize,
				RotateText = rotateText
			}
		};

		plot.Add(boxPlot);

		if (!autoAxis) {
			return boxPlot;
		}

		plot.AxisAuto();
		return boxPlot;
	}
}