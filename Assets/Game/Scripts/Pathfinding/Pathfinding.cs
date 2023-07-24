using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : MonoBehaviour
{
	PathRequestManager requestManager;
	GridSystem _gridSystem;

	void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();
		_gridSystem = GridSystem.Instance;
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos)
	{
		StartCoroutine(FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Tile startTile = _gridSystem.GetTile(startPos);
		Tile targetTile = _gridSystem.GetTile(targetPos);

		Heap<Tile> openSet = new Heap<Tile>(_gridSystem.grid.GetGridSize());

		HashSet<Tile> closedSet = new HashSet<Tile>();

		openSet.Add(startTile);

		while (openSet.Count > 0)
		{
			Tile currentTile = openSet.RemoveFirst();

			closedSet.Add(currentTile);

			if (currentTile == targetTile)
			{
				pathSuccess = true;
				break;
			}

			foreach (Tile neighbour in _gridSystem.GetTilesNeighbourList(currentTile))
			{
				if (neighbour.isOccupied || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);

				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetTile);
					neighbour.parent = currentTile;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
					else
						openSet.UpdateItem(neighbour);
				}
			}
		}

		yield return null;
		if (pathSuccess)
		{
			waypoints = RetracePath(startTile, targetTile);
			pathSuccess = waypoints.Length > 0;
		}
		requestManager.FinishedProcessingPath(waypoints, pathSuccess);
	}

	Vector3[] RetracePath(Tile startTile, Tile endTile)
	{
		List<Tile> path = new List<Tile>();

		Tile currentTile = endTile;

		while (currentTile != startTile)
		{
			path.Add(currentTile);
			currentTile = currentTile.parent;
		}

		Vector3[] waypoints = GetWaypointsFromPath(path);

		Array.Reverse(waypoints);
		
		return waypoints;
	}

	Vector3[] GetWaypointsFromPath(List<Tile> path)
	{
		List<Vector3> waypoints = new List<Vector3>();

		for (int i = 0; i < path.Count; i++)
		{
			waypoints.Add(_gridSystem.GetTileWorldPosition(path[i]));
		}

		return waypoints.ToArray();
	}

	int GetDistance(Tile tileA, Tile tileB)
	{
		int dstX = Mathf.Abs(tileA.x - tileB.x);
		int dstY = Mathf.Abs(tileA.y - tileB.y);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}