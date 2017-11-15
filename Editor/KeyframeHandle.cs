using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BetterHandles
{
	public class KeyframeHandle : CustomHandle
	{
		public Color		pointColor;
		public Color		tangentColor;
		public Color		wireColor = Color.green;
		public float		tangentHandleSpacing = .3f;
		public float		tangentHandleScale = .75f;

		const float			piOf2 = (Mathf.PI / 2f);

		Free2DMoveHandle	pointHandle = new Free2DMoveHandle();
		Free2DMoveHandle	tangentHandle = new Free2DMoveHandle();

		public void DrawHandle(Vector2 zone, ref Keyframe keyframe, float size)
		{
			switch (e.type)
			{
				case EventType.MouseDown:
					//we add the context menu when right clicking on the point Handle:
					if (HandleUtility.nearestControl == pointHandle.controlId && e.button == 1)
						KeyframeContextMenu();
					break ;
			}

			DrawKeyframeHandle(zone, ref keyframe, size);
		}

		Vector2 TangentToDirection(float radTangent)
		{
			if (float.IsInfinity(radTangent))
				return new Vector2(0, -1);
			return (new Vector2(1f, radTangent)).normalized * tangentHandleSpacing;
		}

		float DirectionToTangent(Vector2 direction)
		{
			return direction.y / direction.x;
		}

		void DrawKeyframeHandle(Vector2 zone, ref Keyframe keyframe, float size)
		{
			pointHandle.SetTransform(this);
			tangentHandle.SetTransform(this);

			//point position
			Vector2 keyframePosition = new Vector2(zone.x * keyframe.time, zone.y * keyframe.value);

			//tangent positions:
			Vector2 tangent1Position = TangentToDirection(keyframe.inTangent);
			Vector2 tangent2Position = TangentToDirection(keyframe.outTangent);

			//tangent Wires:
			HandlesMaterials.vertexColor.SetPass(0);
			GL.Begin(GL.LINES);
			{
				GL.Color(wireColor);
				GL.Vertex(matrix.MultiplyPoint(keyframePosition));
				GL.Vertex(matrix.MultiplyPoint(tangent1Position + keyframePosition));
				GL.Vertex(matrix.MultiplyPoint(keyframePosition));
				GL.Vertex(matrix.MultiplyPoint(tangent2Position + keyframePosition));
			}
			GL.End();
			
			//draw main point Handle
			pointHandle.DrawHandle(ref keyframePosition, size);

			//draw tangents Handles
			tangent1Position += keyframePosition;
			tangent2Position += keyframePosition;
			tangentHandle.DrawHandle(ref tangent1Position, size * tangentHandleScale);
			tangentHandle.DrawHandle(ref tangent2Position, size * tangentHandleScale);

			Vector2 d1 = tangent1Position - keyframePosition;
			Vector2 d2 = tangent2Position - keyframePosition;

			/*if (d1.x < -0.0001f)
				keyframe.inTangent = d1.y / d1.x;
			else
				keyframe.inTangent = float.PositiveInfinity;

			if (d2.x > 0.0001f)
				keyframe.outTangent = d2.y / d2.x;
			else
				keyframe.outTangent = float.PositiveInfinity;*/

			keyframe.inTangent = DirectionToTangent(tangent1Position - keyframePosition);
			keyframe.outTangent = DirectionToTangent(tangent2Position - keyframePosition);

			Handles.Label(tangent1Position, (DirectionToTangent(tangent1Position - keyframePosition)) + "");
			Handles.Label(tangent2Position, (DirectionToTangent(tangent2Position - keyframePosition)) + "");

			//set modified keyframe values
			// keyframe.inTangent = DirectionToRadiant(tangent1Position - keyframePosition) + piOf2;
			// keyframe.outTangent = DirectionToRadiant(tangent2Position - keyframePosition) - piOf2;
			keyframe.time = keyframePosition.x / zone.x;
			keyframe.value = keyframePosition.y / zone.y;
		}

		void KeyframeContextMenu()
		{
			GenericMenu	menu = new GenericMenu();

			menu.AddItem(new GUIContent("test"), false, () => Debug.Log("olol"));
			menu.ShowAsContext();
		}
	}
}