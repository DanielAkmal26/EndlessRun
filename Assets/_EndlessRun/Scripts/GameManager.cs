using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Flow")]
    public GameObject panel;
    public GameObject mainUI;
    public GameObject GameUI;
    public GameObject PauseUI;
    public GameObject GameOverUI;
    public Text gameOverText;
    public Text BuffText;
    private int score;
    public Text scoreText;
    public Animator cameraAnim;
    public Text prepareText;

    [Header("Other Script Relation")]
    public PMovement PMovement;
    public CharacterController charControl;

    [Header("Sound")]
    public Slider sfxSlider;
    public Slider bgmSlider;
    public AudioSource coinPickup;
    public AudioSource capsulePickup;
    public AudioSource BGMMusic;

    [Header("Buff Applier")]
    public bool DoubleCoin = false;

    // Start is called before the first frame update

    void Start()
    {
        if (!PlayerPrefs.HasKey("bgmVolume"))
        {
            PlayerPrefs.SetFloat("bgmVolume", 1);
        }
        else
        { 
            Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DoubleCoin == true)
        {
            StartCoroutine(DoubleCoinTimer());
        }
    }

    public IEnumerator DoubleCoinTimer()
    {
        yield return new WaitForSeconds(2.5f);
        DoubleCoin = false;
        BuffText.text = "..";
    }

    public void gotoLobby()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }

    public void playGame()
    {
        cameraAnim.Play("SwitchPlay");
        mainUI.SetActive(false);
        StartCoroutine(GameStart());
    }

    public IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1f);
        prepareText.text = "READY";
        yield return new WaitForSeconds(1f);
        prepareText.text = "GET SET";
        yield return new WaitForSeconds(1f);
        prepareText.text = "GO!";
        yield return new WaitForSeconds(1f);
        GameUI.SetActive(true);
        prepareText.text = "";
        panel.SetActive(false);
        PMovement.enabled = true;
        charControl.enabled = true;
    }

    public void scoreIncrement()
    {
        score++;
        scoreText.text = score.ToString();
        coinPickup.Play();
    }

    public void CapsulePick()
    {
        capsulePickup.Play();
        DoubleCoin = true;
    }

    public void scoreIncrementBuff()
    {
        BuffText.text = "Double Score";
        score += 2;
        scoreText.text = score.ToString();
        coinPickup.Play();
    }

    public void ChangeVolume()
    {
        coinPickup.volume = sfxSlider.value;
        BGMMusic.volume = bgmSlider.value;
        Save();
    }

    private void Save()
    {
        //PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("bgmVolume", bgmSlider.value);
    }
    private void Load()
    {
        //sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PauseUI.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
    }
    public void GameOver()
    {
        GameOverUI.SetActive(true);
        gameOverText.text = scoreText.text;
    }
}
