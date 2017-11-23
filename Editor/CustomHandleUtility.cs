using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CustomHandleUtility
{
	public static bool	GetPointOnPlane(Matrix4x4 planeTransform, Ray ray, out Vector3 position)
	{
		float	dist;
		position = Vector3.zero;
		Plane	p = new Plane(planeTransform * Vector3.forward, planeTransform.MultiplyPoint3x4(position));

		p.Raycast(ray, out dist);

		if (dist < 0)
			return false;

		position = ray.GetPoint(dist);
		
		return true;
	}
}
