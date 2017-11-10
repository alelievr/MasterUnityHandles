using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BetterHandles
{
	public static class HandlesMaterials
	{
		public static Material	vertexColor;

		static HandlesMaterials()
		{
			vertexColor = Resources.Load< Material >("vertexColorMaterial");
		}
	}
}