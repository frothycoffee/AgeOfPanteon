using UnityEngine;
using System.Collections;

public class PathfindingUser : MonoBehaviour
{
    public Transform target;
    public float speed;
    public bool isAttackTriggered;
    public Vector3 currentAttackTarget;

    private Vector3[] _path;
    private int _targetIndex;

    Entity _entity;

    private void OnEnable()
    {
        _entity = GetComponent<Entity>();
    }

    public void RequesPath(Vector3 targetPos)
    {
        PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = newPath;
            _targetIndex = 0;
            
            if (GridSystem.Instance.GetTile(_entity.transform.position).entity == _entity)
            {
                GridSystem.Instance.GetTile(_entity.transform.position).ClearPlacedEntity();
            }
            StopCoroutine("FollowPath");
            if (GetComponent<Entity>().currentHealth > 0)
            {
                StartCoroutine("FollowPath");
            }
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = _path[0];

        while (true)
        {
            _entity.GetComponent<CharacterAnimationController>().ChangeAnimationDirection(transform.position, currentWaypoint);

            if (transform.position == currentWaypoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    GridSystem.Instance.GetTile(_entity.transform.position).SetPlacedEntity(_entity);
                    

                    if (isAttackTriggered)
                    {
                        _entity.GetComponent<CharacterAnimationController>().ActivateAttackAnimation();
                        _entity.GetComponent<MilitaryEntity>().Attack(GridSystem.Instance.GetTilesEntity(currentAttackTarget));
                        _entity.GetComponent<CharacterAnimationController>().ChangeAnimationDirection(transform.position, currentAttackTarget);
                    }
                    else
                    {
                        _entity.GetComponent<CharacterAnimationController>().FinishMoveAnimation();
                    }

                    yield break;
                }
                currentWaypoint = _path[_targetIndex];
            }
            else if (GridSystem.Instance.GetTile(currentWaypoint).entity != null)
            {
                Vector3 newTargetPos = currentWaypoint + Vector3.one;

                PathRequestManager.RequestPath(transform.position, newTargetPos, OnPathFound);

                yield break;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            GetComponent<Entity>().childSpriteRenderer.sortingOrder = - GridSystem.Instance.GetTile(currentWaypoint).y;
            yield return null;
        }
    }
}