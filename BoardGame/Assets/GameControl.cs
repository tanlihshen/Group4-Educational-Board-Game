using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameControl : MonoBehaviour {

    public Question[] questions;
    private static List<Question> unanswered;

    private Question curQues;

    [SerializeField]
    private Text QuestionText;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Text TrueAnswerText;

    [SerializeField]
    private Text FalseAnswerText;

    [SerializeField]
    private Text ButtonText1;

    [SerializeField]
    private Text ButtonText2;

    [SerializeField]
    private Text ButtonText3;

    [SerializeField]
    private Text ButtonText4;

    [SerializeField]
    private float delay = 1f;

    private static GameObject TextEnd, TextScore ,player1MoveText, player2MoveText, Dice, player1ScoreText, player2ScoreText, EventText, EventDesc, EventTitle, trueanswer;

    private static GameObject player1, player2;

    public GameObject PanelQuiz, PanelEvent, PanelEnd, PanelPause;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;
    public static int TilePoint = 0;
    public static bool TileEventCheck = false;

    public static bool gameOver = false;
    public static bool move = false;
    public static bool back = false;

    public static bool player1turn = false;
    public static bool player2turn = false;
    public static int player1score = 0;
    public static int player2score = 0;

    public int randomScore = 0;

    // Use this for initialization
    void Start () {


        TextEnd = GameObject.Find("TextEnd");
        TextScore = GameObject.Find("TextScore");
        EventText = GameObject.Find("EventText");
        EventDesc = GameObject.Find("EventDescription");
        EventTitle = GameObject.Find("EventTitle");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");
        player1ScoreText = GameObject.Find("player1score");
        player2ScoreText = GameObject.Find("player2score");
        trueanswer = GameObject.Find("TrueAnswer");

        //panel object dice variable
        PanelQuiz = GameObject.Find("PanelQuiz");
        PanelEvent = GameObject.Find("PanelEvent");
        PanelEnd = GameObject.Find("PanelEnd");
        PanelPause = GameObject.Find("PanelPause"); 

        //game object dice variable
        Dice = GameObject.Find("Dice");

        //find both player objects
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        player1.GetComponent<FollowThePath>().backAllowed = false;
        player2.GetComponent<FollowThePath>().backAllowed = false;

        TextEnd.gameObject.SetActive(false);
        TextScore.gameObject.SetActive(false);
        //hide panel when game starts
        PanelQuiz.gameObject.SetActive(false);
        PanelEvent.gameObject.SetActive(false);
        PanelEnd.gameObject.SetActive(false);
        PanelPause.gameObject.SetActive(false);
        //player turn text
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);

        trueanswer.gameObject.SetActive(false);

        //load question into list
        if (unanswered == null || unanswered.Count == 0)
        {
            unanswered = questions.ToList<Question>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("tilecheck: " + TileEventCheck);
        if (!TileEventCheck)
        {
            
            //Debug.Log("waypointindex received: " + player1.GetComponent<FollowThePath>().waypointIndex);
            //stop player movement once destination reached
            if (player1.GetComponent<FollowThePath>().waypointIndex >
                player1StartWaypoint + diceSideThrown)
            {
                player1.GetComponent<FollowThePath>().moveAllowed = false;
                player1MoveText.gameObject.SetActive(false);
                //everytime player 1 gets a turn set bool value to true
                player1turn = true;

                StartEvent1();

                player2MoveText.gameObject.SetActive(true);
                player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;

                SetQuestion();
            }

            if (player2.GetComponent<FollowThePath>().waypointIndex >
                player2StartWaypoint + diceSideThrown)
            {
                //Debug.Log("Enter player 2");
                player2.GetComponent<FollowThePath>().moveAllowed = false;
                player2MoveText.gameObject.SetActive(false);
                //everytime player 1 gets a turn set bool value to true
                player2turn = true;

                StartEvent1();

                player1MoveText.gameObject.SetActive(true);
                player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;

                SetQuestion();
            }
        }

        //tile event
        if (TileEventCheck)
        {
            checkWaypoint();
        }

        //if player 1 or 2 reaches first
        if (player1.GetComponent<FollowThePath>().waypointIndex == 
            player1.GetComponent<FollowThePath>().waypoints.Length || 
            player2.GetComponent<FollowThePath>().waypointIndex ==
            player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            if (PanelEvent.gameObject.activeInHierarchy == true)
            {
                PanelEvent.gameObject.SetActive(false);

            }

            if (PanelQuiz.gameObject.activeInHierarchy == true)
            {
                PanelQuiz.gameObject.SetActive(false);
            }
            //if player 1 has a higher score then player 1 wins
            if (player1score > player2score)
            {

                PanelEnd.gameObject.SetActive(true);
                TextEnd.gameObject.SetActive(true);
                TextEnd.GetComponent<Text>().text = "Player 1 Wins";
                TextScore.gameObject.SetActive(true);
                TextScore.GetComponent<Text>().text = "Player 1: " + player1score + "\n" + "Player 2: " + player2score;
                gameOver = true;
            }
            else {
                PanelEnd.gameObject.SetActive(true);
                TextEnd.gameObject.SetActive(true);
                TextEnd.GetComponent<Text>().text = "Player 2 Wins";
                TextScore.gameObject.SetActive(true);
                TextScore.GetComponent<Text>().text = "Player 2: " + player2score + "\n" + "Player 1: " + player1score;  
                gameOver = true;
            }
        }
    }



    public void StartEvent1()
    {
        EventTitle.GetComponent<Text>().text = "Congratulations!";
        EventDesc.GetComponent<Text>().text = "You've earned points for advancing!";
        EventText.GetComponent<Text>().text = "You've earned " + diceSideThrown + " points!";
        //Debug.Log("try again 2");
        PanelEvent.gameObject.SetActive(true);
        Dice.gameObject.SetActive(false);
    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove) { 
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                //player1.GetComponent<FollowThePath>().backAllowed = true;
                break;

            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                //player1.GetComponent<FollowThePath>().backAllowed = true;
                break;
            default:
                break;
        }
    }

    public void StartEvent2()
    {
        //if player 1 then give 1 point, update score, and then set player1turn to false
        if (player1turn == true)
        {
            player1score = player1score + diceSideThrown;
            player1ScoreText.GetComponent<Text>().text = "Current score:" + player1score;
        }
        else if (player2turn == true)
        {
            player2score = player2score + diceSideThrown;
            player2ScoreText.GetComponent<Text>().text = "Current score:" + player2score;
        }

        Debug.Log("player 1 score: " + player1score);
        Debug.Log("player 2 score: " + player2score);
        PanelEvent.gameObject.SetActive(false);
    }

    public void hidePanel()
    {
        trueanswer.gameObject.SetActive(false);
        PanelQuiz.gameObject.SetActive(false);
    }

    //panel event close
    public void closeEvent()
    {
        Debug.Log("tilecheck:" + TileEventCheck);
        if (!TileEventCheck)
        {
            StartEvent2();
            PanelQuiz.gameObject.SetActive(true);
            Dice.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Close panel");
            
            if (move)
            {
                if (player1turn)
                {
                    player1.GetComponent<FollowThePath>().moveAllowed = true;
                    player1score = player1score + TilePoint;
                    player1ScoreText.GetComponent<Text>().text = "Current score:" + player1score;
                }
                else
                {
                    player2.GetComponent<FollowThePath>().moveAllowed = true;
                    player2score = player2score + TilePoint;
                    player2ScoreText.GetComponent<Text>().text = "Current score:" + player2score;
                }
            }
            else
            {
                if (player1turn)
                {
                    player1.GetComponent<FollowThePath>().waypointIndex--; //reset waypoint index
                    player1.GetComponent<FollowThePath>().backAllowed = true;
                    player1score = player1score + TilePoint;
                    player1ScoreText.GetComponent<Text>().text = "Current score:" + player1score;
                }
                else
                {
                    player2.GetComponent<FollowThePath>().waypointIndex--;
                    player2.GetComponent<FollowThePath>().backAllowed = true;
                    player2score = player2score + TilePoint;
                    player2ScoreText.GetComponent<Text>().text = "Current score:" + player2score;
                }
            }

            PanelEvent.gameObject.SetActive(false);
        }
    }

    void SetQuestion()
    {
        int randIndex = Random.Range(0, unanswered.Count - 1);
        curQues = unanswered[randIndex];

        QuestionText.text = curQues.question;

        SetAnswer();
    }

    void SetAnswer()
    {
        //Generate random question choice
        Random rand = new Random();

        List<string> curChoice = curQues.choice.ToList();

        for (int i = 0; i < curChoice.Count; i++)
        {
            int rnd = Random.Range(i, curChoice.Count);
            string temp = curChoice[rnd];
            curChoice[rnd] = curChoice[i];
            curChoice[i] = temp;
        }

        ButtonText1.text = curChoice[0];
        ButtonText2.text = curChoice[1];
        ButtonText3.text = curChoice[2];
        ButtonText4.text = curChoice[3];
    }

    public void ButtonAction()
    {
        var name = EventSystem.current.currentSelectedGameObject.name;

        animator.SetTrigger("True");

        if (GameObject.Find(name).GetComponentInChildren<Text>().text == curQues.ans)
        {
            ButtonTrueAction();
            TrueAnswerText.text = "Correct";
        }
        else
        {
            ButtonFalseAction();
            TrueAnswerText.text = "Wrong";
        }

        trueanswer.gameObject.SetActive(true);

        StartCoroutine(Transition());

        TileEventCheck = true;
    }

    public void ButtonTrueAction()
    {
        Debug.Log("CORRECT");

        if (player1turn == true)
        {
            player1score = player1score + 1;
            //player1turn = false;
            player1ScoreText.GetComponent<Text>().text = "Current score:" + player1score;
            Debug.Log("player 1 score: " + player1score);
            Debug.Log("player 2 score: " + player2score);
        }

        if (player2turn == true)
        {
            player2score = player2score + 1;
            //player2turn = false;
            player2ScoreText.GetComponent<Text>().text = "Current score:" + player2score;
            Debug.Log("player 1 score: " + player1score);
            Debug.Log("player 2 score: " + player2score);
        }
    }

    public void ButtonFalseAction()
    {
        Debug.Log("WRONG");

        if (player1turn == true)
        {
            player1score = player1score - 1;
            //player1turn = false;
            player1ScoreText.GetComponent<Text>().text = "Current score:" + player1score;
            Debug.Log("player 1 score: " + player1score);
            Debug.Log("player 2 score: " + player2score);
        }

        if (player2turn == true)
        {
            player2score = player2score - 1;
            //player2turn = false;
            player2ScoreText.GetComponent<Text>().text = "Current score:" + player2score;
            Debug.Log("player 1 score: " + player1score);
            Debug.Log("player 2 score: " + player2score);
        }
    }

    public void checkWaypoint()
    {
        if(player1turn)
        {
            //Debug.Log("player waypoint: " + player1StartWaypoint);
            waypointEventPlayer(ref player1StartWaypoint, ref player1, ref player1turn);
        }
        
        if(player2turn)
        {
            waypointEventPlayer(ref player2StartWaypoint, ref player2, ref player2turn);
        }
    }

    public void waypointEventPlayer(ref int waypoint, ref GameObject player, ref bool playerTurn)
    {
        //number of tiles movement/points for tile event
        switch (waypoint)
        {
            case 4:
                TilePoint = 2;
                break;
            case 9:
                TilePoint = -1;
                break;
            case 14:
                TilePoint = -2;
                break;
            case 19:
                TilePoint = -3;
                break;
            case 24:
                TilePoint = 2;
                break;
            case 29:
                TilePoint = -4;
                break;
            default:
                playerTurn = false;
                TileEventCheck = false;
                return;
        }

       // Debug.Log("player waypoint: " + waypoint);
        //start moving forward/backward
        switch (waypoint)
        {
            case 4:
            case 24:
                EventTitle.GetComponent<Text>().text = "Congratulations!";
                EventDesc.GetComponent<Text>().text = "You've earned extra points from tile event!";
                EventText.GetComponent<Text>().text = "You've earned " + TilePoint + " points! \n" + "You've moved " + TilePoint + " tiles forward!";
                if (!move)
                {
                    StartCoroutine(TileEventPanel(true, false));
                }
                break;
            case 9:
            case 14:
            case 19:
            case 29:
                //Debug.Log("Back allowed");
                EventTitle.GetComponent<Text>().text = "Better Luck Next Time!";
                EventDesc.GetComponent<Text>().text = "Oh no! You've lost some points from tile event!";
                EventText.GetComponent<Text>().text = "You've lost " + Mathf.Abs(TilePoint) + " points! \n" + "You've moved " + Mathf.Abs(TilePoint) + " tiles backwards!";
                if (!back)
                {
                    Debug.Log("try again");
                    StartCoroutine(TileEventPanel(false, true)) ;
                }
                break;
            default:
                player.GetComponent<FollowThePath>().moveAllowed = false;
                break;
        }

        waypointEventPoint(TilePoint, ref waypoint, ref player, ref playerTurn);
    }

    public void waypointEventPoint(int point, ref int waypoint, ref GameObject player, ref bool playerTurn)
    {
        if (point > 0) //moving forward
        {
            if (player.GetComponent<FollowThePath>().waypointIndex >
                waypoint + point)
            {
                player.GetComponent<FollowThePath>().moveAllowed = false;
                waypoint = player.GetComponent<FollowThePath>().waypointIndex - 1;
                move = false;
                back = false;
                //Debug.Log("dice" + diceSideThrown);
                playerTurn = false;
                TileEventCheck = false;
            }
        }
        else //moving backward
        {
            if (player.GetComponent<FollowThePath>().waypointIndex <
                waypoint + point)
            {
                player.GetComponent<FollowThePath>().backAllowed = false;
                waypoint = player.GetComponent<FollowThePath>().waypointIndex + 1;
                player.GetComponent<FollowThePath>().waypointIndex += 2; //reset its waypoint to proper value
                move = false;
                back = false;
                playerTurn = false;
                TileEventCheck = false;
            }
        }
    }

    IEnumerator TileEventPanel(bool _move, bool _back)
    {
        move = _move;
        back = _back;

        yield return new WaitForSeconds(delay);

        Dice.gameObject.SetActive(false);
        PanelEvent.gameObject.SetActive(true);
    }

    IEnumerator Transition()
    {
        unanswered.Remove(curQues);

        yield return new WaitForSeconds(delay);

        hidePanel();

        animator.SetTrigger("NoAnswer");
    }
}
