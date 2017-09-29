using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH
{
public class Deck
{
    private Card [] deck_ = new Card[52];
    private int currPointer_;

    public void CreateDeck()
    {
        currPointer_ = 0;
        for(int i=0;i<52;++i){
             deck_[i].SetRank(i % 13);
             deck_[i].SetPattern((Pattern)(i % 4));
        }
    }

    public void Shuffle()
    {
        for(int i=0;i<26;++i){
            int randomIndex1 = Random.Range(0, 52);
            int randomIndex2 = Random.Range(0, 52);
            Card temp = deck_[randomIndex1];
            deck_[randomIndex1] = deck_[randomIndex2];
            deck_[randomIndex2] = temp;
        }
    }

    public Card Draw()
    {
        return deck_[currPointer_++];
    }

    }
}
