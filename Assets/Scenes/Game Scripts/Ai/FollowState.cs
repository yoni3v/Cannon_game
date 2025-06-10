using UnityEngine;
using UnityEngine.AI;

public class FollowState : Artificial_Behaviour
{
    public Transform player_Transform;
    public bool PlayerToClose = false;
    
    public FollowState(Animator animator, NavMeshAgent agent) : base(animator, agent)
    {
        //The following data will be provided when the class is created for the first time
    }

    public override void OnStateEnter()
    {
        if (player_Transform == null)
        {
            player_Transform = GameObject.FindWithTag("Player").transform;
        }

        state_agent.SetDestination(player_Transform.position);
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        Vector3 player_position = player_Transform.position;
        Vector3 my_position = state_agent.transform.position;
        Vector3 Direction = player_position - my_position;
        float Distance = Direction.magnitude;

        if (Distance <= state_agent.stoppingDistance)
        {
            //We are close to target now we can attack
            //Switch to attack state
            PlayerToClose = true;
        }
        else { PlayerToClose = false; }
    }
}