using UnityEngine;
using System.Collections;
using UnityUtilLib;
using Danmaku2D;
using Danmaku2D.DanmakuControllers;

public class BlitzKrieg : TimedAttackPattern
{
    [SerializeField]
    private DanmakuPrefab prefab;

    [SerializeField]
    private int bulletCount;

    [SerializeField]
    private Counter burstCount;

    [SerializeField]
    private FrameCounter burstDelay;

    [SerializeField]
    [Range(-180f, 180f)]
    private float burstInitialRotation;

    [SerializeField]
    [Range(-360f, 360f)]
    private float burstRotationDelta;

    [SerializeField]
    private AccelerationController LinearController;

    private DanmakuGroup burstGroup;

    public override void Awake()
    {
        base.Awake();
        burstGroup = new DanmakuGroup();
        burstGroup.AddController(LinearController);
    }

    [SerializeField]
    private FrameCounter crossDelay;

    [SerializeField]
    private Vector2 centerArea;

    private float vertical=0;

    // this is called every time the attack pattern starts
    protected override void OnInitialize()
    {
		base.OnInitialize();
        crossDelay.Reset();
        vertical = 0;
        fire();
    }

    protected override void MainLoop()
    {
        base.MainLoop();
        if (crossDelay.Tick())
        {
            fire();
        }
    }

    void fire()
    {
        Vector2 center = centerArea.Random();
        center.y += vertical;
        vertical += .2f;
        float offset = (burstCount.MaxCount - burstCount.Count) * burstRotationDelta;
        for (int i = 0; i < bulletCount; i++)
        {
            Danmaku temp = SpawnDanmaku(prefab, center, offset + 360f / (float)bulletCount * (float)i);
            burstGroup.Add(temp);
        }
    }
}
