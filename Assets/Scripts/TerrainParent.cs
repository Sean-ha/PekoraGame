using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainParent : MonoBehaviour
{
    private void Update()
    {
        if(transform.position.x < -1000)
        {
            MoveParent();
        }
    }

    // Moves the parent back to the center to avoid issues with extremely small floats
    private void MoveParent()
    {
        List<Transform> children = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
        transform.DetachChildren();
        transform.position = new Vector2(0, 0);
        for(int i = 0; i < children.Count; i++)
        {
            children[i].parent = transform;
        }
    }
}
