using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BetterHandles
{
	public class CustomHandle
	{
		public Quaternion	rotation = Quaternion.identity;
		public Vector3		scale = Vector3.one;
		public Vector3		position;

		public Matrix4x4	matrix { get { return Matrix4x4.TRS(position, rotation, scale); } }

		public void SetTransform(Transform transform)
		{
			rotation = transform.rotation;
			position = transform.position;
			scale = transform.localScale;
		}
	}
}