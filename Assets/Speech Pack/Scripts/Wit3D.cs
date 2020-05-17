/***********************************************************************************
MIT License

Copyright (c) 2016 Aaron Faucher

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.

***********************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO; //Saving and reading of the audio file.
using System.Linq;
using System.Net; //To send the audio file.
using System.Text;
using UnityEngine.Networking; //To send the audio file.
using UnityEngine.UI;
using UnityEngine.Video;

//Partial class means Wit3D and _Handle scripts are part of the same class.
//_Handle is just written separety instead of at the bottom.
public partial class Wit3D : MonoBehaviour
{
	// Class Variables

	// Audio variables
	public AudioClip commandClip; //To overwrite with recorded audio.
	int samplerate; //Sample rate suuitable for wit.ai

	// API access parameters. Variables are private by default if type not specified.
	string url = "https://api.wit.ai/speech?v=20200404";
	string token = "7ZXC5ILLNZMERD46MWYRARQAOZ5TLCPI";

	//Custom 1
 	// GameObject to use as a default spawn point
 	private bool isRecording = false;
	private bool pressedButton = false;
	public Text myResultBox;
	//public VideoPlayer vidScreen;
	//public GameObject vidCanvas;

	// Use this for initialization
	void Start ()
    {

		// If you are a Windows user and receiving a Tlserror
		// See: https://github.com/afauch/wit3d/issues/2
		// Uncomment the line below to bypass SSL
		// System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };

		// set samplerate to 16000 for wit.ai
		samplerate = 16000;
		//vidScreen.GetComponent<VideoPlayer> ();
	}

	//Custom 2
	public void startStopRecord()
    {
		if (isRecording == true)
        {
			pressedButton = true;
			isRecording = false;
 		}
        else if (isRecording == false)
        {
			isRecording = true;
			pressedButton = true;
		}

	}


	//Custom 3
	public void playVideo()
    {
		//vidScreen.Play ();
 		//vidCanvas.SetActive (false);
	}


	//Custom 4
	public void stopVideo()
    {
		//vidScreen.Stop ();
		//vidCanvas.SetActive (true);
	}


	// Update is called once per frame
	void Update ()
    {
		if (pressedButton == true)
        {
			pressedButton = false;
			if (isRecording)
            {
				myResultBox.text = "Listening for command";
				commandClip = Microphone.Start (null, false, 5, samplerate);  //Start recording (rewriting older recordings)
			}

			//Custom 5
			if (!isRecording)
            {
				myResultBox.text = null;
				myResultBox.text = "Saving Voice Request";

				// Save the audio file
				Microphone.End (null);

				if (SavWav.Save ("sample", commandClip))
                {
					myResultBox.text = "Sending audio to AI...";
				}
                else
                {
					myResultBox.text = "FAILED";
				}

				// At this point, we can delete the existing audio clip
				commandClip = null;

 				//Start a coroutine called "WaitForRequest" with that WWW variable passed in as an argument
				StartCoroutine(SendRequestToWitAi());

			}
		}

	}

 	public IEnumerator SendRequestToWitAi()
    {
		//Custom 6
        //To read the stored audio file and convert it to Byte format
		string file = Application.persistentDataPath + "/sample.wav";
 		string API_KEY = token;

		//To rar file to bytes.
		FileStream filestream = new FileStream (file, FileMode.Open, FileAccess.Read);
		BinaryReader filereader = new BinaryReader (filestream);
		byte[] postData = filereader.ReadBytes ((Int32)filestream.Length);
		filestream.Close ();
		filereader.Close ();

		//Custom 7
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers["Content-Type"] = "audio/wav";
		headers["Authorization"] = "Bearer " + API_KEY;
        //Headers are like labels so the AI will know what type of file is sent and knows how to read it.

        //Declaring a new variable time to make a request unique.
		float timeSent = Time.time;
		WWW www = new WWW(url, postData, headers); //WWW is unity function that opens the URL request.

        yield return www;  //This will open the internet connection in the background.
        //Any variable or parameter will be correctly preserved between yields. 
        //By default, a coroutine is resumed on the frame after it yields.


        while (!www.isDone) //waits until what was triggered by www is finished.
        {
			myResultBox.text = "Thinking and deciding ...";
 			yield return null;
            //The yield return null line is the point at which execution will pause and be resumed the following frame.
        }

        float duration = Time.time - timeSent; //To find how much time it has taken to send the request to AI.

		if (www.error != null && www.error.Length > 0)
        {
			UnityEngine.Debug.Log("Error: " + www.error + " (" + duration + " secs)");
			yield break;
		}

        UnityEngine.Debug.Log("Success (" + duration + " secs)");
		UnityEngine.Debug.Log("Result: " + www.text); //From the returned www request, this returns the Json code.
        
        Handle (www.text); //Pass the Json code to a function called Handle.

	}
    

}