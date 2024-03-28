using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A星寻路模块 - A* Pathfinding Module
///     需求：Require:
///         Scripts/Travel System/PathNode.cs
/// </summary>
public class PathController : BaseController<PathController>
{   
    // 地图尺寸 - the size of map
    public Vector2Int map_size;                
    private PathNode[,] map_nodes;          // 全部节点 - all nodes
    private List<PathNode> enabled_nodes;   // 激活节点 - enabled nodes
    private List<PathNode> arrived_nodes;   // 到达节点 - arrived nodes
    private Vector2Int end_pos;             // 终点 - end point
    private BinaryMinHeap min_heap;         // 最小堆 - the min heap

    /// <summary>
    /// 地图初始化
    /// Initial the map
    /// </summary>
    /// <param name="size">地图尺寸 - the size of map</param>
    public void InitialMap(int x, int y)
    {
        map_nodes = new PathNode[x, y];

        for(int i = 0; i < x; i ++)
        {
            for(int j = 0; j < y; j ++)
            {
                // 需要修改，根据地图配置文件设置
                PathNode node = new PathNode(i, j, UnityEngine.Random.Range(0,100) < 20 ? NodeType.Unable : NodeType.Grass);
                map_nodes[i, j] = node;
            }
        }
    }

    /// <summary>
    /// 寻路 - Path Finding
    /// </summary>
    /// <param name="start">起点位置 - start position</param>
    /// <param name="end">终点位置 - end position</param>
    /// <returns>路径表 - the list of positions</returns>
    public List<Vector2> PathFinding(Vector2Int start, Vector2Int end)
    {
        // 起点终点合法性判断 - starting point and ending point invalid
        if( (start.x < 0 || start.x >= map_size.x) || (start.y < 0 || start.y >= map_size.y) ||
            (end.x   < 0 || end.x   >= map_size.x) || (end.y   < 0 || end.y   >= map_size.y))
            return null;
        if( map_nodes[end.x, end.y].node_type == NodeType.Unable )
            return null;

        end_pos = end;

        // 设置起点 - set the start node
        map_nodes[start.x, start.y].cost_curr = 0;
        map_nodes[start.x, start.y].pre_node = null;
        enabled_nodes.Add(map_nodes[start.x, start.y]);
        MoveToNode( map_nodes[start.x, start.y] );

        // 如果没到达终点则持续寻路 - keep finding path while not arrive end point
        while(!map_nodes[end.x, end.y].isArrive)
        {
            // 如果没有剩余可移动位置，返回空 - if no node to move to, return null
            if(enabled_nodes.Count == 0)
                return null;
            // 尝试移动至下一个最小节点 - try to move to next best node
            MoveToNode(min_heap.ExtractMin(enabled_nodes));
        }

        // 完成寻路，从终点开始返回上一节点直到起点 - finding complete, return the list of path
        List<Vector2> path = new List<Vector2>();
        PathNode previous = arrived_nodes[arrived_nodes.Count-1];
        while(previous.pre_node == null)
        {
            path.Add(new Vector2(previous.node_pos.x, previous.node_pos.y));
            previous = previous.pre_node;
        }
        path.Reverse();

        // 清除存储 - clear saves
        foreach(PathNode node in enabled_nodes)
            node.isEnable = false;
        enabled_nodes.Clear();
        foreach(PathNode node in arrived_nodes)
            node.isArrive = false;
        arrived_nodes.Clear();

        return path;
    }

    /// <summary>
    /// 激活目标节点
    /// </summary>
    /// <param name="node">目标节点</param>
    /// <param name="cost_curr">上一节点的行动花费</param>
    /// <param name="is_axis">此节点是否为对角线节点 - </param>
    /// <param name="previous">上一节点</param>
    private void EnableNode(int x, int y, int cost_curr, int position_coe, PathNode previous)
    {   
        // 节点可用性检查 - Node availbility check
        if( x < 0 || x >= map_size.x || y < 0 || y > map_size.y)
            return; 
        PathNode node = map_nodes[x, y];
        if(node.node_type == NodeType.Unable )
            return;  

        int cost = cost_curr + (int)(position_coe * node.TypeCost());
        
        // 如果自身未激活或到达过则添加进激活列表 - add into enabled nodes list if haven't enable nor arrive
        if(!node.isEnable && !node.isArrive)
        {
            FindLeftStraight(node, end_pos);
        }
        // 如果不是更优解则返回 - return if not having better cost
        else{
            if(cost >= node.cost_curr) 
                return;
        }
        // 修改花费 - change cost
        node.cost_curr = cost;
        node.cost_total = node.cost_curr + node.cost_left;
        node.pre_node = previous;

        // 使用二叉树搜索，根据节点的总花费和剩余花费更新列表排序
        // update the list sort by the total cost and left cost of this node, based on binary search
        if(node.isEnable)
            min_heap.MinHeapify(enabled_nodes.IndexOf(node), enabled_nodes);
        else
            min_heap.AddEnable(node, enabled_nodes);
        
        return;
    }

