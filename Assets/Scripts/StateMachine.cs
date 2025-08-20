using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        Attack,
    }

    private PlayerState _currentPlayerState = PlayerState.Idle;

    public event Action<PlayerState> OnPlayerStateChanged;

    private void Start()
    {
        OnPlayerStateChanged?.Invoke(_currentPlayerState);
    }

    public void ChangeState(PlayerState newState)
    {
        if (newState == _currentPlayerState)
            return; 

        _currentPlayerState = newState;
        OnPlayerStateChanged?.Invoke(_currentPlayerState);
    }

    public PlayerState GetCurrentPlayerState()
    {
        return _currentPlayerState;
    }
}
