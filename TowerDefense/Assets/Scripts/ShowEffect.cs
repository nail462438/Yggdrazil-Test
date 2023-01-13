using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEffect : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyEffect());
    }

    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
