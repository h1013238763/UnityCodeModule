using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A星寻路图 - A* path map
/// </summary>
public class AStarMap
{
    public Vector2Int map_size;    
    public float[,] map_nodes;  // 以-1为不可通行，1为标准节点权重
    public int cost_curr;       // 当前花费 - the total cost from start point to this node
    public int cost_left;       // 剩余花费 - the total cost from this node to end point

    // 初始化地图 initial map
    public void InitialMap(Vector2Int size)
    {

    }

    // 设定节点 set map node value
    public void SetNode(Vector2Int pos, float value)
    {

    }
}