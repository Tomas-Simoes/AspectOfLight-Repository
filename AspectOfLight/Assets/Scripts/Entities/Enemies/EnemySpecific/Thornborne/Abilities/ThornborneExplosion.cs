using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornborneExplosion : MonoBehaviour
{
    //TODO when the animation is done update the '_animationTime' to use the GetCurrentAnimatorStateInfo


    [Header("Explosion Information")]
    float _animationTime = 0;

    void Start() { }

    void Update()
    {
        _animationTime += Time.deltaTime;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
