using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    Center,
    None
};
public class GameManager : MonoBehaviour
{
    
    public MovePlayer player;
    public MoveOpponent opponent;

    public float opponentBoundWidth = 1f;
    public float opponentRightBound;
    public float opponentLeftBound;

    public float playerBoundWidth = 1f;
    public float playerRightBound;
    public float playerLeftBound;

    void Start()
    {
        
    }

    void Update()
    {
        CalculateBoundingBoxes();
        UpdateOpponentInfo();
    }
    void CalculateBoundingBoxes()
    {
        playerRightBound = player.transform.position.x + playerBoundWidth;
        playerLeftBound = player.transform.position.x - playerBoundWidth;

        opponentRightBound = opponent.transform.position.x + opponentBoundWidth;
        opponentLeftBound = opponent.transform.position.x - opponentBoundWidth;
    }
    bool IsOpponentHit(float xValue)
    {
        if (xValue < opponentRightBound && xValue > opponentLeftBound)
            return true;
        else
            return false;
    }
    bool IsPlayerHit(float xValue)
    {
        if (xValue < playerRightBound && xValue > playerLeftBound)
            return true;
        else
            return false;
    }
    public void RecievePlayerHit(float xValue, float damage, Direction direction)
    {
        if (IsOpponentHit(xValue))
        {
            opponent.TakeDamage(damage, direction);
        }
        else
            opponent.FinishBlock();
    }
    public void RecieveOpponentHit(float xValue, float damage, Direction direction)
    {
        print(xValue);
        print(playerRightBound);
        print(playerLeftBound);
        if (IsPlayerHit(xValue))
        {
            player.TakeDamage(damage, direction);
        }
    }
    void UpdateOpponentInfo()
    {
        opponent.UpdatePlayerPosition(player.transform.position.x);
    }

    public void RecievePlayerDirection(Direction direction)
    {
        opponent.RecieveAttackDirection(direction);
    }
}
