using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Score : MonoBehaviour
{
    public GameObject[] Stars;
    public TMP_Text scoreText;

    public AudioClip[] good_bad; // 0: 좋은 음식, 1: 나쁜 음식
    public AudioClip[] foodName; // 음식 이름 오디오 (이름 기준으로 매칭)

    private AudioSource audioSource;

    int totalStar;
    string finalText = "";
    float typingSpeed = 0.01f;

    void OnEnable()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        scoreText.text = "";
        finalText = "";
        calcStar();
        ShowSelectedFoods();
        ResetStars();
        StartCoroutine(FillStar());
    }
    private void OnDisable()
    {
        audioSource.Stop();
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
        Debug.Log("점수가 " + totalStar);
        yield return new WaitForSeconds(1f);

        if (totalStar <= 1)
        {
            AnimationController.Instance.playAnimSetCode(3, () =>
            {
                AnimationController.Instance.PlayOneTime("Cry");
            });
            if (totalStar == 1) popStar(0);
        }
        else if (totalStar == 2)
        {
            AnimationController.Instance.playAnimSetCode(3, () =>
            {
                Audio_Manager.Instance.PlayEffect(3);
                AnimationController.Instance.PlayOneTime("Clap");
            });
            popStar(0);
            yield return new WaitForSeconds(0.5f);
            popStar(1);
        }
        else if (totalStar == 3)
        {
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
            if (!string.IsNullOrEmpty(finalText)) finalText += "\n\n";
            finalText += "<size=130%><b><color=#F44336>좋지 않은 음식</color></b></size>\n";
            finalText += $"<size=100%><color=#444444>{badList}</color></size>";
        }

        if (string.IsNullOrEmpty(finalText))
            finalText = "";
    }

    IEnumerator ShowSelectedFoodsCoroutine()
    {
        yield return new WaitForSeconds(1f);

        // 텍스트 출력
        float textSpeed = finalText.Length * typingSpeed;
        scoreText.DOText(finalText, textSpeed).SetEase(Ease.Linear);

        // 오디오 출력 병렬
        yield return StartCoroutine(PlayResultAudio());
    }

    IEnumerator PlayResultAudio()
    {
        // 좋은 음식
        if (Mission2.goodNames.Count > 0)
        {
            Audio_Manager.Instance.Ment_audioSource.PlayOneShot(good_bad[0]); // "좋은 음식"
            yield return new WaitForSeconds(good_bad[0].length + 0.3f);

            foreach (string name in Mission2.goodNames)
            {
                AudioClip clip = FindClipByName(name);
                if (clip != null)
                {
                    Audio_Manager.Instance.Ment_audioSource.PlayOneShot(clip);
                    yield return new WaitForSeconds(clip.length + 0.2f);
                }
            }
        }

        // 나쁜 음식
        if (Mission2.badNames.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);

            Audio_Manager.Instance.Ment_audioSource.PlayOneShot(good_bad[1]); // "나쁜 음식"
            yield return new WaitForSeconds(good_bad[1].length + 0.3f);

            foreach (string name in Mission2.badNames)
            {
                AudioClip clip = FindClipByName(name);
                if (clip != null)
                {
                    Audio_Manager.Instance.Ment_audioSource.PlayOneShot(clip);
                    yield return new WaitForSeconds(clip.length + 0.15f);
                }
            }
        }
    }

    AudioClip FindClipByName(string name)
    {
        foreach (var clip in foodName)
        {
            if (clip.name == name)
                return clip;
        }
        return null;
    }
}
