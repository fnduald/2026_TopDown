using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SimpleLoguelikeManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile baseTile;
    public Tile wallTile;
    public GameObject monsterPrefab;

    private int[,] mapData = new int[11, 11];
    private List<MonsterController> monsterList = new List<MonsterController>();

    void Start()
    {
        if (tilemap != null)
        {
            tilemap.ClearAllTiles();
        }

        GenerateMapData();
        RenderTilemap();

        PlayerController player = Object.FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.SpawnAtCell(5, 5, this);
        }
    }

    void GenerateMapData()
    {
        monsterList.Clear();

        // 외곽 테두리 벽(1), 내부 바닥(0)
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 11; y++)
            {
                if (x == 0 || x == 10 || y == 0 || y == 10)
                    mapData[x, y] = 1;
                else
                    mapData[x, y] = 0;
            }
        }

        // 몬스터 프리팹 연동 체크 경고 추가
        if (monsterPrefab == null)
        {
            Debug.LogError("[에러] LoguelikeManager 인스펙터 창에 Monster Prefab이 비어있습니다! 프리팹을 드래그해서 넣어주세요!");
            return;
        }

        if (tilemap != null)
        {
            int monsterCount = 0;
            while (monsterCount < 3)
            {
                int rx = Random.Range(1, 10);
                int ry = Random.Range(1, 10);

                if (!(rx == 5 && ry == 5))
                {
                    Vector3Int cellPos = new Vector3Int(rx, ry, 0);
                    Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);

                    GameObject monsterObj = Instantiate(monsterPrefab, worldPos, Quaternion.identity);

                    MonsterController monster = monsterObj.GetComponent<MonsterController>();
                    if (monster != null)
                    {
                        monster.SetupMonster(rx, ry, this);
                        monsterList.Add(monster);
                    }

                    monsterCount++;
                }
            }
            Debug.Log($"[몬스터 스폰 완료] 총 {monsterCount}마리의 몬스터가 정상 생성되었습니다.");
        }
    }

    void RenderTilemap()
    {
        if (tilemap == null) return;

        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 11; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);

                if (mapData[x, y] == 1)
                    tilemap.SetTile(tilePos, wallTile);
                else
                    tilemap.SetTile(tilePos, baseTile);
            }
        }
    }

    public bool CanMoveTo(int x, int y)
    {
        if (x < 0 || x >= 11 || y < 0 || y >= 11) return false;
        return mapData[x, y] == 0;
    }

    public Vector3 GetWorldPositionOfCell(int x, int y)
    {
        if (tilemap == null) return Vector3.zero;
        Vector3Int cellPos = new Vector3Int(x, y, 0);
        return tilemap.GetCellCenterWorld(cellPos);
    }

    public void MoveAllMonsters()
    {
        foreach (MonsterController monster in monsterList)
        {
            if (monster != null)
            {
                monster.MoveRandomly();
            }
        }
    }
}