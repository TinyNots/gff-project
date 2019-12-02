using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState <T>
{
    void Enter(T entity);
    void Execute(T entity);
    void Exit(T entity);
}

public class StateMachine <T>
{
    private T owner;
    IState<T> currentState;
    public StateMachine()
    {
        currentState = null;
    }

    public IState<T> GetCurrentState
    {
        get { return currentState; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (currentState != null) currentState.Execute(owner);
    }

    public void ChangeState(IState<T> newState)
    {
        if (currentState != null)
        {
            currentState.Exit(owner);
        }
        currentState = newState;
        currentState.Enter(owner);
    }

    public void Setup(T newOwner,IState<T> newState)
    {
        owner = newOwner;
        currentState = newState;
        currentState.Enter(owner);
    }
}
