using System.Collections.Generic;
using UnityEngine;

public class EntitySpawnManager : Singleton<EntitySpawnManager>
{
    public EntityData[] entityDatas;

    public List<Entity> entityPool;

    private EntityData _entityData;
    private Vector3 _spawnPoint;

    private EntitySpawner<Entity> spawner;

    protected override void Awake()
    {

    }

    private void OnEnable()
    {
        EventManager.AddHandler(gameEvent: GameEvent.RightMouseButtonActionPerformed, OnRightMouseButtonActionPerformed);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(gameEvent: GameEvent.RightMouseButtonActionPerformed, OnRightMouseButtonActionPerformed);
    }

    public Entity Spawn(string entityName, Vector3 spawnPoint)
    {
        _entityData = GetEntityDataByName(entityName);
        _spawnPoint = spawnPoint;

        Entity entityInstance = TrySpawnFromPool(entityName, spawnPoint);

        if (entityInstance == null)
        {
            spawner = new EntitySpawner<Entity>(entityFactory: new EntityFactory<Entity>(_entityData), _spawnPoint);

            entityInstance = spawner.Spawn();

            return entityInstance;
        }
        return entityInstance;
    }

    public EntityData GetEntityDataByName(string entityName)
    {
        foreach (var itemData in entityDatas)
        {
            if (itemData.entityName == entityName)
            {
                return itemData;
            }
        }

        Debug.LogError("unidentifed entity");
        return null;
    }

    public void AddDestroyedObjectToPool(Entity entity)
    {
        entityPool.Add(entity);
    }

    public Entity TrySpawnFromPool(string entityName, Vector3 spawnPoint)
    {
        foreach (var item in entityPool)
        {
            if (item.data.name == entityName)
            {
                item.transform.position = spawnPoint;
                item.gameObject.SetActive(true);
                item.currentHealth = item.data.entityMaxHealth;
                entityPool.Remove(item);
                return item;
            }
        }
        return null;
    }

    private void OnRightMouseButtonActionPerformed()
    {
        if (SelectionManager.Instance.selectedEntity != null
            && SelectionManager.Instance.selectedEntity.GetComponent<BuildingEntity>()
            && !InputManager.Instance.IsMouseOverUI())
        {
            SetObjectsSpawnPoint(SelectionManager.Instance.selectedEntity);
        }
    }

    private void SetObjectsSpawnPoint(Entity entity)
    {
        Vector3 spawnPos = GridSystem.Instance.GetMouseWorldSnappedPosition();
        entity.SetSpawnPoint(spawnPos);
    }
}