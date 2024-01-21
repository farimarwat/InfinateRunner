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
    [SerializeField] float roadMoveSpeed = 5f;
    Vector3 movedirection;

    //Spawning Buildings
    [Header("Buildings")]
    [SerializeField] GameObject[] buildings;
    [SerializeField] Transform[] buildingSpawnPoints;

    //Spawning Street Lights
    [Header("Street Lights")]
    [SerializeField] private GameObject[] streetlights;
    [SerializeField] private Transform[] streetlightspawnpoints;

    [SerializeField, Header("Threats")] private Threat[] threats;
    [SerializeField] private Transform[] threatLanes;

    void Start()
    {
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

    Vector3 GetThreatRandomSpawnPoint()
    {
        if(threatLanes.Length == 0)
        {
            return StartingPoint.position;
        }
        int picked = Random.Range(0, threatLanes.Length);
        Vector3 pickedLane = threatLanes[picked].position;
        return pickedLane + new Vector3(0,0,StartingPoint.position.z);
    }

    private void StartSpawnThreats()
    {
       foreach(Threat  threat in threats)
        {
            StartCoroutine(SpawnThreatCoroutine(threat));
        }
    }
    IEnumerator SpawnThreatCoroutine(Threat threatToSpawn)
    {
        while (true)
        {
            Threat newThreat = Instantiate(threatToSpawn,GetThreatRandomSpawnPoint(), Quaternion.identity);
            newThreat.GetMovementScript().SetDestination(EndingPoint.position);
            newThreat.GetMovementScript().SetMoveDirection(movedirection);
            yield return new WaitForSeconds(newThreat.SpawnInterval);
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
            rm.SetSpeed(roadMoveSpeed);
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
}
