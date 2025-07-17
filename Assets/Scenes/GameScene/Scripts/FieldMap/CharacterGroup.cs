using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterGroup")]
public class CharacterGroup : ScriptableObject
{
    [SerializeField] List<Character> CharacterList;

    public List<Character> GetCharacterList()
    {
        return CharacterList;
    }

    public List<Character> GetRandomCharacterList()
    {
        List<Character> copy = new List<Character>(CharacterList);

        if (copy.Count == 0)
            return new List<Character>();

        int count = Random.Range(1, copy.Count + 1);

        // シャッフル
        for (int i = copy.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (copy[i], copy[j]) = (copy[j], copy[i]); // C# 7.0 以降のタプルスワップ
        }

        return copy.Take(count).ToList();
    }
}
