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
	}
}