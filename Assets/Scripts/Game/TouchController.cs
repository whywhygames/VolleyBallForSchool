using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [Header("Components:")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ScoreController _scoreController;

    [Header("UI:")]
    [SerializeField] private TouchPanel _touchPanel;

    public int TeamTouchRight => _gameManager.RightTeam[0].NumTouches + _gameManager.RightTeam[1].NumTouches;
    public int TeamTouchLeft => _gameManager.LeftTeam[0].NumTouches + _gameManager.LeftTeam[1].NumTouches;
    public Team BallSide => _gameManager.BallSide;

    private void Start()
    {
        _gameManager.RightTeam[0].Touched += CheckTeamTouch;
        _gameManager.RightTeam[1].Touched += CheckTeamTouch;
        _gameManager.LeftTeam[0].Touched += CheckTeamTouch;
        _gameManager.LeftTeam[1].Touched += CheckTeamTouch;
    }

    public int GetTeamTouch(Team isRight)
    {
        if (isRight == Team.Right)
            return TeamTouchRight;
        else
            return TeamTouchLeft;
    }

    public void ResetTouches(Team isSide)
    {
        if (isSide == Team.Right)
        {
            _touchPanel.ResetLights();
            ResetTouches(_gameManager.RightTeam);
        }
        else
        {
            _touchPanel.ResetLights();
            ResetTouches(_gameManager.LeftTeam);
        }
    }

    public void CheckTeamTouch(PlayerController currentControl)
    {
        Debug.Log(TeamTouchLeft);
        _touchPanel.AddLight();
        if (_gameManager.BallSide == Team.Right)
        {
            if (TeamTouchRight > 3)
            {
                Time.timeScale = 0;
                _scoreController.AddPoints(Team.Left);
            }
            if (TeamTouchRight >= 1)
            {
                _gameManager.RightTeam[0].ChangeState(State.Setting);
                _gameManager.RightTeam[1].ChangeState(State.Setting);
            }
        }
 
        else if (_gameManager.BallSide == Team.Left)
        {
            if (TeamTouchLeft > 3)
            {
                Time.timeScale = 0;
                _scoreController.AddPoints(Team.Right);
            }
            if (TeamTouchLeft >= 1)
            {
                _gameManager.LeftTeam[0].ChangeState(State.Setting);
                _gameManager.LeftTeam[1].ChangeState(State.Setting);
            }
        }
    }

    private void ResetTouches(List<PlayerController> controllers)
    {
        foreach (var player in controllers)
            player.ResetTouches();
    }
}
