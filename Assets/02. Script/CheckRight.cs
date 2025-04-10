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
            ment.DOText($"{touchObj.name}{particle} ���� ���� ������ �¾ƿ�!", 1f);
            Debug.Log("�����Դϴ�.");
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
            ment.DOText($"{touchObj.name}{particle} ���� ���� ������ �ƴϿ���!", 1f);
            Debug.Log("�����Դϴ�.");
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
            Debug.Log("��� ������Ʈ�� Ȯ���߽��ϴ�.");
        }
    }

    string GetPostPosition_EunNeun(string word)
    {
        if (string.IsNullOrEmpty(word)) return "";

        char lastChar = word[word.Length - 1];
        int unicode = lastChar - 0xAC00;

        if (unicode < 0 || unicode > 11171) return "��";

        int jongseongIndex = unicode % 28;

        return (jongseongIndex == 0) ? "��" : "��";
    }

    IEnumerator AllDone()   //������ ������Ʈ�� Ȯ���� �� ������ �ð��� ������ ����â�� ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(2f);
        SuccessAll.SetActive(true);
    }
}
