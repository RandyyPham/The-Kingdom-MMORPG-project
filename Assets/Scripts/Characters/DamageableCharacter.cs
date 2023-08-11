using Unity.Netcode;
using UnityEngine;

public class DamageableCharacter : NetworkBehaviour, IDamageable
{
    public float Health
    {
        get
        {
            return _character.GetHealth();
        }
        set
        {
            _character.SetHealth(value);

            if (_character.GetHealth() <= 0)
            {
                _animator.SetBool("IsAlive", false);
            }
        }
    }

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Character _character;

    private void Initialize()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    public void OnHit(float damage, Vector2 knockbackForce)
    {
        _animator.SetTrigger("HitTrigger");
        Debug.Log("Hit for " + damage);
        Health -= damage;
        Debug.Log("Health decreased by " + damage + ". Now health is " + Health);
        _rigidbody.AddForce(knockbackForce);
        // OnHitServerRpc(damage, knockbackForce);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnHitServerRpc(float damage, Vector2 knockbackForce)
    {
        Health -= damage;
        Debug.Log("Health decreased by " + damage + ". Now health is " + Health);
        _rigidbody.AddForce(knockbackForce);
    }

    public void OnHit(float damage)
    {
        _animator.SetTrigger("HitTrigger");
        Health -= damage;
    }
}
