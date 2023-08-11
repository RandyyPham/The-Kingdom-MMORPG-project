using Unity.Netcode;
using UnityEngine;

public class SwordController : NetworkBehaviour
{
    private float _swordDamage;
    private readonly int _knockbackForce = 500; // just some arbitrary number

    private void Initialize()
    {
        _swordDamage = GetComponentInParent<Character>().GetDamage();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out IDamageable damageable))
        {
            Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2)(collider.gameObject.transform.position - parentPosition).normalized;
            damageable.OnHit(_swordDamage, direction * _knockbackForce);
        }
    }
}
