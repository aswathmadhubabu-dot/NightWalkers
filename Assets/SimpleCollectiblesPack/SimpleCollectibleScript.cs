﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public enum CollectibleTypes {NoType, Health, Time, Destroy}; // you can replace this with your own labels for the types of collectibles in your game!

	public CollectibleTypes CollectibleType; // this gameObject's type

	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	public GameObject timerUI;
	public enum Levels {Level1, Level2, Level3};
	public Levels Level;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Collect (other);
		}
	}

	public void Collect(Collider other)
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		//Below is space to add in your code for what happens based on the collectible type

		if (CollectibleType == CollectibleTypes.NoType) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.Health) {

			//Add in code here;

			Debug.Log ("Do Health Command");
			other.gameObject.GetComponent<HealthController>().RestoreHealth();
			
		}
		if (CollectibleType == CollectibleTypes.Time) {

			//Add in code here;
			Debug.Log("Do Time Command");
			int reset;
			if (Level == Levels.Level1)
            {
				reset = 180;
            } else
            {
				if (Level == Levels.Level2)
                {
					reset = 240;
                } else
                {
					reset = 300;
                }
            }
			timerUI.gameObject.GetComponent<Timer>().SetDuration(reset);
		}
		if (CollectibleType == CollectibleTypes.Destroy) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}
	

		Destroy (gameObject);
	}
}
