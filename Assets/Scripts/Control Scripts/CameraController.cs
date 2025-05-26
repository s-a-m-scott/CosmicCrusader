using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject backgroundPlane;
    Vector3 offset;
    Vector3 bgOffset;
    float texSize;
    void Start()
    {
        //Screen.SetResolution(640,480,false);
        offset = transform.position;
        bgOffset = backgroundPlane.transform.position;
        texSize = 20;//backgroundPlane.GetComponent<MeshRenderer>().material.mainTexture.height / 10;//*10 bcos plane is 10 units wide

    }

    void Update()
    {
        Vector3 playerPos = player.gameObject.transform.position;
        transform.position = Vector3.Lerp(transform.position, playerPos + offset, 0.1f);

        backgroundPlane.GetComponent<MeshRenderer>().material.mainTextureOffset = -playerPos / texSize;
        backgroundPlane.transform.position = playerPos + bgOffset;
    }
}