    /// <summary>
    /// 移动至目标节点
    /// move to target node
    /// </summary>
    /// <param name="node">目标节点 - target node</param>
    private void MoveToNode(PathNode node)
    {
        // 将自身添加进已到达节点中 - add itself to the arrived nodes and remove from enable nodes
        arrived_nodes.Add(node);
        node.isArrive = true;

        // 如果到达终点则直接返回 - return if it this node is the end
        if(node == map_nodes[end_pos.x, end_pos.y])
            return;

        // 遍历且尝试激活周边节点 - traverse and try to enable surrounding nodes
        EnableNode( node.node_pos.x-1, node.node_pos.y  , node.cost_curr, 10, node );
        EnableNode( node.node_pos.x-1, node.node_pos.y-1, node.cost_curr, 14, node );
        EnableNode( node.node_pos.x-1, node.node_pos.y+1, node.cost_curr, 14, node );
        EnableNode( node.node_pos.x+1, node.node_pos.y  , node.cost_curr, 10, node );
        EnableNode( node.node_pos.x+1, node.node_pos.y-1, node.cost_curr, 14, node );
        EnableNode( node.node_pos.x+1, node.node_pos.y+1, node.cost_curr, 14, node );
        EnableNode( node.node_pos.x  , node.node_pos.y-1, node.cost_curr, 10, node );
        EnableNode( node.node_pos.x  , node.node_pos.y+1, node.cost_curr, 10, node );
    }

    /// <summary>
    /// 计算无障碍情况下的剩余路径
    /// Calculate the remaining path without obstacles
    /// </summary>
    /// <param name="node_curr">当前节点 - current node</param>
    /// <param name="end">终点 - the end point</param>
    private void FindLeftStraight(PathNode node_curr, Vector2Int end)
    {
        int temp_x;
        int temp_y;
        PathNode temp_node = new PathNode(node_curr);
        
        node_curr.cost_left = 0;
        
        while( temp_node.node_pos.x != end.x || temp_node.node_pos.y != end.y )
        {   
            temp_x = 0;
            temp_y = 0;

            if( temp_node.node_pos.x > end.x )
                temp_x = -1;
            else if(temp_node.node_pos.x < end.x)
                temp_x = 1;

            if( temp_node.node_pos.y > end.y )
                temp_y = -1;
            else if(temp_node.node_pos.y < end.y)
                temp_y = 1;

            temp_node = map_nodes[temp_node.node_pos.x + temp_x, temp_node.node_pos.y + temp_y ];
            node_curr.cost_left += (int)(temp_node.TypeCost() * 10 * ((temp_x != 0 && temp_y != 0) ? 1.4 : 1 )) ;
        }
    }

    private class BinaryMinHeap
    {
        /// <summary>
        /// 添加新节点
        /// Add new PathNode
        /// </summary>
        /// <param name="node">需要添加的节点 - the node to add</param>
        /// <param name="enabled_nodes">the list of nodes</param>
        public void AddEnable(PathNode node, List<PathNode> enabled_nodes)
        {
            enabled_nodes.Add(node);
            node.isEnable = true;
            int index = enabled_nodes.Count-1;
            int parent = (index-1)/2;

            while( index != 0 && enabled_nodes[index] < enabled_nodes[parent])
            {
                Swap(index, parent, enabled_nodes);
                index = parent; 
                parent = (index-1)/2;
            }
        }

        /// <summary>
        /// 获得并移除最小节点
        /// get and remove minmium node
        /// </summary>
        /// <param name="enabled_nodes">the list of nodes</param>
        /// <returns>最小节点 - the minimum</returns>
        public PathNode ExtractMin(List<PathNode> enabled_nodes)
        {
            if(enabled_nodes.Count == 1)
                return enabled_nodes[0];

            PathNode root = enabled_nodes[0];
            enabled_nodes[0] = enabled_nodes[enabled_nodes.Count-1];
            root.isEnable = false;
            MinHeapify(0, enabled_nodes);

            return root;
        }

        /// <summary>
        /// 将堆恢复为最小堆模式
        /// change the list into min heap
        /// </summary>
        /// <param name="index">需要调整的节点 - the node to heapify</param>
        /// <param name="enabled_nodes">the list of nodes</param>
        public void MinHeapify(int index, List<PathNode> enabled_nodes)
        {
            int node_left = 2 * index + 1;
            int node_right = 2 * index + 2;

            int min = index;
            if(node_left < enabled_nodes.Count && enabled_nodes[node_left] < enabled_nodes[min])
                min = node_left;
            if(node_right < enabled_nodes.Count && enabled_nodes[node_right] < enabled_nodes[min])
                min = node_right;

            if( min != index )
            {
                Swap(min, index, enabled_nodes);
                MinHeapify(min, enabled_nodes);
            }
        }

        /// <summary>
        /// 交换两个节点
        /// Swap two PathNode
        /// </summary>
        /// <param name="a">a node</param>
        /// <param name="b">b node</param>
        /// <param name="enabled_nodes">the list of nodes</param>
        public void Swap(int a, int b, List<PathNode> enabled_nodes)
        {
            PathNode temp;
            temp = enabled_nodes[a];
            enabled_nodes[a] = enabled_nodes[b];
            enabled_nodes[b] = temp;
        }
    }
}