public enum EnchantType
{
    // 自身へ付与
    Acceleration,   // 1 加速（スタックにつきSPDを１上げる）
    Lucky,          // 2 幸運（スタックにつきLUKを１上げる）
    Gaze,           // 3 凝視 (敵のステータスを表示する)
    Analysis,       // 4 解析 (敵の詳細情報を表示する)
    Power,          // 5 パワー (アタック時に基本値を１上げる)
    Adrenalin,      // 6 アドレナリン（アタック時に倍率を10％上げる）
    Guard,          // 7 ガード (ガード時に基本値を１上げる)
    Solid,          // 8 堅固 (ガード時に倍率を10%上げる)
    Curing,         // 9 硬化 (アタックを受ける時、ダメージを半分にする、そのかわり行動できない。スタックできない。)
    Splinter,       // 10 トゲ (アタックを受ける時、攻撃者にスタック分のLIFEダメージを与える)
    Reflection,     // 11 反射 (アタックを受ける時、攻撃者にスタック分のBTRYダメージを与える)
    Camouflage,     // 12 迷彩 (アタックを受ける時、当たり判定をスタック＊5％下げる（下限30％）)   
    Clear,          // 13 クリア（ターン開始時に自身に付与されているデバフをランダムで一つ解除する、スタックできない。）
    
    // 装備に付与
    Restraint,      // 14 拘束（スタックにつきSPDを１下げる）
    UnLuckey,       // 15 不運（スタックにつきLUKを１下げる）
    Fatigue,        // 16 疲労（SOULを回復させない）
    Lock,           // 17 ロック（ランダムでスロットが１列実行不能になる（重複なし））
    Cipher,         // 18 暗号化（装備のエナジーコストが20％アップする）
    Bug,            // 19 バグ（コマンド発動時にスタック分BTRYダメージを受ける）
    Atrophy,        // 20 イシュク（アタック時に基本値を１下げる）
    Blind,          // 21 ブラインド (アタック時に成功確率をスタック＊２％下げる)
    Paralysis,      // 22 こんらん（アタック時にターゲット指定をランダムにする（自分も含まれる））
    Crack,          // 23 亀裂（ガード時に基本値を１下げる）
    Poison,         // 24 毒（ターン開始時にスタック分のLIFEダメージを受ける）
    Leakage,        // 25 漏電（ターン開始時にスタック分のBTRYダメージを受ける）
    Hurt,           // 26 怪我 (ターン開始時にスタック分のLIFEダメージを受ける)
    Sleep,          // 27 睡眠 (ターン開始時にスタック分の確率で発動しターンがスキップされる)
}

public static class EnchantTypeExtensions
{
    public static bool AssigneeSelf(this EnchantType enchant)
    {
        return enchant switch
        {
            EnchantType.Acceleration => true,
            EnchantType.Lucky => true,
            EnchantType.Gaze => true,
            EnchantType.Analysis => true,
            EnchantType.Power => true,
            EnchantType.Adrenalin => true,
            EnchantType.Guard => true,
            EnchantType.Solid => true,
            EnchantType.Curing => true,
            EnchantType.Splinter => true,
            EnchantType.Reflection => true,
            EnchantType.Camouflage => true,
            EnchantType.Clear => true,
            _ => false
        };
    }
}