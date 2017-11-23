using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveScriptableObject : ScriptableObject
{
	public AnimationCurve   curve = new AnimationCurve();
    public Gradient         curveGradient = new Gradient();
    public Vector2          curveSize = new Vector2(5, 3);
    public int              sampleCount = 100;
}
