using UnityEngine;

public class Tile : IHeapItem<Tile>
{
    private Grid<Tile> grid;
    public int x;
    public int y;
    private int heapIndex;

    public Entity entity;
    public bool isOccupied;

    public int gCost;
    public int hCost;
    public Tile parent;

    public int HeapIndex { get => heapIndex; set => heapIndex = value; }

    public int FCost { get => gCost + hCost; }


    public Tile(Grid<Tile> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        entity = null;
    }

    public void SetPlacedEntity(Entity entity)
    {
        //Debug.Log(x + "," + y + "---" + "TILE SETTED = " + " " + entity);

        this.entity = entity;
        isOccupied = true;
        grid.TriggerTileChange(x, y, Color.red);
    }

    public void ClearPlacedEntity()
    {
        //Debug.Log(x + "," + y + "---" + "TILE CLEAR");

        entity = null;
        isOccupied = false;
        grid.TriggerTileChange(x, y, Color.white);
    }

    public Entity GetPlacedEntity()
    {
        return entity;
    }

    public bool CanBuild()
    {
        return entity == null;
    }

    public int CompareTo(Tile tileToCompare)
    {
        int compare = FCost.CompareTo(tileToCompare.FCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(tileToCompare.hCost);
        }
        return -compare;
    }
}