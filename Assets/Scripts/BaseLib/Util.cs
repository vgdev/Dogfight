using UnityEngine;
using System.Collections;

namespace BaseLib
{
	public static class Util
	{
		public static float Sign(float e)
		{
			return (e == 0f) ? 0f : Mathf.Sign (e);
		}
	}
}