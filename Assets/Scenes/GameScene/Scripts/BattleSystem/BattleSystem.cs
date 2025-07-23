using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    public UnityAction OnBattleEnd;

    [SerializeField] private BattleActionBoard battleActionBoard;
    [SerializeField] private CharacterSubPanel playerSubPanel; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel enemySubPanel1; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel enemySubPanel2; // キャラクターサブパネル
    [SerializeField] private CharacterSubPanel enemySubPanel3; // キャラクターサブパネル
    [SerializeField] private MessagePanel messagePanel; // キャラクターサブパネル
    [SerializeField] WorldMapPanel worldMapPanel;
    [SerializeField] FieldPlayer fieldPlayer; //キャラクター
    [SerializeField] FieldEnemy fieldEnemyPrefab; //敵キャラクター
    [SerializeField] private GameObject enemyGroupArea; // 敵キャラクターの親オブジェクト
    List<FieldCharacter> fieldEnemies = new List<FieldCharacter>(); // フィールドの敵リスト
    List<CharacterSubPanel> enemySubPanels = new List<CharacterSubPanel>(); // 敵のサブパネルリスト

    int RewardExp = 0; // 経験値のカウント
    int RewardMoney = 0; // ゴールドのカウント
    string RewardItemListText = ""; // アイテムのカウント
    List<Item> RewardItems = new List<Item>(); // アイテムのカウント

    private void Awake()
    {
        battleActionBoard.OnBattleEnd += BattleEnd; // リザーブアクションボードの終了イベントを登録
        battleActionBoard.OnActionEnd += OnActionEnd; // リザーブアクションボードのアクション結果イベントを登録
        enemySubPanel1.OnLifeOutAction += LifeOutCharacter; // 敵のサブパネルのライフアウトイベントを登録
        enemySubPanel2.OnLifeOutAction += LifeOutCharacter; // 敵のサブパネルのライフアウトイベントを登録
        enemySubPanel3.OnLifeOutAction += LifeOutCharacter; // 敵のサブパネルのライフアウトイベントを登録
        enemySubPanels.Add(enemySubPanel1);
        enemySubPanels.Add(enemySubPanel2);
        enemySubPanels.Add(enemySubPanel3);

        playerSubPanel.OnActiveTurn += ActivePlayerTurn;
        foreach (CharacterSubPanel enemySubPanel in enemySubPanels)
        {
            enemySubPanel.OnActiveTurn += ActiveEnemyTurn;
        }
    }

    public void BattleStart()
    {
        battleActionBoard.SetActive(true); // リザーブアクションボードを表示
        playerSubPanel.SetActive(true); // キャラクターサブパネルを表示
        TalkMessage talkMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "なんだよ");
        StartCoroutine(playerSubPanel.SetTalkMessage(talkMessage)); // リザーブアクションボードを開く
        playerSubPanel.BattleStart(); // ターンバーを開始
        messagePanel.SetActive(false); // メッセージパネルを表示
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
        SetEnemy();
    }

    // TODO 敵を倒した時に更新で使用する
    private void SetEnemy()
    {
        List<Character> enemies = FieldController.Instance.GetEnemies();
        battleActionBoard.SetEnemyList(enemies);
        StartCoroutine(AppearanceEnemies(enemies));
    }

    // 敵をフィールドに出現させる
    private IEnumerator AppearanceEnemies(List<Character> enemies)
    {
        int index = 0;
        foreach (Character enemy in enemies)
        {
            (Vector3 targetPos, bool isLeft, bool isFront) = GetRandomAroundFloorPosition();
            FieldEnemy fieldEnemy = Instantiate(fieldEnemyPrefab, targetPos, Quaternion.identity, enemyGroupArea.transform);
            fieldEnemy.SetUp(enemy); // バトラーの設定を行う
            fieldEnemy.Inversion(isLeft); // 向きを反転
            fieldEnemy.SetNumIcon(index); // 敵の番号アイコンを設定
            fieldPlayer.Inversion(!isLeft); // プレイヤーの向きを反転
            SetEnemySubPanel(enemy, index); // 敵のサブパネルを設定
            fieldEnemies.Add(fieldEnemy); // 生成した敵をリストに追加
            yield return new WaitForSeconds(0.3f);
            index++;
        }
        yield break; // 全ての敵を出現させたらnullを返す
    }

    // 敵をサブパネルに出現させる
    private void SetEnemySubPanel(Character enemy, int index)
    {
        if (index < 0 || index >= enemySubPanels.Count) return; // インデックスが範囲外の場合は何もしない
        CharacterSubPanel subPanel = enemySubPanels[index];
        subPanel.SetBattleCharacter(enemy); // 敵のキャラクターを設定
        subPanel.BattleStart(); // ターンバーを開始
        subPanel.SetActive(true); // キャラクターサブパネルを表示
    }

    private (Vector3, bool, bool) GetRandomAroundFloorPosition(int range = 1)
    {
        // フィールドのランダムな位置を取得
        Vector3 pos = fieldPlayer.transform.position;
        // 0 は除外、-range ~ rangeの範囲でランダムな座標を取得
        int x = 0;
        int y = 0;
        bool isFront = true;
        bool isLeft = true;

        // (0,0) 以外になるまでランダムに取得
        while (x == 0 && y == 0)
        {
            x = Random.Range(-range, range + 1); // 上限は含まれないので +1
            y = Random.Range(-range, range + 1);
        }
        if (y < 0)
            isFront = false;
        if (x > 0)
            isLeft = false;

        Vector3 targetPos = new Vector3(pos.x + x, pos.y + y, 0); // プレイヤーの位置にランダムなオフセットを加算

        return (targetPos, isLeft, isFront);
    }

    public void ActivePlayerTurn(Character character)
    {
        StopAllPlayerTurnBar();
        battleActionBoard.ChangeExecuteActionFlg(true); // アクションを実行可能にする
    }

    // TODO：敵の攻撃を実装する
    public void ActiveEnemyTurn(Character character)
    {
        StopAllPlayerTurnBar();
        if (fieldEnemies.Count == 0)
        {
            Debug.LogWarning("敵がいません。");
            return;
        }
        StartCoroutine(EnemyTurn()); // ターンを再開
    }

    // 仮の敵ターン
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);
        // 敵の行動を実行
        OnActionEnd(); // アクション終了イベントを呼び出す
    }

    private void StopAllPlayerTurnBar()
    {
        playerSubPanel.PauseTurnBar(); // プレイヤーのターンバーを停止
        foreach (CharacterSubPanel enemySubPanel in enemySubPanels)
        {
            enemySubPanel.PauseTurnBar(); // 敵のターンバーを停止
        }
    }

    private void OnActionEnd()
    {
        playerSubPanel.ReStartTurnBar();
        foreach (CharacterSubPanel enemySubPanel in enemySubPanels)
        {
            if (enemySubPanel.isActive)
                enemySubPanel.ReStartTurnBar();
        }
    }

    public void LifeOutCharacter(CharacterSubPanel characterSubPanel)
    {
        if (characterSubPanel == null) return;
        characterSubPanel.SetActive(false);

        FieldCharacter fieldEnemy = fieldEnemies.Find(e => e.Character == characterSubPanel.Character);
        RewardExp += fieldEnemy.Character.Exp; // 経験値を加算
        RewardMoney += fieldEnemy.Character.Money; // ゴールドを加算
        foreach (Item item in characterSubPanel.Character.BagItemList)
        {
            if (Random.Range(0, 100) < item.Base.Rarity.GetProbability())
            {
                RewardItems.Add(item); // アイテムを加算
                RewardItemListText += $"{item.Base.Name},";
            }
        }

        if (fieldEnemy != null)
        {
            fieldEnemies.Remove(fieldEnemy); // フィールドの敵リストから削除
            Destroy(fieldEnemy.gameObject); // 敵キャラクターを削除
            characterSubPanel.gameObject.SetActive(false); // キャラクターサブパネルを非表示
        }

        StartCoroutine(CheckEnemies());
    }

    private IEnumerator CheckEnemies()
    {
        if (fieldEnemies.Count == 0)
        {
            yield return new WaitForSeconds(1f); // 少し待機してから処理を続行
            string RewardText = "バトルに勝利した\n";
            if (RewardItemListText != "")
            {
                RewardItemListText = RewardItemListText.TrimEnd(','); // 最後のカンマを削除
                RewardText += $"アイテム: {RewardItemListText}\n";
            }
            RewardText += $"EXP: {RewardExp}, マネー: {RewardMoney}";
            messagePanel.AddMessage(MessageIconType.Battle, RewardText);
            PlayerController.Instance.AddBattleReward(RewardExp, RewardMoney, RewardItems); // バトル報酬を追加
            BattleEnd(); // 全ての敵を倒した場合はバトル終了
        }
        yield break; // 全ての敵を倒した場合はnullを返す
    }

    public void BattleEnd()
    {
        int completed = 0;
        foreach (FieldCharacter fieldEnemy in fieldEnemies)
        {
            Destroy(fieldEnemy.gameObject); // 敵キャラクターを削除
        }
        fieldEnemies.Clear(); // 敵キャラクターのリストをクリア

        void CheckAllComplete()
        {
            completed++;
            if (completed >= 7)
            {
                OnBattleEnd?.Invoke();
                transform.gameObject.SetActive(false);
            }
        }
        RewardExp = 0; // 経験値のリセット
        RewardMoney = 0; // ゴールドのリセット
        RewardItems.Clear(); // アイテムのリセット
        RewardItemListText = ""; // アイテムリストのリセット

        battleActionBoard.SetActive(false, CheckAllComplete); // リザーブアクションボードを表示
        playerSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        enemySubPanel1.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        enemySubPanel2.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        enemySubPanel3.SetActive(false, CheckAllComplete); // キャラクターサブパネルを表示
        messagePanel.SetActive(true, CheckAllComplete); // メッセージパネルを表示
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
    }
}
