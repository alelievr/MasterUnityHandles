using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BetterHandles
{
	public class CurveHandle : CustomHandle
	{
		public Gradient	curveGradient;
		public float	width;
		public float	height;

		public int		curveSamples = 50;

		KeyframeHandle	keyframeHandle = new KeyframeHandle();

		Vector3 pos = Vector3.zero;
		public void		DrawHandle(AnimationCurve curve, bool editableSize = false)
		{
			switch (e.type)
			{
				case EventType.Repaint:
					PushGLContext();
					DrawBorders();
					DrawCurve(curve);
					DrawLabels();
					PopGLContext();
					break ;
			}

			//draw curve handles:
			DrawCurvePointsHandle(curve);
		
			if (editableSize)
				DrawZoneHandles();
		}

		void PushGLContext()
		{
			GL.PushMatrix();
			GL.MultMatrix(matrix);
			HandlesMaterials.vertexColor.SetPass(0);
		}

		void PopGLContext()
		{
			GL.PopMatrix();
		}

		void DrawBorders()
		{
			//hummm good (or not so) legacy OpenGL ...

			Vector3 bottomLeft = Vector3.zero;
			Vector3 bottomRight = new Vector3(width, 0, 0);
			Vector3 topLeft = new Vector3(0, height, 0);
			Vector3 topRight = new Vector3(width, height, 0);

			GL.Begin(GL.LINE_STRIP);
			{
				HandlesMaterials.vertexColor.SetPass(0);
				GL.Color(Color.blue);
				GL.Vertex(bottomLeft);
				GL.Vertex(bottomRight);
				GL.Color(Color.red);
				GL.Vertex(topRight);
				GL.Vertex(topLeft);
				GL.Vertex(bottomLeft);
			}
			GL.End();
		}

		void DrawCurveQuad(AnimationCurve curve, float f0, float f1)
		{
			Vector3 bottomLeft = new Vector3(f0 * width, 0, 0);
			Vector3 topLeft = new Vector3(f0 * width, curve.Evaluate(f0) * height, 0);
			Vector3 topRight = new Vector3(f1 * width, curve.Evaluate(f1) * height, 0);
			Vector3 bottomRight = new Vector3(f1 * width, 0, 0);
			
			GL.Color(curveGradient.Evaluate(f0));
			GL.Vertex(bottomLeft);
			GL.Vertex(topLeft);
			GL.Color(curveGradient.Evaluate(f1));
			GL.Vertex(topRight);
			GL.Vertex(bottomRight);
		}

		void DrawLabels()
		{
			Handles.Label(Vector3.zero, "0");
		}

		void DrawCurvePointsHandle(AnimationCurve curve)
		{
			keyframeHandle.SetTransform(this);

			for (int i = 0; i < curve.length; i++)
			{
				Keyframe kf = curve.keys[i];

				keyframeHandle.DrawHandle(new Vector2(width, height), ref kf);

				curve.MoveKey(i, kf);
			}
		}

		void DrawCurve(AnimationCurve curve)
		{
			if (curveSamples < 0 || curveSamples > 10000)
				return ;
			
			//draw curve
			GL.Begin(GL.QUADS);
			{
				
				for (int i = 0; i < curveSamples; i++)
				{
					float f0 = (float)i / (float)curveSamples;
					float f1 = (float)(i + 1) / (float)curveSamples;

					DrawCurveQuad(curve, f0, f1);
				}
			}
			GL.End();
		}

		void DrawZoneHandles()
		{

		}

		public void	SetColors(Color startColor, Color endColor)
		{
			curveGradient = new Gradient();
			GradientColorKey[]	colorKeys = new GradientColorKey[2]{new GradientColorKey(startColor, 0), new GradientColorKey(endColor, 1)};
			GradientAlphaKey[]	alphaKeys = new GradientAlphaKey[2]{new GradientAlphaKey(startColor.a, 0), new GradientAlphaKey(endColor.a, 1)};

			curveGradient.SetKeys(colorKeys, alphaKeys);
		}

		public void SetColors(Gradient gradient)
		{
			curveGradient = gradient;
		}

		public void Set2DSize(Vector2 size) { Set2DSize(size.x, size.y); }
		public void Set2DSize(float width, float height)
		{
			this.width = width;
			this.height = height;
		}
	}
}