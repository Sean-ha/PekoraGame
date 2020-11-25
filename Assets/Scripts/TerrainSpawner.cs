using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    private const float DISTANCE_TO_SPAWN = 15f;

    public GameObject startPlatform;
    public List<GameObject> platforms;

    private Transform lastEndPosition;

    private float moveSpeed = -4;

    private void Awake()
    {
        lastEndPosition = startPlatform.transform.Find("End");
    }

    private void Start()
    {
        startPlatform.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
        InvokeRepeating("IncreaseSpeed", 1, 1);
    }

    private void Update()
    {
        if(lastEndPosition.position.x < DISTANCE_TO_SPAWN)
        {
            SpawnPlatform();
        }
    }

    // Creates a platform in the correct position, and updates the lastEndPosition
    private void SpawnPlatform()
    {
        GameObject randomPart = platforms[Random.Range(0, platforms.Count)];
        GameObject newSpawned = Instantiate(randomPart, lastEndPosition.position, Quaternion.identity);
        newSpawned.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
        lastEndPosition = newSpawned.transform.Find("End");
    }

    // Called every frame. This increases the speed of the platforms moving
    private void IncreaseSpeed()
    {
        moveSpeed -= 0.05f;
        if(moveSpeed < -18)
        {
            CancelInvoke();
        }
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
