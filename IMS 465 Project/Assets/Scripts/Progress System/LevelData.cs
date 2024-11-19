using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("ID")]
    public int worldIndex;
    public int levelIndex;

    [Header("Timer")]
    public bool counting = true;
    public float time = 0.0f;

    [Header("Star Times")]
    public float star3time = 20;
    public float star2time = 40;

    private void Update()
    {
        if (counting)
        {
            time += Time.deltaTime;
        }
    }
}
