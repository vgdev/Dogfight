using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
	public abstract class MovementPattern : PausableGameObject {

		[SerializeField]
		private bool destroyOnEnd;

		/// <summary>
		/// Whether or not the GameObject this MovementPattern is destroyed at the end of the movement or not.
		/// </summary>
		/// <value><c>true</c> if it is to be destroyed; otherwise, <c>false</c>. <see cref="UnityEngine.Object.Destroy"/> </value>
		public bool DestroyOnEnd {
			get {
				return destroyOnEnd;
			}
			set {
				destroyOnEnd = value;
			}
		}

		/// <summary>
		/// Starts the movement followining the pattern defined by this script.
		/// </summary>
		public void StartMovement() {
			StartCoroutine (MoveImpl ());
		}

		private IEnumerator MoveImpl() {
			yield return StartCoroutine(Move());
			if(destroyOnEnd) {
				Destroy (gameObject);
			}
		}

		/// <summary>
		/// The actual movement coroutine. 
		/// </summary>
		protected abstract IEnumerator Move();
	}
}