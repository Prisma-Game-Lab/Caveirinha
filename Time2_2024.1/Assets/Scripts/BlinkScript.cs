using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration;

    private SpriteRenderer sr;
    private Material originalMaterial;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if(sr == null) 
        {
            sr = GetComponentInChildren<SpriteRenderer>();
        }
        originalMaterial = sr.material;
    }

    public IEnumerator Blink() 
    {
        sr.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        sr.material = originalMaterial;
    }
}
