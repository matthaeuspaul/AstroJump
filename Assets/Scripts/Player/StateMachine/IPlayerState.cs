using UnityEngine;

public interface IPlayerState
{
    // Methods to be implemented by each state
    void Enter();
    void Exit();
    void Update();
    void FixedUpdate();

}
