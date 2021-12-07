using System.Runtime.CompilerServices;

namespace Benchmarks.HelperObjects.Inheritance;

public class VirtualHelper {
	private int _staticField = 4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int UpdateAndGetValue() {
		_staticField++;
		return _staticField;
	}
}