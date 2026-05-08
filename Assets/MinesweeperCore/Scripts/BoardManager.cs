using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public int mineCount = 10;

    public int CorrectFlagsCount; // Số cờ đã cắm đúng vị trí bom

    public GameObject tilePrefab;

    public static BoardManager Instance; // Thêm dòng này

    [HideInInspector]
    public Tile[,] tiles;

    void Awake()
    {
        Instance = this; // Gán instance khi game bắt đầu
    }

    void Start()
    {
        GenerateBoard();
        PlaceMines();
        CalculateAdjacentMines();
    }

    public void GenerateBoard()
    {
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Tính toán vị trí tương đối so với vị trí của BoardManager
                Vector3 spawnPos = transform.position + new Vector3(x, 0, y);

                // Nếu bạn muốn các ô có khoảng cách (padding), hãy nhân x và y với một con số (vd: x * 1.1f)
                GameObject go = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

                Tile tile = go.GetComponent<Tile>();
                tile.Init(x, y, this);
                tiles[x, y] = tile;
            }
        }
    }

    void PlaceMines()
    {
        int placed = 0;
        while (placed < mineCount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (!tiles[x, y].isMine)
            {
                tiles[x, y].isMine = true;
                placed++;
            }
        }
    }

    void CalculateAdjacentMines()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int count = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int nx = x + i;
                        int ny = y + j;

                        if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        {
                            if (tiles[nx, ny].isMine)
                                count++;
                        }
                    }
                }
                tiles[x, y].adjacentMines = count;
            }
        }
    }
/*
    public void FloodReveal(int startX, int startY)
    {
        Queue<Tile> queue = new Queue<Tile>();
        queue.Enqueue(tiles[startX, startY]);

        while (queue.Count > 0)
        {
            Tile t = queue.Dequeue();

            for (int nx = t.x - 1; nx <= t.x + 1; nx++)
            {
                for (int ny = t.y - 1; ny <= t.y + 1; ny++)
                {
                    if (!IsInsideBoard(nx, ny)) continue;

                    Tile neighbor = tiles[nx, ny];

                    if (neighbor.isRevealed || neighbor.isMine) continue;

                    neighbor.Reveal();

                    if (neighbor.adjacentMines == 0)
                        queue.Enqueue(neighbor);
                }
            }
        }
    }

    bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
*/
}
