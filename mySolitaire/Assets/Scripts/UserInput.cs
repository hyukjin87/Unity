using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UserInput : MonoBehaviour
{
    public GameObject slot1;
    private Solitaire solitaire;
    //private float timer;
    //private float doubleClickTime = 0.3f;
    //private int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        slot1 = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (clickCount == 1) 
        //{
        //    timer += Time.deltaTime;
        //}
        //if (clickCount == 3) 
        //{
        //    timer = 0;
        //    clickCount = 1;
        //}
        //if (timer > doubleClickTime)
        //{
        //    timer = 0;
        //    clickCount = 0;
        //}

        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //clickCount++;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    Deck();
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    Card(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Top"))
                {
                    Top(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Bottom"))
                {
                    Bottom(hit.collider.gameObject);
                }
            }
        }
    }

    void Deck()
    {
        solitaire.DealFromDeck();
        slot1 = this.gameObject;
    }

    void Card(GameObject seleced)
    {
        print("click on card");
        Debug.Log(seleced);


        if (!seleced.GetComponent<Selectable>().faceUp)
        {
            if(!Blocked(seleced))
            { 
                seleced.GetComponent<Selectable>().faceUp = true;
                slot1 = this.gameObject;
            }
        }
        else if (seleced.GetComponent<Selectable>().inDeckPile)
        {
            if(!Blocked(seleced))
            {
                if(slot1==seleced)
                {
                    if(!AutoStackTop(seleced))
                    {
                        AutoStackBottom(seleced);
                    }

                    //if (DoubleClick())
                    //{
                    //    AutoStack(seleced);
                    //}
                }
                else
                {
                    slot1 = seleced;
                    if (!AutoStackTop(seleced))
                    {
                        AutoStackBottom(seleced);
                    }
                }
            }
        }
        else
        {
            if (slot1 == this.gameObject)
            {
                slot1 = seleced;
                AutoStackTop(seleced);
            }
            else if (slot1 != seleced)
            {
                if (Stackable(seleced))
                {
                    //Stack(seleced);
                    if (!AutoStackTop(seleced))
                    {
                        AutoStackBottom(seleced);
                    }
                }
                else
                {
                    slot1 = seleced;
                    if (!AutoStackTop(seleced))
                    {
                        AutoStackBottom(seleced);
                    }
                }
            }
            else if (slot1==seleced)
            {
                if (!AutoStackTop(seleced))
                {
                    AutoStackBottom(seleced);
                }
                //if (DoubleClick())
                //{
                //    AutoStack(seleced);
                //}
            }
        }        
    }

    void Top(GameObject selected)
    {
        print("Click on the Top");
        if(slot1.CompareTag("Card"))
        {
            if (slot1.GetComponent<Selectable>().value == 1) 
            {
                //Stack(selected);
                if(!AutoStackTop(selected))
                {
                    AutoStackBottom(selected);
                }

            }
        }
    }

    void Bottom(GameObject selected)
    {
        print("Click on the Bottom");
        if(slot1.CompareTag("Card"))
        {
            if (slot1.GetComponent<Selectable>().value == 13) 
            {
                //Stack(selected);
                if (!AutoStackTop(selected))
                {
                    AutoStackBottom(selected);
                }
            }
        }
    }

    bool Stackable(GameObject selected)
    {
        if (slot1 != null & selected != null)
        {
            Selectable s1 = slot1.GetComponent<Selectable>();
            Selectable s2 = selected.GetComponent<Selectable>();

            if (!s2.inDeckPile)
            {
                if (s2.top)
                {
                    if ((s1.suit == s2.suit) || (s1.value == 1 && s2.suit == null))
                    {
                        if (s1.value == s2.value + 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (s1.value == s2.value - 1)
                    {
                        bool card1Red = true;
                        bool card2Red = true;

                        if (s1.suit == "C" || s1.suit == "S")
                        {
                            card1Red = false;
                        }

                        if (s2.suit == "C" || s2.suit == "S")
                        {
                            card2Red = false;
                        }

                        if (card1Red == card2Red)
                        {
                            print("not stackable");
                            return false;
                        }
                        else
                        {
                            print("stackable");
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    void Stack(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
        Debug.Log(s2);
        float yOffset = 0.2f;

        if (s2.top || (!s2.top && s1.value == 13)) 
        {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset, selected.transform.position.z - 0.01f);
        slot1.transform.parent = selected.transform;

        if (s1.inDeckPile)
        {
            solitaire.tripsOnDisplay.Remove(slot1.name);
        }
        else if (s1.top && s2.top && s1.value == 1) 
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = 0;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = null;
        }
        else if (s1.top)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value - 1;
        }
        else
        {
            solitaire.bottoms[s1.row].Remove(slot1.name);
        }

        s1.inDeckPile = false;
        s1.row = s2.row;

        if(s2.top)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = s1.suit;
            s1.top = true;
        }
        else
        {
            s1.top = false;
        }

        slot1 = this.gameObject;
    }

    bool Blocked(GameObject selected)
    {
        Selectable s2 = selected.GetComponent<Selectable>();
        if(s2.inDeckPile==true)
        {
            if(s2.name==solitaire.tripsOnDisplay.Last())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if(s2.name==solitaire.bottoms[s2.row].Last())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    //bool DoubleClick()
    //{
    //    if(timer<doubleClickTime&&clickCount==2)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    bool AutoStackTop(GameObject selected)
    {
        bool stackCheck = false;
        for (int i = 0; i < solitaire.topPos.Length; i++)
        {
            Selectable stack = solitaire.topPos[i].GetComponent<Selectable>();
            if (selected.GetComponent<Selectable>().value == 1)
            {
                if (solitaire.topPos[i].GetComponent<Selectable>().value == 0)
                {
                    slot1 = selected;
                    Stack(stack.gameObject);
                    stackCheck = true;
                    break;
                }
            }           
            else
            {
                if ((solitaire.topPos[i].GetComponent<Selectable>().suit == slot1.GetComponent<Selectable>().suit)
                    && (solitaire.topPos[i].GetComponent<Selectable>().value == slot1.GetComponent<Selectable>().value - 1))
                {
                    if (HasNoChildren(slot1))
                    {
                        slot1 = selected;
                        string lastCardName = stack.suit + stack.value.ToString();
                        if (stack.value == 1)
                        {
                            lastCardName = stack.suit + "A";
                        }
                        if (stack.value == 11)
                        {
                            lastCardName = stack.suit + "J";
                        }
                        if (stack.value == 12)
                        {
                            lastCardName = stack.suit + "Q";
                        }
                        if (stack.value == 13)
                        {
                            lastCardName = stack.suit + "K";
                        }
                        GameObject lastCard = GameObject.Find(lastCardName);
                        Stack(lastCard);
                        stackCheck = true;
                        break;
                    }
                }                
            }       
        }
        return stackCheck;
    }

    void AutoStackBottom(GameObject selected)
    {
        if (selected.GetComponent<Selectable>().value == 13)
        {
            for (int j = 0; j < solitaire.bottomPos.Length; j++)
            {
                if (solitaire.bottomPos[j].transform.childCount == 0)
                {
                    slot1 = selected;
                    Stack(solitaire.bottomPos[j].gameObject);
                    break;
                }
            }
        }
        else
        {
            for (int k = 0; k < solitaire.bottomPos.Length; k++)
            {
                Selectable stack = solitaire.bottomPos[k].GetComponentsInChildren<Selectable>().Last();
                if (stack.faceUp == true && stack.GetComponent<Selectable>().value == slot1.GetComponent<Selectable>().value + 1)
                {
                    slot1 = selected;
                    string lastCardName2 = stack.suit + stack.value.ToString();
                    if (stack.value == 1)
                    {
                        lastCardName2 = stack.suit + "A";
                    }
                    if (stack.value == 11)
                    {
                        lastCardName2 = stack.suit + "J";
                    }
                    if (stack.value == 12)
                    {
                        lastCardName2 = stack.suit + "Q";
                    }
                    if (stack.value == 13)
                    {
                        lastCardName2 = stack.suit + "K";
                    }
                    GameObject lastCard2 = GameObject.Find(lastCardName2);
                    if (Stackable(lastCard2) == true)
                    {
                        if (CheckLastCard(lastCard2))
                        {
                            Stack(lastCard2);
                            break;
                        }
                    }
                }
            }
        }
    }

    bool HasNoChildren(GameObject card)
    {
        int i = 0;
        foreach(Transform child in card.transform)
        {
            i++;
        }
        if(i==0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckLastCard(GameObject card)
    {
        int count = 0;
        for (int i = 0; i < card.transform.childCount; i++)
        {
            count++;
        }
        if (count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
