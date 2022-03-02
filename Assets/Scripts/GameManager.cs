using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<PlayerController> playerControllers;

    [SerializeField]
    LaneManager laneManager;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;
    [SerializeField] Transform playersPos;
    [SerializeField] int startingLife = 3;
    int currentLifes;

    [SerializeField] float timeLimitPerLife;

    Vector3 InitialPos;
    int currentPlayer = 0;
    int maxHeightReached = 0;
    int score = 0;
    float time = 0;

    private void Awake()
    {
        SetupTexts();
    }

    void SetupTexts()
    {
        currentLifes = startingLife;
        livesText.text = currentLifes.ToString();
        scoreText.text = score.ToString();
    }

    private void Start()
    {
        InitialPos = laneManager.GetStarter().GetLanePosition();

        ActivatePlayer(currentPlayer);
    }

    void OnVerticalMove(float dir)
    {
        Debug.Log("Moved V");
        ILane lane;
        if(dir > 0)//Up
        {
            lane = laneManager.GoUp();
            
        }
        else//Down
        {
            lane = laneManager.GoDown();
        }
        playerControllers[currentPlayer].MoveVertical(lane.GetYPos());

        if (laneManager.CurrentIndex > maxHeightReached)
        {
            maxHeightReached = laneManager.CurrentIndex;
            score += 10;
            scoreText.text = score.ToString();
            if (laneManager.CurrentIndex == laneManager.LanesCount - 1)
            {
                DeActivateCurrentPlayer();
                currentPlayer++;
                if (currentPlayer < playerControllers.Count)
                {
                    ActivatePlayer(currentPlayer);
                }
                else
                {
                    //win
                    winText.SetActive(true);
                }
            }
        }
    }

    
    void OnPlayerDeath()
    {
        if (currentLifes <= 0) return;
        currentLifes--;
        Debug.Log("Player Dead");
        laneManager.ResetIndex();
        livesText.text = currentLifes.ToString();
        playerControllers[currentPlayer].transform.parent = null;
        playerControllers[currentPlayer].transform.position = InitialPos;
        time = 0;
        if(currentLifes <= 0)
        {
            loseText.SetActive(true);
            for (int i = 0; i < playerControllers.Count; i++)
            {
                playerControllers[i].DeActivatePlayer();
            }
            Debug.Log("Lost The Game");
        }
    }

    void ActivatePlayer(int index)
    {
        Debug.Log("ACTIVATED PLAYER");
        time = 0;
        laneManager.ResetIndex();
        maxHeightReached = 0;
        playerControllers[currentPlayer].transform.position = InitialPos;
        playerControllers[index].ActivatePlayer();
        playerControllers[currentPlayer].onVerticalMove = null;
        playerControllers[currentPlayer].onDeath = null;
        playerControllers[index].onVerticalMove += OnVerticalMove;
        playerControllers[index].onDeath += OnPlayerDeath;
    }
    void DeActivateCurrentPlayer()
    {
        playerControllers[currentPlayer].DeActivatePlayer();
        playerControllers[currentPlayer].onVerticalMove -= OnVerticalMove;
        playerControllers[currentPlayer].onDeath -= OnPlayerDeath;
    }

    public void ResetGame()
    {
        loseText.SetActive(false);
        winText.SetActive(false);

        for (int i = 0; i < playerControllers.Count; i++)
        {
            playerControllers[i].DeActivatePlayer();
            playerControllers[i].transform.parent = playersPos;
            playerControllers[i].transform.localPosition = Vector3.up * i;
        }
        currentLifes = startingLife;
        currentPlayer = 0;
        score = 0;
        ActivatePlayer(currentPlayer);
        laneManager.ResetIndex();
        SetupTexts();
    }

    private void Update()
    {
        if (currentLifes <= 0) return;
        time += Time.deltaTime;
        timeText.text = Mathf.Round(timeLimitPerLife - time).ToString();
        if(time > timeLimitPerLife)
        {
            OnPlayerDeath();
        }
    }
}
