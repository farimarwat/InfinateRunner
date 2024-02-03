using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneratorScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Roads")]
    [SerializeField] Transform StartingPoint;
    [SerializeField] Transform EndingPoint;
    [SerializeField] Transform[] roadBlocks;
    [SerializeField] float GlobalSpeed = 5f;
    Vector3 movedirection;

    //Spawning Buildings
    [Header("Buildings")]
    [SerializeField] GameObject[] buildings;
    [SerializeField] Transform[] buildingSpawnPoints;

    //Spawning Street Lights
    [Header("Street Lights")]
    [SerializeField] private GameObject[] streetlights;
    [SerializeField] private Transform[] streetlightspawnpoints;

    //Thread
    [SerializeField, Header("Threats")] private Threat[] threats;
    [SerializeField] private Transform[] threatLanes;
    [SerializeField] private Vector3 occupationsdetections;

    private GlobalSpeedController globalSpeedController;

    private void OnEnable()
    {
        if(globalSpeedController != null)
        {
            globalSpeedController.OnSpeedChanged += SetGlobalSpeed;
        }
    }
    private void Awake()
    {
        globalSpeedController = GetComponent<GlobalSpeedController>();
    }
    private void OnDisable()
    {
        if (globalSpeedController != null)
        {
            globalSpeedController.OnSpeedChanged -= SetGlobalSpeed;
        }
    }
    void Start()
    {
        if(globalSpeedController != null)
        {
            globalSpeedController.SetGlobalSpeed(5);
        }
        Vector3 nextBlockPosition = StartingPoint.position;
        float endPointDistance = Vector3.Distance(StartingPoint.position, EndingPoint.position);
         movedirection = (EndingPoint.position - StartingPoint.position).normalized;

        while (Vector3.Distance(StartingPoint.position,nextBlockPosition) < endPointDistance)
        {
            GameObject newBlock = SpawnBlock(nextBlockPosition,movedirection);
            float blockLength = newBlock.GetComponent<Renderer>().bounds.size.z;
            nextBlockPosition +=  movedirection* blockLength;
            
        }

        StartSpawnThreats();
    }

    bool GetRandomSpawnPoint(out Vector3 spawnPoint)
    {
        Vector3[] spawnpoints = GetAvailableSpawnPoints();
        if(spawnpoints.Length == 0)
        {
            spawnPoint = new Vector3(0, 0, 0);
            return false;
        }
        int picked = Random.Range(0, spawnpoints.Length);
      
        spawnPoint = spawnpoints[picked];
        return true;
    }

    private Vector3[] GetAvailableSpawnPoints()
    {
        List<Vector3> spawnPoints = new List<Vector3>();
        foreach(Transform spawnTrans in threatLanes)
        {
            Vector3 spawnpoint = spawnTrans.position + new Vector3(0, 0, StartingPoint.position.z);
            if (!isPositionOccupied(spawnpoint))
            {
                spawnPoints.Add(spawnpoint);
            }
        }
        return spawnPoints.ToArray();
    }
    private bool isPositionOccupied(Vector3 position)
    {
        Collider[] cols = Physics.OverlapBox(position, occupationsdetections);
        foreach(Collider col in cols)
        {
            if(col.gameObject.tag == "Threat")
            {
                return true;
            }
        }
        return false;
    }
    private void StartSpawnThreats()
    {
        foreach (Threat threat in threats)
        {
            StartCoroutine(SpawnThreatCoroutine(threat));
        }
    }
    IEnumerator SpawnThreatCoroutine(Threat threatToSpawn)
    {
        while (true)
        {
            if(GetRandomSpawnPoint(out Vector3 spawnPoint))
            {
                Threat newThreat = Instantiate(threatToSpawn, spawnPoint, Quaternion.identity);
                newThreat.GetMovementScript().SetDestination(EndingPoint.position);
                newThreat.GetMovementScript().SetMoveDirection(movedirection);
            }
           
            yield return new WaitForSeconds(threatToSpawn.SpawnInterval);
        }
    }

    GameObject SpawnBlock(Vector3 spawnposition, Vector3 movedir)
    {
        int pick = Random.Range(0, roadBlocks.Length);
        GameObject picked = roadBlocks[pick].gameObject;
        GameObject newBlock = Instantiate(picked) as GameObject;
        newBlock.transform.position = spawnposition;
        MovementScript rm = newBlock.GetComponent<MovementScript>();
        if (rm != null)
        {
            rm.SetSpeed(GlobalSpeed);
            rm.SetDestination(EndingPoint.position);
            rm.SetMoveDirection(movedir);
        }
        //Spawning buildings
        SpawnBuildings(newBlock);
        SpawnStreetLights(newBlock);

        return newBlock;
    }

    private void SpawnBuildings(GameObject parentblock)
    {
        foreach (Transform buildingspawnpoint in buildingSpawnPoints)
        {
            Vector3 buildingspawnlocation = (parentblock.transform.position + (buildingspawnpoint.position - StartingPoint.position));
            int rotationby90 = Random.Range(0, 3);
            Quaternion spawnrotation = Quaternion.Euler(0, rotationby90 * 90, 0);
            int buildingpicked = Random.Range(0, buildings.Length);
            GameObject objBuilding = Instantiate(buildings[buildingpicked], buildingspawnlocation, spawnrotation, parentblock.transform);
        }
    }
    private void SpawnStreetLights(GameObject parentblock)
    {
        int pickedLight = Random.Range(0, streetlights.Length);
        GameObject newLight = streetlights[pickedLight];
        foreach (Transform streetLightSpawnPoint in streetlightspawnpoints)
        {
            Vector3 spawnLoc = parentblock.transform.position + (streetLightSpawnPoint.position - StartingPoint.position);
            Quaternion spawnRotation = Quaternion.LookRotation((StartingPoint.position - streetLightSpawnPoint.position).normalized, Vector3.up);
            Quaternion spawnOffset = Quaternion.Euler(0, -90, 0);
            GameObject streetLight = Instantiate(newLight,spawnLoc,spawnRotation * spawnOffset,parentblock.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject != null && other.gameObject.tag == "RoadBlock")
        {
            GameObject newBlock = SpawnBlock(other.transform.position, movedirection);
            float halfOther = other.GetComponent<Renderer>().bounds.size.z / 2f;
            float halfNewBlock = newBlock.GetComponent<Renderer>().bounds.size.z / 2f;
            Vector3 newOffset = -(halfNewBlock + halfOther) * movedirection;
            newBlock.transform.position += newOffset;
        }
    }

    private void SetGlobalSpeed(float speed)
    {
        GlobalSpeed = speed;
    }
}
