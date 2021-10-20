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
		bool useMinSize = true) {
		var boxPlot = new BoxPlot(position, data, errorBelow, errorAbove) {
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
			UseMinSize = useMinSize
		};

		plot.Add(boxPlot);
		
		if (!autoAxis) return boxPlot;
		
		plot.AxisAuto(0.0, 0.0);
		double yValue;
		double[] numArray = plot.Axis(0, 0, 0, 0);
		plot.AxisAuto();

		if (numArray[2] == 0.0) {
			yValue = 0.0;
			plot.Axis(y1: yValue);
		}
		else if (numArray[3] == 0.0) {
			yValue = 0.0;
			plot.Axis(y2: yValue);
		}

		return boxPlot;
	}
}