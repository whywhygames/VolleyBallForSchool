using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _diveSpeed;
    [SerializeField] private Transform _legs;
    [SerializeField] private float _radiusLegs;
    [SerializeField] private LayerMask _maskGround;
    [SerializeField] private Sprite _dive;
    [SerializeField] private AnimationCurve _diveSpeedCurve;
    [SerializeField] private State _currentState;
    [SerializeField] private Sprite _idle;
    [SerializeField] private Color _color;
    [SerializeField] private Vector2 _lowScale;
    [SerializeField] private Vector2 _defaultScale;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private GameManager _gameManager;

    [Header("Super Hit")]
    [SerializeField] private float _radius;
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private LayerMask _ballMask;
    [SerializeField] private SuperHit _superHit;

    [field: SerializeField] public Serve Serve { get; private set; }
    [field: SerializeField] public Setting Setting { get; private set; }

    private bool _isTimeAllowed = true;
    private float _firstRightTime;
    private float _firstLeftTime;
    private float _timeBetween = 0.5f;
    private int _diveClicksRight;
    private int _diveClickLeft;
    private float _elapsedTime;
    private bool _firstClickR = false;
    private bool _firstClickL = false;
    private bool _doubleR = false;
    private bool _doubleL = false;
    private int _speed = 5;
    private bool _jumping;
    private int _numTouches;
    private bool _controlled = false;
    public event UnityAction<PlayerController> Touched;
    private KeyCode _leftButton;
    private KeyCode _rightButton;
    private KeyCode _upButton;
    private KeyCode _downButton;
    private Team _team;

    public int NumTouches => _numTouches;
    public State CurrentState => _currentState;
    public bool Controlled => _controlled;

    public GameManager GameManager { get => _gameManager; private set => _gameManager = value; }
    public SuperHit SuperHit { get => _superHit; set => _superHit = value; }
    public Team Team { get => _team; set => _team = value; }

    private void Update()
    {
        MoveController();
        CheckBallHitSphere();
    }
    private void FixedUpdate()
    {
        OnGround(!Physics2D.OverlapCircle(_legs.position, _radiusLegs, _maskGround));
    }
    private void OnGround(bool onGround)
    {
        _jumping = onGround;
    }

    public void ChangeControl(bool controlled)
    {
        _controlled = controlled;
        IsControl(_controlled);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(_checkPoint.position, _radius);
    }

    private bool testVar;
    public void CheckBallHitSphere()
    {
        testVar = Physics2D.OverlapCircle(_checkPoint.position, _radius, _ballMask);

        if (Physics2D.OverlapCircle(_checkPoint.position, _radius, _ballMask) && Input.GetKeyDown(KeyCode.M) && _controlled)
        {
            SuperHit.SetBall();
        }
    }

    public void IsControl(bool control)
    {
        if(control == false)
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            _spriteRenderer.color = _color;
            //_spriteRenderer.transform.localScale = _lowScale;
            _collider.enabled = false;
            // transform.position = new Vector2(transform.position.x, -3);
            StartCoroutine(MoveDown());
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }
        if(control == true)
        {
            _spriteRenderer.color = Color.white;
            //_spriteRenderer.transform.localScale = _defaultScale;
            _collider.enabled = true;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private IEnumerator MoveDown()
    {
        float time = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -3), elapsedTime / time);
            yield return null;
        }
    }

    public void AddTouch()
    {
        _numTouches++;
        Touched?.Invoke(this);
    }

    public void ChangeState(State state)
    {
        if (state == State.Setting)
        {
            _currentState = State.Setting;
        }
        else if (state == State.Hitting)
        {
            _currentState = State.Setting;
        }
        else if(state == State.Recieving)
        {
            _currentState = State.Recieving;
        }
        else if(state == State.Diving)
        {
            _currentState = State.Diving;
            _spriteRenderer.sprite = _dive;
        }
        else if(state == State.Serving)
        {
            _currentState = State.Serving;
        }
        else
        {
            _currentState = State.Idle;
            _spriteRenderer.sprite = _idle;
            StopMove();
        }
    }

    public void ResetTouches()
    {
        _numTouches = 0;
    }

    private void Move(KeyCode button1, int direction, bool flipx)
    {
        if(_currentState != State.Hitting && _currentState != State.Diving && _currentState != State.Serving && _currentState != State.SuperHit)
        {
            if (Input.GetKey(button1))
            {
                _rigidbody.velocity = new Vector2(direction * _speed, _rigidbody.velocity.y);
                _spriteRenderer.flipX = flipx;
                _elapsedTime = 0;
            }
        }
    }
    private void StopMove()
    {
        if (Input.GetKeyUp(_leftButton) || Input.GetKeyUp(_rightButton))
        {
            _rigidbody.velocity = new Vector2(0, 0);
            _elapsedTime = 0;
        }
    }
    private void Jump()
    {
        if(_currentState != State.Recieving && _currentState != State.Diving)
        {
            if (Input.GetKey(_upButton))
            {
                if (_jumping == false)
                {
                    _rigidbody.velocity = Vector2.zero;
                    _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
                }

            }
        }
    }

    private void DiveCheck()
    {
        if (Input.GetKeyDown(_rightButton) && _firstClickR == false)
        {
            _doubleR = false;
            _firstClickR = true;
            _diveClicksRight += 1;
            _firstRightTime = Time.time;
            Debug.Log(22);
        }
        else if (Input.GetKeyDown(_rightButton) && _firstClickR == true)
        {
            if (Time.time - _firstRightTime <= _timeBetween)
            {
                _doubleR = true;
                _diveClicksRight += 1;
            }
            _firstClickR = false;
        }
        if (Input.GetKeyDown(_leftButton) && _firstClickL == false)
        {
            _doubleL = false;
            _firstClickL = true;
            _diveClickLeft += 1;
            _firstLeftTime = Time.time;
        }
        else if (Input.GetKeyDown(_leftButton) && _firstClickL == true)
        {
            if (Time.time - _firstLeftTime <= _timeBetween)
            {
                _doubleL = true;
                _diveClickLeft += 1;
            }
            _firstClickL = false;
        }
        /*if (_diveClicksRight == 1 && _isTimeAllowed)
        {
            StartCoroutine(detectDoubleRight());
        }*/
    }

    public void ButtonInnit(KeyCode upButton, KeyCode downButton, KeyCode rightButton, KeyCode leftButton)
    {
        _upButton = upButton;
        _downButton = downButton;
        _rightButton = rightButton;
        _leftButton = leftButton;
    }

    public void SetCommand(Team team, GameManager gameManager)
    {
        _team = team;
        _gameManager = gameManager;
    }

    private void MoveController()
    {
        if(Time.timeScale > 0 && _controlled)
        {
            Move(_leftButton, -1, false);
            Move(_rightButton, 1, true);
            StopMove();
            DiveCheck();
            Jump();
            Dive();
        }
    }
    
    private void Dive()
    {
        if (_doubleR && !_jumping && _currentState != State.Hitting && _currentState != State.Setting && _currentState != State.Serving)
        {
            _rigidbody.velocity = Vector2.zero;
            //_rigidbody.AddForce(Vector2.right * _diveSpeed, ForceMode2D.Impulse);
            StartCoroutine(ControlDive(true));
            _spriteRenderer.sprite = _dive;
            StartCoroutine(EndDive());
            _doubleR = false;
        }
        else if (_doubleL && !_jumping && _currentState != State.Hitting && _currentState != State.Setting && _currentState != State.Serving)
        {
            _rigidbody.velocity = Vector2.zero;
            //_rigidbody.AddForce(Vector2.right * _diveSpeed, ForceMode2D.Impulse);
            StartCoroutine(ControlDive(false));
            _spriteRenderer.sprite = _dive;
            StartCoroutine(EndDive());
            _doubleL = false;
        }
        //Debug.Log("L: " + _doubleL);
        //Debug.Log("R: "+ _doubleR);
    }

    private IEnumerator ControlDive(bool isRight)
    {
        float delay = 0.5f;
        float elapsedTime = 0;
        
        while (elapsedTime < delay)
        {
            float multiplay = _diveSpeedCurve.Evaluate(elapsedTime / delay);
            if (isRight)
            {
                _rigidbody.velocity = Vector2.right * _diveSpeed * multiplay;
            }
            else
            {
                _rigidbody.velocity = Vector2.left * _diveSpeed * multiplay;
            }   
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _rigidbody.velocity = Vector2.zero;
    }
    IEnumerator detectDoubleRight()
    {
        _isTimeAllowed = false;
        while(Time.time < _firstRightTime + _timeBetween)
        {
            if(_diveClicksRight == 2)
            {
                _doubleR = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        _diveClicksRight = 0;
        _isTimeAllowed = true;
        
    }

    private IEnumerator EndDive()
    {
        ChangeState(State.Diving);
        yield return new WaitForSeconds(1);
        ChangeState(State.Idle);
    }
}