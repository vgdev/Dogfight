using UnityEngine;
using UnityUtilLib;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Danmaku2D {

	[Serializable]
	public class AttackPatternExecution {
		[SerializeField]
		public bool Active {
			get;
			set;
		}
	}

	public abstract class AbstractAttackPattern : PausableGameObject {

		private List<AttackPatternExecution> executions;

		private AbstractDanmakuField targetField;
		public AbstractDanmakuField TargetField {
			get {
				return targetField;
			}
			set {
				targetField = value;
			}
		}

		protected abstract bool IsFinished {
			get; 
		}

		protected float AngleToPlayer {
			get {
				return targetField.AngleTowardPlayer(Transform.position);
			}
		}

		public override void Awake () {
			base.Awake ();
			executions = new List<AttackPatternExecution> ();
		}

		private bool attackActive;

		protected virtual void OnExecutionStart() {
		}

		protected abstract void MainLoop(AttackPatternExecution execution);

		protected virtual void OnExecutionFinish() {
		}

		protected Projectile FireLinearBullet(ProjectilePrefab bulletType, 
		                                Vector2 location, 
		                                float rotation, 
		                                float velocity) {
			return FireCurvedBullet(bulletType, location, rotation, velocity, 0f);
		}

		protected Projectile FireCurvedBullet(ProjectilePrefab bulletType,
		                                      Vector2 location,
		                                      float rotation,
		                                      float velocity,
		                                      float angularVelocity) {
			Projectile bullet = targetField.SpawnProjectile (bulletType, location, rotation);
			bullet.Velocity = velocity;
			bullet.AngularVelocity = angularVelocity;
			return bullet;
		}

		protected virtual AttackPatternExecution OnGenerateAttackPatternExecution() {
			return new AttackPatternExecution ();
		}

		protected void Terminate(AttackPatternExecution execution) {
			execution.Active = false;
		}

		public void Fire() {
			StartCoroutine (Execute ());
		}

		public void TerminateAll() {
			for(int i = 0; i < executions.Count; i++) {
				executions[i].Active = false;
			}
		}

		private IEnumerator Execute() {
			AttackPatternExecution execution = OnGenerateAttackPatternExecution ();
			execution.Active = true;
			executions.Add (execution);
			OnExecutionStart ();
			while(!IsFinished && execution.Active) {
				MainLoop(execution);
				yield return WaitForUnpause();
			}
			OnExecutionFinish ();
			executions.Remove (execution);
		}
	}
}