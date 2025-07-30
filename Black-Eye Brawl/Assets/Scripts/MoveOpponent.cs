using UnityEngine;
using System.Collections;

public class MoveOpponent : MonoBehaviour
{
    public AnimationController animationController;

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
    public int baseAttackChance = 50;
    public bool isBlocking;

    public float actionInterval = 3f;
    public float actionCooldown = 0.5f;

    public BlockScript block;

    [Header("Attack Damages")]
    public float crossDamage = 10;
    public float jabDamage = 5;
    public float rightHookDamage = 20;
    public float leftHookDamage = 15;
    public float uppercutDamage = 20;
    public float hammerDamage = 25;

    [Header("Attack Speeds")]
    public float crossSpeed = 0.5f;
    public float jabSpeed = 0.25f;
    public float hookSpeed = 1.5f;
    public float uppercutSpeed = 1f;
    public float hammerSpeed = 1.5f;
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
        if (randInt < blockChance)
            Block(direction);
    }

    void DecideAction()
    {
        int randInt = RNG();
        float attackChance = baseAttackChance;

        float distanceToPlayer = Mathf.Abs(transform.position.x - playerHorizontalPosition);

        if (distanceToPlayer < 1)
            attackChance += 20;
        else
            attackChance -= 20;

        if(randInt < attackChance)
        {
            block.DisableTargets();
            animationController.SwitchToIdle();
            StartCoroutine(Attack());
        }
        else
        {
            block.EnableTargets();
            animationController.SwitchToWalk();
            Move();
        }

        StartCoroutine(WaitForNextAction());
    }
    void Block(Direction direction)
    {
        block.EnableTargets();
        animationController.SwitchToIdle();

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
    public void FinishBlock()
    {
        isBlocking = false;
        blockDirection = Direction.None;
        block.BlockDirection(blockDirection);
    }
    IEnumerator Attack()
    {
        if (isBlocking)
            yield break;

        float randInt = RNG();

        if (randInt <= 20)
        {
            animationController.Cross();
            yield return new WaitForSeconds(crossSpeed);
            Cross();
        }           
        else if (randInt <= 40)
        {
            animationController.RightHook();
            yield return new WaitForSeconds(hookSpeed);
            RightHook();
        }         
        else if (randInt <= 60)
        {
            animationController.LeftHook();
            yield return new WaitForSeconds(hookSpeed);
            LeftHook();
        }          
        else if (randInt <= 80)
        {
            animationController.Uppercut();
            yield return new WaitForSeconds(hookSpeed);
            Uppercut();
        }          
        else
        {
            animationController.Hammer();
            yield return new WaitForSeconds(hookSpeed);
            Hammer();
        }
            

            isAttacking = true;
        isBlocking = false;
        isMoving = false;
    }
    void FinishAttack()
    {
        isAttacking = false;
    }
    void SendHitPosition(float xValue, float damage, Direction direction)
    {
        manager.RecieveOpponentHit(xValue, damage, direction);
    }
    void Cross()
    {
        float hitPosition = transform.position.x;
        SendHitPosition(hitPosition, crossDamage, Direction.Center);
        FinishAttack();
    }
    void RightHook()
    {
        animationController.RightHook();

        float hitPosition = transform.position.x - 0.5f;
        SendHitPosition(hitPosition, rightHookDamage, Direction.Right);
        FinishAttack();
    }
    void LeftHook()
    {
        float hitPosition = transform.position.x + 0.5f;
        SendHitPosition(hitPosition, leftHookDamage, Direction.Left);
        FinishAttack();
    }
    void Uppercut()
    {
        float hitPosition = transform.position.x;
        SendHitPosition(hitPosition, uppercutDamage, Direction.Down);
        FinishAttack();
    }
    void Hammer()
    {
        float hitPosition = transform.position.x;
        SendHitPosition(hitPosition, hammerDamage, Direction.Up);
        FinishAttack();
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
