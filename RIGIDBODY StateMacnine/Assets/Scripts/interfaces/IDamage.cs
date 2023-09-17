using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void tookLighthit();
    void tookHeavyhit(Vector3 thing);
    void TookKnockBack();
}
