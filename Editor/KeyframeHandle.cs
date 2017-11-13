using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BetterHandles
{
	public class KeyframeHandle : CustomHandle
	{
		int	keyframeHandleHash = "KeyframeHandle".GetHashCode();
		Free2DMoveHandle	free2DMoveHandle = new Free2DMoveHandle();

		public void DrawHandle(Vector2 zone, ref Keyframe keyframe)
		{
			int	controlId = EditorGUIUtility.GetControlID(keyframeHandleHash, FocusType.Passive);

			switch (e.type)
			{
				case EventType.MouseDown:
					if (HandleUtility.nearestControl == controlId && e.button == 1)
					{
						Debug.Log("Context click !");
						KeyframeContextMenu();
					}
					break ;
				case EventType.Layout:
					//TODO: raycast and 2D distance
					HandleUtility.AddControl(controlId, 100000f);
					break ;
			}

			DrawKeyframeHandle(zone, keyframe);
		}

		void DrawKeyframeHandle(Vector2 zone, Keyframe keyframe)
		{
			free2DMoveHandle.SetTransform(this);

			//global position
			Vector2 keyframePosition = new Vector2(zone.x * keyframe.time, zone.y * keyframe.value);
			free2DMoveHandle.DrawHandle(ref keyframePosition, 1f);

			//tangents:

			keyframe.time = keyframePosition.x / zone.x;
			keyframe.value = keyframePosition.y / zone.y;

			/*Vector3 pos = matrix.MultiplyPoint(keyframePosition);
			pos = Handles.FreeMoveHandle(pos, Quaternion.identity, .02f, Vector3.zero, Handles.DotHandleCap);

			//tangents:
			Vector3 t1Direction = new Vector3(Mathf.Sin(keyframe.inTangent), Mathf.Cos(keyframe.inTangent), 0);
			Vector3 t1 = t1Direction / 3f;
			Handles.color = Color.green;
			t1 = Handles.FreeMoveHandle(t1 + pos, Quaternion.identity, 0.015f, Vector3.zero, Handles.DotHandleCap) - pos;
			Handles.color = Color.white;*/
		}

		void KeyframeContextMenu()
		{
			GenericMenu	menu = new GenericMenu();

			menu.AddItem(new GUIContent("test"), false, () => Debug.Log("olol"));
			menu.ShowAsContext();
		}
	}
}