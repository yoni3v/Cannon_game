using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, iDamageable, iHealable
{
    public float MaxHealth = 200;
    public float CurrentHealth;
    public float HealRate = 1;
    public float DamageMultiplier = 1;

    public bool IsAlive = true;

    //events
    public UnityEvent OnDamageEvent;
    public UnityEvent OnDead;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            CurrentHealth = MaxHealth;
        }
#endif
    }

    public void OnDamage(float damage)
    {
        if (!IsAlive)
        {
            return;
        }

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDead?.Invoke();
            IsAlive = false;
        }
        else
        {
            CurrentHealth -= damage * DamageMultiplier;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDead?.Invoke();
                IsAlive = false;
                return;
            }

            OnDamageEvent?.Invoke();
        }
    }

    public void OnHeal(float heal)
    {
        CurrentHealth += heal;
        IsAlive = true;

        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
}

public interface iDamageable
{
    public void OnDamage(float damage);
}

public interface iHealable
{
    public void OnHeal(float heal);
}