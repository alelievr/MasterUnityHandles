using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyframeExtension
{
	public static bool Equal(this Keyframe k1, Keyframe k2)
	{
		return (k1.time == k2.time && k1.value == k2.value && k1.inTangent == k2.inTangent && k1.outTangent == k2.outTangent);
	}
}
