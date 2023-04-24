using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MazeGenerator : MonoBehaviourPunCallbacks {

    public int mazeWidth;
    public int mazeHeight;
    public GameObject wallPrefab;
    public GameObject tankPrefab;

    private MazeCell[,] mazeCells;

    void Start () {
        GenerateMaze();
    }

    void GenerateMaze() {
        // Initialize the maze cells
        mazeCells = new MazeCell[mazeWidth, mazeHeight];
        for (int x = 0; x < mazeWidth; x++) {
            for (int y = 0; y < mazeHeight; y++) {
                mazeCells[x, y] = new MazeCell();
            }
        }

        // Generate the maze using a recursive backtracker algorithm
        RecursiveBacktrackerAlgorithm.GenerateMaze(mazeCells, mazeWidth, mazeHeight);

        // Spawn the tanks inside the maze
        int numPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = 0; i < numPlayers; i++) {
            Vector2Int tankPos = RecursiveBacktrackerAlgorithm.GetRandomMazePosition(mazeWidth, mazeHeight);
            GameObject tank = PhotonNetwork.Instantiate(tankPrefab.name, new Vector3(tankPos.x, 0, tankPos.y), Quaternion.identity);
        }

        // Instantiate the maze walls
        for (int x = 0; x < mazeWidth; x++) {
            for (int y = 0; y < mazeHeight; y++) {
                if (mazeCells[x, y].walls[0]) {
                    Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity);
                }
                if (mazeCells[x, y].walls[1]) {
                    Instantiate(wallPrefab, new Vector3(x+1, 0, y), Quaternion.Euler(0, 90, 0));
                }
                if (mazeCells[x, y].walls[2]) {
                    Instantiate(wallPrefab, new Vector3(x, 0, y+1), Quaternion.identity);
                }
                if (mazeCells[x, y].walls[3]) {
                    Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0));
                }
            }
        }
    }
}

public class MazeCell {
    public bool[] walls = new bool[] { true, true, true, true };
}

public static class RecursiveBacktrackerAlgorithm {
    public static void GenerateMaze(MazeCell[,] mazeCells, int mazeWidth, int mazeHeight) {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int currentCell = new Vector2Int(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight));
        stack.Push(currentCell);

        while (stack.Count > 0) {
            currentCell = stack.Peek();
            List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(currentCell, mazeCells, mazeWidth, mazeHeight);
            if (unvisitedNeighbors.Count > 0) {
                Vector2Int nextCell = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                RemoveWall(currentCell, nextCell, mazeCells);
                stack.Push(nextCell);
            } else {
                stack.Pop();
            }
        }
    }

    public static Vector2Int GetRandomMazePosition(int mazeWidth, int mazeHeight) {
        return new Vector2Int(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight));
    }

    public static List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell, MazeCell[,] mazeCells, int mazeWidth, int mazeHeight)
    {
        List<Vector2Int> unvisitedNeighbors = new List<Vector2Int>();
        int x = cell.x;
        int y = cell.y;

        if (x > 0 && !mazeCells[x - 1, y].walls[1])
        {
            unvisitedNeighbors.Add(new Vector2Int(x - 1, y));
        }
        if (x < mazeWidth - 1 && !mazeCells[x, y].walls[1])
        {
            unvisitedNeighbors.Add(new Vector2Int(x + 1, y));
        }
        if (y > 0 && !mazeCells[x, y - 1].walls[2])
        {
            unvisitedNeighbors.Add(new Vector2Int(x, y - 1));
        }
        if (y < mazeHeight - 1 && !mazeCells[x, y].walls[2])
        {
            unvisitedNeighbors.Add(new Vector2Int(x, y + 1));
        }

        return unvisitedNeighbors;
    }

    private static void RemoveWall(Vector2Int currentCell, Vector2Int nextCell, MazeCell[,] mazeCells)
    {
        int dx = nextCell.x - currentCell.x;
        int dy = nextCell.y - currentCell.y;

        if (dx == 1)
        {
            mazeCells[currentCell.x, currentCell.y].walls[1] = false;
            mazeCells[nextCell.x, nextCell.y].walls[0] = false;
        }
        else if (dx == -1)
        {
            mazeCells[currentCell.x, currentCell.y].walls[0] = false;
            mazeCells[nextCell.x, nextCell.y].walls[1] = false;
        }
        else if (dy == 1)
        {
            mazeCells[currentCell.x, currentCell.y].walls[2] = false;
            mazeCells[nextCell.x, nextCell.y].walls[3] = false;
        }
        else if (dy == -1)
        {
            mazeCells[currentCell.x, currentCell.y].walls[3] = false;
            mazeCells[nextCell.x, nextCell.y].walls[2] = false;
        }
    }
}

