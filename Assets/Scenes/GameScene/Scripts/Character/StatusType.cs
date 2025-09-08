public enum StatusType
{
    LIFE, // ライフ
    BTRY, // バッテリー
    POW, // アタック
    DEF, // ディフェンス
    TEC, // テクニック
    SPD, // スピード
    LUK, // ラック
    MMR, // メモリ
    STG, // ストレージ
    POC, // ポーチ
    BAG, // バッグ
}

public static class StatusTypeExtensions
{
    public static int GetStatusIncreaseAmount(this StatusType status)
    {
        return status switch
        {
            StatusType.LIFE => 10,
            StatusType.BTRY => 5,
            StatusType.POW => 1,
            StatusType.DEF => 1,
            StatusType.TEC => 1,
            StatusType.SPD => 1,
            StatusType.LUK => 1,
            StatusType.MMR => 1,
            StatusType.STG => 1,
            StatusType.POC => 1,
            StatusType.BAG => 1,
            _ => 0
        };
    }
}