using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 数学库
/// Math support Library
///     为其他类提供数学计算方法
///     provide math related support for other classes
/// v.2024.2.15.1
/// </summary>
public static class MathLib
{
    // 进度计算 ( 当前时间，总时间，进度类型 ) 返回：当前进度float
    public static float ProgressPercentage(float curr_time, float total_time, ProgressType type)
    {
        switch(type)
        {
            case ProgressType.Linear:
            case ProgressType.Parabola:
            case ProgressType.FadeIn:
            case ProgressType.FadeOut:
        }
    }

    public static enum ProgressType
    {
        Linear,     // 线性函数
        Parabola,   // 抛物线
        FadeIn,     // 渐缓
        FadeOut,    // 渐快
    }
}