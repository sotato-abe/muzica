public enum AttackType
{
    SoloLifeUp, // 単体ライフアップ
    GroupLifeUp, // 複数ライフアップ
    SoloBatteryUp, // 単体バッテリーアップ
    GroupBatteryUp, // 複数バッテリーアップ
    SoloSoulUp, // ソウルアップ
    GroupSoulUp, // 複数ソウルアップ
    SoloGuard, // 単体防御
    GroupGuard, // 複数防御
    SoloSecurity, // 単体防護
    GroupSecurity, // 複数防護
    SoloBuff, // 単体バフ
    GroupBuff, // 複数バフ

    SoloLifeDown, // 単体ライフダウン
    GroupLifeDown, // 複数ライフダウン
    SoloBatteryDown, // 単体バッテリーダウン
    GroupBatteryDown, // 複数バッテリーダウン
    SoloSoulDown, // ソウルダウン
    GroupSoulDown, // 複数ソウルダウン
    SoloDebuff, // 単体デバフ
    GroupDebuff // 複数デバフ
}

public static class AttackTypeExtensions
{
    public static bool AssigneeSelf(this AttackType attack)
    {
        return attack switch
        {
            AttackType.SoloLifeUp => true,
            AttackType.GroupLifeUp => true,
            AttackType.SoloBatteryUp => true,
            AttackType.GroupBatteryUp => true,
            AttackType.SoloSoulUp => true,
            AttackType.GroupSoulUp => true,
            AttackType.SoloGuard => true,
            AttackType.GroupGuard => true,
            AttackType.SoloSecurity => true,
            AttackType.GroupSecurity => true,
            AttackType.SoloBuff => true,
            AttackType.GroupBuff => true,
            _ => false
        };
    }

    public static bool IsGroupAttack(this AttackType attack)
    {
        return attack switch
        {
            AttackType.GroupLifeUp => true,
            AttackType.GroupBatteryUp => true,
            AttackType.GroupSoulUp => true,
            AttackType.GroupGuard => true,
            AttackType.GroupSecurity => true,
            AttackType.GroupBuff => true,
            AttackType.GroupLifeDown => true,
            AttackType.GroupBatteryDown => true,
            AttackType.GroupSoulDown => true,
            AttackType.GroupDebuff => true,
            _ => false
        };
    }
}