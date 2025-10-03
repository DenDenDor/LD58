using System;

[Serializable]
public class SaveData
{
    [AutoGenerateSaveMethod] public float MusicValue = 0.5f;
    [AutoGenerateSaveMethod] public float SoundValue = 0.5f;
    [AutoGenerateSaveMethod] public int Coins = 0;

    [AutoGenerateSaveMethod] public int LevelIncome;
    [AutoGenerateSaveMethod] public int LevelLuck;
    [AutoGenerateSaveMethod] public bool IsLanguageSelected;
    
    public string Language;
}