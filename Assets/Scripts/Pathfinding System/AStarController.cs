using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A星寻路模块 - A* Pathfinding Module
///     需求：Require:
///         AStarController.cs
/// </summary>
public class AStarController : BaseController<PathController>
{
    private AStarMap curr_map;

    // 根据起点和终点寻路 - find path base on start and end pos
    public List<Vector2Int> FindPath(Vector2Int start_pos, Vector2Int end_pos)
    {
        // 起点终点合法性判断 - starting point and ending point invalid
        if( (start_pos.x < 0 || start_pos.x >= curr_map.map_size.x) || 
            (start_pos.y < 0 || start_pos.y >= curr_map.map_size.y) ||
            (end_pos.x   < 0 || end_pos.x   >= curr_map.map_size.x) || 
            (end_pos.y   < 0 || end_pos.y   >= curr_map.map_size.y))
            return null;
        if( curr_map.map_nodes[end_pos.x,   end_pos.y]   == -1 ||
            curr_map.map_nodes[start_pos.x, start_pos.y] == -1 )
            return null;
        
    }
}