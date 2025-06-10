using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthSystem))]
public class AI : MonoBehaviour
{
    [Header("Settings")]
    public float DamagePerSecond;

    FollowState FollowStatePTR;
    AttackState AttackStatePTR;
    HealthSystem health_system;

    Artificial_Behaviour CurrentState;

    private void Awake()
    {
        //Get component References
        NavMeshAgent my_Agent = GetComponent<NavMeshAgent>();
        Animator my_Animator = GetComponent<Animator>();
        health_system = GetComponent<HealthSystem>();

        //Initialize the states
        FollowStatePTR = new FollowState(my_Animator, my_Agent);
        AttackStatePTR = new AttackState(my_Animator, my_Agent, DamagePerSecond);
        health_system.OnDead.AddListener(AiDead);

        //Call the state start log
        ChangeState(FollowStatePTR);
    }

    private void Update()
    {
        CurrentState.OnStateUpdate();

        if (FollowStatePTR.PlayerToClose && CurrentState != AttackStatePTR)
        {
            ChangeState(AttackStatePTR);
            Debug.Log("Changed the state to attacking");
        }
    }

    public void ChangeState(Artificial_Behaviour New_State)
    {
        if (CurrentState == null)
        {
            CurrentState = New_State;
        }
        else
        {
            CurrentState.OnStateExit();
            CurrentState = New_State;
        }

        CurrentState.OnStateEnter();
    }

    private void AiDead()
    {
        Destroy(gameObject, 0.1f);
    }
}