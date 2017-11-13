using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CustomHandleUtility
{
	public static Vector3	GetPointOnPlane(Matrix4x4 planeTransform, Ray ray)
	{
		float	dist;
		Vector3	ret;
		Plane	p = new Plane(planeTransform * Vector3.forward, planeTransform * Vector3.zero);

		p.Raycast(ray, out dist);

		ret = ray.GetPoint(dist);
		
		return ret;
	}
}
