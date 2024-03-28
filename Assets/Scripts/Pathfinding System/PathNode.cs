using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A星寻路节点类 - A* pathfinding node
/// </summary>
public class PathNode
{   
    
    public Vector2Int node_pos; // 节点位置 - the position of this node
    public NodeType node_type;  // 节点类型 - the type of this node
    public int cost_curr;       // 当前花费 - the total cost from start point to this node
    public int cost_left;       // 剩余花费 - the total cost from this node to end point
    public int cost_total;      // 总花费 - the sum of cost_curr and cost_left
    public PathNode pre_node;   // 上一节点 - the preview node
    public bool isEnable;       // 是否被激活 - is this node be enabled
    public bool isArrive;       // 是否已到达 - is this node be arrived

    public PathNode(int x, int y, NodeType type)
    {
        node_pos.x = x;
        node_pos.y = y;
        node_type = type;
        cost_curr = 0;
        cost_left = 0;
        cost_total = 0;
        pre_node = null;
    }

    public PathNode(PathNode node)
    {
        node_type = node.node_type;
        node_pos.x = node.node_pos.x;
        node_pos.y = node.node_pos.y;
    }

    /// <summary>
    /// 根据自身类型返回移动花费系数
    /// </summary>
    /// <returns></returns>
    public float TypeCost()
    {
        switch (node_type)
        {
            case NodeType.Path:
                return 0.5f;
            case NodeType.Grass:
                return 1f;
            case NodeType.Forest:
                return 1.5f;
            default:
                return 2f;
        }
    }

    public static bool operator < (PathNode a, PathNode b)
    {
        if(a.cost_total == b.cost_total)
            return (a.cost_left < b.cost_left);
        return (a.cost_total < b.cost_total);
    }

    public static bool operator > (PathNode a, PathNode b)
    {
        if(a.cost_total == b.cost_total)
            return (a.cost_left > b.cost_left);
        return (a.cost_total > b.cost_total);
    }
}

public enum NodeType
{
    Path,
    Grass,
    Forest,
    Unable
}