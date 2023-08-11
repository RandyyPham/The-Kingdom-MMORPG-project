using UnityEngine;

public interface IDamageable
{
    public float Health { get; set; }

    public void OnHit(float damage);
    public void OnHit(float damage, Vector2 knockbackForce);
}
