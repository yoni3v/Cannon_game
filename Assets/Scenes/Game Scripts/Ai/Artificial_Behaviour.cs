using UnityEngine;
using UnityEngine.AI;

public abstract class Artificial_Behaviour
{
    protected Animator state_animator;
    protected NavMeshAgent state_agent;

    //Constructor
    public Artificial_Behaviour(Animator animator, NavMeshAgent agent)
    {
        state_animator = animator;
        state_agent = agent;
    }

    //When state initializes
    public abstract void OnStateEnter();

    //When state updates
    public abstract void OnStateUpdate();

    //When state leaves
    public abstract void OnStateExit();
}