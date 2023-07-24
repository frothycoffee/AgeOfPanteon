using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    public EntityType entityType;

    public bool isEntityPreviewable;

    public bool isEntityManufacturer;

    [ConditionalHide("isEntityManufacturer", true)]
    public EntityType manufactureType;

    public string entityName;

    public string entityInfo;

    public Sprite productSprite;

    public Sprite previewSprite;

    public Sprite infoSprite;

    public GameObject entityPrefab;

    public int width;

    public int height;

    public float entityMaxHealth;

    public List<Vector2Int> GetGridPositionList(Vector2Int origin)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridPositionList.Add(origin + new Vector2Int(x, y));
            }
        }

        return gridPositionList;
    }
}

public enum EntityType
{
    Building,
    Commander,
    Soldier,
    Peasant,
    Animal
}