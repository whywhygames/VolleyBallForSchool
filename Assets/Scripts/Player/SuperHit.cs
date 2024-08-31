using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperHit : MonoBehaviour
{
    [SerializeField] private Image _superImage;
    [SerializeField] private PlayerController _player;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _hand;
    [SerializeField] private Animator _animator;
    private Transform _target;


    public void SuperHitSetup()
    {
        Vector2 direction = _target.position - transform.position;
        _player.GameManager.CurrentBall.transform.parent = null;
        _player.GameManager.CurrentBall.GetComponent<Rigidbody2D>().AddForce(direction.normalized*60, ForceMode2D.Impulse) ;
    }

    private IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.1f);
        
    }

    public void SetBall()
    {
        _player.GameManager.CurrentBall.transform.parent = _hand;
       // _player.GameManager.CurrentBall.transform.position = _hand.position;
        _player.GameManager.CurrentBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _player.ChangeState(State.SuperHit);
        _animator.SetTrigger("SuperHit");
     //   SuperHitSetup();
    }
    public void Innit(Transform target)
    {
        _target = target;
    }
}
