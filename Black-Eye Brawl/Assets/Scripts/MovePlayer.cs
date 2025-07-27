using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MovePlayer : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction attackAction;
    public InputAction blockAction;

    public Direction blockDirection;

    public GameManager manager;

    public float moveVal;
    float attackVal;
    float blockVal;

    public bool isAttacking;
    public bool isBlocking;
    public bool isBlocked;
    public bool isBusy;

    public float playerSpeed;

    public float leftBound = -2.75f;
    public float rightBound = 2.75f;

    public float crossBoundary = 200f;

    public Vector3 moveVector;

    Vector2 screenCenter = new Vector2(557, 273);

    float startingY;
    float startingZ;

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
    public float hookSpeed = 0.75f;
    public float uppercutSpeed = 1f;
    public float hammerSpeed = 1.5f;

    [Header("Player Stats")]
    public float playerHealth = 100;
    public float playerStamina = 100;
    public float staminaRecoveryCooldown = 1;
    public float regenRate = 0.1f;

    public Transform rightArmTarget;
    public Transform leftArmTarget;

    public Vector3 rightTargetHomePosition;
    public Vector3 leftTargetHomePosition;

    public Transform rightFist;
    public Transform leftFist;
    public float armSpeed;

    [Header("Block Targets")]
    public Vector3 centerRight = new Vector3(0.1f, 0.2f, 6f);
    public Vector3 centerLeft = new Vector3(-0.1f, 0.2f, 6f);

    public Vector3 rightBlockRightArm = new Vector3(0.59f, 0.32f, 5.8f);

    public Vector3 leftBlockLeftArm = new Vector3(-0.75f, 0.3f, 7.4f);

    public Vector3 upBlockRightArm = new Vector3(0.17f, 0.46f, 4.7f);
    public Vector3 upBlockLeftArm = new Vector3(-0.1f, 0.48f, 5.89f);
    public Vector3 upRightRotationTarget = new Vector3(0f, 180f, 67f);
    public Vector3 upLeftRotationTarget = new Vector3(0f, 180f, -61f);

    public Vector3 downBlockRightArm = new Vector3(0.4f, 0.39f, 5.8f);
    public Vector3 downBlockLeftArm = new Vector3(-0.4f, 0.29f, 7.4f);
    public Vector3 downRightRotationTarget = new Vector3(0f, 180f, 85f);
    public Vector3 downLeftRotationTarget = new Vector3(0f, 180f, -85f);

    public Quaternion homeRotationTarget;

    public Quaternion rightRotationTarget;
    public Quaternion leftRotationTarget;
    public float rotationSpeed = 10;

    public Coroutine parentCoroutine;
    public Coroutine childCoroutine;

    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        attackAction = playerInput.actions.FindAction("Attack");
        blockAction = playerInput.actions.FindAction("Block");

        startingY = transform.position.y;
        startingZ = transform.position.z;
      
        rightTargetHomePosition = rightArmTarget.localPosition;
        leftTargetHomePosition = leftArmTarget.localPosition;

        rightArmTarget.localPosition = rightTargetHomePosition;
        leftArmTarget.localPosition = leftTargetHomePosition;

        homeRotationTarget = rightFist.rotation;

        LockCursor();
    }

    void Update()
    {
        ReadActions();
        ArmsToTarget();
    }

    void ReadActions()
    {
        moveVal = moveAction.ReadValue<Vector2>().x;
        attackVal = attackAction.ReadValue<float>();
        blockVal = blockAction.ReadValue<float>();

        if (attackVal == 1 || blockVal == 1)
            isBusy = true;
    
        if (!isBusy)
        {
            LockCursor();
            Move();
        }
        else
        {
            UnlockCursor();
            if ((attackVal == 1 || isAttacking) && !isBlocking)
                Attack();
            else if (blockVal == 1 || isBlocking)
                Block();
        }
    }
    public void TakeDamage(float damage, Direction direction)
    {
        if (parentCoroutine != null)
            StopCoroutine(parentCoroutine);
        if (childCoroutine != null)
            StopCoroutine(childCoroutine);


        if (CheckBlock(direction))
        {
            playerStamina -= (damage * 2);
            if (playerStamina < 0)
                playerStamina = 0;
        }
        else
        {
            playerHealth -= damage;
        }

        if (playerStamina < 0)
            playerStamina = 0;

        parentCoroutine = StartCoroutine(WaitForStaminaRegen());
    }
    IEnumerator WaitForStaminaRegen()
    {
        yield return new WaitForSeconds(staminaRecoveryCooldown);
        childCoroutine = StartCoroutine(RegenStamina());
    }
    IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(regenRate);
        playerStamina += 1;
        if (playerStamina >= 100)
        {
            playerStamina = 100;
            yield break;
        }
        childCoroutine = StartCoroutine(RegenStamina());
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
    void Move()
    {
        moveVector.x = -moveVal * playerSpeed * Time.deltaTime;

        transform.position += moveVector;
        if (transform.position.x > rightBound)
            transform.position = new Vector3(rightBound, startingY, startingZ);
        else if (transform.position.x < leftBound)
            transform.position = new Vector3(leftBound, startingY, startingZ);
    }

    void Attack()
    {
        isAttacking = true;
        if (attackVal == 0)
        {
            Vector2 mousePosition = GetMousePosition();
            float vectorMagnitude = mousePosition.magnitude;
            float angle = CalculateAngle(mousePosition);
            StartCoroutine(DecideAttack(vectorMagnitude, angle));
            isBusy = false;
        }
    }
    IEnumerator DecideAttack(float magnitude, float angle)
    {
        playerInput.DeactivateInput();
        if (magnitude < crossBoundary)
        {
            yield return new WaitForSeconds(crossSpeed);
            Cross();
        }
        else
        {
            if (angle > 45 && angle <= 135)
            {
                yield return new WaitForSeconds(uppercutSpeed);
                
                Uppercut();
            }               
            else if (angle > 135 && angle <= 225)
            {
                yield return new WaitForSeconds(hookSpeed);
                LeftHook();
            }                
            else if (angle > 225 && angle <= 315)
            {
                yield return new WaitForSeconds(hammerSpeed);
                Hammer();
            }               
            else if (angle > 315 || angle <= 45)
            {
                yield return new WaitForSeconds(hookSpeed);
                RightHook();
            }             
        }
    }
    void FinishAttack()
    {
        playerInput.ActivateInput();
        isBusy = false;
        isAttacking = false;
    }
    float CalculateAngle(Vector2 mouseVector)
    {
        float angleInDegrees;

        angleInDegrees = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;

        if (angleInDegrees < 0)
            angleInDegrees += 365;

        return angleInDegrees;
    }
    void SendHitPosition(float xValue, float damage, Direction direction)
    {
        manager.RecievePlayerHit(xValue, damage, direction);
    }
    void Cross()
    {
        float hitPosition = transform.position.x;
        SendHitPosition(hitPosition, crossDamage, Direction.Center);
        FinishAttack();
    }
    void RightHook()
    {;
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
    void FinishBlock()
    {
        playerInput.ActivateInput();
        isBusy = false;
        isBlocking = false;
        rightArmTarget.localPosition = rightTargetHomePosition;
        leftArmTarget.localPosition = leftTargetHomePosition;
        ResetArmRotationTarget();
    }
    void ArmsToTarget()
    {
        rightFist.position = Vector3.Lerp(rightFist.position, rightArmTarget.position, armSpeed * Time.deltaTime);
        leftFist.position = Vector3.Lerp(leftFist.position, leftArmTarget.position, armSpeed * Time.deltaTime);

        rightFist.rotation = Quaternion.Slerp(rightFist.rotation, rightRotationTarget, rotationSpeed * Time.deltaTime);
        leftFist.rotation = Quaternion.Slerp(leftFist.rotation, leftRotationTarget, rotationSpeed * Time.deltaTime);

        float rightDistance = Vector3.Distance(rightFist.position, rightArmTarget.position);
        float leftDistance = Vector3.Distance(leftFist.position, leftArmTarget.position);

        if (rightDistance < 0.2f && leftDistance < 0.2f && isBlocking)
            isBlocked = true;
        else
            isBlocked = false;
    }

    void Block()
    {
        isBlocking = true;

        Vector2 mousePosition = GetMousePosition();
        float vectorMagnitude = mousePosition.magnitude;
        float angle = CalculateAngle(mousePosition);

        DecideBlock(vectorMagnitude, angle);

        if (blockVal == 0)
        {
            FinishBlock();
        }  
    }
    void DecideBlock(float magnitude, float angle)
    {
        if (magnitude < crossBoundary)
            CenterBlock();
        else
        {
            if (angle > 45 && angle <= 135)
                UpBlock();
            else if (angle > 135 && angle <= 225)
                LeftBlock();
            else if (angle > 225 && angle <= 315)
                DownBlock();
            else if (angle > 315 || angle <= 45)
                RightBlock();
        }
    }
    void CenterBlock()
    {
        rightArmTarget.localPosition = centerRight;
        leftArmTarget.localPosition = centerLeft;

        ResetArmRotationTarget();

        blockDirection = Direction.Center;
    }
    void RightBlock()
    {
        rightArmTarget.localPosition = rightBlockRightArm;
        leftArmTarget.localPosition = leftTargetHomePosition;

        ResetArmRotationTarget();

        blockDirection = Direction.Right;
    }
    void LeftBlock()
    {
        leftArmTarget.localPosition = leftBlockLeftArm;
        rightArmTarget.localPosition = rightTargetHomePosition;

        ResetArmRotationTarget();

        blockDirection = Direction.Left;
    }
    void UpBlock()
    {
        rightArmTarget.localPosition = upBlockRightArm;
        leftArmTarget.localPosition = upBlockLeftArm;

        rightRotationTarget.eulerAngles = upRightRotationTarget;
        leftRotationTarget.eulerAngles = upLeftRotationTarget;

        blockDirection = Direction.Up;
    }
    void DownBlock()
    {
        rightArmTarget.localPosition = downBlockRightArm;
        leftArmTarget.localPosition = downBlockLeftArm;

        rightRotationTarget.eulerAngles = downRightRotationTarget;
        leftRotationTarget.eulerAngles = downLeftRotationTarget;

        blockDirection = Direction.Down;
    }
    void ResetArmRotationTarget()
    {
        rightRotationTarget = homeRotationTarget;
        leftRotationTarget = homeRotationTarget;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    Vector2 GetMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition -= screenCenter;
        return mousePosition;
    }
}

