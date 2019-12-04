using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    /// <summary>
    /// by준희, BGM
    /// </summary>
    public AudioClip BGMSound;

    #region SoundList(추가시 변수 추가하기)
    public AudioClip GoalSuccess;
    public AudioClip GoalFail;
    public AudioClip BallKick;
    #endregion

    public delegate void PlaySound(AudioClip audio);
    public static PlaySound SoundEffect;

    public delegate void BGMSoundMgr(float volume);
    public static BGMSoundMgr BGMmgr;

    private AudioSource audiosource;
    public struct Sound
    {
        public static AudioClip GoalSuccess;
        public static AudioClip GoalFail;
        public static AudioClip BallKick;

        public Sound(AudioClip sound1, AudioClip sound2, AudioClip sound3)
        {
            GoalSuccess = sound1;
            GoalFail = sound2;
            BallKick = sound3;
        }

    }
    private void OnEnable()
    {
        SoundEffect += Play;
        BGMmgr += BGM;
    }

    private void OnDisable()
    {
        SoundEffect -= Play;
        BGMmgr -= BGM;
    }

    private void Start()
    {
        //by준희, 배경음 관중 소리 시작
        audiosource = this.gameObject.GetComponent<AudioSource>();
        audiosource.clip = BGMSound;
        audiosource.Play();
        
        //by준희, 효과음 초기화
        Sound soundList = new Sound(GoalSuccess, GoalFail, BallKick);
    }
    /// <summary>
    /// by준희, 어떤 효과음을 낼 것인지 조정.
    /// </summary>
    /// <param name="audio"></param>
    public void Play(AudioClip audio)
    {
        audiosource.PlayOneShot(audio);
    }

    /// <summary>
    /// by준희, 배경음 볼륨 조정
    /// </summary>
    /// <param name="volume"></param>
    public void BGM(float volume)
    {
        audiosource.volume = volume;
    }

}

