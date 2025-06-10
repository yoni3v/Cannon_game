using UnityEngine;
using UnityEngine.AI;

public class AttackState : Artificial_Behaviour
{
    iDamageable player_object;
    float DamagePerSecond = 20;
    float Time_Elapsed = 1;

    public AttackState(Animator animator, NavMeshAgent agent, float damagePerSecond) : base(animator, agent)
    {
        DamagePerSecond = damagePerSecond;
    }

    public override void OnStateEnter()
    {
        if (player_object == null)
        {
            player_object = GameObject.FindWithTag("Player").GetComponent<iDamageable>();
        }

        Time_Elapsed = 1;
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        if (Time_Elapsed > 0)
        {
            Time_Elapsed -= Time.deltaTime;

            if (Time_Elapsed <= 0)
            {
                player_object.OnDamage(DamagePerSecond);
                Time_Elapsed = 1;
                state_animator.SetTrigger("Attack");
            }
        }
    }
}