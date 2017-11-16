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

		Free2DMoveHandle	pointHandle = new Free2DMoveHandle();
		Free2DMoveHandle	tangentHandle = new Free2DMoveHandle();

		public enum TangentDirection
		{
			In,
			Out,
		}

		public void DrawHandle(Vector2 zone, ref Keyframe keyframe, float size)
		{
			switch (e.type)
			{
				case EventType.MouseDown:
					//we add the context menu when right clicking on the point Handle:
					if (HandleUtility.nearestControl == pointHandle.controlId && e.button == 1)
						KeyframeContextMenu(keyframe);
					break ;
			}

			DrawKeyframeHandle(zone, ref keyframe, size);
		}

		Vector2 TangentToDirection(float radTangent)
		{
			if (float.IsInfinity(radTangent))
				return new Vector2(0, -tangentHandleSpacing);
			return (new Vector2(1f, radTangent)).normalized * tangentHandleSpacing;
		}

		float DirectionToTangent(Vector2 direction, TangentDirection tangentDirection)
		{
			if (tangentDirection == TangentDirection.In && direction.x > 0.0001f)
				return float.PositiveInfinity;
			if (tangentDirection == TangentDirection.Out && direction.x < -0.0001f)
				return float.PositiveInfinity;
	
			return direction.y / direction.x;
		}

		void DrawKeyframeHandle(Vector2 zone, ref Keyframe keyframe, float size)
		{
			pointHandle.SetTransform(this);
			tangentHandle.SetTransform(this);

			//point position
			Vector2 keyframePosition = new Vector2(zone.x * keyframe.time, zone.y * keyframe.value);

			//tangent positions:
			Vector2 inTangentPosition = -TangentToDirection(keyframe.inTangent);
			Vector2 outTangentPosition = TangentToDirection(keyframe.outTangent);

			//tangent Wires:
			HandlesMaterials.vertexColor.SetPass(0);
			GL.Begin(GL.LINES);
			{
				GL.Color(wireColor);
				GL.Vertex(matrix.MultiplyPoint(keyframePosition));
				GL.Vertex(matrix.MultiplyPoint(inTangentPosition + keyframePosition));
				GL.Vertex(matrix.MultiplyPoint(keyframePosition));
				GL.Vertex(matrix.MultiplyPoint(outTangentPosition + keyframePosition));
			}
			GL.End();
			
			//draw main point Handle
			pointHandle.DrawHandle(ref keyframePosition, size);

			//draw tangents Handles
			inTangentPosition += keyframePosition;
			outTangentPosition += keyframePosition;
			tangentHandle.DrawHandle(ref inTangentPosition, size * tangentHandleScale);
			tangentHandle.DrawHandle(ref outTangentPosition, size * tangentHandleScale);

			Vector2 d1 = inTangentPosition - keyframePosition;
			Vector2 d2 = outTangentPosition - keyframePosition;

			//set back keyframe values
			keyframe.inTangent = DirectionToTangent(inTangentPosition - keyframePosition, TangentDirection.In);
			keyframe.outTangent = DirectionToTangent(outTangentPosition - keyframePosition, TangentDirection.Out);
			keyframe.time = keyframePosition.x / zone.x;
			keyframe.value = keyframePosition.y / zone.y;
		}

		void KeyframeContextMenu(Keyframe keyframe)
		{
			GenericMenu	menu = new GenericMenu();

			//TODO: generic menu with keyframe tangent mode

			// AnimationUtility.SetKeyLeftTangentMode
			//AnimationUtility.SetKeyBroken

			menu.AddItem(new GUIContent("TODO"), false, () => Debug.Log("olol"));
			menu.ShowAsContext();
		}
	}
}