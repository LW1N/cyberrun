using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Renderer playerRenderer; // Reference to the player's renderer
    public Color damageColor = Color.red; // Color to indicate damage
    public float colorChangeDuration = 0.2f;

    public AudioClip hitSoundClip; // Sound effect for when the player gets hit
    private AudioSource audioSource; // Reference to the AudioSource component

    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthSlider == null)
        {
            Debug.Log("No healthSlider object");
        }
        else
        {
            healthSlider.maxValue = maxHealth;
            // Update the UI at the start
            UpdateHealthUI();
            Debug.Log("Player Health system initialized.");
        }

        // Get the AudioSource component from the current GameObject or add one if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // Ensure health doesn't go below 0 or above max
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player took {damage} damage. Current Health: {currentHealth}/{maxHealth}");
        UpdateHealthUI();

        // Play hit sound effect if available
        if (hitSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSoundClip, 3f);
        }

        if (playerRenderer != null)
        {
            // Change color temporarily to indicate damage
            StartCoroutine(ChangeColorTemporary(damageColor));
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died");
            GameManager.instance.ShowGameOverScreen(true);
        }

    }
    
    // Coroutine to temporarily change the player's color
    IEnumerator ChangeColorTemporary(Color color)
    {
        playerRenderer.material.color = color;
        yield return new WaitForSeconds(colorChangeDuration);
        playerRenderer.material.color = Color.white;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player healed {amount}. Current Health: {currentHealth}/{maxHealth}");
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            Debug.Log($"Updating UI with Current Health: {currentHealth}");
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.Log("Health Text component not assigned!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Eventually replace the tag to "Enemy" 
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(50);
            Debug.Log("Collision with Enemy detected.");
        }
    }
}
