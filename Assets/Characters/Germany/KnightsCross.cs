using UnityEngine;
using System.Collections;
using UnityUtilLib;


public class KnightsCross : AbstractTimedAttackPattern
{
    [SerializeField]
    private CountdownDelay crossDelay;

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
    protected override void OnExecutionStart()
    {
        base.OnExecutionStart();
        crossDelay.Reset();
        Vector2 center = new Vector2(.5f, .5f);//Util.RandomVect2(centerArea);
        float angleToPlayer = TargetField.AngleTowardPlayer(TargetField.WorldPoint(center));
        fire();
    }

    protected override void MainLoop(float dt)
    {
        base.MainLoop(dt);
        if (crossDelay.Tick(dt))
        {
            fire();
        }
    }

    void fire()
    {

        Debug.Log(angleToPlayer);

        //left
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x - .05f, center.y), angleToPlayer), angleToPlayer + 90f, bulletVelocity);
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x - .05f, center.y + .05f + bulletSeparationDist), angleToPlayer), angleToPlayer + 90f, bulletVelocity);
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x - .05f, center.y - .05f - bulletSeparationDist), angleToPlayer), angleToPlayer + 90f, bulletVelocity);

        //right
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x + .05f, center.y), angleToPlayer), angleToPlayer + 270f, bulletVelocity);
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x + .05f, center.y + .05f + bulletSeparationDist), angleToPlayer), angleToPlayer + 270f, bulletVelocity);
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x + .05f, center.y - .05f - bulletSeparationDist), angleToPlayer), angleToPlayer + 270f, bulletVelocity);

        //up
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x, center.y - .05f), angleToPlayer), angleToPlayer + 0f, bulletVelocity);
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x - .05f - bulletSeparationDist, center.y - .05f), angleToPlayer), angleToPlayer + 0f, bulletVelocity);
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x + .05f + bulletSeparationDist, center.y - .05f), angleToPlayer), angleToPlayer + 0f, bulletVelocity);

        //down
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x, center.y + .05f), angleToPlayer), angleToPlayer + 180f, bulletVelocity);
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x - .05f - bulletSeparationDist, center.y + .05f), angleToPlayer), angleToPlayer + 180f, bulletVelocity);
        FireLinearBullet(fatBullet, rotate(new Vector2(center.x + .05f + bulletSeparationDist, center.y + .05f), angleToPlayer), angleToPlayer + 180f, bulletVelocity);

        //upperLeft
        FireLinearBullet(thinBullet, rotate(new Vector2(center.x - .0125f - bulletSeparationDist * 4, center.y + 0.125f + bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 0f, bulletVelocity);
        FireLinearBullet(thinBullet, rotate(new Vector2(center.x - .0125f - bulletSeparationDist * 4, center.y + 0.125f + bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 90f, bulletVelocity);

        //uperRight
        FireLinearBullet(thinBullet, rotate(new Vector2(center.x + .0125f + bulletSeparationDist * 4, center.y + 0.125f + bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 0f, bulletVelocity);
        FireLinearBullet(thinBullet, rotate(new Vector2(center.x + .0125f + bulletSeparationDist * 4, center.y + 0.125f + bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 270f, bulletVelocity);

        //lowerLeft
        FireLinearBullet(thinBullet, rotate(new Vector2(center.x - .0125f - bulletSeparationDist * 4, center.y - 0.125f - bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 180f, bulletVelocity);
        FireLinearBullet(thinBullet, rotate(new Vector2(center.x - .0125f - bulletSeparationDist * 4, center.y - 0.125f - bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 90f, bulletVelocity);

        //lowerRight
        FireLinearBullet(thinBullet, rotate(new Vector2(center.x + .0125f + bulletSeparationDist * 4, center.y - 0.125f - bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 180f, bulletVelocity);
        FireLinearBullet(thinBullet, rotate(new Vector2(center.x + .0125f + bulletSeparationDist * 4, center.y - 0.125f - bulletSeparationDist * 2), angleToPlayer), angleToPlayer + 270f, bulletVelocity);
    }

    Vector2 rotate(Vector2 orig, float angle)
    {
        return new Vector2(
            orig.x * Mathf.Cos(angle * Util.Degree2Rad) - orig.y * Mathf.Sin(angle * Util.Degree2Rad),
            orig.x * Mathf.Sin(angle * Util.Degree2Rad) + orig.y * Mathf.Cos(angle * Util.Degree2Rad));
    }
}
