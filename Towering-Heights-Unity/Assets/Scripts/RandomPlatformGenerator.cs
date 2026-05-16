using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomPlatformGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> possiblePlatforms;
    
    void Start()
    {
        GameObject randomPlatform = possiblePlatforms.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        Instantiate(randomPlatform, randomPlatform.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
