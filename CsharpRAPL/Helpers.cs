using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CsharpRAPL;

public static class Helpers {
	[Pure]
	public static IEnumerable<(int index, TSource value)> WithIndex<TSource>(this IEnumerable<TSource> enumerable) {
		return enumerable.Select((value, index) => (index, value));
	}
}