using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileFunctions : ScriptableObject
{
    public TileBase[] tiles;

    public Boolean walkable;
}
