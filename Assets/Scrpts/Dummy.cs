/******************************************************************************
* File         : Fire.cs
* Authors      : Toni Westerlund (MaChilli/machillix)
* Lisence      : MIT Licence
* Copyright    : Toni Westerlund (MaChilli/machillix)
* 
* MIT License
* 
* Copyright (c) 2024 Toni Westerlund (MaChilli/machillix)
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*****************************************************************************/

using UnityEngine;
using UnityEngine.AI;

public class Dummy : MonoBehaviour
{


    private bool isRunning = false;
    private GameObject[] targetAreas;
    private NavMeshAgent agent;

    private Vector3 lastPosition = Vector3.zero;

    private float standingTime = 0f;

    [SerializeField]private Material stopMaterial;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        SimulationManager.StartSimulation += StartSimulation;
        SimulationManager.StopSimulation += StopSimulation;
    }

    void OnDisable()
    {
        SimulationManager.StartSimulation -= StartSimulation;
        SimulationManager.StopSimulation -= StopSimulation;
    }

    void Update()
    {
        if(!isRunning)return;

        if(!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
        {
            FindClosestAndAvailableTarget();
        }

        if (Vector3.Distance(transform.position, lastPosition) > 0.005f) // Adjust the threshold based on your needs
        {
            standingTime = 0f;
        }
        else
        {
            standingTime += Time.deltaTime;
        }


        if (standingTime > 2f)
        {
            ChangeMaterial();
        }

    // Update the last position for the next frame's comparison
        lastPosition = transform.position;
    }


    private void StartSimulation()
    {
        targetAreas = GameObject.FindGameObjectsWithTag("Target");
        isRunning = true;
        FindClosestAndAvailableTarget();
    }

    private void ChangeMaterial()
    {
        GetComponent<Renderer>().material = stopMaterial;

        agent.enabled = false;
        StopSimulation();
    }

    private void StopSimulation()
    {
        isRunning = false;
    }

    private void FindClosestAndAvailableTarget()
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject target in targetAreas)
        {
            Vector3 directionToSafeSpace = target.transform.position - currentPosition;
            float distance = directionToSafeSpace.sqrMagnitude;
            if (distance < closestDistance)
            {
                NavMeshPath path = new NavMeshPath();
                bool found = agent.CalculatePath(target.transform.position, path);
                if (found && path.status == NavMeshPathStatus.PathComplete)
                {
                    closest = target;
                    closestDistance = distance;
                }

            }
        }

        agent.SetDestination(closest.transform.position);

    }
}
