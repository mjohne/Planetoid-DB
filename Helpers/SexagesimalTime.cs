using System.Diagnostics;
using System.Numerics;

namespace Planetoid_DB.Helpers;

[DebuggerDisplay($"{{{nameof(DebuggerDisplay)}(),nq}}")]
public record struct SexagesimalTime<T>(int Hours, int Minutes, T Seconds) where T : IFloatingPoint<T>
{
	private readonly string DebuggerDisplay => ToString();
}
