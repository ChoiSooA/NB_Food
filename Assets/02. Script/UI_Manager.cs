using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LeiaUnity;

public class UI_Manager : MonoBehaviour
{
    public LeiaDisplay display;
    public Button BT_Option;
    public Button BT_Exit;
    public Button BT_Restart;
    public Button BT_Close;
    public Toggle BT_3D;
    public Toggle BT_BGM_Sound;
    public Slider Slider_BGM_Sound;

    public DoTEffect Setting_Effect;    //setting에 사용할 효과
    Audio_Manager audioManager;

    public GameObject Option_Panel;

    Button[] buttons;


    bool is3D = true;
    bool isMute = false;

    void Awake()
    {
        display = FindObjectOfType<LeiaDisplay>();
        buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => audioManager.PlayEffect(0));
        }
        audioManager = FindAnyObjectByType<Audio_Manager>();

        audioManager.Current_BGM_Volume = Slider_BGM_Sound.value;
        BT_Option.onClick.AddListener(() =>
        {
            Option_Panel.SetActive(true);
        });
        BT_Exit.onClick.AddListener(() =>
        {
            Quit();
        });
        BT_Restart.onClick.AddListener(() =>
        {
            Restart();
        });
        BT_Close.onClick.AddListener(() =>
        {
            Setting_Effect.Close();
        });
        BT_3D.onValueChanged.AddListener((bool value) =>
        {
            audioManager.PlayEffect(0);
            OnOff3D();
        });
        BT_BGM_Sound.onValueChanged.AddListener((bool value) =>
        {
            audioManager.PlayEffect(0);
            OnOffBGM();
        });
        Slider_BGM_Sound.onValueChanged.AddListener((float value) =>
        {
            SetBgmVolume();
        });
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Setting_Effect.Close();
    }
    void OnOff3D()
    {
        is3D = !is3D;
#if UNITY_EDITOR
        Debug.Log("3D모드 : " + is3D);
#endif
        display.Set3DMode(is3D);
    }

    private void OnOffBGM()
    {
        isMute = !isMute;
        audioManager.SetMute(isMute);
        if (!isMute)
        {
            Slider_BGM_Sound.value = audioManager.Current_BGM_Volume;
        }
        else { Slider_BGM_Sound.value = 0; }
    }

    void SetBgmVolume()
    {
        Audio_Manager.Instance.SetBGMVolume(Slider_BGM_Sound.value);
        if (Slider_BGM_Sound.value != 0)
        {
            BT_BGM_Sound.isOn = false;
        }
        else
        {
            BT_BGM_Sound.isOn = true;
        }
    }
}
