using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    public List<GameObject> player_A_Stock = new List<GameObject>();
    public List<GameObject> player_B_Stock = new List<GameObject>();
    public List<GameObject> player_C_Stock = new List<GameObject>();

    List<List<GameObject>> playersStocks = new List<List<GameObject>>();
    Dictionary<int, int> playersRoundScores = new Dictionary<int, int>();

    public int playersMoved = 0;
    public int currentPlayer;

    Dictionary<string,int> allCardsValues = new Dictionary<string,int>();
    Dictionary<string, string> allCardsColors = new Dictionary<string, string>();

    public GameObject buttonTemplate;
    public GameObject Layout;

    // Start is called before the first frame update
    void Start()
    {
        AddPlayers();
        AddAllCards();
        DrawStartCards();
        SpawnCards();
    }

    void SpawnCards()
    {
        int k = 0;
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 10; i++)
            {

                //Button button = GetComponent<Button>();
                GameObject button = new GameObject("button" + k);
                button.AddComponent<Button>();
                button.GetComponent<Button>().onClick.AddListener(() => { int tmp = k; ButtonPressedID(k); });
                button.name = "Button " + k;
                button.transform.SetParent(Layout.transform);
                k++;
            }
        }
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
            playersRoundScores.Add(i,0);
            
        }
        playersStocks.Add(player_A_Stock);
        playersStocks.Add(player_B_Stock);
        playersStocks.Add(player_C_Stock);
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
        var id = 0;
        foreach(GameObject gameObject in stock)
        {

            int score = 0;
            int value;
            string color;
            allCardsValues.TryGetValue(gameObject.name, out value);
            allCardsColors.TryGetValue(gameObject.name, out color);
            score += value;
            if (color == dominant) score += 100;
            playersRoundScores[id] += score;
            Debug.LogWarning("player ID: " + id +" scored: " +  playersRoundScores[id]);
            id++;
           // gameObject.SetActive(false);
        }

        //player ID of a player with highest score
        int roundWinner = FindKeyWithHighestValue(playersRoundScores);
        Debug.LogWarning("player with ID: " + roundWinner + " scored highest: " + playersRoundScores[roundWinner]);
        
        foreach(GameObject gameObject in stock)
        {
            playersStocks[roundWinner].Add(gameObject);
        }
        playersMoved = 0;
        currentPlayer = roundWinner;
        for (int i = 0; i<3; i++)
        {
            playersRoundScores[i] = 0;
        }

        PutCardsToPlayerStock(roundWinner);

    }

    void PutCardsToPlayerStock(int id)
    {
        float y = 0;
        float x = 30;
        foreach(GameObject card in playersStocks[id])
        {
            x += 3.5f;
        }
        switch (id)
        {
            case 0:
                y = 0;
                break;
            case 1:
                y = -5;
                break;
            case 2:
                y = -10;
                break;
        }

        for (int i = 0; i<3; i++)
        {
            stock[i].transform.position = new Vector3(x, y, 0);
            x += 3.5f;
        }
    }

    int FindKeyWithHighestValue(Dictionary<int,int> results)
    {
        KeyValuePair<int, int> max= new KeyValuePair<int, int>();

        foreach (KeyValuePair<int,int> kvp in results)
        {
            if (kvp.Value > max.Value)
                max = kvp;
        }
        return max.Key;
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

    void SwapCards(List<GameObject> cardsA, int cardA, List<GameObject> cardsB, int cardB)
    {
        Vector3 tempPos = cardsA[cardA].transform.position;
        cardsA[cardA].transform.position = cardsB[cardB].transform.position;
        cardsB[cardB].transform.position = tempPos;

        GameObject tempCard = cardsA[cardA];
        cardsA[cardA] = cardsB[cardB];
        cardsB[cardB] = tempCard;


       // string tempName = cardA.name;
       // cardA.name = cardB.name;
       // cardB.name = tempName;
    }

    GameObject SetCard(GameObject card)
    {
        return card;
    }
    void DrawCardsFromList(List<GameObject> list, List<int> i)
    {
        for (int j =0; j< list.Count(); j++)
        {
            SwapCards(list,j, all_Cards,i[j]);
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
                if (IsCurrentPlayer(0))
                {
                    playerAMoved = true;
                    playersMoved++;
                    currentPlayer = 1;
                }
                break;
            case 1:
                if (IsCurrentPlayer(1))
                {
                    playerBMoved = true;
                    playersMoved++;
                    currentPlayer = 2;
                }
                break;
            case 2:
                if (IsCurrentPlayer(2))
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
        Debug.LogWarning("ID: " + id);

        if (id < 10 && id >= 0)
        {
            if (IsCurrentPlayer(0))
            {
                SwapCards(player_A_Hand,id, stock,0);
            }
            else { Debug.LogError("It is not your turn, A!"); }
        }
        else if (id < 20 && id >= 10)
        {
            if (IsCurrentPlayer(1))
            {
                SwapCards(player_B_Hand,id-10, stock,1);
            }
            else { Debug.LogError("It is not your turn, B!"); }
        }
        else if (id < 30 && id >= 20)
        {
            if (IsCurrentPlayer(2))
            {
                SwapCards(player_C_Hand,id-20, stock,2);
            }
            else { Debug.LogError("It is not your turn, C!"); }
        }
        else { Debug.LogError("idOfButtonTooHigh"); }

    }
}
