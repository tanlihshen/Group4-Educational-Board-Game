using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour {

    private static GameObject DiceObj;

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whosTurn = 1;
    private bool coroutineAllowed = true;

    public AudioClip sound;
    private AudioSource source { get { return GetComponent<AudioSource>(); } }

	// Use this for initialization
	private void Start () {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];

        DiceObj = GameObject.Find("Dice");

        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
	}

    void playSound() 
    {
        source.PlayOneShot(sound);
    }

    private void OnMouseDown()
    {

        playSound();

        if (!GameControl.gameOver && coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log(randomDiceSide);
        GameControl.diceSideThrown = randomDiceSide + 1;
        if (whosTurn == 1)
        {
            yield return new WaitForSeconds(0.5f);
            DiceObj.gameObject.SetActive(false);
            GameControl.MovePlayer(1);
        } else if (whosTurn == -1)
        {
            yield return new WaitForSeconds(0.5f);
            DiceObj.gameObject.SetActive(false);
            GameControl.MovePlayer(2);
        }
        whosTurn *= -1;
        coroutineAllowed = true;
    }
}
