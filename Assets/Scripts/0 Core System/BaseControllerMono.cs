using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载单例模式基类模块
/// Mono Singleton pattern base module
/// v.2024.2.15.1
/// </summary>
/// <typeparam name="T"> 子类 Child class </typeparam>
public class BaseControllerMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T controller;

    /// <summary>
    /// 创建一个挂载该脚本的DDOL物体，定义并返回一个唯一静态管理器
    /// Create a DDOL object to mount the script, define and return a unique static controller
    /// </summary>
    /// <returns> 控制器对象 Controller object </returns>
    public static T Control()
    {
        if(controller == null)
        {
            GameObject obj = new GameObject();
            obj.name = typeof(T).ToString();
            controller = obj.AddComponent<T>();

            GameObject.DontDestroyOnLoad(obj);
        }
        return controller;
    }


}
