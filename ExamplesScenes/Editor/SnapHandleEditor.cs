using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BetterHandles;

[InitializeOnLoad]
public class SnapHandleEditor
{
	static float	snapHandleDistance = 3.5f;
	static float	snapHandleSize = .15f;
	static float 	shiftSnapUnit = .1f;
	static float 	commandSnapUnit = .25f;

	static Mesh			snapToolMesh;
	static Transform	transform;
	static Material		handleMat;

	static Quaternion	currentRotation;
	static Color		currentColor;

	static SnapHandleEditor()
	{
		snapToolMesh = Resources.Load< Mesh >("snapToolHandle");
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	static void OnSceneGUI(SceneView sv)
	{
		if (Tools.current != Tool.Move)
			return ;

		if (Selection.activeGameObject == null)
			return ;

		transform = Selection.activeGameObject.transform;
		
		//x axis
		DrawAxisHandle(Quaternion.Euler(0, 0, 90), transform.right, Handles.xAxisColor);
		//y axis
		DrawAxisHandle(Quaternion.Euler(0, 0, 0), transform.up, Handles.yAxisColor);
		//z axis
		DrawAxisHandle(Quaternion.Euler(90, 0, 0), transform.forward, Handles.zAxisColor);
	}

	static void DrawAxisHandle(Quaternion rotation, Vector3 direction, Color color)
	{
		var e = Event.current;

		float snapUnit = 0;
		float size = HandleUtility.GetHandleSize(transform.position) * snapHandleSize;
		float dist = size * snapHandleDistance;

		if (EditorGUI.actionKey)
			snapUnit = (e.shift) ? shiftSnapUnit : commandSnapUnit;

		currentRotation = rotation;
		currentColor = color;

		Vector3 addPos = direction * dist;
		Vector3 newPosition = Handles.Slider(transform.position + addPos, direction, size, SnapHandleCap, snapUnit) - addPos;

		transform.position = newPosition;
	}

	static void SnapHandleCap(int controlId, Vector3 position, Quaternion rotation, float size, EventType eventType)
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
}
