using UnityEngine;
using System.Collections;
using UnityUtilLib;
using System.Collections.Generic;

namespace Danmaku2D {

	/// <summary>
	/// A container behavior used on prefabs to define how a bullet looks or behaves
	/// </summary>
	[RequireComponent(typeof(CircleCollider2D)), RequireComponent(typeof(SpriteRenderer)), AddComponentMenu("Danmaku 2D/Danmaku Prefab")]
	public sealed class DanmakuPrefab : DanmakuObjectPrefab {

		private DanmakuPrefab runtime;
		
		private DanmakuControlBehavior[] extraControllers;
		internal DanmakuControlBehavior[] ExtraControllers {
			get {
				return extraControllers;
			}
		}

		public override void Awake() {
			base.Awake ();
			extraControllers = GetComponents<DanmakuControlBehavior>();
		}

		internal DanmakuPrefab GetRuntime() {
			if(runtime == null)
				runtime = CreateRuntimeInstance(this);
			return runtime;
		}

		private static DanmakuPrefab CreateRuntimeInstance(DanmakuPrefab prefab) {
			DanmakuPrefab runtime = (DanmakuPrefab)Instantiate (prefab);
			runtime.gameObject.hideFlags = HideFlags.HideInHierarchy;
			runtime.gameObject.SetActive (false);
			return runtime;
		}
	}
}