using UnityEngine;

public class Fire : MonoBehaviour
{

    [SerializeField]public GameObject fire;
    [SerializeField]private int fireDistance = 1; 
    private bool isRunning = false;


    private Vector3[] directions = {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartSimulation()
    {
        isRunning = true;
        InvokeRepeating("FireSphread", SimulationManager.SimulationInterval, SimulationManager.SimulationInterval);
    }


    private void StopSimulation()
    {
        isRunning = false;
    }

    public void FireSphread()
    {
        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, direction, out hit, fireDistance))
            {
                Vector3 newPosition = transform.position + (direction * fireDistance);

                GameObject newObject = Instantiate(fire, newPosition, Quaternion.identity);

                newObject.GetComponent<Fire>().StartSimulation();


                
            }
        }
    }

}
