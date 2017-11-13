using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BetterHandles
{
	public class Free2DMoveHandle : CustomHandle
	{
		public Texture2D	handleTexture = EditorGUIUtility.whiteTexture;
		public Texture2D	handleHoveredTexture = null;
		public Texture2D	handleSelectTexture = null;
		public Color		handleHoveredColor = Handles.preselectionColor;
		public Color		handleSelectedColor = Handles.selectedColor;

		int free2DMoveHandleHash = "Free2DMoveHandle".GetHashCode();
		bool				hover = false;

		public void DrawHandle(ref Vector2 position, float size)
		{
			int controlId = EditorGUIUtility.GetControlID(free2DMoveHandleHash, FocusType.Passive);
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
					GUIUtility.hotControl = 0;
					e.Use();
					break ;
				case EventType.MouseDrag:
					hover = false;
					if (GUIUtility.hotControl == controlId)
						Move2DHandle();
					else if (HandleUtility.nearestControl == controlId)
						hover = true;
					break ;
				case EventType.Repaint:
					break ;
				case EventType.Layout:
					if (e.type == EventType.Layout)
						SceneView.RepaintAll();
					Vector3 mouseWorldPos = GetMousePositionInWorld();
					Vector3 pointWorldPos = matrix * position;
					float distance = Vector3.Distance(mouseWorldPos, pointWorldPos);
					HandleUtility.AddControl(controlId, distance);
					break ;
			}
		}

		Vector3	GetMousePositionInWorld()
		{
			Camera cam = Camera.current;

			Ray r = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			return CustomHandleUtility.GetPointOnPlane(matrix, r);
		}

		void Move2DHandle()
		{
			Vector3 mouseWorldPos = GetMousePositionInWorld();
		}

	}
}