using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

public static class HandlesExtended
{

	#region Handes Draw Functions

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

	#endregion

	#region Handles controls Functions

	static ArcHandle				arcHandle = new ArcHandle();
	static BoxBoundsHandle			boxBoundsHandle = new BoxBoundsHandle();
	static CapsuleBoundsHandle		capsuleBoundsHandle = new CapsuleBoundsHandle();
	static JointAngularLimitHandle	jointAngularLimitHandle = new JointAngularLimitHandle();
	static SphereBoundsHandle		sphereBoundsHandle = new SphereBoundsHandle();

	public static void ArcHandle(Vector3 center, Quaternion rotation, Vector3 size, ref float angle, ref float radius) { ArcHandle(center, rotation, size, ref angle, ref radius, arcHandle.fillColor, arcHandle.wireframeColor); }
	public static void ArcHandle(Vector3 center, Quaternion rotation, Vector3 size, ref float angle, ref float radius, Color fillColor, Color wireframeColor)
	{
		Matrix4x4	trs = Matrix4x4.TRS(center, rotation, size);

		using (new Handles.DrawingScope(trs))
		{
			arcHandle.angle = angle;
			arcHandle.radius = radius;
			arcHandle.radiusHandleColor = Color.white;
			arcHandle.fillColor = fillColor;
			arcHandle.wireframeColor = wireframeColor;
			arcHandle.DrawHandle();
			angle = arcHandle.angle;
			radius = arcHandle.radius;
		}
	}

	public static void BoxBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref Vector3 boxSize) { BoxBoundsHandle(center, rotation, size, ref boxSize, PrimitiveBoundsHandle.Axes.All); }
	public static void BoxBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref Vector3 boxSize, PrimitiveBoundsHandle.Axes handleAxes) { BoxBoundsHandle(center, rotation, size, ref boxSize, handleAxes, boxBoundsHandle.wireframeColor, boxBoundsHandle.handleColor); }
	public static void BoxBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref Vector3 boxSize, PrimitiveBoundsHandle.Axes handleAxes, Color wireframeColor, Color handleColor)
	{
		Matrix4x4	trs = Matrix4x4.TRS(center, rotation, size);

		using (new Handles.DrawingScope(trs))
		{
			boxBoundsHandle.axes = handleAxes;
			boxBoundsHandle.size = boxSize;
			boxBoundsHandle.handleColor = handleColor;
			boxBoundsHandle.wireframeColor = wireframeColor;
			boxBoundsHandle.DrawHandle();
			boxSize = boxBoundsHandle.size;
		}
	}

	public static void CapsuleBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref float height, ref float radius) { CapsuleBoundsHandle(center, rotation, size, ref height, ref radius, capsuleBoundsHandle.heightAxis, PrimitiveBoundsHandle.Axes.All); }
	public static void CapsuleBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref float height, ref float radius,CapsuleBoundsHandle.HeightAxis heightAxis, PrimitiveBoundsHandle.Axes handleAxes) { CapsuleBoundsHandle(center, rotation, size, ref height, ref radius, heightAxis, handleAxes, capsuleBoundsHandle.handleColor, capsuleBoundsHandle.wireframeColor); }
	public static void CapsuleBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref float height, ref float radius, CapsuleBoundsHandle.HeightAxis heightAxis, PrimitiveBoundsHandle.Axes handleAxes, Color handleColor, Color wireframeColor)
	{
		Matrix4x4	trs = Matrix4x4.TRS(center, rotation, size);

		using (new Handles.DrawingScope(trs))
		{
			capsuleBoundsHandle.heightAxis = heightAxis;
			capsuleBoundsHandle.radius = radius;
			capsuleBoundsHandle.height = height;
			capsuleBoundsHandle.handleColor = handleColor;
			capsuleBoundsHandle.wireframeColor = wireframeColor;
			capsuleBoundsHandle.DrawHandle();
			radius = capsuleBoundsHandle.radius;
			height = capsuleBoundsHandle.height;
		}
	}

	public static void JointAngularLimitHandle(Vector3 center, Quaternion rotation, Vector3 size, ref Vector3 minAngles, ref Vector3 maxAngles) { JointAngularLimitHandle(center, rotation, size, ref minAngles, ref maxAngles, jointAngularLimitHandle.xHandleColor, jointAngularLimitHandle.yHandleColor, jointAngularLimitHandle.zHandleColor); }
	public static void JointAngularLimitHandle(Vector3 center, Quaternion rotation, Vector3 size, ref Vector3 minAngles, ref Vector3 maxAngles, Color xHandleColor, Color yHandleColor, Color zHandleColor)
	{
		Matrix4x4	trs = Matrix4x4.TRS(center, rotation, size);

		using (new Handles.DrawingScope(trs))
		{
			jointAngularLimitHandle.xHandleColor = xHandleColor;
			jointAngularLimitHandle.yHandleColor = yHandleColor;
			jointAngularLimitHandle.zHandleColor = zHandleColor;

			jointAngularLimitHandle.xMin = minAngles.x;
			jointAngularLimitHandle.yMin = minAngles.y;
			jointAngularLimitHandle.zMin = minAngles.z;
			jointAngularLimitHandle.xMax = maxAngles.x;
			jointAngularLimitHandle.yMax = maxAngles.y;
			jointAngularLimitHandle.zMax = maxAngles.z;

			jointAngularLimitHandle.DrawHandle();
			
			minAngles.x = jointAngularLimitHandle.xMin;
			minAngles.y = jointAngularLimitHandle.yMin;
			minAngles.z = jointAngularLimitHandle.zMin;
			maxAngles.x = jointAngularLimitHandle.xMax;
			maxAngles.y = jointAngularLimitHandle.yMax;
			maxAngles.z = jointAngularLimitHandle.zMax;
		}
	}

	public static void SphereBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref float radius) { SphereBoundsHandle(center, rotation, size, ref radius, PrimitiveBoundsHandle.Axes.All); }
	public static void SphereBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref float radius, PrimitiveBoundsHandle.Axes handleAxes) { SphereBoundsHandle(center, rotation, size, ref radius, handleAxes, sphereBoundsHandle.handleColor, sphereBoundsHandle.wireframeColor); }
	public static void SphereBoundsHandle(Vector3 center, Quaternion rotation, Vector3 size, ref float radius, PrimitiveBoundsHandle.Axes handleAxes, Color handleColor, Color wireframeColor)
	{
		Matrix4x4	trs = Matrix4x4.TRS(center, rotation, size);

		using (new Handles.DrawingScope(trs))
		{
			sphereBoundsHandle.radius = radius;

			sphereBoundsHandle.axes = handleAxes;
			sphereBoundsHandle.wireframeColor = wireframeColor;
			sphereBoundsHandle.handleColor = handleColor;

			sphereBoundsHandle.DrawHandle();

			radius = sphereBoundsHandle.radius;
		}
	}

	#endregion

	#region Full custom Handles

	public static void CurveHandle(Vector3 startPoint, Vector3 EndPoint, Gradient curveGradient, float yScale = 1)
	{
		
	}

	#endregion

}
