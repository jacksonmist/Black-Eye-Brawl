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
    public UIController ui;
    public GameObject playerObj;
    public GameObject opponentObj;

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

    public void StartGame()
    {
        playerObj.SetActive(true);
        opponentObj.SetActive(true);
    }
    void EndGame()
    {
        player.UnlockCursor();
        playerObj.SetActive(false);
        opponentObj.SetActive(false);
    }
    public void PlayerLoss()
    {
        EndGame();
        ui.Loss();
    }
    public void OpponentLoss()
    {
        EndGame();
        ui.Win();
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
