using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
public class SnapHandleEditor : Editor
{
	void OnEnable()
	{
		
	}

	void OnSceneGUI()
	{
		Handles.Label(Vector3.zero, "snap handle editro here", EditorStyles.whiteLabel);
	}
}
