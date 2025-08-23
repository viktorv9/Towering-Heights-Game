using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomBackgroundGenerator : MonoBehaviour {

    [SerializeField] private Vector2 backgroundSize;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<Tile> randomTileOptions;

    void Start() {
        for (int j = 0; j < backgroundSize.y; j++)
        {
            for (int i = 0; i < backgroundSize.x; i++) {
                
                List<Tile> localRandomTileOptions = new List<Tile>(randomTileOptions);
                
                // neighboring means west, southwest, south and southeast (since only those are ever filled in, left to right BOTTOM to TOP!!)
                List<Tile> neighboringTiles = new List<Tile>();
                neighboringTiles.Add(tilemap.GetTile<Tile>(new Vector3Int(i-1, j, 0)));
                neighboringTiles.Add(tilemap.GetTile<Tile>(new Vector3Int(i-1, j-1, 0)));
                neighboringTiles.Add(tilemap.GetTile<Tile>(new Vector3Int(i, j-1, 0)));
                neighboringTiles.Add(tilemap.GetTile<Tile>(new Vector3Int(i+1, j-1, 0)));

                // remove neighboring tiles from current options
                foreach (Tile neighboringTile in neighboringTiles) {
                    localRandomTileOptions.Remove(neighboringTile);
                }

                tilemap.SetTile(new Vector3Int(i, j, 0), localRandomTileOptions[Random.Range(0, localRandomTileOptions.Count)]);
            }
        }

        transform.position = new Vector3(-backgroundSize.x / 2, -backgroundSize.y / 2, 0);
    }
}
