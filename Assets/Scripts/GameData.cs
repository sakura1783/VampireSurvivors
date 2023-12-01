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
    //public int thirdScore;
    //public string thirdName;

    //public Dictionary<string, int> playersData = new();  //プレイヤーの名前とスコアのデータ
    public List<(string playerName, int score)> playersDataList = new();  //ディクショナリだとKeyが一意でなければならない(プレイヤー同士の名前の重複が許可されない)のでtuple(タプル)型に変更 

    //private Text txt1stName;
    //private Text txt1stScore;
    //private Text txt2ndName;
    //private Text txt2ndScore;
    //private Text txt3rdName;
    //private Text txt3rdScore;

    //PlayerPrefs用
    //private string firstName;
    //private int firstScore;
    //private string secondName;
    //private int secondScore;
    //private string thirdName;
    //public int thirdScore;  //3位以内かを判断するために外部クラスで使う必要があるのでこれだけpublicにする

    public int highScore;  //自己ベスト


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

        //セーブしておいたplayersDataListの情報を取得(実行ボタンを押してPlayモードに入る時は、GameDataのplayersDataListには何も情報が入っていない。情報が空っぽのままだと、ランキングが更新されない原因になる)
        playersDataList = PlayerPrefsUtility.LoadList<(string, int)>("PlayersDataList_Key");

        Debug.Log($"playersDataListの情報を取得しました：{playersDataList.Count}");
    }

    /// <summary>
    /// 各Listの初期化(Listからいなくなった敵の情報などはMissingとして残り続けるため)
    /// </summary>
    public void ResetGameData()
    {
        enemiesList.Clear();
        targetList.Clear();
    }

    /// <summary>
    /// 初期設定の準備
    /// </summary>
    //public void PrepareSetUpGameData(TitlePopUp titlePop)
    //{
    //    StartCoroutine(SetUpGameData(titlePop));
    //}

    /// <summary>
    /// 初期設定の実処理
    /// </summary>
    /// <param name="titlePop"></param>
    //private IEnumerator SetUpGameData(TitlePopUp titlePop)
    //{
    //    //シーン遷移後にアサイン情報が消えるので毎回ゲーム開始時に外部クラスから情報を代入
    //    txt1stName = titlePop.Txt1stName;
    //    txt1stScore = titlePop.Txt1stScore;
    //    txt2ndName = titlePop.Txt2ndName;
    //    txt2ndScore = titlePop.Txt2ndScore;
    //    txt3rdName = titlePop.Txt3rdName;
    //    txt3rdScore = titlePop.Txt3rdScore;

    //    yield return null;

    //    //Debug.Log("SetUpGameData動きました");
    //}

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
    //public void OrganizePlayersDataList()
    //{
    //何回foreach文が回ったか
    //int count = 0;

    //ランキングに同じプレイヤーが入らないようにプレイヤーの名前を保管するリストを宣言
    //List<string> playerList = new();

    //スコアが高い順にPlayersDataListを並び替える
    //var sortedPlayersDataList = playersDataList.OrderByDescending(data => data.score);

    //sortedPlayersDataDicから順に要素を取り出す
    //foreach (var playerData in sortedPlayersDataList)
    //{
    //    //同じプレイヤーが含まれていた場合、リストから削除
    //    if (playerList.Contains(playerData.playerName))
    //    {
    //        playersDataList.Remove((playerData.playerName, playerData.score));
    //    }

    //    //ランキング4位以下もリストから削除
    //    if (count >= 3)
    //    {
    //        playersDataList.Remove((playerData.playerName, playerData.score));

    //        count++;

    //        //必要なデータのみリストに保存できたらメソッドから抜ける
    //        continue;
    //    }

    //    //同じプレイヤーが含まれておらず、かつスコアが0以上の場合
    //    if (!playerList.Contains(playerData.playerName) && playerData.score > 0)
    //    {
    //        if (count == 2)
    //        {
    //            //ランキングに入るかどうかを決めるthirdScore変数に値を代入
    //            GameData.instance.thirdScore = playerData.score;

    //            //thirdScoreをセーブ
    //            PlayerPrefs.SetInt("ThirdScore_Key", thirdScore);
    //            PlayerPrefs.Save();
    //        }
    //    }

    //    //ランキングに同じプレイヤーが重複しないようにリストに追加
    //    playerList.Add(playerData.playerName);

    //    count++;
    //}

    //上記の書き換え
    //var filteredDataList = sortedPlayersDataList.Where((data, index) =>  //<= indexはsortedPlayersDataListの各要素に対するインデックスが自動的に割り当てられる。
    //{
    //    //4位以下、または同じプレイヤーが含まれている場合
    //    if (index >= 3 || playerList.Contains(data.playerName) || data.score <= 0)
    //    {
    //        //データを削除
    //        playersDataList.Remove((data.playerName, data.score));
    //    }

    //    //重複を防ぐためにプレイヤーの名前をリストに追加
    //    playerList.Add(data.playerName);

    //    //3位のスコアを保持
    //    if (index == 2)
    //    {
    //        thirdScore = data.score;

    //        PlayerPrefs.SetInt("ThirdScore_Key", thirdScore);
    //        PlayerPrefs.Save();
    //    }

    //    return true;
    //}).ToList();  //<= LINQで利用するメソッド(WhereやSelectなど)は戻り値がIEnumerable型になるので、filteredDataListはIEnumerable型。今回は下でList型を要求しているのでToList()でList型にする。

    ////デバッグ用
    //foreach (var data in filteredDataList)
    //{
    //    Debug.Log($"PlayerName：{data.playerName}, Score：{data.score}");
    //}

    //foreach (var playerName in playerList)
    //{
    //    Debug.Log($"PlayerName：{playerName}");
    //}

    //playersDataList = filteredDataList;

    //整理されたデータを保存
    //PlayerPrefsUtility.SaveList<(string, int)>("PlayersDataList_Key", filteredDataList);
    //}

    /// <summary>
    /// PlayersDataListの整理と並び替え。上記ではうまくいかないので修正
    /// </summary>
    /// <param name="newPlayerName"></param>
    /// <param name="newTotalScore"></param>
    public void OrganizePlayersDataList(string newPlayerName, int newTotalScore)
    {
        //セーブしておいた3位の記録を取得
        int thirdScore = PlayerPrefs.GetInt("ThirdScore_Key");

        //スコアが3位以内でなければ処理しない
        if (newTotalScore <= thirdScore)
        {
            return;
        }

        //同じ名前の情報がList内に登録されている場合
        if (playersDataList.Exists(playerData => playerData.playerName == newPlayerName))
        {
            //playersDataList.RemoveAll(playerData => playerData.playerName == newPlayerName);  <= これだと、スコアが何であろうと常に古い方のデータが消えてしまう

            //同じ名前の情報を抽出
            var sameNameData = playersDataList.Where(data => data.playerName == newPlayerName).ToList();
            //sameNameData.Add((newPlayerName, newTotalScore));  //<= 新しいデータを追加しないと、リストの中身は1個なので常にplayersDataListに入っている古いデータが削除されることになるので注意

            //抽出された情報の中から、スコアが小さい方を取り出す
            //var minScoreData = sameNameData.OrderBy(data => data.score).FirstOrDefault();

            //スコアが小さい方の情報を削除
            //playersDataList.Remove(minScoreData);  //<= newScoreが上回った場合、まだplayersDataListにminScoreDataの情報が入っていないため、消せない


            //newTotalScoreが上回る場合
            if (sameNameData[0].score < newTotalScore)
            {
                //古いデータを消す
                playersDataList.RemoveAll(playerData => playerData.playerName == newPlayerName);
            }
            //newTotalScoreが上回らない場合(例：1位に同じ名前が入っていて、newScoreが2位に入る値を持っている時など)
            else
            {
                //新しいデータは追加せず(スコアが高い方のデータを残すので、新しいデータはいらない)、そのままplayersDataListの並び替えと保存だけ行って処理を終了する。(理解が難しかったら絵を描いて想像する)
                playersDataList = playersDataList.OrderByDescending(data => data.score).ToList();

                PlayerPrefsUtility.SaveList<(string, int)>("PlayersDataList_Key", playersDataList);

                return;
            }
        }

        //3位まで情報が入っている場合、追加するまでに3位の情報を削除
        //(1位と2位しかない場合には、ここは処理しない)
        if (playersDataList.Count >= 3)
        {
            playersDataList.RemoveAt(2);
        }

        //スコアが3位以内なのでリストに値を追加
        playersDataList.Add((newPlayerName, newTotalScore));

        Debug.Log($"スコアを追加します：{playersDataList.Count}");

        //スコアの降順に並び替えてListを更新
        playersDataList = playersDataList.OrderByDescending(data => data.score).ToList();

        //整理されたデータを保存
        PlayerPrefsUtility.SaveList<(string, int)>("PlayersDataList_Key", playersDataList);

        //3位の更新
        thirdScore = playersDataList.Count > 2 ? playersDataList[2].score : 0;
        PlayerPrefs.SetInt("ThirdScore_Key", thirdScore);

        //ログ表示してデバッグする
        foreach (var playerData in playersDataList)
        {
            Debug.Log($"{playerData.playerName}, {playerData.score}");
        }

        Debug.Log("OrganizeListが動きました");
    }

    /// <summary>
    /// ランキング読み込み
    /// </summary>
    //public void LoadRanking()
    //{
    //    //セーブしておいたplayersDataリストをロード
    //    playersDataList = PlayerPrefsUtility.LoadList<(string, int)>("PlayersDataList_Key");

    //    //何回foreach文が回ったか。この値でどの順位にどの値を入れるかを決める
    //    int count = 0;

    //    //スコアが高い順にリストを並び替え
    //    //var sortedPlayersDataList = playersData.OrderByDescending(data => data.score);
    //    var sortedPlayersDataList = playersDataList.OrderByDescending(data => data.score);

    //    foreach (var playerData in sortedPlayersDataList)
    //    {
    //        //スコアが0以上であればタイトル画面の対応するランキングに表示
    //        if (playerData.score >= 0)
    //        {
    //            if (count == 0)
    //            {
    //                txt1stName.text = playerData.playerName;
    //                txt1stScore.text = playerData.score.ToString();
    //            }
    //            if (count == 1)
    //            {
    //                txt2ndName.text = playerData.playerName;
    //                txt2ndScore.text = playerData.score.ToString();
    //            }
    //            if (count == 2)
    //            {
    //                txt3rdName.text = playerData.playerName;
    //                txt3rdScore.text = playerData.score.ToString();

    //                Debug.Log("ランキングを読み込みました");

    //                //ランキングが全て埋まったら抜ける
    //                return;
    //            }
    //        }

    //        count++;
    //    }
    //}
}
