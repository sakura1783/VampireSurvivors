using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public List<EnemyController> enemiesList = new();

    public List<EnemyController> targetList = new();  //追尾弾の追尾対象のリスト(追尾対象が被らないようにする)

    //順位ごとのデータ
    //public int firstScore;
    //public string firstName;
    //public int secondScore;
    //public string secondName;
    public int thirdScore;
    //public string thirdName;

    //public Dictionary<string, int> playersData = new();  //プレイヤーの名前とスコアのデータ
    public List<(string playerName, int score)> playersDataList = new();  //ディクショナリだとKeyが一意でなければならない(プレイヤー同士の名前の重複が許可されない)のでtuple(タプル)型に変更 

    private Text txt1stName;
    private Text txt1stScore;
    private Text txt2ndName;
    private Text txt2ndScore;
    private Text txt3rdName;
    private Text txt3rdScore;

    //PlayerPrefs用
    //private string firstName;
    //private int firstScore;
    //private string secondName;
    //private int secondScore;
    //private string thirdName;
    //public int thirdScore;  //3位以内かを判断するために外部クラスで使う必要があるのでこれだけpublicにする


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            //playersDataディクショナリの初期化
            //playersData = new Dictionary<string, int>() { { "", 0 }, { "", 0 }, { "", 0 } };  //各Keyは一意でなければならない。これだとエラーになる
            //playersData = new Dictionary<string, int>() { { "", 0 }, { "-", 0 }, { "--", 0 } };

            //Debug.Log("GameDataのAwakeメソッドが動きました");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 初期設定の準備
    /// </summary>
    public void PrepareSetUpGameData(TitlePopUp titlePop)
    {
        StartCoroutine(SetUpGameData(titlePop));
    }

    /// <summary>
    /// 初期設定の実処理
    /// </summary>
    /// <param name="titlePop"></param>
    private IEnumerator SetUpGameData(TitlePopUp titlePop)
    {
        //シーン遷移後にアサイン情報が消えるので毎回ゲーム開始時に外部クラスから情報を代入
        txt1stName = titlePop.Txt1stName;
        txt1stScore = titlePop.Txt1stScore;
        txt2ndName = titlePop.Txt2ndName;
        txt2ndScore = titlePop.Txt2ndScore;
        txt3rdName = titlePop.Txt3rdName;
        txt3rdScore = titlePop.Txt3rdScore;

        yield return null;

        //Debug.Log("SetUpGameData動きました");
    }

    /// <summary>
    /// プレイヤーのスコアと名前をランキングに追加
    /// </summary>
    //public void AddRanking()
    //{
    //    //何回foreach文が回ったか。この値で何位に値を入れるか決定
    //    int count = 0;

    //    //ランキングに同じプレイヤーが入らないようにプレイヤーの名前を保管するリストを宣言
    //    List<string> playerList = new();

    //    //スコアが高い順にPlayersDataリストを並び替える
    //    //var sortedPlayersDataDic = GameData.instance.playersData.OrderByDescending(x => x.Value);  //xはplayersDataを表し、これはplayersDataのvalueを降順に並べている
    //    var sortedPlayersDataList = playersData.OrderByDescending(data => data.score);

    //    //sortedPlayersDataDicから順に要素を取り出す
    //    foreach (var playerData in sortedPlayersDataList)
    //    {
    //        //スコアが初期値(0)の場合は「--」と表示する
    //        if (playerData.score <= 0)
    //        {
    //            //if (count == 0)
    //            //{
    //            //    GameData.instance.txt1stName.text = "--";
    //            //    GameData.instance.txt1stScore.text = "--";
    //            //}
    //            //if (count == 1)
    //            //{
    //            //    GameData.instance.txt2ndName.text = "--";
    //            //    GameData.instance.txt2ndScore.text = "--";
    //            //}
    //            if (count == 2)
    //            {
    //                //GameData.instance.txt3rdName.text = "--";
    //                //GameData.instance.txt3rdScore.text = "--";

    //                //値が無いにしろランキングが埋まったら(3回処理を回したら)処理を抜ける
    //                //return;
    //            }
    //        }

    //        //同じプレイヤーが含まれていなかった場合のみ、ランキングに表示
    //        if (!playerList.Contains(playerData.playerName) && playerData.score >= 0)  //<= 2つめの条件で、スコアが初期値の場合はこのブロック内に入らないようにする
    //        {
    //            //if (count == 0)
    //            //{
    //            //    GameData.instance.txt1stName.text = playerData.playerName;
    //            //    GameData.instance.txt1stScore.text = playerData.score.ToString();
    //            //}
    //            //if (count == 1)
    //            //{
    //            //    GameData.instance.txt2ndName.text = playerData.playerName;
    //            //    GameData.instance.txt2ndScore.text = playerData.score.ToString();
    //            //}
    //            if (count == 2)
    //            {
    //                //GameData.instance.txt3rdName.text = playerData.playerName;
    //                //GameData.instance.txt3rdScore.text = playerData.score.ToString();

    //                //ランキングに入るか否かを決めるthirdScore変数にここで値を代入
    //                GameData.instance.thirdScore = playerData.score;

    //                //ランキングが埋まったら処理を抜ける
    //                //return;
    //            }
    //        }

    //        //同じプレイヤーが含まれていた場合、リストから削除
    //        if (playerList.Contains(playerData.playerName))
    //        {
    //            playersData.Remove((playerData.playerName, playerData.score));
    //        }

    //        //ランキング4位以下もリストから削除
    //        if (count >= 3)
    //        {
    //            playersData.Remove((playerData.playerName, playerData.score));

    //            //必要なデータのみリストに保存できたらメソッドから抜ける
    //            return;
    //        }

    //        //ランキングに同じプレイヤーが重複しないようにリストに追加
    //        playerList.Add(playerData.playerName);

    //        count++;
    //    }
    //}

    /// <summary>
    /// PlayersDataListの整理と並び替え
    /// </summary>
    public void OrganizePlayersDataList()
    {
        //何回foreach文が回ったか
        int count = 0;

        //ランキングに同じプレイヤーが入らないようにプレイヤーの名前を保管するリストを宣言
        List<string> playerList = new();

        //スコアが高い順にPlayersDataListを並び替える
        var sortedPlayersDataList = playersDataList.OrderByDescending(data => data.score);

        //sortedPlayersDataDicから順に要素を取り出す
        foreach (var playerData in sortedPlayersDataList)
        {
            //同じプレイヤーが含まれていた場合、リストから削除
            if (playerList.Contains(playerData.playerName))
            {
                playersDataList.Remove((playerData.playerName, playerData.score));
            }

            //ランキング4位以下もリストから削除
            if (count >= 3)
            {
                playersDataList.Remove((playerData.playerName, playerData.score));

                count++;

                //必要なデータのみリストに保存できたらメソッドから抜ける
                continue;
            }

            //同じプレイヤーが含まれておらず、かつスコアが0以上の場合
            if (!playerList.Contains(playerData.playerName) && playerData.score > 0)
            {
                if (count == 2)
                {
                    //ランキングに入るかどうかを決めるthirdScore変数に値を代入
                    GameData.instance.thirdScore = playerData.score;

                    //thirdScoreをセーブ
                    PlayerPrefs.SetInt("ThirdScore_Key", thirdScore);
                    PlayerPrefs.Save();
                }
            }

            //ランキングに同じプレイヤーが重複しないようにリストに追加
            playerList.Add(playerData.playerName);

            count++;
        }

        //playersDataListをセーブ
        PlayerPrefsUtility.SaveList<(string, int)>("PlayersDataList_Key", GameData.instance.playersDataList);
    }

    /// <summary>
    /// ランキング読み込み
    /// </summary>
    public void LoadRanking()
    {
        //セーブしておいたplayersDataリストをロード
        playersDataList = PlayerPrefsUtility.LoadList<(string, int)>("PlayersDataList_Key");

        //何回foreach文が回ったか。この値でどの順位にどの値を入れるかを決める
        int count = 0;

        //スコアが高い順にリストを並び替え
        //var sortedPlayersDataList = playersData.OrderByDescending(data => data.score);
        var sortedPlayersDataList = playersDataList.OrderByDescending(data => data.score);

        foreach (var playerData in sortedPlayersDataList)
        {
            //スコアが0以上であればタイトル画面の対応するランキングに表示
            if (playerData.score >= 0)
            {
                if (count == 0)
                {
                    txt1stName.text = playerData.playerName;
                    txt1stScore.text = playerData.score.ToString();
                }
                if (count == 1)
                {
                    txt2ndName.text = playerData.playerName;
                    txt2ndScore.text = playerData.score.ToString();
                }
                if (count == 2)
                {
                    txt3rdName.text = playerData.playerName;
                    txt3rdScore.text = playerData.score.ToString();

                    Debug.Log("ランキングを読み込みました");

                    //ランキングが全て埋まったら抜ける
                    return;
                }
            }

            count++;
        }
    }
}
