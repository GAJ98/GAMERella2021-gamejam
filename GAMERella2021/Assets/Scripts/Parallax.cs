using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(0f, 1f)] public float textureSpeed;

    private void Update()
    {
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", GetComponent<Renderer>().material.GetTextureOffset("_MainTex") + new Vector2(textureSpeed, 0) * Time.deltaTime);
    }
}
