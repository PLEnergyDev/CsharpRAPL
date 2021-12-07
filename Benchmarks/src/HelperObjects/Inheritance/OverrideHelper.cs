using System.Runtime.CompilerServices;

namespace Benchmarks.HelperObjects.Inheritance;

public class OverrideHelper : VirtualHelper {
	private int _field = 4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int UpdateAndGetValue() {
		_field++;
		return _field;
	}
}