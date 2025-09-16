using UnityEngine;

public class DecreaseOxygen : MonoBehaviour
{
    public float decreaseAmount = 10f; // Amount of oxygen to decrease
    public float decreaseInterval = 1f; // Interval in seconds to decrease oxygen
    private float timer;

    private void Update()
    {
        // <summary>
        // Decrease oxygen over time
        // </summary>
        timer += Time.deltaTime;
        if (timer >= decreaseInterval)
        {
            PlayerStatsManager.instance.currentOxygen -= decreaseAmount; // Decrease oxygen
            PlayerStatsManager.instance.currentOxygen = Mathf.Clamp(PlayerStatsManager.instance.currentOxygen, 0, PlayerStatsManager.instance.maxOxygen); // Clamp oxygen between 0 and maxOxygen
            timer = 0f; // Reset timer
        }
        Debug.Log("Current Oxygen: " + PlayerStatsManager.instance.currentOxygen);

        if (PlayerStatsManager.instance.currentOxygen <= 0)
        {
            PlayerStatsManager.instance.currentHealth -= 5f * Time.deltaTime; // Decrease health when oxygen is zero
            PlayerStatsManager.instance.currentHealth = Mathf.Clamp(PlayerStatsManager.instance.currentHealth, 0, PlayerStatsManager.instance.maxHealth); // Clamp health between 0 and maxHealth
        }
        Debug.Log("Current Health: " + PlayerStatsManager.instance.currentHealth);
    }
}