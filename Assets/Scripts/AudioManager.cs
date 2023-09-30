using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BgmType
{
    Title,
    Battle,
    Result,
}

public enum SeType
{
    Hit,
    GameStartBtn,
    ApplyItemEffect,
    OpenTreasureChest,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource playingBgm;

    [SerializeField] private AudioSource[] bgmSources;
    [SerializeField] private AudioSource[] seSources;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// BGM再生の準備
    /// </summary>
    /// <param name="bgmType"></param>
    public void PreparePlayBGM(BgmType bgmType)
    {
        StartCoroutine(PlayBGM(bgmType));
    }

    /// <summary>
    /// BGMを再生
    /// </summary>
    /// <param name="bgmType"></param>
    /// <returns></returns>
    private IEnumerator PlayBGM(BgmType bgmType)
    {
        //再生中のBGMがある場合、徐々にボリュームを下げる
        foreach (AudioSource audioSource in bgmSources)
        {
            if (audioSource.isPlaying)
            {
                playingBgm = audioSource;

                playingBgm.DOFade(0, 0.75f);
            }
        }

        yield return new WaitForSeconds(0.45f);

        //新しい指定された曲を再生
        bgmSources[(int)bgmType].Play();

        bgmSources[(int)bgmType].DOFade(1, 0.75f);

        //前に流れている曲がある場合
        if (playingBgm)
        {
            //前の曲のボリュームが0になったらそのBGMの再生を停止(上ではボリュームを下げただけ)
            yield return new WaitUntil(() => playingBgm.volume == 0);

            playingBgm.Stop();
        }
    }

    /// <summary>
    /// SEを再生
    /// </summary>
    /// <param name="seType"></param>
    public void PlaySE(SeType seType)
    {
        seSources[(int)seType].PlayOneShot(seSources[(int)seType].clip);  //PlayOneShotで音を重ねて再生できる
    }
}
