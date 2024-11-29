using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public float mass;
    public Vector3 correction;
    public Vector3 velocity;

    public Vector3 position;
    public Color col;

    Vector3 ini;

    LineRenderer lineRenderer;
    List<Vector3> tracer = new List<Vector3>();

    bool change = false;
    bool run = false;

    private void Awake()
    {
        ini = transform.position;
        velocity = correction;
        position = transform.position;

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

    public void Clear(int steps, int thickness)
    {
        velocity = correction;
        position = transform.position;
        if (!run)
            ini = position;

        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;

        lineRenderer.positionCount = 0;
        tracer.Clear();
    }

    public void UpdateVelocity(Body[] bodies, float timeStep, Simul l)
    {
        run = l.runSim;
        foreach (Body b in bodies)
        {
            if (b != this)
            {
                float sqrDist = (b.position - position).sqrMagnitude;
                Vector3 forceDir = (b.position - position).normalized;

                Vector3 force = forceDir * l.gravConst * mass * b.mass / sqrDist;
                Vector3 acc = force / mass;
                velocity += acc * timeStep;
            }
        }
    }

    public void UpdatePos(float timeStep, GameObject inert)
    {
        position += velocity * timeStep;

        if (inert != null)
            tracer.Add(position - (inert.GetComponent<Body>().velocity * timeStep));
        else
            tracer.Add(position);

        lineRenderer.positionCount++;
        lineRenderer.SetPositions(tracer.ToArray());

        if (run) {
            GetComponent<Rigidbody>().position = position;
            change = true;
        }
        else {
            if (change)
            {
                position = ini;
                change = false;
                GetComponent<Rigidbody>().position = position;
                lineRenderer.positionCount = 0;
                tracer.Clear();
            }
            else
                ini = position;
        }
    }
}
