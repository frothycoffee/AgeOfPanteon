using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryEntity : LiveUnitEntity
{
    private MilitaryEntityData militaryData;

    public float currentDamage;

    protected override void Start()
    {
        base.Start();

        militaryData = (MilitaryEntityData)data;

        currentDamage = militaryData.entityDamage;
    }

    public void AttackPhase(Vector3 targetPos)
    {
        GetComponent<PathfindingUser>().isAttackTriggered = false;
        GetComponent<PathfindingUser>().RequesPath(GridSystem.Instance.GetClosestPosOfNeighbours(transform.position, GridSystem.Instance.GetTilesEntity(targetPos)));
        GetComponent<PathfindingUser>().currentAttackTarget = targetPos;
        charAnimationController.StartMoveAnimation();
        GetComponent<PathfindingUser>().isAttackTriggered = true;
    }

    public void Attack(Entity targetEntity)
    {
        StartCoroutine(AttackWithInterval(targetEntity, militaryData.attackInterval));
    }

    IEnumerator AttackWithInterval(Entity targetEntity , float attackInterval)
    {
        while (true)
        {
            if (GetComponent<CharacterAnimationController>().isOnAttack && targetEntity != null)
            {
                yield return new WaitForSeconds(attackInterval);

                targetEntity.TakeDamage(currentDamage);

                if (targetEntity.currentHealth <= 0)
                {
                    GetComponent<CharacterAnimationController>().isOnAttack = false;
                    GetComponent<CharacterAnimationController>().DeactivateAttackAnimation();
                    GetComponent<CharacterAnimationController>().FinishMoveAnimation();
                    break;
                }

                GetComponent<CharacterAnimationController>().isOnAttack = false;

                yield return new WaitForSeconds(attackInterval);

                GetComponent<CharacterAnimationController>().isOnAttack = true;
            }
            yield return null;
        }
    }
}