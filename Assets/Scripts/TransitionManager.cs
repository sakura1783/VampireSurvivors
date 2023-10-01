using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    [SerializeField] private Fade fade;

    [SerializeField] private float fadeDuration;


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
    /// シーン遷移の準備
    /// </summary>
    public void PrepareLoadNextScene()
    {
        fade.FadeIn(fadeDuration, () => { StartCoroutine(LoadNextScene()); });
    }

    /// <summary>
    /// シーン遷移の実処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadNextScene()
    {
        SceneManager.LoadScene("Main");

        //読み込んだMainシーンの情報を取得
        Scene scene = SceneManager.GetSceneByName("Main");

        //シーンの読み込み終了を待つ
        yield return new WaitUntil(() => scene.isLoaded);

        //シーンの読み込みが終了してからフェードアウトして、場面転換を完了する
        fade.FadeOut(fadeDuration);

        yield return null;
    }
}
