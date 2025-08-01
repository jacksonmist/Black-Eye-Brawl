using UnityEngine;

public class UIController : MonoBehaviour
{
    public Transform playerHealthBar;
    public Transform playerStaminaBar;

    public Transform opponentHealthBar;
    public Transform opponentStaminaBar;

    float playerHealth;
    float playerStamina;

    float opponentHealth;
    float opponentStamina;

    void Start()
    {

    }

    void Update()
    {

    }

    public void RecievePlayerValues(float health, float stamina)
    {
        playerHealth = (health / 100f);
        playerStamina = (stamina / 100f);

        Vector2 healthVector = new Vector2(playerHealth, 1f);
        Vector2 staminaVector = new Vector2(playerStamina, 1f);

        playerHealthBar.localScale = healthVector;
        playerStaminaBar.localScale = staminaVector;
    }
    public void RecieveOpponentValues(float health, float stamina)
    {
        opponentHealth = (health / 100f);
        opponentStamina = (stamina / 100f);

        Vector2 healthVector = new Vector2(opponentHealth, 1f);
        Vector2 staminaVector = new Vector2(opponentStamina, 1f);

        opponentHealthBar.localScale = healthVector;
        opponentStaminaBar.localScale = staminaVector;
    }
}

