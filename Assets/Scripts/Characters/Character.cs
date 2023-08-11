using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float _health, _damage, _speed;

    public float GetHealth()
    {
        return _health;
    }

    public void SetHealth(float health)
    {
        _health = health;
    }

    public float GetDamage()
    {
        return _damage;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
