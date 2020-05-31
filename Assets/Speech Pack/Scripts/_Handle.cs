using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Newtonsoft.Json; //DLL file works with C sharp.

//Custom 8
//Partial class means Wit3D and _Handle scripts are part of the same class.
//_Handle is just written separety instead of at the bottom.
//Advantage: Can be used in multiple applications. Flexible with data that is passed.
public partial class Wit3D : MonoBehaviour
{

	public Text myHandleTextBox;
	private bool actionFound = false;

	void Handle (string jsonString)
    {
		
		if (jsonString != null)
        {

			RootObject theAction = new RootObject ();
			Newtonsoft.Json.JsonConvert.PopulateObject (jsonString, theAction);
            //The job of the DLL file is to read Json code, break and assign it to an object of a certain type.
            //PopulateObject - receives file (here jsonString), passed from Wit3D script. This is json code format.
            //It will then dynamically populate the object that is mentioned ("theAction") here. This is class of type RootObject.

			if (theAction.entities.open != null)
            {
				foreach (Open aPart in theAction.entities.open)
                {
                    //For Debugging
					Debug.Log (aPart.value);
                    //myHandleTextBox.text = aPart.value;

                    carController.instance.triggerAnimation("openDriversDoor");
					actionFound = true;
				}
			}

			if (theAction.entities.close != null)
            {
				foreach (Close aPart in theAction.entities.close)
                {
                    //For Debugging
                    Debug.Log (aPart.value);
                    //myHandleTextBox.text = aPart.value;

                    carController.instance.triggerAnimation("closeDriversDoor");
					actionFound = true;
				}
			}

            if (theAction.entities.color != null)
            {
                foreach (carColor aPart in theAction.entities.color)
                {
                    //For Debugging
                    Debug.Log(aPart.value);
                    //myHandleTextBox.text = aPart.value;

                    colourSwitcher.instance.colours(aPart.value);
                    actionFound = true;
                }
            }

            if (theAction.entities.start != null)
            {
                foreach (Start aPart in theAction.entities.start)
                {
                    //For Debugging
                    Debug.Log(aPart.value);
                    //myHandleTextBox.text = aPart.value;

                    carController.instance.engineStart();
                    actionFound = true;
                }
            }

            if (theAction.entities.stop != null)
            {
                foreach (Stop aPart in theAction.entities.stop)
                {
                    //For Debugging
                    Debug.Log(aPart.value);
                    //myHandleTextBox.text = aPart.value;

                    carController.instance.engineStop();
                    actionFound = true;
                }
            }

            if (actionFound != true)
            {
				myHandleTextBox.text = "Request unknown, please ask a different way.";
			}
            else
            {
				actionFound = false;
			}

 		}//END OF IF

 	}//END OF HANDLE VOID

}//END OF CLASS


//Custom 9
//Handles the response.
//Wit.ai replies with a Entities: Open/Close/any Trait, Confidence, Value (Drivers door, bonnet, color etc), value type.
//It also gives a message id at the bottom.

public class RootObject //Expects a text, entity, message id
{
    public string _text { get; set; }
    public Entities entities { get; set; } //Entities has nested children.
    public string msg_id { get; set; }
}

//Nested class of Entities.
public class Entities //Entities created as a class. Will expand as we add more traits.
{
    public List<Open> open { get; set; }
    public List<Close> close { get; set; }
    public List<carColor> color { get; set; }
    public List<Start> start { get; set; }
    public List<Stop> stop { get; set; }

}

public class Open
{
	public bool suggested { get; set; }
	public double confidence { get; set; }
	public string value { get; set; } //Value would be door or bonet or trunk..
	public string type { get; set; }
}

public class Close
{
	public bool suggested { get; set; }
	public double confidence { get; set; }
	public string value { get; set; } //Value would be door or bonet or trunk..
    public string type { get; set; }
}

public class carColor
{
    public bool suggested { get; set; }
    public double confidence { get; set; }
    public string value { get; set; } //Value would be red or blue or orange or black..
    public string type { get; set; }
}

public class Start
{
    public bool suggested { get; set; }
    public double confidence { get; set; }
    public string value { get; set; } //Value would be engine..
    public string type { get; set; }
}

public class Stop
{
    public bool suggested { get; set; }
    public double confidence { get; set; }
    public string value { get; set; } //Value would be engine..
    public string type { get; set; }
}