using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
	public abstract class AbstractTimedAttackPattern : AbstractAttackPattern {

		[SerializeField]
		private FrameCounter timeout;

		protected override void MainLoop (AttackPatternExecution execution) {
			if(timeout.Tick (false)) {
				return;
			}
		}

		protected override void OnExecutionStart () {
			timeout.Reset ();
		}

		protected sealed override bool IsFinished {
			get {
				return timeout.Ready();
			}
		}
	}
}