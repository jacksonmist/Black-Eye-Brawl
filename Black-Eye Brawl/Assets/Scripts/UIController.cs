using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameManager manager;
    public AudioManager audioManager;

    public Transform playerHealthBar;
    public Transform playerStaminaBar;

    public Transform opponentHealthBar;
    public Transform opponentStaminaBar;

    float playerHealth;
    float playerStamina;

    float opponentHealth;
    float opponentStamina;

    public GameObject bars;

    public GameObject startButton;
    public GameObject retryButton;

    public GameObject countdownBox;
    public GameObject threeObject;
    public GameObject twoObject;
    public GameObject oneObject;
    public GameObject fightObject;

    public GameObject winObj;
    public GameObject loseObj;

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
    public void StartButton()
    {
        startButton.SetActive(false);
        StartCoroutine(Countdown());
    }
    public void Win()
    {
        winObj.SetActive(true);
        retryButton.SetActive(true);
    }
    public void Loss()
    {
        loseObj.SetActive(true);
        retryButton.SetActive(true);
    }
    public void RetryButton()
    {
        SceneManager.LoadScene("Scene");
    }
    IEnumerator Countdown()
    {
        countdownBox.SetActive(true);
        threeObject.SetActive(true);
        yield return new WaitForSeconds(1);
        threeObject.SetActive(false);
        twoObject.SetActive(true);
        yield return new WaitForSeconds(1);
        twoObject.SetActive(false);
        oneObject.SetActive(true);
        yield return new WaitForSeconds(1);
        oneObject.SetActive(false);
        countdownBox.SetActive(false);
        fightObject.SetActive(true);
        audioManager.PlayFightSound();
        yield return new WaitForSeconds(1);
        fightObject.SetActive(false);
        bars.SetActive(true);
        StartGame();
    }
    void StartGame()
    {
        manager.StartGame();
    }

}

