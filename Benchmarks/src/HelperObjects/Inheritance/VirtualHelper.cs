using System.Runtime.CompilerServices;

namespace Benchmarks.HelperObjects.Inheritance;

public class VirtualHelper {
	private int _field = 4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int UpdateAndGetValue() {
		_field++;
		return _field;
	}
}