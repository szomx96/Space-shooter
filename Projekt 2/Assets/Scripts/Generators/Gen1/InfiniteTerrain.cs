using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{  
    const float viewerPieceUpdateDist = 25f;
    const float sqrViewerPieceUpdateDist = viewerPieceUpdateDist * viewerPieceUpdateDist;

    public static float viewDistance;
    public LevelOfDetailInfo[] detailLevels;

    public Transform player;

    public Material mapMaterial;
    public Material waterMat;

    public static Vector2 playerPos;
    Vector2 prevPlayerPos;
    public static MapGenerator mapGenerator;
    int pieceSize;
    int piecesVisibleInViewDist;
    public float waterHeight = 1.5f;

    Dictionary<Vector2, TerrainPiece> terrainPieceDict = new Dictionary<Vector2, TerrainPiece>();
    public static List<TerrainPiece> terrainPiecesVisibleLastUpdate = new List<TerrainPiece>();

    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();

        viewDistance = detailLevels[detailLevels.Length - 1].distance;
        pieceSize = MapGenerator.mapPieceSize - 1;
        piecesVisibleInViewDist = Mathf.RoundToInt(viewDistance / pieceSize);

        UpdateVisiblePieces();
    }

    private void Update()
    {
        playerPos = new Vector2(player.position.x, player.position.z) / mapGenerator.terrainInfo.scale;

        if ((prevPlayerPos - playerPos).sqrMagnitude > sqrViewerPieceUpdateDist)
        {
            prevPlayerPos = playerPos;
            UpdateVisiblePieces();
        }
    }

    void UpdateVisiblePieces()
    {

        int currentX = Mathf.RoundToInt(playerPos.x / pieceSize);
        int currentY = Mathf.RoundToInt(playerPos.y / pieceSize);

        for (int i = 0; i<terrainPiecesVisibleLastUpdate.Count; i++)
        {
            terrainPiecesVisibleLastUpdate[i].SetVisible(false);
        }
        terrainPiecesVisibleLastUpdate.Clear();


        for(int y = -piecesVisibleInViewDist; y<= piecesVisibleInViewDist; y++)
        {
            for (int x = -piecesVisibleInViewDist; x <= piecesVisibleInViewDist; x++)
            {
                Vector2 currPieceCoord = new Vector2(currentX + x, currentY + y);

                if (terrainPieceDict.ContainsKey(currPieceCoord))
                {
                    terrainPieceDict[currPieceCoord].UpdateTerrainPiece();

                }
                else
                {
                    terrainPieceDict.Add(currPieceCoord, new TerrainPiece(currPieceCoord, pieceSize, detailLevels, transform, waterMat, mapMaterial));
                }
                
            }
        }
    }

    




}
