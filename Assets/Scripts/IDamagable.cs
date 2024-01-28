using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage,GameObject other, Vector3 velocity);
}