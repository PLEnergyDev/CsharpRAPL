using System.Runtime.CompilerServices;

namespace Benchmarks.HelperObjects.Inheritance;

public class ClassHelper {
	private ulong _field = 4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ulong UpdateAndGetValue() {
		_field++;
		return _field;
	}
}