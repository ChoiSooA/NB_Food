using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Score : MonoBehaviour
{
    public GameObject[] Stars;
    public TMP_Text scoreText;
    int totalStar;
    string finalText = "";
    float typingSpeed = 0.01f;
    void OnEnable()
    {
        calcStar();
        ShowSelectedFoods();
        ResetStars();
        StartCoroutine(FillStar()); 
    }
    void ResetStars()
    {
        foreach (var star in Stars)
        {
            star.SetActive(false);
        }
    }
    void calcStar()
    {
        if (Mission2.howManyObjects == 0)
        {
            totalStar = 0;
            return;
        }

        float ratio = (float)Mission2.score / Mission2.howManyObjects;
        totalStar = Mathf.RoundToInt(ratio * 3f);
    }

    void popStar(int num)
    {
        Stars[num].SetActive(true);
        Stars[num].transform.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack);
    }

    IEnumerator FillStar()
    {
        
        Debug.Log("점수가 "+totalStar);
        yield return new WaitForSeconds(1f);
        if (totalStar <= 1)
        {
            AnimationController.Instance.playAnimSetCode(3, () => {
                AnimationController.Instance.PlayAnimation("Cry");
            });
            if (totalStar == 1) popStar(0);
            yield break;
        }
        else if (totalStar == 2)
        {
            Debug.Log("2개 별");
            AnimationController.Instance.playAnimSetCode(3, () =>
            {
                Audio_Manager.Instance.PlayEffect(3);
                AnimationController.Instance.PlayAnimation("Clap");
            });
            popStar(0);
            yield return new WaitForSeconds(0.5f);
            popStar(1);
        }
        else if (totalStar == 3)
        {
            Debug.Log("3개 별"); 
            AnimationController.Instance.playAnimSetCode(3, () =>
            {
                Audio_Manager.Instance.PlayEffect(3);
                AnimationController.Instance.PlayAnimation("Dance");
            });
            popStar(0);
            yield return new WaitForSeconds(0.5f);
            popStar(1);
            yield return new WaitForSeconds(0.5f);
            popStar(2);
        }
        StartCoroutine(ShowSelectedFoodsCoroutine());
    }
    void ShowSelectedFoods()
    {
        string goodList = string.Join(", ", Mission2.goodNames);
        string badList = string.Join(", ", Mission2.badNames);

        if (Mission2.goodNames.Count > 0)
        {
            finalText += "<size=130%><b><color=#4CAF50>좋은 음식</color></b></size>\n";
            finalText += $"<size=100%><color=#444444>{goodList}</color></size>";
        }
        if (Mission2.badNames.Count > 0)
        {
            if (!string.IsNullOrEmpty(finalText)) finalText += "\n\n"; // 단락 간격

            finalText += "<size=130%><b><color=#F44336>좋지 않은 음식</color></b></size>\n";
            finalText += $"<size=100%><color=#444444>{badList}</color></size>";
        }
        // 아무것도 없으면 텍스트 비우기
        if (string.IsNullOrEmpty(finalText))
            finalText = "";

    }
    IEnumerator ShowSelectedFoodsCoroutine()
    {
        yield return new WaitForSeconds(1f);
        float textSpeed = finalText.Length * typingSpeed;
        scoreText.DOText(finalText, textSpeed).SetEase(Ease.Linear);
    }
}
