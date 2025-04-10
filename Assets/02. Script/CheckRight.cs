using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class CheckRight : MonoBehaviour
{
    public TMP_Text ment;

    int totalCount = 9;
    int healthTotalCount = 4;
    int curruntCount = 0;
    int healthCount = 0;

    public GameObject SuccessAll;

    private void Start()
    {
        
    }

    public void IsItRight()
    {
        GameObject touchObj = TouchObjectDetector.touchObj;
        string particle = GetPostPosition_EunNeun(touchObj.name);
        ment.DOKill();
        ment.text = "";

        if (touchObj.transform.parent.name == "Good")
        {
            AnimationController.Instance.PlayOneTime("Clap");
            Audio_Manager.Instance.PlayEffect(1);
            ment.DOText($"{touchObj.name}{particle} 몸에 좋은 음식이 맞아요!", 1f);
            Debug.Log("정답입니다.");
            healthCount++;
            if (healthCount == healthTotalCount)
            {
                AnimationController.Instance.PlayOneTime("Cheer");
                Audio_Manager.Instance.PlayEffect(3);
                StartCoroutine(AllDone());
            }
        }
        else if (touchObj.transform.parent.name == "Bad")
        {
            AnimationController.Instance.PlayOneTime("SideToSide");
            Audio_Manager.Instance.PlayEffect(2);
            ment.DOText($"{touchObj.name}{particle} 몸에 좋은 음식이 아니에요!", 1f);
            Debug.Log("오답입니다.");
        }
        else
        {
            return;
        }
        touchObj.gameObject.SetActive(false);
        curruntCount++;

        if (curruntCount == totalCount)
        {
            StartCoroutine(AllDone());
            Debug.Log("모든 오브젝트를 확인했습니다.");
        }
    }

    string GetPostPosition_EunNeun(string word)
    {
        if (string.IsNullOrEmpty(word)) return "";

        char lastChar = word[word.Length - 1];
        int unicode = lastChar - 0xAC00;

        if (unicode < 0 || unicode > 11171) return "는";

        int jongseongIndex = unicode % 28;

        return (jongseongIndex == 0) ? "는" : "은";
    }

    IEnumerator AllDone()   //마지막 오브젝트를 확인한 후 적당한 시간이 지나면 성공창을 띄우는 코루틴
    {
        yield return new WaitForSeconds(2f);
        SuccessAll.SetActive(true);
    }
}
