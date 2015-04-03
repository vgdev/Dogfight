using UnityEngine;
using System.Collections;
using UnityUtilLib;
using Danmaku2D;
using Danmaku2D.DanmakuControllers;

public class RussianWinter : TimedAttackPattern {

	[SerializeField]
	private DanmakuPrefab snowPrefab;

	[SerializeField]
	private FrameCounter delay;

	[SerializeField]
	private int spawnCount;

	[SerializeField]
	private Vector2 areaCenter;

	[SerializeField]
	private Vector2 areaSize;

	[SerializeField]
	private float angleMin;

	[SerializeField]
	private float angleMax;

	[SerializeField]
	private AccelerationController linearController;

	private DanmakuGroup group;

	protected override void OnInitialize () {
		base.OnInitialize ();
		group = new DanmakuGroup ();
		group.AddController(linearController);
	}

	protected override void MainLoop () {
		base.MainLoop ();
		if(delay.Tick ()) {
			for(int i = 0; i < spawnCount; i++) {
				group.Add (SpawnDanmaku(snowPrefab, areaCenter - areaSize / 2f + areaSize.Random(), Random.Range(angleMin, angleMax)));
			}
		}
	}
}
