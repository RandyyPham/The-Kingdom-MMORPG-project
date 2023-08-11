using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private Vector2 _movementVector;
    private Character _character;
    private bool _canMove = true;

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

    private void Update()
    {
        if (!IsLocalPlayer) return;
        // using the movement vector to determine the animation of the player
        Animate();
    }

    private void FixedUpdate()
    {
        if (!IsLocalPlayer) return;
        // calculating speed of player here
        Vector2 playerVelocity = _canMove ? _movementVector * _character.GetSpeed() : Vector2.zero;
        MovePlayerServerRpc(playerVelocity);
    }

    [ServerRpc]
    private void MovePlayerServerRpc(Vector2 velocity)
    {
        // reset the force acted on the body
        _rigidbody.totalForce = Vector2.zero;
        _rigidbody.velocity = velocity;
    }

    public void OnMovement(InputValue value)
    {
        if (!IsLocalPlayer) return;
        _movementVector = value.Get<Vector2>();
    }

    public void OnAttack(InputValue _)
    {
        if (!IsLocalPlayer) return;
        _animator.SetTrigger("PlayerAttack");
    }

    public void LockMovement()
    {
        _canMove = false;
    }

    public void UnlockMovement()
    {
        _canMove = true;
    }

    public void OnDeath()
    {
        if (!IsLocalPlayer) return;
        OnDeathServerRpc();
    }

    [ServerRpc]
    private void OnDeathServerRpc()
    {
        _rigidbody.simulated = false;
    }

    /// <summary>
    /// This function is responsible for setting the parameters of the animations.
    /// </summary>
    private void Animate()
    {
        bool isMoving = _movementVector.sqrMagnitude > 0;

        _animator.SetFloat("Horizontal", _movementVector.x);
        _animator.SetFloat("Vertical", _movementVector.y);
        _animator.SetBool("IsMoving", isMoving);

        // remember our last "direction" so we can control where the player is facing
        if (isMoving && _canMove)
        {
            _animator.SetFloat("LastHorizontal", _movementVector.x);
            _animator.SetFloat("LastVertical", _movementVector.y);
        }
    }
}
