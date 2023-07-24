using Managers;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamagable
{
    public SpriteRenderer childSpriteRenderer;
    public float currentHealth;

    [HideInInspector]
    public EntityData data;

    [HideInInspector]
    public Vector3 manufactureSpawnPoint; // This field is for manufacturable entities.
                                          // In case of isEntityManufacturable bool in Entity Data is false this will not be usable.
    [HideInInspector]
    public Vector2Int origin;

    protected virtual void Start()
    {
        GetComponent<ToolTipTrigger>().header = data.entityName;
        GetComponent<ToolTipTrigger>().content = data.entityInfo;
        manufactureSpawnPoint = transform.position + Vector3.up; //Default pos.
        currentHealth = data.entityMaxHealth;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return data.GetGridPositionList(origin);
    }

    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        manufactureSpawnPoint = spawnPoint;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        GridSystem.Instance.grid.GetXY(transform.position, out int x, out int y);
        Vector2Int origin = new Vector2Int(x, y);

        for (int i = 0; i < data.GetGridPositionList(origin).Count; i++)
        {
            if (GridSystem.Instance.grid.GetTile(GetGridPositionList()[i].x, GetGridPositionList()[i].y).entity == this)
                GridSystem.Instance.grid.GetTile(GetGridPositionList()[i].x, GetGridPositionList()[i].y).ClearPlacedEntity();
        }

        if (GridSystem.Instance.GetTile(transform.position).entity == this)
            GridSystem.Instance.GetTile(transform.position).ClearPlacedEntity();

        gameObject.SetActive(false);
        EntitySpawnManager.Instance.AddDestroyedObjectToPool(this);
    }
}