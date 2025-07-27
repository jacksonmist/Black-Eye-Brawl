using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    Center
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
        return true;
    }
    public void RecievePlayerHit(float xValue, float damage, Direction direction)
    {
        if(IsOpponentHit(xValue))
        {
            opponent.TakeDamage(damage, direction);
        }
    }
    void UpdateOpponentInfo()
    {
        opponent.UpdatePlayerPosition(player.transform.position.x);
    }
}
