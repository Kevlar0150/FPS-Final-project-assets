using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    private Image healthBar;
    public float currentHealth;
    private float maxHealth;
    Player player;

    private void Start()
    {
        healthBar = GetComponent<Image>(); // Get Image component
        player = FindObjectOfType<Player>(); // Get player object
    }

    private void Update()
    {
        currentHealth = player.health; // Current health variable gets Player health variable.
        maxHealth = player.maxHealth; // Max health variable gets player's max Health variable.
        healthBar.fillAmount = currentHealth / maxHealth; // fill the healthbar based on currentHealth/maxHealth
    }
}
