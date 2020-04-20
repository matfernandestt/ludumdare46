using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBattleData", menuName = "Game Design/Player Battle Data")]
public class PlayerBattleData : ScriptableObject
{
    public int MaxHealthPoints;
    public int HealthPoints;
    public int Strength;

    public void Heal(int healQuanity)
    {
        HealthPoints += healQuanity;
        if (HealthPoints > MaxHealthPoints)
            HealthPoints = MaxHealthPoints;
    }
    
    public void ResetHealth()
    {
        HealthPoints = MaxHealthPoints;
    }
    
    public void ReceiveDamage(int dmg)
    {
        HealthPoints -= dmg;
        if (HealthPoints <= 0)
        {
            HealthPoints = 0;
            GameEvents.OnGameOver?.Invoke();
        }
    }
}
