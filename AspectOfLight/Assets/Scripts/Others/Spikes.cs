using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator Go(string upOrDown)
    {
        anim.SetBool("spikes" + upOrDown, true);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("spikes" + upOrDown, false);

        while (true)
        {
            float _animationTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (_animationTime >= 1)
            {
                if (upOrDown == "Down")
                {
                    Destroy(this.gameObject);
                }

                yield return null;
            }

            yield return new WaitForSeconds(0.001f);
        }
    }
}
