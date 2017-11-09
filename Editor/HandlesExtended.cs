using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class HandlesExtended
{
	public static void DrawScaledCap(Handles.CapFunction capFunction, Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		Handles.color = color;

		Matrix4x4 scaleMatrix = Matrix4x4.Scale(size);
	
		using (new Handles.DrawingScope(scaleMatrix))
		{
			capFunction(0, center, rotation, 1, EventType.Repaint);
		}
	}

	public static void DrawWireCube(Vector3 center, Quaternion rotation, Vector3 size) { DrawWireCube(center, rotation, size, Handles.color); }
	public static void DrawWireCube(Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);

		Handles.color = color;

		using (new Handles.DrawingScope(rotationMatrix))
		{
			Handles.DrawWireCube(center, size);
		}
	}

	public static void DrawSolidCube(Vector3 center, Quaternion rotation, Vector3 size) { DrawSolidCube(center, rotation, size, Handles.color); }
	public static void DrawSolidCube(Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		DrawScaledCap(Handles.CubeHandleCap, center, rotation, size, color);
	}

	public static void DrawCylinder(Vector3 center, Quaternion rotation, Vector3 size) { DrawCylinder(center, rotation, size, Handles.color); }
	public static void DrawCylinder(Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		DrawScaledCap(Handles.CylinderHandleCap, center, rotation, size, color);
	}

	public static void DrawCone(Vector3 center, Quaternion rotation, Vector3 size) { DrawCone(center, rotation, size, Handles.color); }
	public static void DrawCone(Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		DrawScaledCap(Handles.ConeHandleCap, center, rotation, size, color);
	}
	
	public static void DrawCircle(Vector3 center, Quaternion rotation, Vector3 size) { DrawCircle(center, rotation, size, Handles.color); }
	public static void DrawCircle(Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		DrawScaledCap(Handles.CircleHandleCap, center, rotation, size, color);
	}
	
	public static void DrawArrow(Vector3 center, Quaternion rotation, Vector3 size) { DrawArrow(center, rotation, size, Handles.color); }
	public static void DrawArrow(Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		DrawScaledCap(Handles.ArrowHandleCap, center, rotation, size, color);
	}
	
	public static void DrawRectange(Vector3 center, Quaternion rotation, Vector3 size) { DrawRectange(center, rotation, size, Handles.color); }
	public static void DrawRectange(Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		DrawScaledCap(Handles.RectangleHandleCap, center, rotation, size, color);
	}
	
	public static void DrawSphere(Vector3 center, Quaternion rotation, Vector3 size) { DrawSphere(center, rotation, size, Handles.color); }
	public static void DrawSphere(Vector3 center, Quaternion rotation, Vector3 size, Color color)
	{
		DrawScaledCap(Handles.SphereHandleCap, center, rotation, size, color);
	}

}
