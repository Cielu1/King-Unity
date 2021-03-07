using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Game_Handler : MonoBehaviour
{
    public List<GameObject> player_A_Hand;
    public List<GameObject> player_B_Hand;
    public List<GameObject> player_C_Hand;
    public List<GameObject> all_Cards;




    // Start is called before the first frame update
    void Start()
    {
        //create a dictionary with key values for all cards
        ShuffleCards();
        DrawCards(player_A_Hand, 0, 9);
        DrawCards(player_B_Hand, 10, 19);
        DrawCards(player_C_Hand, 20, 29);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //list of random numbers without duplicating
    List<int> RandomNumbersWithoutDuplicates(int count, int min, int max)
    {
        int randomNumber;
        List<int> randomNumbers = new List<int>();

        for (int i = 0; i < count; i++)                   {
            do { randomNumber = Random.Range(min, max)  ; }
            while (randomNumbers.Contains(randomNumber));
            randomNumbers.Add(randomNumber)             ;
            //randomNumbers.Sort();
        }

        return randomNumbers;
    }

    void ShuffleCards()
    {
        List<int> randomNumbers = RandomNumbersWithoutDuplicates(32, 0, 32);
        int i = 0;
        foreach (int k in randomNumbers)
        {
            GameObject tempCard = all_Cards[i];
            Vector3 tempPos = all_Cards[i].transform.position;

            all_Cards[i].transform.position = all_Cards[k].transform.position;
            all_Cards[i] = all_Cards[k];

            all_Cards[k].transform.position = tempPos;
            all_Cards[k] = tempCard;

            i++;
        }

    }



    void DrawCards(List<GameObject> hand, int startCard, int endCard)
    {
        int j = 0;
       for (int i = startCard; i < endCard+1; i++) { 
            GameObject  tempCard = hand[j];
            Vector3 tempPos = hand[j].transform.position;

            hand[j].transform.position = all_Cards[i].transform.position;
            hand[j] = all_Cards[i];

            all_Cards[i].transform.position = tempPos;
            all_Cards[i] = tempCard;
            j++;
        }

    }

}
