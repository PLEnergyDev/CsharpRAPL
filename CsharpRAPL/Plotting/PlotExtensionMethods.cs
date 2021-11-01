using ScottPlot;

namespace CsharpRAPL.Plotting;

public static class PlotExtensionMethods {
	public static BoxPlot PlotBoxPlot(this Plot plot,
		double position,
		double[] data,
		double errorBelow,
		double errorAbove,
		PlotOptions? plotOptions = null,
		bool autoAxis = true) {
		var boxPlot = new BoxPlot(position, data, errorBelow, errorAbove, plotOptions ?? new PlotOptions());

		plot.Add(boxPlot);
		if (!autoAxis) {
			return boxPlot;
		}

		plot.AxisAuto();
		return boxPlot;
	}
}