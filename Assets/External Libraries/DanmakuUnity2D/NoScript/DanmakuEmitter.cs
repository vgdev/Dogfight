using UnityEngine;
using UnityUtilLib;
using System.Collections;
using Danmaku2D.NoScript;

namespace Danmaku2D {

	public sealed class DanmakuEmitter : DanmakuTriggerReciever {

		[SerializeField]
		private DanmakuSource[] sources;

		[SerializeField]
		private FireBuilder fireData;

		[SerializeField]
		private Modifier modifier;

		[SerializeField]
		private ProjectileControlBehavior[] controllers;

		public FireModifier Modifier {
			get {
				if(modifier == null)
					return null;
				return modifier.WrappedModifier;
			}
		}

		public override void Trigger () {
			Fire ();
		}

		public void Fire() {
			fireData.Controller = null;
			for(int i = 0; i < controllers.Length; i++)
				fireData.Controller += controllers[i].UpdateProjectile;
			fireData.Modifier = Modifier;
			for(int i = 0; i < sources.Length; i++)
				sources[i].Fire (fireData);
		}
	}
}