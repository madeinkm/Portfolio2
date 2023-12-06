using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackGround : MonoBehaviour
{
    private float speed;
    private Transform[] backGrounds;

    private float leftPosX = 0f;
    private float rightPosX = 0f;
    private float xScreenHalfSize;
    private float yScreenHalfSize;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        backGrounds = GetComponentsInChildren<Transform>();

    }
    void Start()
    {        
        yScreenHalfSize = mainCam.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * mainCam.aspect;

        leftPosX = -(xScreenHalfSize * 2);
        rightPosX = xScreenHalfSize * 2 * backGrounds.Length;
    }

    
    void Update()
    {
        scrollbackGrounds();
    }

    private void scrollbackGrounds()
    {
        for (int iNum = 0; iNum < backGrounds.Length; iNum++)
        {
            backGrounds[iNum].position += new Vector3(-speed, 0, 0) * Time.deltaTime;

            if (backGrounds[iNum].position.x < leftPosX)
            {
                Vector3 nextPos = backGrounds[iNum].position;
                nextPos = new Vector3(nextPos.x + rightPosX, nextPos.y, nextPos.z);
                backGrounds[iNum].position = nextPos;
            }
        }
    }
}
