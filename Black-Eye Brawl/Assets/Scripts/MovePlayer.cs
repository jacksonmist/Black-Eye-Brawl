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

    public float playerHealth = 100;

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

    public Vector3 upBlockRightArm = new Vector3(0.4f, 0.65f, 5.8f);
    public Vector3 upBlockLeftArm = new Vector3(-0.4f, 0.6f, 7.4f);

    public Vector3 downBlockRightArm = new Vector3(0.4f, 0.39f, 5.8f);
    public Vector3 downBlockLeftArm = new Vector3(-0.4f, 0.29f, 7.4f);
    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        attackAction = playerInput.actions.FindAction("Attack");
        blockAction = playerInput.actions.FindAction("Block");

        startingY = transform.position.y;
        startingZ = transform.position.z;
      
        rightTargetHomePosition = rightFist.localPosition;
        leftTargetHomePosition = leftFist.localPosition;
        rightArmTarget.localPosition = rightTargetHomePosition;
        leftArmTarget.localPosition = leftTargetHomePosition;

        print(rightTargetHomePosition);

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
    }
    void ArmsToTarget()
    {
        rightFist.localPosition = Vector3.Lerp(rightFist.localPosition, rightArmTarget.localPosition, armSpeed * Time.deltaTime);
        leftFist.localPosition = Vector3.Lerp(leftFist.localPosition, leftArmTarget.localPosition, armSpeed * Time.deltaTime);

        float rightDistance = Vector3.Distance(rightFist.localPosition, rightArmTarget.localPosition);
        float leftDistance = Vector3.Distance(leftFist.localPosition, leftArmTarget.localPosition);

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
        blockDirection = Direction.Center;
    }
    void RightBlock()
    {
        rightArmTarget.localPosition = rightBlockRightArm;
        leftArmTarget.localPosition = leftTargetHomePosition;
        blockDirection = Direction.Right;
    }
    void LeftBlock()
    {
        leftArmTarget.localPosition = leftBlockLeftArm;
        rightArmTarget.localPosition = rightTargetHomePosition;
        blockDirection = Direction.Left;
    }
    void UpBlock()
    {
        rightArmTarget.localPosition = upBlockRightArm;
        leftArmTarget.localPosition = upBlockLeftArm;
        blockDirection = Direction.Up;
    }
    void DownBlock()
    {
        rightArmTarget.localPosition = downBlockRightArm;
        leftArmTarget.localPosition = downBlockLeftArm;
        blockDirection = Direction.Down;
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

