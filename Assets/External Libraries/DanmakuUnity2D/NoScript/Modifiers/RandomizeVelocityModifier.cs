using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	[System.Serializable]
	public class RandomizeVelocityModifier : FireModifier {

		[SerializeField]
		private float range;

		#region implemented abstract members of FireModifier
		public override void Fire (Vector2 position, float rotation) {
			float oldVelocity = Velocity;
			Velocity = oldVelocity + Random.Range (-0.5f * range, 0.5f * range);
			FireSingle (position, rotation);
			Velocity = oldVelocity;
		}
		#endregion

	}

	namespace Wrapper {

		internal class RandomizeVelocityModifier : ModifierWrapper<Danmaku2D.RandomizeVelocityModifier> {
		}

	}

}
