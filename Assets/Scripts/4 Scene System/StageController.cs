using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manage all game stages like scene switching, save loading etc
/// </summary>
public class StageController : BaseControllerMono<StageController>
{
    PlayerData player_data;     // 玩家数据

    // 执行游戏
    void Start()
    {
        InitialGame();
    }

    // 初始化游戏
    private void InitialGame()
    {
        
    }
}

[System.Serializable]
public class PlayerData     // 玩家数据类
{
    // 新游戏
    public void NewGame()
    {
        
    }

    // 读取游戏
    public void LoadGame()
    {
       
    }

    // 保存游戏
    public void SaveGame()
    {
        
    }
}