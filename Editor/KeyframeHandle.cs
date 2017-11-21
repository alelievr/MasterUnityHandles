using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace BetterHandles
{
	public class KeyframeHandle : CustomHandle
	{
		public Color		pointColor;
		public Color		tangentColor;
		public Color		wireColor = Color.green;
		public float		tangentHandleSpacing = .3f;
		public float		tangentHandleScale = .75f;

		public Free2DMoveHandle	pointHandle = new Free2DMoveHandle();
		public Free2DMoveHandle	tangentHandle = new Free2DMoveHandle();

		readonly int		mainHandleHash = "mainCurve2Dhandle".GetHashCode();
		readonly int		tangentHandleHash = "tangentCurve2Dhandle".GetHashCode();

		AnimationCurve		curve = null;

		public enum TangentDirection
		{
			In,
			Out,
		}

		public void DrawHandle(Vector2 zone, ref Keyframe keyframe, float size, bool rightEditable = true, bool leftEditable = true)
		{
			switch (e.type)
			{
				case EventType.MouseDown:
					//we add the context menu when right clicking on the point Handle:
					if (HandleUtility.nearestControl == pointHandle.controlId && e.button == 1)
						KeyframeContextMenu(keyframe);
					break ;
			}

			DrawKeyframeHandle(zone, ref keyframe, size, rightEditable, leftEditable);
		}

		public void SetCurve(AnimationCurve curve)
		{
			this.curve = curve;
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

		bool IsSelected(int controlId)
		{
			return (EditorGUIUtility.hotControl == controlId || EditorGUIUtility.keyboardControl == controlId);
		}

		void DrawKeyframeHandle(Vector2 zone, ref Keyframe keyframe, float size, bool rightEditable, bool leftEditable)
		{
			pointHandle.SetTransform(this);
			tangentHandle.SetTransform(this);

			int		pointControlId = EditorGUIUtility.GetControlID(mainHandleHash, FocusType.Keyboard);
			int		inTangentControlId = EditorGUIUtility.GetControlID(tangentHandleHash, FocusType.Keyboard);
			int		outTangentControlId = EditorGUIUtility.GetControlID(tangentHandleHash, FocusType.Keyboard);

			// Debug.Log("hotContorl: " + EditorGUIUtility.keyboardControl + ", " + outTangentControlId + ", " + inTangentControlId + ", selected: " + selected);

			//point position
			Vector2 keyframePosition = new Vector2(zone.x * keyframe.time, zone.y * keyframe.value);

			//tangent positions:
			Vector2 inTangentPosition = -TangentToDirection(keyframe.inTangent);
			Vector2 outTangentPosition = TangentToDirection(keyframe.outTangent);

			if (e.type == EventType.Repaint)
			{
				//tangent Wires:
				HandlesMaterials.vertexColor.SetPass(0);
				GL.Begin(GL.LINES);
				{
					GL.Color(wireColor);
					if (rightEditable)
					{
						GL.Vertex(matrix.MultiplyPoint(keyframePosition));
						GL.Vertex(matrix.MultiplyPoint(inTangentPosition + keyframePosition));
					}
					if (leftEditable)
					{
						GL.Vertex(matrix.MultiplyPoint(keyframePosition));
						GL.Vertex(matrix.MultiplyPoint(outTangentPosition + keyframePosition));
					}
				}
				GL.End();
			}
			
			//draw main point Handle
			pointHandle.DrawHandle(pointControlId, ref keyframePosition, size);

			//draw tangents Handles
			inTangentPosition += keyframePosition;
			outTangentPosition += keyframePosition;

			if (rightEditable)
			{
				tangentHandle.DrawHandle(inTangentControlId, ref inTangentPosition, size * tangentHandleScale);
				keyframe.inTangent = DirectionToTangent(inTangentPosition - keyframePosition, TangentDirection.In);
			}
			if (leftEditable)
			{
				tangentHandle.DrawHandle(outTangentControlId, ref outTangentPosition, size * tangentHandleScale);
				keyframe.outTangent = DirectionToTangent(outTangentPosition - keyframePosition, TangentDirection.Out);
			}

			//set back keyframe values
			keyframe.time = keyframePosition.x / zone.x;
			keyframe.value = keyframePosition.y / zone.y;
		}

		void KeyframeContextMenu(Keyframe keyframe)
		{
			GenericMenu	menu = new GenericMenu();

			if (curve != null)
			{
				int	keyframeIndex = curve.keys.ToList().FindIndex(k => k.value == keyframe.value && k.time == keyframe.time) - 1;
				Action< bool, string, AnimationUtility.TangentMode > SetTangentModeMenu = (right, text, tangentMode) => {
					menu.AddItem(new GUIContent(text), false, () => {
						if (right)
							AnimationUtility.SetKeyRightTangentMode(curve, keyframeIndex, tangentMode);
						else
							AnimationUtility.SetKeyLeftTangentMode(curve, keyframeIndex, tangentMode);
					});
				};
				SetTangentModeMenu(false, "Left Tangent/Auto", AnimationUtility.TangentMode.Auto);
				SetTangentModeMenu(false, "Left Tangent/ClampedAuto", AnimationUtility.TangentMode.ClampedAuto);
				SetTangentModeMenu(false, "Left Tangent/Constant", AnimationUtility.TangentMode.Constant);
				SetTangentModeMenu(false, "Left Tangent/Free", AnimationUtility.TangentMode.Free);
				SetTangentModeMenu(false, "Left Tangent/Linear", AnimationUtility.TangentMode.Linear);
				SetTangentModeMenu(true, "Right Tangent/Auto", AnimationUtility.TangentMode.Auto);
				SetTangentModeMenu(true, "Right Tangent/ClampedAuto", AnimationUtility.TangentMode.ClampedAuto);
				SetTangentModeMenu(true, "Right Tangent/Constant", AnimationUtility.TangentMode.Constant);
				SetTangentModeMenu(true, "Right Tangent/Free", AnimationUtility.TangentMode.Free);
				SetTangentModeMenu(true, "Right Tangent/Linear", AnimationUtility.TangentMode.Linear);

				menu.AddItem(new GUIContent("remove"), false, () => curve.RemoveKey(keyframeIndex));
			}
			else
				menu.AddDisabledItem(new GUIContent("Curve not set for keyframe !"));
			menu.ShowAsContext();
		}
	}
}