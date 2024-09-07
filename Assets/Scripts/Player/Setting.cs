using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    private Ball _ball;
    private KeyCode _leftButton;
    private KeyCode _rightButton;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Ball ball) && _player.CurrentState == State.Setting)
        {
         //   Debug.Log("touch");
         //   Debug.Log(_player.NumTouches);
            if (_player.CurrentState == State.Setting && _player.GameManager.TouchController.GetTeamTouch(_player.Team) >= 1)
            {
                _ball = ball;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Ball ball))
        {
            _ball = null;
        }
    }

    private void Update()
    {
        if(_player.CurrentState == State.Setting)
            Set();
    }

    public void ButtonSet(KeyCode rightButton, KeyCode leftButton)
    {
        _rightButton = rightButton;
        _leftButton = leftButton;
    }

    public void Set()
    {
        if (_ball != null && Input.GetKey(_rightButton))
        {
            _ball.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _ball.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 4), ForceMode2D.Impulse);
            _ball = null;
          //  Debug.Log("set");
        }
        else if (_ball != null && Input.GetKey(_leftButton))
        {
            _ball.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _ball.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10, 4), ForceMode2D.Impulse);
            _ball = null;
        }
        else if (_ball != null)
        {
            _ball.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            _ball.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            _ball = null;
        }
    }
}
