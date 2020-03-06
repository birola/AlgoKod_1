using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
    public GameObject Correct;
    public GameObject InCorrect;
    private int counter=0;
    public static int playerScore = 0;
    private static string playerScoreKey = "PLAYER_SCORE";
    int _score_text = 0, scorecount = 0;
    public Text scoreLabel2;
    public GameObject[] animals;
    public GameObject[] blackAnimals;
    public GameObject[] location;
    Vector3[] animalStartPos;
    Vector3[] animalBlackStartPos;
    public GameObject[] sabit;
    public GameObject[] images;
    public GameObject[] places;
    public GameObject pnl_oyun_sonu;
    public bool[] controlObje;
    public Text oyun_sonu_skor;
    public Text score_oyun_sonu;
    
    public Text sure;
    public int count = 0;
    public int length;

    CountDown cd;


    public void Awake()
    {
        //PlayerPrefs.DeleteAll();
        _score_text = PlayerPrefs.GetInt(playerScoreKey, 0);
        scoreLabel2.text = "" + _score_text;
    }

    public void DeleteScore()
    {
        PlayerPrefs.DeleteAll();
    }
    void Start()
    {

        PlayerPrefs.SetInt(playerScoreKey, _score_text);
        scoreLabel2.text = "" + _score_text;

        EventTrigger trigger = GetComponent<EventTrigger>();
        animalStartPos = new Vector3[animals.Length];
        animalBlackStartPos = new Vector3[blackAnimals.Length];
        for (int i = 0; i < animals.Length; i++)
        {
            animalStartPos[i] = animals[i].transform.position;
           // animalStartPos[i] = new Vector3(100, 100, 0);
            animalBlackStartPos[i] = blackAnimals[i].transform.position;
        }

        controlObje = new bool[animals.Length];
        for (int i = 0; i < animals.Length; i++)
        {
            controlObje[i] = false;
        }

        cd = GetComponent<CountDown>();
        length = animals.Length;
    }

    private void Update()
    {
        if (_score_text < 0 )
        {
            _score_text = 0;
            scoreLabel2.text = "" + _score_text;
        }

        sureDoldu();
        tebrikler();

        if (count == animals.Length)
        {
            pnl_oyun_sonu.SetActive(true);
            Correct.SetActive(false);
            InCorrect.SetActive(false);
            
        }

       
    }


    public void dragAnimal(GameObject animal)
    {
        if (cd.sure_bitti == true)
        {
            return;
        }

        int index = System.Array.IndexOf(animals, animal);
        if (controlObje[index] == false)
        {
            animal.transform.position = Input.mousePosition;
            images[index].SetActive(true);

            // animal.transform.parent = images[index].transform;
            //animal.transform.parent = GameObject.Find("hedef").transform;
            animal.transform.parent = location[index].transform;
        }

        
        
        //for(int i = 0; i < 5; i++)
        //{
        //    images[i].SetActive(true);
        //}



    }
    public void dropAnimal(GameObject animal)
    {
        if (cd.sure_bitti == true)
        {
            return;
        }

        Correct.SetActive(false);
        InCorrect.SetActive(false);
        int index = System.Array.IndexOf(animals, animal);
        float dist = Vector3.Distance(animal.transform.position, blackAnimals[index].transform.position);

        if (controlObje[index] == true)
        {
            return;
        }

        if (dist < 20)
        {
           
            animal.transform.position = blackAnimals[index].transform.position;
            // yerlesti[0] = true;
            counter++;
            _score_text = _score_text + playerScore;

            _score_text = _score_text + 100;
            scoreLabel2.text = "" + _score_text;
            scorecount += 100;
            wait(Correct);
            Correct.SetActive(true);

            count++;

            
            
            PlayerPrefs.SetInt(playerScoreKey, _score_text);
            PlayerPrefs.SetInt("scorecount", scorecount);
            //int scorenext=2900;
            //scorecount = scorecount + scorenext;
            //if (scorecount==2900 || scorecount == 5800 || scorecount == 8700 || scorecount == 11600 || scorecount == 14500 || scorecount == 17400 || scorecount == 20300)
            //{
            //    SceneManager.LoadScene("organlar");
            //}
            Debug.Log(counter);

            controlObje[index] = true;

            DeleteScore();
        }

        //else if(animal.transform.position == blackAnimals[index].transform.position)
        //    {

        //}

        //else animal.transform.position = animalStartPos[index];
        else {
            animal.transform.position = sabit[index].transform.position;
            animal.transform.parent = places[index].transform;
            _score_text = _score_text - 100;
            scorecount -= 100;
            scoreLabel2.text = "" + _score_text;
            wait(InCorrect);
            InCorrect.SetActive(true);
            

            PlayerPrefs.SetInt(playerScoreKey, _score_text);
            PlayerPrefs.SetInt("scorecount", scorecount);
            //    animal.transform.parent = GameObject.Find("hedef").transform;

            controlObje[index] = false;

            DeleteScore();
        }

        //Correct.SetActive(false);
        //InCorrect.SetActive(false);
        images[index].SetActive(false);
        
    }
    IEnumerator wait(GameObject correctT)
    {
        
        yield return new WaitForSeconds(2f);
        
    }

    public void sureDoldu()
    {
        oyun_sonu_skor.text = scoreLabel2.text;

    }
    public void tebrikler()
    {
        score_oyun_sonu.text = scoreLabel2.text;
        sure.text = cd.timeLeft.ToString();
    }
}


