using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class SlimeController : NetworkBehaviour
{
    public GameObject prefab;
    private Animator _animator;
    private Character _character;
    private DetectionZone _detectionZone;
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _canMove = true;
    private float _damage;
    private readonly int _knockbackForce = 500;

    private void Initialize()
    {
        _animator = GetComponent<Animator>();
        _character = gameObject.GetComponent<Character>();
        _detectionZone = gameObject.GetComponentInChildren<DetectionZone>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _damage = _character.GetDamage();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void FixedUpdate()
    {
        if (_detectionZone.DetectedObjects.Any() && _canMove)
        {
            // calculates direction to the first target player
            _direction = (_detectionZone.DetectedObjects[0].transform.position - gameObject.transform.position).normalized;

            // move towards the first target player
            _rigidbody.velocity = _character.GetSpeed() * _direction;
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!NetworkManager.Singleton.IsServer) return;

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        Vector3 parentPosition = GetComponentInParent<Transform>().position;
        Vector2 direction = (Vector2)(collision.gameObject.transform.position - parentPosition).normalized;
        damageable.OnHit(_damage, direction * _knockbackForce);
    }

    public void LockMovement()
    {
        _canMove = false;
    }

    public void UnlockMovement()
    {
        _canMove = true;
    }

    public void OnSlimeDeath()
    {
        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        if (NetworkObject.IsSpawned) NetworkObject.Despawn();
    }
}
