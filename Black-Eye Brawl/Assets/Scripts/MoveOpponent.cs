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

    public bool isMoving;
    public bool isAttacking;
    public int attackChance = 50;
    public bool isBlocking;

    public float actionInterval = 3f;
    public float actionCooldown = 0.5f;

    public BlockScript block;

    void Start()
    {
        blockDirection = Direction.Left;
        startingY = transform.position.y;
        startingZ = transform.position.z;

        DecideAction();
        blockDirection = Direction.None;
    }

    void Update()
    {
        if(isMoving)
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
            FinishBlock();
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

    int RNG()
    {
        int randInt = Random.Range(1, 100);
        return randInt;
    }
    public void RecieveAttackDirection(Direction direction)
    {
        if (isAttacking)
            return;

        int randInt = RNG();

        int blockChance = 100;

        switch(direction)
        {
            case Direction.Up:
                blockChance = 75;
                break;
            case Direction.Down:
                blockChance = 50;
                break;
            case Direction.Left:
                blockChance = 30;
                break;
            case Direction.Right:
                blockChance = 40;
                break;
            case Direction.Center:
                blockChance = 20;
                break;
        }
        blockChance = 100;
        if (randInt < blockChance)
            Block(direction);
    }

    void DecideAction()
    {
        int randInt = RNG();

        if(randInt < attackChance)
        {
            //Attack();
        }
        else
        {
            Move();
        }

        StartCoroutine(WaitForNextAction());
    }
    void Block(Direction direction)
    {
        isBlocking = true;
        isAttacking = false;
        isMoving = false;
        DecideBlockDirection(direction);

        block.BlockDirection(direction);
    }
    void DecideBlockDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                blockDirection = Direction.Up;
                break;
            case Direction.Down:
                blockDirection = Direction.Down;
                break;
            case Direction.Left:
                blockDirection = Direction.Right;
                break;
            case Direction.Right:
                blockDirection = Direction.Left;
                break;
            case Direction.Center:
                blockDirection = Direction.Center;
                break;
            case Direction.None:
                FinishBlock();
                break;
        }
    }
    void FinishBlock()
    {
        blockDirection = Direction.None;
        block.BlockDirection(blockDirection);
    }
    void Attack()
    {
        if (isBlocking)
            return;

        isAttacking = true;
        isBlocking = false;
        isMoving = false;
    }
    void Move()
    {
        isMoving = true;
        isAttacking = false;
        isBlocking = false;
    }
    IEnumerator WaitForNextAction()
    {
        yield return new WaitForSeconds(actionInterval);
        DecideAction();
    }
}
