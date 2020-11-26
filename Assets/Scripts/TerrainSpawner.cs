﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    private const float DISTANCE_TO_SPAWN = 15f;

    public GameObject startPlatform;
    public List<GameObject> platforms;
    public List<GameObject> buffNousagiPlatforms;

    public GameObject startPicture;
    public GameObject pictureToSpawn;

    private Transform lastEndPosition;
    public Transform currentTerrainParent;
    private Rigidbody2D currentRB;

    private Transform pictureEndPosition;
    public Transform currentPictureParent;
    private Rigidbody2D currentPictureRB;

    private float moveSpeed = -6;
    private float maxMoveSpeed = -14;

    private void Awake()
    {
        lastEndPosition = startPlatform.transform.Find("End");
        pictureEndPosition = startPicture.transform.Find("End");
        currentRB = currentTerrainParent.GetComponent<Rigidbody2D>();
        currentPictureRB = currentPictureParent.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SpawnPlatform();
        startPlatform.transform.parent = currentTerrainParent;
        currentRB.velocity = new Vector2(moveSpeed, 0);
        currentPictureRB.velocity = new Vector2(moveSpeed / 32, 0);

        InvokeRepeating("IncreaseSpeed", 1, 1);
        // Invoke("AddBuffNousagi", 30);
    }

    private void Update()
    {
        currentRB.velocity = new Vector2(moveSpeed, 0);
        currentPictureRB.velocity = new Vector2(moveSpeed / 32, 0);

        if (lastEndPosition.position.x < DISTANCE_TO_SPAWN)
        {
            SpawnPlatform();
        }
        if(pictureEndPosition.position.x < DISTANCE_TO_SPAWN)
        {
            SpawnPicture();
        }
    }

    // Creates a platform in the correct position, and updates the lastEndPosition
    private void SpawnPlatform()
    {
        GameObject randomPart = platforms[Random.Range(0, platforms.Count)];
        GameObject newSpawned = Instantiate(randomPart, lastEndPosition.position, Quaternion.identity);
        newSpawned.transform.parent = currentTerrainParent;
        lastEndPosition = newSpawned.transform.Find("End");
    }

    // Creates a picture in the proper location.
    private void SpawnPicture()
    {
        GameObject newSpawned = Instantiate(pictureToSpawn, pictureEndPosition.position, Quaternion.identity);
        newSpawned.transform.parent = currentPictureParent;
        pictureEndPosition = newSpawned.transform.Find("End");
    }

    // Called every frame. This increases the speed of the platforms moving
    private void IncreaseSpeed()
    {
        moveSpeed -= 0.05f;
        if(moveSpeed < maxMoveSpeed)
        {
            CancelInvoke();
        }
    }

    private void AddBuffNousagi()
    {

    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void StopPlatforms()
    {
        CancelInvoke();
        moveSpeed = 0;
    }
}
