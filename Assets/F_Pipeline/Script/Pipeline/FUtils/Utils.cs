using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public static class Utils
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 ColorToFloat4(this Color color) => new float4(color.r, color.g, color.b, color.a);
}
