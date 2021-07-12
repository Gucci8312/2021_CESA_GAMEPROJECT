using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//くそ雑魚スクリプト
public class GetSprayMove : MonoBehaviour
{
    public Transform targetObjPos;
    Vector3 targetUpMove;
    Vector3 m_initPos;
    Score m_scoreScript;
    public float upSpeed;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetScoreObj", 3.0f);
        targetUpMove = this.gameObject.transform.position;
        targetUpMove.y += 30.0f;
        m_initPos = this.gameObject.transform.position;
        m_scoreScript = gameObject.GetComponent<Score>();
    }

    void GetScoreObj()
    {
        targetObjPos = GameObject.Find("Score").GetComponent<Transform>();
    }

    private void Update()
    {
        if (m_scoreScript.col)
        {
            //現在位置を更新
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetObjPos.position, speed * 10f);
            if (gameObject.transform.position == targetObjPos.position)
            {
                Destroy(this.gameObject);
            }
        }
    }
    IEnumerator Step1_UpMove()
    {
        while (true)
        {
            if (m_scoreScript.col && gameObject.transform.position != targetUpMove)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetUpMove, upSpeed * 10f);
                if (gameObject.transform.position == targetUpMove)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine("Step2_TargetMove");
                    break;
                }
            }
            yield return null;
        }
    }
}
