using UnityEngine;
using System.Collections;

public class MoveOpponent : MonoBehaviour
{
    public float opponentHealth = 100;
    public float opponentStamina = 100;
    public float staminaRecoveryCooldown = 1;
    public float regenRate = 0.1f;

    public Direction blockDirection;

    public GameManager manager;

    public Coroutine parentCoroutine;
    public Coroutine childCoroutine;

    public float opponentSpeed;
    public float playerHorizontalPosition;

    float startingY;
    float startingZ;

    void Start()
    {
        blockDirection = Direction.Left;
        startingY = transform.position.y;
        startingZ = transform.position.z;
    }

    void Update()
    {
        MoveToPlayer();
    }

    public void TakeDamage(float damage, Direction direction)
    {
        if(parentCoroutine != null)
            StopCoroutine(parentCoroutine);
        if(childCoroutine != null)
            StopCoroutine(childCoroutine);


        if (CheckBlock(direction))
        {
            opponentStamina -= (damage * 2);
            if (opponentStamina < 0)
                opponentStamina = 0;         
        }
        else
        {
            opponentHealth -= damage;
        }
        
        if (opponentHealth < 0)
            opponentHealth = 0;

        parentCoroutine = StartCoroutine(WaitForStaminaRegen());
        print($"Opponent Health {opponentHealth}");
        print($"Opponent Stamina {opponentStamina}");
    }
    bool CheckBlock(Direction direction)
    {
        bool isBlocked = false;

        if (direction == Direction.Right && blockDirection == Direction.Left)
        {
            isBlocked = true;
        }          
        else if (direction == Direction.Left && blockDirection == Direction.Right)
        {
            isBlocked = true;
        }          
        else if (direction == blockDirection && (direction == Direction.Down || direction == Direction.Up || direction == Direction.Center))
            isBlocked = true;

        return isBlocked;
    }
    IEnumerator WaitForStaminaRegen()
    {
        yield return new WaitForSeconds(staminaRecoveryCooldown);
        childCoroutine = StartCoroutine(RegenStamina());
    }
    IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(regenRate);
        opponentStamina += 1;
        if(opponentStamina >= 100)
        {
            opponentStamina = 100;
            yield break;
        }    
        childCoroutine = StartCoroutine(RegenStamina());
    }
    public void UpdatePlayerPosition(float xValue)
    {
        playerHorizontalPosition = xValue;
    }
    void MoveToPlayer()
    {
        float step = opponentSpeed * Time.deltaTime;
        Vector3 target = new Vector3(playerHorizontalPosition, startingY, startingZ);
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
