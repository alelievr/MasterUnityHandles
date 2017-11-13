using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BetterHandles
{
	public class CustomHandle
	{
		Quaternion			rotation = Quaternion.identity;
		Vector3				scale = Vector3.one;
		Vector3				position;

		public Matrix4x4	matrix = Matrix4x4.identity;
		public Event		e { get { return Event.current; } }

		public void SetTransform(Transform transform)
		{
			rotation = transform.rotation;
			position = transform.position;
			scale = transform.localScale;
			matrix = Matrix4x4.TRS(position, rotation, scale);
		}
		
		public void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.rotation = rotation;
			this.position = position;
			this.scale = scale;
			matrix = Matrix4x4.TRS(position, rotation, scale);
		}
		
		public void SetTransform(CustomHandle handle)
		{
			this.rotation = handle.rotation;
			this.position = handle.position;
			this.scale = handle.scale;
			matrix = Matrix4x4.TRS(position, rotation, scale);
		}
	}
}