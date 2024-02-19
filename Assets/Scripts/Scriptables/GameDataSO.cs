using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameDataSO : ScriptableObject
{
    public int bestScore;
    public int totalCoins;

void OnEnable(){

}

    public void SaveGameData()
    {
        PlayerPrefs.SetInt(nameof(bestScore), bestScore);
        PlayerPrefs.SetInt(nameof(totalCoins), totalCoins);
        PlayerPrefs.Save();
    }

    public void LoadGameData()
    {
        bestScore = PlayerPrefs.GetInt(nameof(bestScore));
        totalCoins = PlayerPrefs.GetInt(nameof(totalCoins));
    }
}
