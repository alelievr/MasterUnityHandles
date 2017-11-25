using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BetterHandles
{
	public static class HandlesMaterials
	{
		public static Material	vertexColor;
		public static Material	textured;
		public static Material	overlayColor;

		static HandlesMaterials()
		{
			vertexColor = Resources.Load< Material >("VertexColorMaterial");
			textured = Resources.Load< Material >("TexturedMaterial");
			overlayColor = Resources.Load< Material >("OverlayColorHandle");
		}
	}
}