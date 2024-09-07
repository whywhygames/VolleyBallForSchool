using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Components: ")]
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private TouchController _touchController;

    [Header("Prefabs:")]
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private Ball _ballPrefab;

    private Ball _currentBall;

    [Header("Teams:")]
    [SerializeField] private List<Transform> _rightTeamSpawns = new List<Transform>();
    [SerializeField] private List<Transform> _leftTeamSpawns = new List<Transform>();

    [SerializeField] private List<Transform> _rightTeamAi = new List<Transform>();
    [SerializeField] private List<Transform> _leftTeamAi = new List<Transform>();

    private List<PlayerController> _rightTeam = new List<PlayerController>();
    private List<PlayerController> _leftTeam = new List<PlayerController>();
    public List<PlayerController> RightTeam => _rightTeam;
    public List<PlayerController> LeftTeam => _leftTeam;

    [Header("Start points:")]
    [SerializeField] private Transform _startRightPoint;
    [SerializeField] private Transform _startLeftPoint;
    [SerializeField] private GameObject _net;

    //PROPERIES
    public Ball CurrentBall => _currentBall;

    public Team BallSide => _currentBall.CurrentSide;
    public ScoreController ScoreController => _scoreController;
    public TouchController TouchController => _touchController;

    private void Awake()
    {
        InitBall();
        CraeatePlayers();
        InitRightTeam();
        InitLeftTeam();
        SetServer();
        InitComand();

        _rightTeam[0].AiInit(_rightTeamAi[0], _rightTeamAi[1], _rightTeam[1]);
        _leftTeam[0].AiInit(_leftTeamAi[0], _leftTeamAi[1], _leftTeam[1]);
        _rightTeam[1].AiInit(_rightTeamAi[0], _rightTeamAi[1], _rightTeam[0]);
        _leftTeam[1].AiInit(_leftTeamAi[0], _leftTeamAi[1], _leftTeam[0]);

        _rightTeam[0].Touched += SwapController;
        _rightTeam[1].Touched += SwapController;
        _leftTeam[0].Touched += SwapController;
        _leftTeam[1].Touched += SwapController;

        _rightTeam[0].ChangeControl(true);
        _rightTeam[1].ChangeControl(false);
        _leftTeam[0].ChangeControl(true);
        _leftTeam[1].ChangeControl(false);
    }

    public void SetSide(Team isSide)
    {
        if (isSide == Team.Right)
        {
            SetupSide(_rightTeam);
        }
        else
        {
            SetupSide(_leftTeam);
        }
    }

    private void SetupSide(List<PlayerController> controllers)
    {
        foreach (var item in controllers)
        {
            if (item.CurrentState != State.Serving)
                item.ChangeState(State.Recieving);
        }

        controllers[0].ChangeControl(true);
        controllers[1].ChangeControl(false);
    }

    private void SwapController(PlayerController currentControl)
    {
        if (currentControl.gameObject == _leftTeam[0].gameObject)
        {
            _leftTeam[0].ChangeControl(false);
            _leftTeam[1].ChangeControl(true);
        }
        else if (currentControl.gameObject == _leftTeam[1].gameObject && _leftTeam[0].NumTouches > 0)
        {
            _leftTeam[0].ChangeControl(true);
            _leftTeam[1].ChangeControl(false);
        }

        if (currentControl.gameObject == _rightTeam[0].gameObject)
        {
            _rightTeam[0].ChangeControl(false);
            _rightTeam[1].ChangeControl(true);
        }
        else if (currentControl.gameObject == _rightTeam[1].gameObject && _rightTeam[0].NumTouches > 0)
        {
            _rightTeam[0].ChangeControl(true);
            _rightTeam[1].ChangeControl(false);
        }
    }

    private void InitComand()
    {
        _rightTeam[0].SetCommand(Team.Right, this);
        _rightTeam[1].SetCommand(Team.Right, this);
        _leftTeam[0].SetCommand(Team.Left, this);
        _leftTeam[1].SetCommand(Team.Left, this);
    }

    private void SetServer()
    {
        if (Random.Range(0, 2) == 0)
        {
            _rightTeam[0].Serve.SetUp();
            //_rightTeam[1].ChangeControl();
        }
        else
        {
            _leftTeam[0].Serve.SetUp();
            //_leftTeam[1].ChangeControl();
        }
    }

    private void InitLeftTeam()
    {
        foreach (var item in _leftTeam)
        {
            item.Serve.Initialized(_currentBall.GetComponent<Rigidbody2D>(), _startLeftPoint, KeyCode.D, true);
            item.ButtonInnit(KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A);
            item.Setting.ButtonSet(KeyCode.D, KeyCode.A);
            item.SuperHit.Innit(_startRightPoint);
        }
    }

    private void InitRightTeam()
    {
        foreach (var item in _rightTeam)
        {
            item.Serve.Initialized(_currentBall.GetComponent<Rigidbody2D>(), _startRightPoint, KeyCode.LeftArrow, false);
            item.ButtonInnit(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow);
            item.Setting.ButtonSet(KeyCode.RightArrow, KeyCode.LeftArrow);
            item.SuperHit.Innit(_startLeftPoint);
        }
    }

    private void CraeatePlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            var leftPlayer = Instantiate(_playerPrefab, _leftTeamSpawns[i].position, Quaternion.identity);
            var rightPlayer = Instantiate(_playerPrefab, _rightTeamSpawns[i].position, Quaternion.identity);
            _leftTeam.Add(leftPlayer);
            _rightTeam.Add(rightPlayer);
        }
    }

    private void InitBall()
    {
        _currentBall = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        _currentBall.Init(this, _touchController);
    }
}
