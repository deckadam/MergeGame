using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControllerScript : MonoBehaviour
{

    private ParticleSystem pSystem;
    void Start()
    {
        pSystem = gameObject.GetComponent<ParticleSystem>();
    }

    //Set position to merging position and play the animation
    public void playMergingParticlesAtPosition(Vector3 pos) {
        gameObject.transform.position = pos;
        pSystem.Play();
    }

}
