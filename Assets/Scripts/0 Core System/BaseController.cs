using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式基类模块
/// Singleton pattern base module
/// v.2024.2.15.1
/// </summary>
/// <typeparam name="T"> 子类 Child class</typeparam>
public class BaseController<T> where T: new()
{
    private static T controller;

    /// <summary>
    /// 定义并返回一个唯一静态管理器
    /// Define and return a unique static controller
    /// </summary>
    /// <returns> 控制器对象 Controller object</returns>
    public static T Control(){
        if(controller == null)
            controller = new T();
        return controller;
    }
}