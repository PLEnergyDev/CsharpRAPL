using ScottPlot;

namespace CsharpRAPL.Plotting;

public static class PlotExtensionMethods {
	public static BoxPlot AddBoxPlot(this Plot plot,
		double position,
		double[] data,
		double errorBelow,
		double errorAbove,
		PlotOptions? plotOptions = null,
		bool autoAxis = true) {
		//Note this makes a copy of plot options so we don't change it for everyone.
		PlotOptions plotOpts = plotOptions != null ? new PlotOptions(plotOptions) : new PlotOptions();

		var boxPlot = new BoxPlot(position, data, errorBelow, errorAbove, plotOpts);

		plot.Add(boxPlot);
		if (!autoAxis) {
			return boxPlot;
		}

		plot.AxisAuto();
		return boxPlot;
	}
}