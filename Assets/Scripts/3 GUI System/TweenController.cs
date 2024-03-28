using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  GUI动画控制模块
/// </summary>
/// 
public class TweenController : BaseControllerMono<TweenController>
{
    // 队列动画表
    private Dictionary<string, Queue<TweenAnime>> queue_anime = new Dictionary<string, Queue<TweenAnime>>();
    // 并行动画表
    private List<TweenAnime> anime_list = new List<TweenAnime>();
    // 移除动画表
    private List<TweenAnime> remove_list = new List<TweenAnime>();

    void FixedUpdate()
    {

    }


    // GUI动画类
    public class TweenAnime
    {
        public string anime_id;
        public GameObject anime_object;
        public Vector3 start_vector;
        public Vector3 end_vector;
        public float anime_time;
        public ProgressType anime_type;
        public UnityAction anime_callback;
    }
}