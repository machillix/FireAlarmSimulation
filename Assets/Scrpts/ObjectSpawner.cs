/******************************************************************************
* File         : ObjectSpawner.cs
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
using UnityEngine.UI;
public class ObjectSpawner : MonoBehaviour
{

    [SerializeField]private float radius = 20;
    [SerializeField]private GameObject spawnObject;

    [SerializeField]private Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObjects(Mathf.RoundToInt(slider.value));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpawnedObjects(float value){
        SpawnObjects(Mathf.RoundToInt(value));
    }

    private void SpawnObjects(int count)
    {
        DestroyObjects();

        for (int i = 0; i < count; i++)
        {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * radius;
        randomPosition.y = 0.9f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, NavMesh.AllAreas))
        {
            GameObject newObject = Instantiate(spawnObject, hit.position, Quaternion.identity);
            NavMeshAgent agent = newObject.GetComponent<NavMeshAgent>();
        }
    }
}


    private void DestroyObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Dummy");
        foreach (GameObject single in objects)
        {
            Destroy(single);
        }
    }
}
