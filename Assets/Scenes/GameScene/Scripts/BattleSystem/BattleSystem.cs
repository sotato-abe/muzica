using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    public UnityAction OnBattleEnd;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private BattleActionBoard battleActionBoard;
    [SerializeField] private PlayerSubPanel playerSubPanel; // キャラクターサブパネル
    [SerializeField] private EnemySubPanel enemySubPanel1; // キャラクターサブパネル
    [SerializeField] private EnemySubPanel enemySubPanel2; // キャラクターサブパネル
    [SerializeField] private EnemySubPanel enemySubPanel3; // キャラクターサブパネル
    [SerializeField] private MessagePanel messagePanel; // キャラクターサブパネル
    [SerializeField] WorldMapPanel worldMapPanel;
    [SerializeField] GameOverWindow gameOverWindow;
    [SerializeField] FieldPlayer fieldPlayer; //キャラクター
    [SerializeField] FieldEnemy fieldEnemyPrefab; //敵キャラクター
    [SerializeField] private GameObject enemyGroupArea; // 敵キャラクターの親オブジェクト
    [SerializeField] AgeTimePanel ageTimePanel;
    [SerializeField] SlidePanel optionActionBoard;

    List<FieldCharacter> fieldEnemies = new List<FieldCharacter>(); // フィールド上の敵キャラクターリスト
    List<EnemySubPanel> enemySubPanels = new List<EnemySubPanel>(); // 敵のサブパネルリスト

    int RewardExp = 0; // 経験値のカウント
    int RewardCoin = 0; // ゴールドのカウント
    string RewardItemListText = ""; // アイテムのカウント
    List<Item> RewardItems = new List<Item>(); // アイテムのカウント

    private void Awake()
    {
        battleActionBoard.OnBattleEnd += BattleEnd; // リザーブアクションボードの終了イベントを登録
        battleActionBoard.OnActionEnd += OnActionEnd; // リザーブアクションボードのアクション結果イベントを登録
        playerSubPanel.OnLifeOutAction += LifeOutPlayer; // プレイヤーのサブパネルのライフアウトイベントを登録
        enemySubPanel1.OnLifeOutAction += LifeOutEnemy; // 敵のサブパネルのライフアウトイベントを登録
        enemySubPanel2.OnLifeOutAction += LifeOutEnemy; // 敵のサブパネルのライフアウトイベントを登録
        enemySubPanel3.OnLifeOutAction += LifeOutEnemy; // 敵のサブパネルのライフアウトイベントを登録

        enemySubPanels.Add(enemySubPanel1);
        enemySubPanels.Add(enemySubPanel2);
        enemySubPanels.Add(enemySubPanel3);

        playerSubPanel.OnActiveTurn += ActivePlayerTurn;
        foreach (EnemySubPanel enemySubPanel in enemySubPanels)
        {
            enemySubPanel.OnActiveTurn += ActiveEnemyTurn;
        }
    }

    public void BattleStart()
    {
        if (PlayerController.Instance.CurrentEventType != EventType.Default)
            return;
        PlayerController.Instance.ChangeEventType(EventType.Battle); // イベントタイプをバトルに変更

        cameraManager.SetEventType(EventType.Battle); // バトル時のカメラ位置を設定
        // messagePanel.SetActive(false); // メッセージパネルを表示
        worldMapPanel.SetActive(false); // ワールドマップパネルを非表示
        optionActionBoard.SetActive(false); // セーブパネルを非表示

        playerSubPanel.SetActive(true); // キャラクターサブパネルを表示
        battleActionBoard.SetActive(true); // リザーブアクションボードを表示
        TalkMessage talkMessage = new TalkMessage(MessageType.Talk, MessagePanelType.Default, "なんだよ");
        StartCoroutine(playerSubPanel.SetTalkMessage(talkMessage)); // リザーブアクションボードを開く
        playerSubPanel.BattleStart(); // ターンバーを開始
        SoundSystem.Instance.PlaySE(SeType.BattleStart);
        SoundSystem.Instance.PlayBGM(BgmType.Battle);
        SetEnemy();
    }

    // TODO 敵を倒した時に更新で使用する
    private void SetEnemy()
    {
        fieldEnemies.Clear();
        List<Character> enemies = FieldController.Instance.GetEnemies();
        battleActionBoard.SetEnemyList(enemies);
        StartCoroutine(AppearanceEnemies(enemies));
    }

    // 敵をフィールドに出現させる
    private IEnumerator AppearanceEnemies(List<Character> enemies)
    {
        int index = 0;
        int LevelIncreaseCount = ageTimePanel.yearsElapsed * 2; // 1年ごとに2レベル上昇

        foreach (Character enemy in enemies)
        {
            enemy.Init();
            if (enemy is EnemyCharacter enemyCharacter)
            {
                enemyCharacter.LevelUp(LevelIncreaseCount); // レベルアップ
            }
            (Vector3 targetPos, bool isLeft, bool isFront) = GetRandomAroundFloorPosition();
            FieldEnemy fieldEnemy = Instantiate(fieldEnemyPrefab, targetPos, Quaternion.identity, enemyGroupArea.transform);
            fieldEnemies.Add(fieldEnemy);
            fieldEnemy.Inversion(!isLeft); // 向きを反転
            fieldEnemy.SetNumIcon(index); // 敵の番号アイコンを設定
            fieldPlayer.OrientationChange(isLeft); // プレイヤーの向きを反転
            SetEnemySubPanel(enemy, fieldEnemy, enemies.Count - (index + 1), enemies.Count); // 敵のサブパネルを設定
            yield return new WaitForSeconds(0.3f);
            index++;
        }
        battleActionBoard.EnemyAppearanced();
        yield break; // 全ての敵を出現させたらnullを返す
    }

    // 敵をサブパネルに出現させる
    private void SetEnemySubPanel(Character enemy, FieldEnemy fieldEnemy, int index, int maxCount)
    {
        if (index < 0 || index >= enemySubPanels.Count) return; // インデックスが範囲外の場合は何もしない
        EnemySubPanel subPanel = enemySubPanels[index];
        subPanel.SetEnemy(enemy, fieldEnemy); // 敵のキャラクターを設定
        subPanel.SetNumber(maxCount - index - 1); // 敵の番号を設定
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

    public void ActivePlayerTurn(CharacterSubPanel playerSubPanel)
    {
        StopAllCharacterTurnBar();
        battleActionBoard.ChangeExecuteActionFlg(true); // アクションを実行可能にする
    }

    // TODO：敵の攻撃を実装する
    public void ActiveEnemyTurn(BattleCharacterSubPanel enemySubPanel)
    {
        StopAllCharacterTurnBar();
        EnemyCharacter enemyCharacter = enemySubPanel.Character as EnemyCharacter;
        TotalAttackCount totalAttackCount = enemyCharacter.EnemyAttack();
        StartCoroutine(enemySubPanel.UpdateEnergyGauges());
        StartCoroutine(EnemyAttack(totalAttackCount)); // ターンを再開
    }

    // 仮の敵ターン
    private IEnumerator EnemyAttack(TotalAttackCount totalAttackCount)
    {
        fieldPlayer.SetAnimation(AnimationType.Damage);
        yield return StartCoroutine(playerSubPanel.TakeAttackCoroutine(totalAttackCount));
        OnActionEnd(); // アクション終了イベントを呼び出す
    }

    private void StopAllCharacterTurnBar()
    {
        playerSubPanel.PauseTurnBar(); // プレイヤーのターンバーを停止
        foreach (EnemySubPanel enemySubPanel in enemySubPanels)
        {
            enemySubPanel.PauseTurnBar(); // 敵のターンバーを停止
        }
    }

    private void OnActionEnd()
    {
        playerSubPanel.ReStartTurnBar();
        foreach (EnemySubPanel enemySubPanel in enemySubPanels)
        {
            if (enemySubPanel.isOpen)
                enemySubPanel.ReStartTurnBar();
        }
    }

    public void LifeOutPlayer(CharacterSubPanel characterSubPanel)
    {
        if (characterSubPanel == null) return;
        StopAllCharacterTurnBar();
        SoundSystem.Instance.PlayBGM(BgmType.GameOver);
        foreach (EnemySubPanel enemySubPanel in enemySubPanels)
        {
            enemySubPanel.PauseTurnBar(); // 敵のターンバーを停止
        }
        gameOverWindow.Show(); // ゲームオーバーウィンドウを表示
    }

    public void LifeOutEnemy(CharacterSubPanel enemySubPanel)
    {
        if (enemySubPanel == null) return;
        RewardExp += enemySubPanel.Character.Exp; // 経験値を加算
        RewardCoin += enemySubPanel.Character.Coin; // ゴールドを加算
        foreach (Item item in enemySubPanel.Character.BagItemList)
        {
            if (Random.Range(0, 100) < item.Base.Rarity.GetProbability())
            {
                RewardItems.Add(item); // アイテムを加算
                RewardItemListText += $"{item.Base.Name},";
            }
        }
        StartCoroutine(CheckEnemies());
    }

    private IEnumerator CheckEnemies()
    {
        // ActiveなEnemySubPanelの数を数える
        int activeEnemyCount = 0;
        foreach (EnemySubPanel enemySubPanel in enemySubPanels)
        {
            if (enemySubPanel.isOpen)
                activeEnemyCount++;
        }
        if (activeEnemyCount == 0)
        {
            string RewardText = "バトルに勝利した\n";
            if (RewardItemListText != "")
            {
                RewardItemListText = RewardItemListText.TrimEnd(','); // 最後のカンマを削除
                RewardText += $"Item: {RewardItemListText}";
            }
            RewardText += $" EXP: {RewardExp}, COIN: {RewardCoin}";
            messagePanel.AddMessage(MessageIconType.Battle, RewardText);
            PlayerController.Instance.AddBattleReward(RewardExp, RewardCoin, RewardItems); // バトル報酬を追加
            BattleEnd(); // 全ての敵を倒した場合はバトル終了
        }
        yield break; // 全ての敵を倒した場合はnullを返す
    }

    public void BattleEnd()
    {
        int completed = 0;
        if (fieldEnemies != null)
        {
            foreach (FieldCharacter fieldEnemy in fieldEnemies)
            {
                if (fieldEnemy != null)
                {
                    Destroy(fieldEnemy.gameObject); // フィールド上の敵キャラクターを削除
                }
            }
            fieldEnemies.Clear();
        }

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
        RewardCoin = 0; // ゴールドのリセット
        RewardItems.Clear(); // アイテムのリセット
        RewardItemListText = ""; // アイテムリストのリセット
        PlayerController.Instance.ClearGuard(); // ガードをクリア
        playerSubPanel.SetTarget(false); // ターゲットをリセット

        battleActionBoard.SetActive(false, CheckAllComplete); // リザーブアクションボードを非表示にする
        playerSubPanel.SetActive(false, CheckAllComplete); // キャラクターサブパネルを非表示にする
        enemySubPanel1.SetActive(false, CheckAllComplete); // キャラクターサブパネルを非表示にする
        enemySubPanel2.SetActive(false, CheckAllComplete); // キャラクターサブパネルを非表示にする
        enemySubPanel3.SetActive(false, CheckAllComplete); // キャラクターサブパネルを非表示にする

        // messagePanel.SetActive(true, CheckAllComplete); // メッセージパネルを表示
        worldMapPanel.SetActive(true, CheckAllComplete); // ワールドマップパネルを表示
        optionActionBoard.SetActive(true, CheckAllComplete); // セーブパネルを表示
        cameraManager.SetEventType(EventType.Default); // バトル時のカメラ位置を設定
        PlayerController.Instance.ChangeEventType(EventType.Default); // イベントタイプをデフォルトに変更
        SoundSystem.Instance.PlayBGM(BgmType.Field);
    }
}
