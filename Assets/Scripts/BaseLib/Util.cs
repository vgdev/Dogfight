using UnityEngine;
using System.Collections;

namespace BaseLib
{
	public static class Util
	{
		public const float Degree2Rad = Mathf.PI / 180f;
		public const float Rad2Degree = 180 / Mathf.PI;

		public static float Sign(float e)
		{
			return (e == 0f) ? 0f : Mathf.Sign (e);
		}

		public static Vector2 To2D(Vector3 v) {
			return new Vector2(v.x, v.y);
		}

		public static Vector3 To3D(Vector2 v) {
			return new Vector3 (v.x, v.y, 0f);
		}
		
		public static Vector2 ComponentProduct2(Vector2 v1, Vector2 v2) {
			return new Vector2(v1.x * v2.x, v1.y * v2.y);
		}
		
		public static Vector3 ComponentProduct3(Vector3 v1, Vector3 v2) {
			return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
		}
	}
}