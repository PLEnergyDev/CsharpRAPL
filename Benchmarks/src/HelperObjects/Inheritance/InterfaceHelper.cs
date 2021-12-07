using System.Runtime.CompilerServices;

namespace Benchmarks.HelperObjects.Inheritance;

public class InterfaceHelper : IDoable {
	private int _field = 4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int UpdateAndGetValue() {
		_field++;
		return _field;
	}
}