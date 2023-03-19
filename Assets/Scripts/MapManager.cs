using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    // Singleton for instances of each tile

    private static MapManager _instance;
    public static MapManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject overlayPrefab;
    public GameObject overlayContainer;

    //Dictionary of tiles that the player can move to
    public Dictionary<Vector2Int, GameObject> MovementMap;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        MovementMap = new Dictionary<Vector2Int, GameObject>();

        //tilemap objects
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();

        BoundsInt bounds = tileMap.cellBounds;

        //gets every avaiable tile on the tilemap

        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                var tileLocation = new Vector3Int(x, y);
                var tileKey = new Vector2Int(x, y);
                if (tileMap.HasTile(tileLocation) && !MovementMap.ContainsKey(tileKey))
                {
                    var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                    var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);
                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z+1);
                    MovementMap.Add(tileKey, overlayTile);
                }
            }
        }
    }
}
    
