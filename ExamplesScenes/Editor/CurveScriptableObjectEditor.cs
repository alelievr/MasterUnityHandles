using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using BetterHandles;

[CustomEditor(typeof(CurveScriptableObject))]
public class CurveScriptableObjectEditor : Editor
{
	CurveHandle		curveHandle;

	[MenuItem("Assets/Create/Curve scriptable object")]
	public static void CreateCurveObject()
	{
		var cso = ScriptableObject.CreateInstance< CurveScriptableObject >();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (String.IsNullOrEmpty(path))
            path = "Assets";
        else if (!String.IsNullOrEmpty(Path.GetExtension(path)))
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
		
		path = AssetDatabase.GenerateUniqueAssetPath(path + "/Curve.asset");

        ProjectWindowUtil.CreateAsset(cso, path);
	}

	void OnEnable()
	{
		curveHandle = new CurveHandle();

		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	void OnDisable()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;	
	}

	public void OnSceneGUI(SceneView sv)
	{
		var cso = target as CurveScriptableObject;
		
		curveHandle.Set2DSize(cso.curveSize);
		curveHandle.SetColors(cso.curveGradient);
		curveHandle.curveSamples = cso.sampleCount;

		AnimationCurve modifiedCurve = curveHandle.DrawHandle(cso.curve);
		if (GUI.changed)
		{
			Undo.RecordObject(target, "Changed curve");
			cso.curve = modifiedCurve;
		}
	}

}
