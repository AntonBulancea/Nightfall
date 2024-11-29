using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Simul : MonoBehaviour
{
    public float gravConst;
    public float timeStep;
    public int thickness;
    public int numSteps = 1000;
    public GameObject inetrial;
    public bool runSim;

    public bool change = false;
    public Body[] nBody;
    float t;

    private void Update()
    {
        nBody = GameObject.FindObjectsByType<Body>(FindObjectsSortMode.None);
        if(runSim && change == false) {
            change = true;
            t = 0;
            for (int i = 0; i < nBody.Length; i++)
                nBody[i].Clear(numSteps, thickness);
        }

        if (!runSim)
            change = false;

        if (t >= numSteps && !runSim)
        {
            t = 0;
            for (int i = 0; i < nBody.Length; i++)
                nBody[i].Clear(numSteps,thickness);

            Debug.Log("Clear");
        }

        //Update Velocity
        for (int i = 0; i < nBody.Length; i++)
        {
            nBody[i].UpdateVelocity(nBody, timeStep, this);
        }

        //Update Position
        for (int i = 0; i < nBody.Length; i++)
        {
            nBody[i].UpdatePos(timeStep, inetrial);
        }

        t+=timeStep;
    }
}