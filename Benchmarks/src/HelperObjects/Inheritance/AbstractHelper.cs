using System.Runtime.CompilerServices;

namespace Benchmarks.HelperObjects.Inheritance;

public class AbstractHelper : ADoable {
	private ulong _field = 4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override ulong UpdateAndGetValue() {
		_field++;
		return _field;
	}
}