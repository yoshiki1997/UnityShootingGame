using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
    void TakeHit(float damege, Vector3 hitPoint, Vector3 hitDirection);

    void TakeDamage(float damege);
}
