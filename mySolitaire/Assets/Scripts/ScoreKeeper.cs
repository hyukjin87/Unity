using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Selectable[] topStacks;
    public GameObject highScorePanel;
    public float playTime;
    public Text textTime;
    public Text recordTime;
    private float bestTime;

    // Start is called before the first frame update
    void Start()
    {
        playTime = 0;
        bestTime = 0;
    }


    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;
        textTime.text = "Time : " + Mathf.Round(playTime);
        if(HasWon())
        {
            Win();
        }
    }

    public bool HasWon()
    {
        int i = 0;
        foreach(Selectable topStack in topStacks)
        {
            i += topStack.value;
        }
        if(i>=52)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Win()
    {        
        if(playTime>bestTime)
        {
            bestTime = playTime;
            recordTime.text = "Best Record: "+ Mathf.Round(bestTime);
        }
        highScorePanel.SetActive(true);
    }
}
