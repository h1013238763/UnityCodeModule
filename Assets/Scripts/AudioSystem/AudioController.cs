using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 声音控制模块
/// Audio controller module
/// </summary>
public class AudioController : BaseController<AudioController>
{
    private List<GameObject> audio_obj = new List<GameObject>();    // 音效物体表 - audio objects list
    private GameObject master_obj;
    private List<List<AudioSource>> audio_list = new List<List<AudioSource>>(); // 音效列表 - audio list
    private List<float> audio_volume = new List<float>();   // 音量表 - audio volume list
    private float master_volume;    // 主音量 - master volume
    private List<AudioSource> audio_pool = new List<AudioSource>(); // 音效片物品池 - audio source pool
    
    public AudioController()
    {
        MonoController.Controller().AddUpdateListener(Update);
        Initial();
    }

    private void Update()
    {
        for(int i = 0; i < audio_list.Count; i ++)
        {
            for(int j = 0; j < audio_list[i].Count; j ++)
            {
                if(!audio_list[i][j].isPlaying)
                    StopSound(sound_list[i]);
            }
        }
    }

    // 初始化控制器参数 - initial controller instances
    private void Initial()
    {
        // create audio handler
        if(master_obj == null)
        {
            master_obj = new GameObject("Audio Player");
            GameObject.DontDestroyOnLoad(master_obj);
            master_volume = 0.5f;

            int index = 0;
            foreach(AudioType type in Enum.GetValues(typeof(AudioType)))
            {
                if(type == AudioType.Master){ continue; }   // ignore master type
                // object
                GameObject obj = new GameObject(type);
                obj.transform.SetParent(master_obj.transform);
                audio_obj.Add(obj);
                GameObject.DontDestroyOnLoad(obj);
                // audio list
                audio_list.Add(new List<AudioSource>());
                // volume( with default volume 0.5 )
                audio_volume.Add(0.5f);

                index ++;
            }
        }
    }

    // 播放音效 Start Audio
    public void StartAudio(string audio_name, AudioType audio_type, bool is_loop, UnityAction<AudioSource> callback = null)
    {
        // error handle
        if(audio_type == AudioType.Master){ return; }

        // get pool object
        AudioSource source;
        if(inactive_list.Count > 0)
            source = inactive_list[0]; 
        else
            source = sound_player.AddComponent<AudioSource>();

        // load audioclip and add it into object
        ResourceController.Controller().LoadAsync<AudioClip>("Audio/Sound/" + name, (m) =>
        {   
            source.enabled = true;
            inactive_list.Remove(source);
            source.clip = m;
            source.volume = master_volume * sound_volume;
            source.loop = isLoop;
            source.Play();
            sound_list.Add(source);
            if(callback != null)
                callback(source);
        });

        
        ResourceController.Controller().LoadAsync<AudioClip>("Audio/Music/" + audio_name, (audio) =>
        {
            // active pool object
            source.enabled = true;
            inactive_list.Remove(source);
            // change object values
            source.clip = audio;
            source.volume = master_volume * audio_volume[audio_type];
            source.loop = is_loop;
            source.Play();
            // Add to list
            audio_list[audio_type].Add(source);

        });
    }

    // 更改音量 Change Audio Volume
    public void ChangeVolume(AudioType audio_type, float value)
    {
        // 调整音量数值 adjust volume value
        if(audio_type == AudioType.Master)
            master_volume = value;
        else
            audio_volume[audio_type] = value;

        // 初始化循环节点 register loop pos
        int start = 0, end = audio_list.Count;
        if(audio_type != AudioType.Master){ start = audio_type; end = audio_type+1; }
        
        // 逐一调整音量 change clip volume one by one
        while(start < end)
        {
            foreach(var clip in audio_list[start])
                clip.volume = master_volume * audio_volume[start];
            
            start ++;
        }
    }

    // 结束音效 Stop Audio
    public void StopAudio(AudioSource audio_source, AudioType audio_type)
    {
        if(audio_type == AudioType.Master){ return; }

        if(audio_list[audio_type].Contains(audio_source))
        {
            audio_list[audio_type].Remove(audio_source);
            audio_pool.Add(audio_source);
            audio_source.Stop();
            audio_source.enabled = false;
        }
    }

    // 结束所有该类音效 Stop all audios of given type
    public void StopAudioType(AudioType audio_type)
    {
        if(audio_type == AudioType.Master){ return; }

        foreach(AudioSource source in audio_list[audio_type])
        {
            audio_pool.Add(source);
            source.Stop();
            source.enabled = false;        
        }
        audio_list[audio_type].Clear();
    }

    // 检查音效是否存在 Check if audio exist
    public bool ExistAudio(AudioSource audio_source, AudioType audio_type)
    {
        if(audio_type == AudioType.Master){ return false; }
        return audio_list[audio_type].Contains(audio_source);
    }
}

public enum AudioType
{
    Master = -1,
    Music = 0,
    Sound = 1,
    Environment = 2
}