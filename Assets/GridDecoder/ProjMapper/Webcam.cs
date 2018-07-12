﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Webcam : MonoBehaviour
{
  	public static WebCamTexture webcamera;
	private Texture originalTexture;
	public GameObject webcamQuad;
    private Vector3[] vertices;
    
    void start()
    {
        vertices = new Vector3[] {new Vector3(-1, -1, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0), new Vector3(1, -1, 0)};
    }

    void OnEnable()
    {
		if (webcamera == null) {
            if (WebCamTexture.devices.Length == 0)
            {
                #if UNITY_EDITOR
                    // Application.Quit() does not work in the editor so
                    // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
            else
            {
			string webcamName = WebCamTexture.devices[0].name;
			webcamera = new WebCamTexture (webcamName); //SET up the cam
			Debug.Log("Webcam texture set from " + webcamName);

			Setup();
            }
		}
		else {
			Play ();
			Renderer renderer = webcamQuad.GetComponent<Renderer> ();
			renderer.material.mainTexture = webcamera;
			Debug.Log("Webcam restarted.");
		}

    }

	void OnDisable() {
		Stop ();
		Renderer renderer = webcamQuad.GetComponent<Renderer> ();
		renderer.material.mainTexture = originalTexture; //put cam tex onto quad
	}
    
    void Setup()
    {
        Play(); // play camera
		Renderer renderer = webcamQuad.GetComponent<Renderer> ();
		originalTexture = renderer.material.mainTexture; // save current material
        renderer.material.mainTexture = webcamera; //put cam tex onto quad
        Debug.Log("Webcam assigned and playing: " + webcamera.isPlaying);
    }

    public static bool isPlaying()
    {
        return webcamera.isPlaying;
    }

    public static void Pause()
    {
        webcamera.Pause();
    }

    public static void Play()
    {
        int counter = 0;
        while (!isPlaying() && counter < 50)
        {
            webcamera.Play();
            counter++;
        }
    }

    public static void Stop()
    {
		if (webcamera)
        	webcamera.Stop();
    }
}

