using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Game_Handler : MonoBehaviour
{
    public List<GameObject> player_A_Hand;
    public List<GameObject> player_B_Hand;
    public List<GameObject> player_C_Hand;

    public List<GameObject> player_A_Buttons;
    public List<GameObject> player_B_Buttons;
    public List<GameObject> player_C_Buttons;

    public List<GameObject> stock;

    public List<GameObject> player_R_Hand;

    public List<GameObject> clubs;
    public List<GameObject> diamonds;
    public List<GameObject> spades;
    public List<GameObject> hearts;

    List<int> playersID = new List<int>();

    List<GameObject> all_Cards = new List<GameObject>();

    List<GameObject> player_A_Stock;
    List<GameObject> player_B_Stock;
    List<GameObject> player_C_Stock;

    List<int> playersScores = new List<int>();

    int playersMoved = 0;
    int currentPlayer;

    Dictionary<string,int> allCardsValues = new Dictionary<string,int>();
    Dictionary<string, string> allCardsColors = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        AddPlayers();
        AddAllCards();
        DrawStartCards();
    }

    void AddAllCards()
    {
        int value = 0;
        foreach (GameObject gameObject in clubs)
        {
            allCardsColors.Add(gameObject.name, "clubs");
            allCardsValues.Add(gameObject.name, value);
            value++;
            all_Cards.Add(gameObject);
        }

        value = 0;
        foreach (GameObject gameObject in diamonds)
        {
            allCardsColors.Add(gameObject.name, "diamonds");
            allCardsValues.Add(gameObject.name, value);
            value++;
            all_Cards.Add(gameObject);
        }

        value = 0;
        foreach (GameObject gameObject in spades)
        {
            allCardsColors.Add(gameObject.name, "spades");
            allCardsValues.Add(gameObject.name, value);
            value++;
            all_Cards.Add(gameObject);
        }

        value = 0;
        foreach (GameObject gameObject in hearts)
        {
            allCardsColors.Add(gameObject.name, "hearts");
            allCardsValues.Add(gameObject.name, value);
            value++;
            all_Cards.Add(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(playersMoved == 3)
        {
            Debug.LogError("ALL PLAYERS MOVED, RESOLVING TURN");
            ResolveRound();
        }
 
    }

    void AddPlayers() 
    {
        currentPlayer = 0;
        for (int i = 0; i < 3; i++)
        {
            playersID.Add(i);
            playersScores.Add(0);
        }
    }
    
    bool IsCurrentPlayer(int id)
    {
        if (id == currentPlayer) return true;
        else return false;
    }

    void ResolveRound()
    {
        string dominant;
        allCardsColors.TryGetValue(stock[0].name, out dominant);
        Debug.LogError(dominant);
        var id = currentPlayer;
        foreach(GameObject gameObject in stock)
        {
            int score = 0;
            int value;
            string color;
            allCardsValues.TryGetValue(gameObject.name, out value);
            allCardsColors.TryGetValue(gameObject.name, out color);
            score += value;
            if (color == dominant) score += 100;
            playersScores[id] += score;
            Debug.LogWarning("player ID: " + id +" scored: " +  playersScores[id]);
            id++;
        }
        playersMoved = 0;
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

    void SortCards(List<GameObject> list)
    {
        foreach (GameObject gameObject in list)
        {

        }
    }


    void  DrawStartCards()
    {
        List<int> randomNumbers = RandomNumbersWithoutDuplicates(32, 0, 32);
        List<int> randomNumbersA = new List<int>();
        List<int> randomNumbersB = new List<int>();
        List<int> randomNumbersC = new List<int>();
        List<int> randomNumbersR = new List<int>();

        randomNumbersR.Add(randomNumbers[30]);
        randomNumbersR.Add(randomNumbers[31]);

        for (int i = 0; i<10; i++)
        {
            randomNumbersA.Add(randomNumbers[i]);
            randomNumbersB.Add(randomNumbers[i+10]);
            randomNumbersC.Add(randomNumbers[i+20]);
        }

        randomNumbersA.Sort();
        randomNumbersB.Sort();
        randomNumbersC.Sort();
        randomNumbersR.Sort();

        DrawCardsFromList(player_A_Hand, randomNumbersA);
        DrawCardsFromList(player_B_Hand, randomNumbersB);
        DrawCardsFromList(player_C_Hand, randomNumbersC);
        DrawCardsFromList(player_R_Hand, randomNumbersR);

    }

    void SwapCards(GameObject cardA, GameObject cardB)
    {
        Vector3 tempPos = cardA.transform.position;
        cardA.transform.position = cardB.transform.position;
        cardB.transform.position = tempPos;

        string tempName = cardA.name;
        cardA.name = cardB.name;
        cardB.name = tempName;
    }

    GameObject SetCard(GameObject card)
    {
        return card;
    }
    void DrawCardsFromList(List<GameObject> list, List<int> i)
    {
        for (int j =0; j< list.Count(); j++)
        {
            SwapCards(list[j], all_Cards[i[j]]);
        }
    }

    bool playerAMoved = false;
    bool playerBMoved = false;
    bool playerCMoved = false;

    public void PlayerMoved(int id)
    {
        switch (id)
        {
            case 0:
                if (playerAMoved != true)
                {
                    playerAMoved = true;
                    playersMoved++;
                    currentPlayer = 1;
                }
                break;
            case 1:
                if (playerBMoved != true)
                {
                    playerBMoved = true;
                    playersMoved++;
                    currentPlayer = 2;
                }
                break;
            case 2:
                if (playerCMoved != true)
                {
                    playerCMoved = true;
                    playersMoved++;
                    currentPlayer = 0;
                }
                break;
        }
    }

    public void ButtonPressedID(int id)
    {
        if (id < 10 && id >= 0)
        {
            if (IsCurrentPlayer(0))
            {
                SwapCards(player_A_Hand[id], stock[0]);
            }
            else { Debug.LogError("It is not your turn, A!"); }
        }
        else if (id < 20 && id >= 10)
        {
            if (IsCurrentPlayer(1))
            {
                SwapCards(player_B_Hand[id-10], stock[1]);
            }
            else { Debug.LogError("It is not your turn, B!"); }
        }
        else if (id < 30 && id >= 20)
        {
            if (IsCurrentPlayer(2))
            {
                SwapCards(player_C_Hand[id-20], stock[2]);
            }
            else { Debug.LogError("It is not your turn, C!"); }
        }
        else { Debug.LogError("idOfButtonTooHigh"); }

    }
}
