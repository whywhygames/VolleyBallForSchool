using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager _gameManager;
    private TouchController _touchController;

    public Team CurrentSide { get; private set; }

    public void Init(GameManager gameManager, TouchController touchController)
    {
        _gameManager = gameManager;
        _touchController = touchController;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out RightSide rightSide))
        {
            _gameManager.SetSide(Team.Right);
            _touchController.ResetTouches(Team.Right);
            CurrentSide = Team.Right;
        }
        else if(collision.TryGetComponent(out LeftSide leftSide))
        {
            _gameManager.SetSide(Team.Left);
            _touchController.ResetTouches(Team.Left);
            CurrentSide = Team.Left;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Floor floor))
        {
            if (CurrentSide == Team.Right)
                _gameManager.ScoreController.AddPoints(Team.Left);
            else
                _gameManager.ScoreController.AddPoints(Team.Right);
        }
    }
}
