using UnityEngine;

public class EntityFactory<T> : IEntityFactory<T> where T : Entity
{
    EntityData data;

    public EntityFactory(EntityData data)
    {
        this.data = data;
    }

    public T Create(Vector3 spawnPoint)
    {
        EntityData entityData = data;
        
        GameObject instance = Object.Instantiate(entityData.entityPrefab, spawnPoint, Quaternion.identity);

        instance.GetComponent<Entity>().data = entityData;

        return instance.GetComponent<T>();
    }
}
