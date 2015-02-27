using UnityEngine;
using System.Collections;
using UnityUtilLib;
using Danmaku2D;

public class KnightsCross : AbstractTimedAttackPattern {
    [SerializeField]
    private FrameCounter crossDelay;

    [SerializeField]
    private ProjectilePrefab fatBullet;

    [SerializeField]
    private ProjectilePrefab thinBullet;

    [SerializeField]
    private float bulletVelocity;

    [SerializeField]
    private Vector2 centerArea;

    [SerializeField]
    private float bulletSeparationDist;

    private Vector2 center;
    private float angleToPlayer;

    // this is called every time the attack pattern starts
    protected override void OnExecutionStart() {
        base.OnExecutionStart();
        crossDelay.Reset();
        Vector2 center = new Vector2(.5f, .5f);//Util.RandomVect2(centerArea);
        float angleToPlayer = TargetField.AngleTowardPlayer(TargetField.WorldPoint(center));
        fire();
    }

    protected override void MainLoop(AttackPatternExecution execution) {
        base.MainLoop(execution);
        if (crossDelay.Tick()) {
            fire();
        }
    }

    void fire() {
        Debug.Log(angleToPlayer);

		float left = angleToPlayer + 90f;
		float right = angleToPlayer + 270f;
		float away = angleToPlayer + 180f;

        //left
        FireLinearBullet(fatBullet, rotate(center + new Vector2(-0.05f, 0f), angleToPlayer), left, bulletVelocity);
		FireLinearBullet(fatBullet, rotate(center + new Vector2(-0.05f, 0.05f + bulletSeparationDist), angleToPlayer), left, bulletVelocity);
		FireLinearBullet(fatBullet, rotate(center + new Vector2(-0.05f, -0.05f - bulletSeparationDist), angleToPlayer), left, bulletVelocity);

        //right
        FireLinearBullet(fatBullet, rotate(center + new Vector2(0.05f, 0f), angleToPlayer), right, bulletVelocity);
		FireLinearBullet(fatBullet, rotate(center + new Vector2(0.05f, 0.05f + bulletSeparationDist), angleToPlayer), right, bulletVelocity);
		FireLinearBullet(fatBullet, rotate(center + new Vector2(0.05f, -0.05f - bulletSeparationDist), angleToPlayer), right, bulletVelocity);

        //up
		FireLinearBullet(fatBullet, rotate(center + new Vector2(0f, -0.05f), angleToPlayer), angleToPlayer + 0f, bulletVelocity);
		FireLinearBullet(fatBullet, rotate(center + new Vector2(-0.05f - bulletSeparationDist, -0.05f), angleToPlayer), angleToPlayer + 0f, bulletVelocity);
		FireLinearBullet(fatBullet, rotate(center + new Vector2(0.05f + bulletSeparationDist, -0.05f), angleToPlayer), angleToPlayer + 0f, bulletVelocity);

        //down
		FireLinearBullet(fatBullet, rotate(center + new Vector2(0f, 0.05f), angleToPlayer), away, bulletVelocity);
		FireLinearBullet(fatBullet, rotate(center + new Vector2( - .05f - bulletSeparationDist,  + .05f), angleToPlayer), away, bulletVelocity);
		FireLinearBullet(fatBullet, rotate(center + new Vector2( + .05f + bulletSeparationDist,  + .05f), angleToPlayer), away, bulletVelocity);

        //upperLeft
		FireLinearBullet(thinBullet, rotate(center + new Vector2(-0.0125f - bulletSeparationDist * 4, 0.125f + bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 0f, bulletVelocity);
		FireLinearBullet(thinBullet, rotate(center + new Vector2(-0.0125f - bulletSeparationDist * 4, 0.125f + bulletSeparationDist * 2), angleToPlayer), left, bulletVelocity);

        //uperRight
		FireLinearBullet(thinBullet, rotate(center + new Vector2(0.0125f + bulletSeparationDist * 4, 0.125f + bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 0f, bulletVelocity);
		FireLinearBullet(thinBullet, rotate(center + new Vector2(0.0125f + bulletSeparationDist * 4, 0.125f + bulletSeparationDist * 2), angleToPlayer), right, bulletVelocity);

        //lowerLeft
		FireLinearBullet(thinBullet, rotate(center + new Vector2(-0.0125f - bulletSeparationDist * 4, -0.125f - bulletSeparationDist * 2), angleToPlayer), away, bulletVelocity);
		FireLinearBullet(thinBullet, rotate(center + new Vector2(-0.0125f - bulletSeparationDist * 4, -0.125f - bulletSeparationDist * 2), angleToPlayer), left, bulletVelocity);

        //lowerRight
		FireLinearBullet(thinBullet, rotate(center + new Vector2( + .0125f + bulletSeparationDist * 4, - 0.125f - bulletSeparationDist * 2), angleToPlayer), away, bulletVelocity);
		FireLinearBullet(thinBullet, rotate(center + new Vector2( + .0125f + bulletSeparationDist * 4, - 0.125f - bulletSeparationDist * 2), angleToPlayer), right, bulletVelocity);
    }

    Vector2 rotate(Vector2 orig, float angle)
    {
        return new Vector2(
            orig.x * Mathf.Cos(angle * Util.Degree2Rad) - orig.y * Mathf.Sin(angle * Util.Degree2Rad),
            orig.x * Mathf.Sin(angle * Util.Degree2Rad) + orig.y * Mathf.Cos(angle * Util.Degree2Rad));
    }
}
