using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BetterHandles
{
	public class Free2DMoveHandle : CustomHandle
	{
		public Texture2D	texture = EditorGUIUtility.whiteTexture;
		public Texture2D	hoveredTexture = null;
		public Texture2D	selectedTexture = null;
		public Color		color = Handles.color;
		public Color		hoveredColor = Handles.preselectionColor;
		public Color		selectedColor = Handles.selectedColor;
		public bool			faceCamera = true;

		public float		distance { get; private set; }
		public int			controlId { get; private set; }

		int					free2DMoveHandleHash = "Free2DMoveHandle".GetHashCode();
		bool				hovered = false;
		bool				selected = false;

		public void DrawHandle(ref Vector2 position, float size)
		{
			DrawHandle(EditorGUIUtility.GetControlID(free2DMoveHandleHash, FocusType.Keyboard), ref position, size);
		}

		public void DrawHandle(int controlId, ref Vector2 position, float size)
		{
			this.controlId = controlId;
			selected = GUIUtility.hotControl == controlId || GUIUtility.keyboardControl == controlId;
			hovered = HandleUtility.nearestControl == controlId;

			switch (e.type)
			{
				case EventType.MouseDown:
					if (HandleUtility.nearestControl == controlId && e.button == 0)
					{
						GUIUtility.hotControl = controlId;
						GUIUtility.keyboardControl = controlId;
						e.Use();
					}
					break ;
				case EventType.MouseUp:
					if (GUIUtility.hotControl == controlId && (e.button == 0 || e.button == 2))
					{
						GUIUtility.hotControl = 0;
						e.Use();
					}
					break ;
				case EventType.MouseDrag:
					if (selected)
						Move2DHandle(ref position);
					break ;
				case EventType.Repaint:
					DrawDot(position, size);
					break ;
				case EventType.Layout:
					if (e.type == EventType.Layout)
						SceneView.RepaintAll();
					Vector3 pointWorldPos = matrix * position;
					distance = HandleUtility.DistanceToRectangle(pointWorldPos, Camera.current.transform.rotation, size);
					HandleUtility.AddControl(controlId, distance);
					break ;
			}
		}

		void DrawDot(Vector2 position, float size)
		{
			Vector3 worldPos = matrix * position;
			Vector3 camRight;
			Vector3 camUp;

			if (faceCamera)
			{
				camRight = Camera.current.transform.right * size;
				camUp = Camera.current.transform.up * size;
			}
			else
			{
				camRight = matrix.MultiplyPoint(Vector3.right) * size;
				camUp = matrix.MultiplyPoint(Vector3.up) * size;
			}

			Texture2D t = texture;
			Color c = (t == null) ? color : Color.white;

			if (selected && selectedTexture == null)
				c = selectedColor;
			else if (hovered && hoveredTexture == null)
				c = hoveredColor;
			
			if (selected && selectedTexture != null)
				t = selectedTexture;
			else if (hovered && hoveredTexture != null)
				t = hoveredTexture;
			
			HandlesMaterials.textured.SetColor("_Color", c);
			HandlesMaterials.textured.SetTexture("_MainTex", t);
			HandlesMaterials.textured.SetPass(0);
			GL.Begin(GL.QUADS);
			{
				GL.TexCoord2(1, 1);
				GL.Vertex(worldPos + camRight + camUp);
				GL.TexCoord2(1, 0);
				GL.Vertex(worldPos + camRight - camUp);
				GL.TexCoord2(0, 0);
				GL.Vertex(worldPos - camRight - camUp);
				GL.TexCoord2(0, 1);
				GL.Vertex(worldPos - camRight + camUp);
			}
			GL.End();
		}

		bool	GetMousePositionInWorld(out Vector3 position)
		{
			Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			return CustomHandleUtility.GetPointOnPlane(matrix, r, out position);
		}

		void Move2DHandle(ref Vector2 position)
		{
			Vector3 mouseWorldPos;
			if (GetMousePositionInWorld(out mouseWorldPos))
			{
				Vector3 pointOnPlane = matrix.inverse.MultiplyPoint(mouseWorldPos);

				if (e.delta != Vector2.zero)
					GUI.changed = true;

				position = pointOnPlane;
			}
		}

	}
}