using UnityEngine;
using System.Collections;
using BaseLib;

public abstract class AbstractTimedAttackPattern : AbstractAttackPattern {

	private CountdownDelay timeout;

	protected override void MainLoop (float dt) {
		timeout.Tick (dt);
	}

	protected override void OnExecutionStart () {
		timeout.Reset ();
	}

	protected override bool IsFinished {
		get {
			return timeout.Ready();
		}
	}
}
