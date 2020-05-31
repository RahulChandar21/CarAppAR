using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
{
	AudioSource carSound;
	Animator anim;
 	public static carController instance;

	//Create a cloned object so we can access the functions
	void Awake()
    {
		if (instance == null)
        {
			instance = this;
		}
	}

	// Use this for initialization
	void Start ()
    {
		
		//Loop through the child items activating the correct item by name
		for (int i = 0; i < transform.childCount; ++i)
        {
			
			//Check the current selected item and activate
 			if (transform.GetChild (i).gameObject.name == GameController.currentSelectedCar)
            {
				transform.GetChild (i).gameObject.SetActive (true);

				//Get the animator componant from the active item
				anim = transform.GetChild (i).gameObject.GetComponent<Animator> ();
			}

            else
            {
				//Deactivate all other cars
				transform.GetChild (i).gameObject.SetActive (false);
			}
 		}

		 
	}

	//Called from _Handle
 	public void triggerAnimation(string action)
    {
		anim = GameObject.Find("UserDefinedTarget-1/activeItems/" + GameController.currentSelectedCar).GetComponent<Animator>();
		anim.SetTrigger (action);
	}

	public void engineStart()
	{
		carSound = GameObject.Find("UserDefinedTarget-1/activeItems/" + GameController.currentSelectedCar).GetComponent<AudioSource>();
		carSound.Play();
	}

	public void engineStop()
	{
		carSound = GameObject.Find("UserDefinedTarget-1/activeItems/" + GameController.currentSelectedCar).GetComponent<AudioSource>();
		carSound.Stop();
	}

	//Called from _Handle
	public void showMessage()
    {
		//TODO
	}

}
