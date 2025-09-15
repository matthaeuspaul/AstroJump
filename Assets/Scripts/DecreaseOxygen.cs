using UnityEngine;

public class DecreaseOxygen : MonoBehaviour
{
    public float decreaseAmount = 10f; // Amount of oxygen to decrease
    public float decreaseInterval = 1f; // Interval in seconds to decrease oxygen
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= decreaseInterval)
        {
            PlayerStatsManager.instance.currentOxygen -= decreaseAmount;
            PlayerStatsManager.instance.currentOxygen = Mathf.Clamp(PlayerStatsManager.instance.currentOxygen, 0, PlayerStatsManager.instance.maxOxygen);
            timer = 0f;
        }
    }
}