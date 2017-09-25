using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH
{
enum Pattern { CLUBS, SPADES, HEARTS, DIAMONDS };

public class Card : MonoBehaviour
{
    private Pattern pattern;
    private int rank;

    private bool isMoving;
    private float delta;
    private float percentage;
    private Vector3 startPos;
    private Vector3 endPos;


    void Start()
    {
        isMoving = false;
    }

    void Update()
    {
        if(isMoving)
        {
            percentage += delta;
            transform.position = Vector3.Lerp(transform.position, endPos, percentage);
            if(percentage>=1.0f){
                isMoving = false;
                percentage = 0;
            }
        }
    }

    void MoveTo(Vector3 dest, float timeToReach)
    {
        startPos = transform.position;
        endPos = dest;
        delta = Time.deltaTime / timeToReach;
        isMoving = true;
    }
}
}
