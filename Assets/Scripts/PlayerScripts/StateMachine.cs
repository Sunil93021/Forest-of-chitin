using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum PlayerState { Idle, Running, Jumping, Attack }

    private PlayerState _currentState = PlayerState.Idle;
    public event Action<PlayerState> OnPlayerStateChanged;

    private void Start()
    {
        OnPlayerStateChanged?.Invoke(_currentState);
    }

    public void ChangeState(PlayerState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        OnPlayerStateChanged?.Invoke(_currentState);
    }

    public PlayerState GetCurrentPlayerState() => _currentState;
}
