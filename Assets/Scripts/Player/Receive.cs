using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receive : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent(out Ball ball))
        {
     //       Debug.Log("touch");
        //    Debug.Log(_player.NumTouches);
            if(_player.CurrentState == State.Recieving && _player.GameManager.TouchController.GetTeamTouch(_player.Team) == 0)
            {
           //     Debug.Log("receive");
                ball.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                ball.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            }
          
            _player.AddTouch();
        }
    }
}
