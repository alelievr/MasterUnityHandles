using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditor.IMGUI.Controls;
using BetterHandles;

public class HandlesExampleWindow : EditorWindow
{
	[NonSerialized]
	bool			focus = false;
	[SerializeField]
	string			currentKey = null;

	static float			angle = 42, radius = 2, capsRadius = .5f, height = 2;
	static Vector3			boxSize = new Vector3(2, 1, 2);
	static Vector3			minAngles = new Vector3(0, 0, 0), maxAngles = new Vector3(45, 45, 45);
	static AnimationCurve	curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(.5f, .7f), new Keyframe(1, 0));
	static Keyframe			keyframe = new Keyframe(0, 0);
	static Vector2			pos;
	static Vector2			point1, point2, point3, point4, point5, point6, point7;
	new static Vector3		position;

	static Texture2D		normaltexture, selectedTexture;
	// static AnimationCurve	curve = new AnimationCurve(new Keyframe(0, 1));

	static Mesh				cylinderMesh;

	static DrawPerformances	perfsTest = new DrawPerformances();

	Dictionary< string, Action >	handlesActions = new Dictionary< string, Action >()
	{
		{"Simple custom Handles (using existing Handles)", null},
		{"Solid scaled cube", () => HandlesExtended.DrawSolidCube(Vector3.zero, Quaternion.identity, new Vector3(5, 1, 3), new Color(1f, 0, .2f, .2f))},
		{"Wire rotated cube", () => HandlesExtended.DrawWireCube(Vector3.zero, Quaternion.Euler(45, 45, 0), new Vector3(2, 1, 2), new Color(1f, 0, .2f, 1f))},
		{"Solid scaled cylinder", () => HandlesExtended.DrawCylinder(Vector3.zero, Quaternion.Euler(90, 0, 90), new Vector3(1, 3, 2), new Color(0, 1, 0, .3f))},
		{"Solid scaled cone", () => HandlesExtended.DrawCone(Vector3.zero, Quaternion.Euler(-90, 0, 0), new Vector3(1, 3, 2), new Color(0, 0, 1, .3f))},
		{"Solid scaled circle", () => HandlesExtended.DrawCircle(Vector3.zero, Quaternion.Euler(45, 0, 45), new Vector3(1, 3, 2), new Color(0, 1, 0, 1f))},
		{"Solid scaled arrow", () => HandlesExtended.DrawArrow(Vector3.zero, Quaternion.Euler(-90, 0, 0), new Vector3(1, 3, 1), new Color(0, 0, 1, .3f))},
		{"Solid scaled rectangle", () => HandlesExtended.DrawRectange(Vector3.zero, Quaternion.Euler(90, 0, 90), new Vector3(1, 3, 2), new Color(0, 0, 1, 1f))},
		{"Solid scaled sphere", () => HandlesExtended.DrawSphere(Vector3.zero, Quaternion.Euler(90, 0, 90), new Vector3(1, 3, 2), new Color(0, 0, 1, .3f))},
		{"Full custom Handles", null},
		{"Keyframe Handle", () => HandlesExtended.KeyframeHandle(3, 2, ref keyframe, Vector3.zero, Quaternion.Euler(45, 45, 0), Color.white, Color.yellow)},
		{"Curve Handle", () => HandlesExtended.CurveHandle(3, 2, curve, Vector3.one, Quaternion.Euler(0, 0, 0), new Color(1, 0, 0, .6f), new Color(0, 0, 1, .6f))},
		{"2D Move Handle", () => HandlesExtended.Free2DMoveHandle(ref point1, .1f, Quaternion.Euler(45, 0, 45), new Color(1, 0, 0, .3f), new Color(0, 0, 1, .3f))},
		{"2D Textured Move Handle", () => {
			HandlesExtended.Free2DMoveHandle(ref point2, .1f, Quaternion.identity, normaltexture , selectedTexture, selectedTexture);
			HandlesExtended.Free2DMoveHandle(ref point3, .1f, Quaternion.identity, normaltexture , selectedTexture, selectedTexture);
			HandlesExtended.Free2DMoveHandle(ref point4, .1f, Quaternion.identity, normaltexture , selectedTexture, selectedTexture);
			HandlesExtended.Free2DMoveHandle(ref point5, .1f, Quaternion.identity, normaltexture , selectedTexture, selectedTexture);
			HandlesExtended.Free2DMoveHandle(ref point6, .1f, Quaternion.identity, normaltexture , selectedTexture, selectedTexture);
			HandlesExtended.Free2DMoveHandle(ref point7, .1f, Quaternion.identity, normaltexture , selectedTexture, selectedTexture);
		}},
		{"IMGUI Handles", null},
		{"Arc Handle", () => HandlesExtended.ArcHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref angle, ref radius, new Color(1, 0, 0, .1f), new Color(0, 0, 1, 1f))},
		{"Box Bounds Handle", () => HandlesExtended.BoxBoundsHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref boxSize, PrimitiveBoundsHandle.Axes.All, new Color(1, 0, 0, 1f), new Color(0, 0, 1, 1f))},
		{"Capsule Bounds Handle", () => HandlesExtended.CapsuleBoundsHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref height, ref capsRadius)},
		{"Joint Angular Limit Handle", () => HandlesExtended.JointAngularLimitHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref minAngles, ref maxAngles)},
		{"Sphere Bounds Handle", () => HandlesExtended.SphereBoundsHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref radius)},
		{"Others", null},
		{"Perfs ('Mesh draw' and 'GL draw' in profiler)", () => perfsTest.Test()},
		{"Cylinder cap function", () => position = Handles.FreeMoveHandle(position, Quaternion.identity, 1f, Vector3.zero, CylinderHandleCap)}
	};

	Vector2 scrollbar;

	[MenuItem("Window/Handles Examples")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow< HandlesExampleWindow >().Show();
	}

	void OnEnable()
	{
		SceneView.onSceneGUIDelegate += OnSceneGUI;
		if (currentKey == null || !handlesActions.ContainsKey(currentKey))
			currentKey = handlesActions.FirstOrDefault(k => k.Value != null).Key;
		
		normaltexture = Resources.Load< Texture2D >("normal");
		selectedTexture = Resources.Load< Texture2D >("selected");

		cylinderMesh = Resources.Load< Mesh >("cylinder");

		perfsTest.Init();
	}

	void OnGUI()
	{
		scrollbar = EditorGUILayout.BeginScrollView(scrollbar);
		if (GUILayout.Button("Focus preview"))
		{
			focus = true;
			SceneView.RepaintAll();
		}

		EditorGUILayout.Space();

		EditorGUILayout.BeginVertical(new GUIStyle("box"));
		{
			foreach (var kp in handlesActions)
			{
				if (kp.Value == null)
				{
					EditorGUILayout.LabelField(kp.Key, EditorStyles.boldLabel);
					continue ;
				}

				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.LabelField(kp.Key);
					if (GUILayout.Button("Show"))
					{
						currentKey = kp.Key;
						SceneView.RepaintAll();
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			if (GUILayout.Button("Hide"))
				currentKey = "";
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical(new GUIStyle("box"));
		{
			EditorGUILayout.CurveField(curve);
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.EndScrollView();
	}

	void OnSceneGUI(SceneView sv)
	{
		//draw the current shown handles
		if (handlesActions.ContainsKey(currentKey) && handlesActions[currentKey] != null)
			handlesActions[currentKey]();

		if (focus)
		{
			sv.LookAt(Vector3.zero, Quaternion.Euler(45, -45, 0), 8);
			focus = false;
		}
	}

	void OnDisable()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	#region Cap functions

	static void CylinderHandleCap(int controlId, Vector3 position, Quaternion rotation, float size, EventType eventType)
	{
		if (eventType == EventType.Repaint)
		{
			HandlesMaterials.vertexColor.SetPass(0);
			Graphics.DrawMeshNow(cylinderMesh, position, Quaternion.identity);
		}
		else if (eventType == EventType.Layout)
		{
			float cylinderHeight = 3.5f;
			Vector3 startPosition = position + Vector3.up * (cylinderHeight / 2);
			float distance = 1e20f;

			for (int i = 0; i < 9; i++)
			{
				distance = Mathf.Min(distance, HandleUtility.DistanceToCircle(startPosition, size / 2));
				startPosition -= Vector3.up * cylinderHeight / 8;
			}
			
			HandleUtility.AddControl(controlId, distance);
		}
	}

	#endregion
}
