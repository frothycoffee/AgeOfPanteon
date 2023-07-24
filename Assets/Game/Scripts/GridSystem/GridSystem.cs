using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInput;
using Managers;

public class GridSystem : Singleton<GridSystem>
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 10f;

    public Grid<Tile> grid;

    protected override void Awake()
    {
        grid = new Grid<Tile>(gridWidth, gridHeight, cellSize, Vector3.zero, (Grid<Tile> g, int x, int y) => new Tile(g, x, y));
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXY(worldPosition, out int x, out int y);
        return new Vector2Int(x, y);
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = InputManager.Instance.mouseUser.mouseInWorldPosition;

        grid.GetXY(mousePosition, out int x, out int y);

        Vector3 SnappedPosition = grid.GetWorldPosition(x, y) * grid.GetCellSize();

        return SnappedPosition;
    }

    public List<Tile> GetTilesNeighbourList(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();
        int gridSizeX = grid.GetWidth();
        int gridSizeY = grid.GetHeight();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = tile.x + x;
                int checkY = tile.y + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if (!grid.GetTile(checkX, checkY).isOccupied)
                    {
                        neighbours.Add(grid.GetTile(checkX, checkY));
                    }
                }
            }
        }

        return neighbours;
    }

    public Vector3 GetClosestPosOfNeighbours(Vector3 currentTilePos, Tile targetTile)
    {
        List<Tile> neighbours = GetTilesNeighbourList(targetTile);

        float tempDistance;
        float closestDistance = Vector3.Distance(GetTileWorldPosition(neighbours[0]), currentTilePos);
        Vector3 closestPos = currentTilePos;

        for (int i = 0; i < neighbours.Count; i++)
        {
            tempDistance = Vector3.Distance(GetTileWorldPosition(neighbours[i]), currentTilePos);

            if (tempDistance <= closestDistance)
            {
                closestDistance = tempDistance;
                closestPos = GetTileWorldPosition(neighbours[i]);
            }
        }

        return closestPos;
    }

    public Vector3 GetClosestPosOfNeighbours(Vector3 currentTilePos, Entity targetEntity)
    {
        List<Tile> neighbours = new List<Tile>();

        List<Vector2Int> gridPosList = targetEntity.data.GetGridPositionList(GetGridPosition(targetEntity.transform.position));

        for (int i = 0; i < gridPosList.Count; i++)
        {
            for (int k = 0; k < GetTilesNeighbourList(grid.GetTile(gridPosList[i].x, gridPosList[i].y)).Count; k++)
            {
                neighbours.Add(GetTilesNeighbourList(grid.GetTile(gridPosList[i].x, gridPosList[i].y))[k]);
            }
        }

        if (neighbours.Count == 0)
        {
            return currentTilePos;
        }
        else
        {
            float tempDistance;

            float closestDistance = Vector3.Distance(GetTileWorldPosition(neighbours[0]), currentTilePos);

            Vector3 closestPos = currentTilePos;

            for (int i = 0; i < neighbours.Count; i++)
            {
                tempDistance = Vector3.Distance(GetTileWorldPosition(neighbours[i]), currentTilePos);

                if (tempDistance <= closestDistance)
                {
                    closestDistance = tempDistance;
                    closestPos = GetTileWorldPosition(neighbours[i]);
                }
            }

            return closestPos;
        }
    }

    public Tile GetTile(Vector3 worldPos)
    {
        grid.GetXY(worldPos, out int x, out int y);
        return grid.GetTile(x, y);
    }

    public Vector3 GetTileWorldPosition(Tile tile)
    {
        Vector3 worldPos = grid.GetWorldPosition(tile.x, tile.y);
        return worldPos;
    }

    public Entity GetTilesEntity(Tile tile)
    {
        return tile.entity;
    }

    public Entity GetTilesEntity(Vector3 mousePos)
    {
        return GetTile(mousePos).entity;
    }

    public Entity GetTilesEntity(Vector2Int gridPosition)
    {
        return grid.GetTile(gridPosition.x, gridPosition.y).entity;
    }

    public bool IsGridAreaSuitable(EntityData entityData, Vector2Int origin)
    {
        List<Vector2Int> gridPositionList = entityData.GetGridPositionList(origin);

        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!grid.GetTile(gridPosition.x, gridPosition.y).CanBuild())
            {
                return false;
            }
        }
        return true;
    }

    public Vector3 GetNearestSuitablePosition(EntityData entityData, Vector3 spawnPos)
    {
        Vector3 suitablePos = spawnPos;

        while (true)
        {
            int x = UnityEngine.Random.Range(-1, 2);
            int y = UnityEngine.Random.Range(-1, 2);

            suitablePos = new Vector3(suitablePos.x + x, suitablePos.y + y);

            Vector2Int newOrigin = GetGridPosition(suitablePos);

            if (IsGridAreaSuitable(entityData, newOrigin))
                break;
        }

        return suitablePos;
    }
}