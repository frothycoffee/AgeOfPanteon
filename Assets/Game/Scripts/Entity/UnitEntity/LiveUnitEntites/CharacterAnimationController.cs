using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator _animator;

    public bool isOnAttack;

    private float _dirX = 0;
    private float _dirY = 0;
    private float _angle = 0;

    private void OnEnable()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void ActivateAttackAnimation()
    {
        _animator.SetBool("attack", true);
        isOnAttack = true;
    }

    public void DeactivateAttackAnimation()
    {
        _animator.SetBool("attack", false);
        isOnAttack = false;
        GetComponent<PathfindingUser>().isAttackTriggered = false;
    }

    public void StartMoveAnimation()
    {
        DeactivateAttackAnimation();
        _animator.SetBool("isMoving", true);
    }

    public void FinishMoveAnimation()
    {
        _dirX = 0;
        _dirY = 0;
        _animator.SetBool("isMoving", false);
    }

    public void ChangeAnimationDirection(Vector2 currentPos, Vector2 targetPos)
    {
        GetCharDirection(currentPos, targetPos);
        
        _animator.SetFloat("horizontalMovement", _dirX);
        _animator.SetFloat("verticalMovement", _dirY);
        _animator.SetFloat("attackAngle", _angle);
    }

    private void GetCharDirection(Vector2 currentPos, Vector2 targetPos)
    {
        if (targetPos.x > currentPos.x)
        {
            //Right
            _dirX = 1;
            _dirY = 0;
            _angle = 0;
        }
        else if (targetPos.y > currentPos.y)
        {
            //Top
            _dirX = 0;
            _dirY = 1;
            _angle = 90;
        }
        else if (targetPos.x < currentPos.x)
        {
            //Left
            _dirX = -1;
            _dirY = 0;
            _angle = 180;
        }
        else if (targetPos.y < currentPos.y)
        {
            //Bottom
            _dirX = 0;
            _dirY = -1;
            _angle = 270;
        }
    }
}
