using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BetterHandles;
using UnityEngine.Profiling;
using UnityEditor;

public class DrawPerformances : CustomHandle
{
	List< Vector3 > vertices;

	public void Init()
	{
		vertices = new List< Vector3 >();

		for (int i = 0; i < 3 * 10000; i++)
			vertices.Add(new Vector3(Random.value * 20, Random.value * 20, Random.value * 20));
	}

	public void Test()
	{
		DrawHandle(true, vertices, MeshTopology.Triangles);
		DrawHandle(false, vertices, MeshTopology.Triangles);
	}

	public void DrawHandle(bool useMesh, List< Vector3 > vertices, MeshTopology topology)
	{
		if (e.type != EventType.Repaint)
			return ;
		
		if (useMesh)
			DrawMesh(vertices, topology);
		else
			DrawGL(vertices, topology);
		
		SceneView.RepaintAll();
	}

	void DrawMesh(List< Vector3 > vertices, MeshTopology topology)
	{
		Profiler.BeginSample("Mesh draw");
		Mesh m = new Mesh();
		int[] indices = new int[vertices.Count];

		for (int i = 0; i < vertices.Count; i++)
		{
			indices[i] = i;
		}

		m.SetVertices(vertices);
		m.SetIndices(indices, topology, 0);

		HandlesMaterials.vertexColor.SetPass(0);
		Graphics.DrawMeshNow(m, Vector3.right * 30, Quaternion.identity);
		Profiler.EndSample();
	}

	void DrawGL(List< Vector3 > vertices, MeshTopology topology)
	{
		HandlesMaterials.vertexColor.SetPass(0);
		Profiler.BeginSample("GL draw");
		GL.Begin(GL.TRIANGLES);
		{
			foreach (var vert in vertices)
				GL.Vertex(vert);
		}
		GL.End();
		Profiler.EndSample();
	}
}
