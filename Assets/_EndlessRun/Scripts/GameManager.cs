using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Flow")]
    public GameObject panel;
    public GameObject mainUI;
    public Animator cameraAnim;
    public Text prepareText;

    [Header("Other Script Relation")]
    public PlayerMovement PMovement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        prepareText.text = "";
        panel.SetActive(false);
        PMovement.enabled = true;
    }
}
