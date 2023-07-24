using UnityEngine;

public class EntitySpawner<T> where T : Entity
{
    IEntityFactory<T> entityFactory;
    Vector3 spawnPoint;

    public EntitySpawner(IEntityFactory<T> entityFactory, Vector3 spawnPoint)
    {
        this.entityFactory = entityFactory;
        this.spawnPoint = spawnPoint;
    }

    public T Spawn()
    {
        return entityFactory.Create(spawnPoint);
    }
}