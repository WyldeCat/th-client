using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH
{
public enum Pattern { CLUBS, SPADES, HEARTS, DIAMONDS };

public class Card : MonoBehaviour
{
    private Pattern pattern_;
    private int rank_;

    private bool isMoving_;
    private float delta_;
    private float percentage_;
    private Vector3 startPos_;
    private Vector3 endPos_;

    void Start()
    {
        isMoving_ = false;
    }


    void Update()
    {
        if(isMoving_){
            percentage_ += delta_;
            transform.position = Vector3.Lerp(transform.position, endPos_, percentage_);
            if(percentage_>=1.0f){
                isMoving_ = false;
                percentage_ = 0;
            }
        }
    }

    public void SetRank(int rank)
    {
        rank_ = rank;
    }

    public void SetPattern(Pattern pattern)
    {
        pattern_ = pattern;
    }

    void MoveTo(Vector3 dest, float timeToReach)
    {
        startPos_ = transform.position;
        endPos_ = dest;
        delta_ = Time.deltaTime / timeToReach;
        isMoving_ = true;
    }
}
}
