public enum MessageType
{
    Encount,  //遭遇
    Attack,   //攻撃
    Recovery,   //回復        
    Miss,       //ミス 
    Damage,   //ダメージ 
    Escape,   //逃亡
    Win,     //勝利
    Lose,     //敗北
    Talk,     //トーク
    Question,     //ナゾ
}

public static class MessageTypeExtensions
{
    public static string GetDefaultMessage(this MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.Encount:
                return "ちょっとつきあえや";
            case MessageType.Attack:
                return "くらえ";
            case MessageType.Recovery:
                return "これで大丈夫";
            case MessageType.Damage:
                return "いてぇ";
            case MessageType.Miss:
                return "ミスった！";
            case MessageType.Escape:
                return "まて !";
            case MessageType.Win:
                return "よし";
            case MessageType.Lose:
                return "くそぅ";
            case MessageType.Talk:
                return "おい、話があるんだ";
            case MessageType.Question:
                return "?";
            default:
                return "";
        }
    }
}