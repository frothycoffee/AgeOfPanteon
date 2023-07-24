using UnityEngine;

public interface IEntityFactory<T> where T : Entity
{
    T Create(Vector3 spawnPoint);
}
