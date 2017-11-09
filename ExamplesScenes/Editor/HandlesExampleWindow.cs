using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class HandlesExampleWindow : EditorWindow
{
	[NonSerialized]
	bool		focus = false;
	[SerializeField]
	string		currentKey = null;

	Dictionary< string, Action >	handlesActions = new Dictionary< string, Action >()
	{
		{"Simple custom handles (using existing Handles)", null},
		{"Solid scaled cube", () => HandlesExtended.DrawSolidCube(Vector3.zero, Quaternion.identity, new Vector3(5, 1, 3), new Color(1f, 0, .2f, .2f))},
		{"Wire rotated cube", () => HandlesExtended.DrawWireCube(Vector3.zero, Quaternion.Euler(45, 45, 0), new Vector3(2, 1, 2), new Color(1f, 0, .2f, 1f))},
		{"Solid scaled cylinder", () => HandlesExtended.DrawCylinder(Vector3.zero, Quaternion.Euler(90, 0, 90), new Vector3(1, 3, 2), new Color(0, 1, 0, .3f))},
		{"Solid scaled cone", () => HandlesExtended.DrawCone(Vector3.zero, Quaternion.Euler(-90, 0, 0), new Vector3(1, 3, 2), new Color(0, 0, 1, .3f))},
		{"Solid scaled circle", () => HandlesExtended.DrawCircle(Vector3.zero, Quaternion.Euler(45, 0, 45), new Vector3(1, 3, 2), new Color(0, 0, 1, .3f))},
		{"Solid scaled arrow", () => HandlesExtended.DrawArrow(Vector3.zero, Quaternion.Euler(-90, 0, 0), new Vector3(1, 3, 1), new Color(0, 0, 1, .3f))},
		{"Solid scaled rectangle", () => HandlesExtended.DrawRectange(Vector3.zero, Quaternion.Euler(90, 0, 90), new Vector3(1, 3, 2), new Color(0, 0, 1, .3f))},
		{"Solid scaled sphere", () => HandlesExtended.DrawSphere(Vector3.zero, Quaternion.Euler(90, 0, 90), new Vector3(1, 3, 2), new Color(0, 0, 1, .3f))},
		{"Full custom Handles", null},
		{"", () => {}},
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
