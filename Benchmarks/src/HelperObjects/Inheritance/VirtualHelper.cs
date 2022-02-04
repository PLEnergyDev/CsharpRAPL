using System.Runtime.CompilerServices;

namespace Benchmarks.HelperObjects.Inheritance;

public class VirtualHelper {
	private ulong _field = 4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual ulong UpdateAndGetValue() {
		_field++;
		return _field;
	}
}