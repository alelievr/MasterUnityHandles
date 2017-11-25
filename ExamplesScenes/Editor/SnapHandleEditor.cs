using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BetterHandles;

[CustomEditor(typeof(Transform), true)]
public class SnapHandleEditor : Editor
{
	const float	snapHandleDistance = 3.5f;
	const float	snapHandleSize = .15f;
	const float defaultSnapUnit = .1f;
	const float shiftSnapUnit = .5f;
	const float commandSnapUnit = .05f;

	Mesh		snapToolMesh;
	Transform	transform;
	Material	handleMat;
	
	Quaternion	currentRotation;
	Color		currentColor;

	Editor		defaultTransformEditor;

	void OnEnable()
	{
		snapToolMesh = Resources.Load< Mesh >("snapToolHandle");
		Debug.Log("target: " + target + ", type: " + target.GetType());
		transform = target as Transform;
	}

	void OnSceneGUI()
	{
		// if (Tools.current != Tool.Move)
		// 	return ;
		
		//x axis
		DrawAxisHandle(Quaternion.Euler(0, 0, 90), transform.right, Handles.xAxisColor, 0);
		//y axis
		DrawAxisHandle(Quaternion.Euler(0, 0, 0), transform.up, Handles.yAxisColor, 1);
		//z axis
		DrawAxisHandle(Quaternion.Euler(90, 0, 0), transform.forward, Handles.zAxisColor, 2);
	}

	void DrawAxisHandle(Quaternion rotation, Vector3 direction, Color color, int index)
	{
		var e = Event.current;

		float snapUnit = (e.shift) ? shiftSnapUnit : (e.command) ? commandSnapUnit : defaultSnapUnit;
		float size = HandleUtility.GetHandleSize(transform.position) * snapHandleSize;
		float dist = size * snapHandleDistance;

		currentRotation = rotation;
		currentColor = color;

		Vector3 addPos = direction * dist;
		Vector3 newPosition = Handles.Slider(transform.position + addPos, direction, size, SnapHandleCap, 0) - addPos;

		if (newPosition != transform.position)
		{
			//handmade snapping selected axis
			newPosition[index] = Mathf.Round(newPosition[index] / snapUnit) * snapUnit;

			transform.position = newPosition;
		}
	}

	void SnapHandleCap(int controlId, Vector3 position, Quaternion rotation, float size, EventType eventType)
	{
		if (eventType == EventType.Repaint)
		{
			//we set the material color or preselected color if mouse is near from our handle
			Color color = (HandleUtility.nearestControl == controlId || GUIUtility.hotControl == controlId) ? Handles.preselectionColor : currentColor;
			HandlesMaterials.overlayColor.SetColor("_Color", color);

			//we draw the cylinder with overlay material
			HandlesMaterials.overlayColor.SetPass(0);
			Matrix4x4 trs = Matrix4x4.TRS(position, transform.rotation * currentRotation, Vector3.one * size);
			Graphics.DrawMeshNow(snapToolMesh, trs);
		}
		else if (eventType == EventType.Layout)
		{
			float distance = HandleUtility.DistanceToCircle(position, size);
			HandleUtility.AddControl(controlId, distance);
		}
	}

	public override void OnInspectorGUI()
	{
		if (defaultTransformEditor == null)
			defaultTransformEditor = Editor.CreateEditor(transform);
		
		Debug.Log("deafultTransformEditor: " + defaultTransformEditor);
		if (defaultTransformEditor.GetType() != typeof(SnapHandleEditor))
				defaultTransformEditor.OnInspectorGUI();
	}
}
