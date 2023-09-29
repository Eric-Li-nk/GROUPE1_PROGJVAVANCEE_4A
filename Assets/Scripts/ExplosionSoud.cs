using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSoud : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;

    private void Update()
    {
        if(!AudioSource.isPlaying)
            Destroy(gameObject);
    }
}
