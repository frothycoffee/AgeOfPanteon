using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveUnitEntity : Entity
{
    private LiveUnitEntityData liveUnitData;
    public CharacterAnimationController charAnimationController;

    protected override void Start()
    {
        base.Start();

        liveUnitData = (LiveUnitEntityData)data;

        gameObject.AddComponent<PathfindingUser>();
        gameObject.AddComponent<CharacterAnimationController>();

        GetComponent<PathfindingUser>().speed = liveUnitData.speed;
        charAnimationController = GetComponent<CharacterAnimationController>();
    }

    public void Move(Vector3 targetPos)
    {
        GetComponent<PathfindingUser>().RequesPath(targetPos);
        charAnimationController.StartMoveAnimation();
    }
}