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

    public IEnumerator GoUp()
    {
        anim.SetBool("spikesUp", true);

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        anim.SetBool("spikesUp", false);
    }

    public IEnumerator GoDown()
    {
        anim.SetBool("spikesDown", true);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        anim.SetBool("spikesDown", false);
        gameObject.SetActive(false);
    }
}
