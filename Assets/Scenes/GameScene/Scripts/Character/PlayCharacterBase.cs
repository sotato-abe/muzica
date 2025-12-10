using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/PlayCharacterBase")]
public class PlayCharacterBase : ScriptableObject
{
    [SerializeField] CharacterBase characterBase;

    [Header("Story information")]
    [SerializeField] string storyName;
    [SerializeField] CharacterIndex characterIndex;
    [SerializeField] Vector2Int startPosition;
    [SerializeField] int startYear;
    [SerializeField] int startMonth;
    [SerializeField] int startDay;
    [SerializeField, TextArea] string memo;

    public CharacterBase Base { get => characterBase; }
    public string StoryName { get => storyName; }
    public CharacterIndex CharacterIndex { get => characterIndex; }
    public Vector2Int StartPosition { get => startPosition; }
    public int StartYear { get => startYear; }
    public int StartMonth { get => startMonth; }
    public int StartDay { get => startDay; }
    public string Memo { get => memo; }
}
