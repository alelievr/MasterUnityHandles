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
		{"Curve Handle", () => HandlesExtended.CurveHandle(3, 2, curve, Quaternion.identity, new Color(1, 0, 0, .3f), new Color(0, 0, 1, .3f))},
		{"IMGUI Handles", null},
		{"Arc Handle", () => HandlesExtended.ArcHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref angle, ref radius, new Color(1, 0, 0, .1f), new Color(0, 0, 1, 1f))},
		{"Box Bounds Handle", () => HandlesExtended.BoxBoundsHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref boxSize, PrimitiveBoundsHandle.Axes.All, new Color(1, 0, 0, 1f), new Color(0, 0, 1, 1f))},
		{"Capsule Bounds Handle", () => HandlesExtended.CapsuleBoundsHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref height, ref capsRadius)},
		{"Joint Angular Limit Handle", () => HandlesExtended.JointAngularLimitHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref minAngles, ref maxAngles)},
		{"Sphere Bounds Handle", () => HandlesExtended.SphereBoundsHandle(Vector3.zero, Quaternion.identity, Vector3.one, ref radius)},
	};

	[MenuItem("Window/Handles Examples")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow< HandlesExampleWindow >().Show();
	}

	void OnEnable()
	{
		SceneView.onSceneGUIDelegate += OnSceneGUI;
		if (currentKey == null || !handlesActions.ContainsKey(currentKey))
			currentKey = handlesActions.First().Key;
	}

	void OnGUI()
	{
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
		}
		EditorGUILayout.EndVertical();
	}

	void OnSceneGUI(SceneView sv)
	{
		//draw the current shown handles
		Debug.Log("evt: " + Event.current.type);
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
}
