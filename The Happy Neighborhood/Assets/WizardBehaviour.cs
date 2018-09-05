using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBehaviour : MonoBehaviour {

    AudioSource audioSource;
    Animator animator;
	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void WizardClicked()
    {
        audioSource.Play();
        animator.SetTrigger("WizardClicked");
    }
}
