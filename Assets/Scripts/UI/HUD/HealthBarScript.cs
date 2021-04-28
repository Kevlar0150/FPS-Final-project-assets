using UnityEngine;
using UnityEngine.UI;

// Entire script has been produced following the tutorial by Wayra Codes(2020), https://www.youtube.com/watch?v=NE5cAlCRgzo
// The resulting script has been adapted to retrieve the player using the FindGameObjectWithTag function.
public class HealthBarScript : MonoBehaviour
{
    private Image healthBar;
    public float currentHealth;
    private float maxHealth;
    [SerializeField]Player player;

    private void Start()
    {
        healthBar = GetComponent<Image>(); // Get Image component
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();// Get player object
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();// Get player object
        currentHealth = player.health; // Current health variable gets Player health variable.
        maxHealth = player.maxHealth; // Max health variable gets player's max Health variable.
        healthBar.fillAmount = currentHealth / maxHealth; // fill the healthbar based on currentHealth/maxHealth
    }
}
