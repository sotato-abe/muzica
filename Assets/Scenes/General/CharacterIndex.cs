using UnityEngine;

public enum CharacterIndex
{
    Huh,
    Tera,
    Sola,
}

public static class CharacterIndexExtensions
{
    public static string GetCharacterFileName(this CharacterIndex characterIndex)
    {
        return characterIndex switch
        {
            CharacterIndex.Huh => "huh_",
            CharacterIndex.Tera => "tera_",
            CharacterIndex.Sola => "sola_",
            _ => "sola_"
        };
    }
}