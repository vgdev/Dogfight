using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	[System.Serializable]
	public class RandomizeAngleModifier : FireModifier {

		[SerializeField, Range(0f, 360f)]
		private float range = 0;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, float rotation) {
			FireSingle (position, Random.Range (rotation - range, rotation + range));
		}
		#endregion

	}

	namespace Wrapper {

		internal class RandomizeAngleModifier : ModifierWrapper<Danmaku2D.RandomizeAngleModifier> {
		}

	}
}