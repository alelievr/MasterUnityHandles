using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BetterHandles;

public class MeshPreviewHandle : CustomHandle
{
	int	meshPreviewHash = "MeshPreviewHandle".GetHashCode();

	struct MeshInfo
	{
		public Mesh			mesh;
		public Matrix4x4	trs;
		public Material		material;

		public MeshInfo(Mesh mesh, Matrix4x4 trs, Material material)
		{
			this.mesh = mesh;
			this.material = material;
			this.trs = trs;
		}
	}

	Dictionary< int, MeshInfo > meshInfos = new Dictionary< int, MeshInfo >();

	Mesh		currentMesh;
	Matrix4x4	currentTRS;

	public void DrawHandle(Mesh mesh, Material material, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		int controlId = EditorGUIUtility.GetControlID(meshPreviewHash, FocusType.Passive);
		Matrix4x4 trs = Matrix4x4.TRS(position, rotation, scale);
		meshInfos[controlId] = new MeshInfo(mesh, trs, material);

		Handles.FreeMoveHandle(controlId, position, rotation, 0f, Vector3.zero, MeshHandleCap);
	}

	public void MeshHandleCap(int controlId, Vector3 position, Quaternion rotation, float size, EventType eventType)
	{
		MeshInfo meshInfo = meshInfos[controlId];

		if (eventType == EventType.Repaint)
		{
			meshInfo.material.SetPass(0);
			Graphics.DrawMeshNow(meshInfo.mesh, meshInfo.trs);
		}
		else if (eventType == EventType.Layout)
		{
			Ray mouseRay = HandleUtility.GUIPointToWorldRay(e.mousePosition); 
			bool intersect = meshInfo.mesh.bounds.IntersectRay(mouseRay);
			if (intersect)
				HandleUtility.AddControl(controlId, 0);
			else
				HandleUtility.AddControl(controlId, 1e20f);
		}
	}
}
