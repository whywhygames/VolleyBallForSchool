using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Setting,
    Hitting,
    Recieving,
    Diving,
    Serving,
    SuperHit
}

public class States : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private State _model;
}
