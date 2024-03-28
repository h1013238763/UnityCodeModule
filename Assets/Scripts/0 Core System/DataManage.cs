using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 数据储存接口
/// Data Save Interface
///     用于需要数据储存功能的类的接口
///     interface for controller class which has data manager requirements 
/// v.2024.2.13.1
/// </summary>
public interface DataManage
{
    void SaveData();    // 存储数据 Save data
    void LoadData();    // 读取数据 Load data
    void InitialData(); // 初始化数据 Initial data
    void DeleteData();  // 删除数据 Delete data
}