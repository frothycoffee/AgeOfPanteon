using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacer : MonoBehaviour
{
    private GridSystem _gridSystem;

    private void Awake()
    {
        _gridSystem = GridSystem.Instance;
    }

    private void OnEnable()
    {
        EventManager.AddHandler(gameEvent: GameEvent.LeftMouseButtonActionPerformed, OnLeftMouseButtonActionPerformed);
        EventManager.AddHandler(gameEvent: GameEvent.RightMouseButtonActionPerformed, OnRightMouseButtonActionPerformed);
        EventManager.AddHandler(gameEventString: GameEvent.SpawnEntity, actionString: Spawn);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(gameEvent: GameEvent.LeftMouseButtonActionPerformed, OnLeftMouseButtonActionPerformed);
        EventManager.RemoveHandler(gameEvent: GameEvent.RightMouseButtonActionPerformed, OnRightMouseButtonActionPerformed);
        EventManager.RemoveHandler(gameEventString: GameEvent.SpawnEntity, actionString: Spawn);
    }

    private void Spawn(string entityName)
    {
        EntityData entityData = EntitySpawnManager.Instance.GetEntityDataByName(entityName);
        Vector3 spawnPos = SelectionManager.Instance.selectedEntity.manufactureSpawnPoint;

        PlaceEntityOnGrid(entityData, spawnPos);
    }

    public void PlaceEntityOnGrid(EntityData entityData, Vector3 spawnPosition)
    {
        _gridSystem.grid.GetXY(spawnPosition, out int x, out int y);

        Vector2Int entityOrigin = new Vector2Int(x, y);

        if (_gridSystem.IsGridAreaSuitable(entityData, entityOrigin))
        {
            Vector3 entityGridWorldPosition = _gridSystem.grid.GetWorldPosition(entityOrigin.x, entityOrigin.y) * _gridSystem.grid.GetCellSize();

            Entity placedEntity = EntitySpawnManager.Instance.Spawn(entityData.entityName, entityGridWorldPosition);
            placedEntity.childSpriteRenderer.sortingOrder = -entityOrigin.y;

            placedEntity.origin = entityOrigin;

            List<Vector2Int> gridPositionList = entityData.GetGridPositionList(entityOrigin);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                _gridSystem.grid.GetTile(gridPosition.x, gridPosition.y).SetPlacedEntity(placedEntity);
            }
        }
        else
        {
            Vector3 newPos = _gridSystem.GetNearestSuitablePosition(entityData, spawnPosition);
            PlaceEntityOnGrid(entityData, newPos);
        }
    }

    public void PlaceEntityOnGrid(EntityData entityData)
    {
        Vector3 mousePosition = InputManager.Instance.mouseUser.mouseInWorldPosition;

        _gridSystem.grid.GetXY(mousePosition, out int x, out int y);

        Vector2Int entityOrigin = new Vector2Int(x, y);

        if (_gridSystem.IsGridAreaSuitable(entityData, entityOrigin))
        {
            Vector3 entityGridWorldPosition = _gridSystem.grid.GetWorldPosition(entityOrigin.x, entityOrigin.y) * _gridSystem.grid.GetCellSize();

            Entity placedEntity = EntitySpawnManager.Instance.Spawn(entityData.entityName, entityGridWorldPosition);
            placedEntity.childSpriteRenderer.sortingOrder = -entityOrigin.y;

            placedEntity.origin = entityOrigin;

            List<Vector2Int> gridPositionList = entityData.GetGridPositionList(entityOrigin);

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                _gridSystem.grid.GetTile(gridPosition.x, gridPosition.y).SetPlacedEntity(placedEntity);
            }

            SelectionManager.Instance.onPreviewEntityData = null;

            EventManager.Brodcast(gameEvent: GameEvent.ClearEntityPreview);
        }
    }

    private void OnLeftMouseButtonActionPerformed()
    {
        if (SelectionManager.Instance.onPreviewEntityData != null && !InputManager.Instance.IsMouseOverUI())
        {
            PlaceEntityOnGrid(SelectionManager.Instance.onPreviewEntityData);
        }
    }

    private void OnRightMouseButtonActionPerformed()
    {
        if (SelectionManager.Instance.onPreviewEntityData != null && !InputManager.Instance.IsMouseOverUI())
        {
            SelectionManager.Instance.onPreviewEntityData = null;
            EventManager.Brodcast(GameEvent.ClearEntityPreview);
        }
    }
}