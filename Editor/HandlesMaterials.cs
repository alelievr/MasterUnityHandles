using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BetterHandles
{
	public static class HandlesMaterials
	{
		public static Material	vertexColor;
		public static Material	textured;

		static HandlesMaterials()
		{
			vertexColor = Resources.Load< Material >("vertexColorMaterial");
			textured = Resources.Load< Material >("texturedMaterial");
		}
	}
}