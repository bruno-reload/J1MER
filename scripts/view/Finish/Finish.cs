using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public Player p1;
    public Player p2;
    public List<Player> list;
    public bool endGame;

    public GameManager gameManager;

    public int maxTurns = 3;
    public void CountStart()
    {
        p1.pData.countStart++;
    }
    public void CountEnd()
    {
        p1.pData.countEnd++;

        if (p1.pData.countEnd == p1.pData.countStart)
        {
            gameManager.GetComponent<GameManager>().ActiveGroup();
            gameManager.GetComponent<GameManager>().LoadEllements();
            gameManager.GetComponent<GameManager>().PositionRandomDraw();
            gameManager.GetComponent<GameManager>().Write();

            if (maxTurns == p1.pData.countEnd)
            {
                Debug.Log("terminou");
                if (p2.pData.matchPosition > 0)
                {
                    p1.pData.matchPosition = 2;
                    p1.pData.endGame = true;
                    endGame = true;
                }
                else
                {
                    p1.pData.matchPosition = 1;
                    endGame = true;
                }
            }
        }
    }
    private void Update()
    {
        Debug.Log("p1" + p1.pData.matchPosition);
        Debug.Log("p2" + p2.pData.matchPosition);
        if (p1.pData.matchPosition > 0 && endGame == true)
        {
            endGame = false;
            gameManager.SaveElements(p1.pData);
        }
    }
}
