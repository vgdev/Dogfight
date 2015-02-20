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
    private Vector2 center;

    [SerializeField]
    private Vector2 upperLeft;
    [SerializeField]
    private Vector2 upperRight;
    [SerializeField]
    private Vector2 lowerLeft;
    [SerializeField]
    private Vector2 lowerRight;

    // this is called every time the attack pattern starts
    protected override void OnExecutionStart()
    {
        base.OnExecutionStart();
        crossDelay.Reset();
        //left
        FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y), 90f, bulletVelocity);
        FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y + .05f), 90f, bulletVelocity);
        FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y - .05f), 90f, bulletVelocity);

        //right
        FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y), 270f, bulletVelocity);
        FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y + .05f), 270f, bulletVelocity);
        FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y - .05f), 270f, bulletVelocity);

        //up
        FireLinearBullet(fatBullet, new Vector2(center.x, center.y - .05f), 0f, bulletVelocity);
        FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y - .05f), 0f, bulletVelocity);
        FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y - .05f), 0f, bulletVelocity);

        //down
        FireLinearBullet(fatBullet, new Vector2(center.x, center.y + .05f), 180f, bulletVelocity);
        FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y + .05f), 180f, bulletVelocity);
        FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y + .05f), 180f, bulletVelocity);

        FireLinearBullet(thinBullet, upperLeft, 0f, bulletVelocity);
        FireLinearBullet(thinBullet, upperLeft, 90f, bulletVelocity);

        FireLinearBullet(thinBullet, upperRight, 0f, bulletVelocity);
        FireLinearBullet(thinBullet, upperRight, 270f, bulletVelocity);

        FireLinearBullet(thinBullet, lowerLeft, 180f, bulletVelocity);
        FireLinearBullet(thinBullet, lowerLeft, 90f, bulletVelocity);

        FireLinearBullet(thinBullet, lowerRight, 180f, bulletVelocity);
        FireLinearBullet(thinBullet, lowerRight, 270f, bulletVelocity);
    }

    protected override void MainLoop(float dt)
    {
        base.MainLoop(dt);
        if (crossDelay.Tick(dt))
        {
            //left
            FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y), 90f, bulletVelocity);
            FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y + .05f), 90f, bulletVelocity);
            FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y - .05f), 90f, bulletVelocity);

            //right
            FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y), 270f, bulletVelocity);
            FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y + .05f), 270f, bulletVelocity);
            FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y - .05f), 270f, bulletVelocity);

            //up
            FireLinearBullet(fatBullet, new Vector2(center.x, center.y - .05f), 0f, bulletVelocity);
            FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y - .05f), 0f, bulletVelocity);
            FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y - .05f), 0f, bulletVelocity);

            //down
            FireLinearBullet(fatBullet, new Vector2(center.x, center.y + .05f), 180f, bulletVelocity);
            FireLinearBullet(fatBullet, new Vector2(center.x - .05f, center.y + .05f), 180f, bulletVelocity);
            FireLinearBullet(fatBullet, new Vector2(center.x + .05f, center.y + .05f), 180f, bulletVelocity);

            FireLinearBullet(thinBullet, upperLeft, 0f, bulletVelocity);
            FireLinearBullet(thinBullet, upperLeft, 90f, bulletVelocity);

            FireLinearBullet(thinBullet, upperRight, 0f, bulletVelocity);
            FireLinearBullet(thinBullet, upperRight, 270f, bulletVelocity);

            FireLinearBullet(thinBullet, lowerLeft, 180f, bulletVelocity);
            FireLinearBullet(thinBullet, lowerLeft, 90f, bulletVelocity);

            FireLinearBullet(thinBullet, lowerRight, 180f, bulletVelocity);
            FireLinearBullet(thinBullet, lowerRight, 270f, bulletVelocity);
        }
    }
}
