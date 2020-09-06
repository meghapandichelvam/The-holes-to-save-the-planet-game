using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManagerUI : MonoBehaviour
{
    public GameObject VolumeOn;
    public GameObject VolumeOff;
    public GameObject MainMenuImage;
    public GameObject MainMenuUI;
    public GameObject SettingMenuUI;
    public AudioSource volumeval;

     void Start()
    {
        PlayerPrefs.SetInt("Volume", 1);
        SettingMenuUI.SetActive(false);
        MainMenuImage.SetActive(true);
        MainMenuUI.SetActive(true);
        //volumeval = GetComponent<AudioSource>();
    }
    public void Play()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("Story");
    }
    public void Setting()
    {
        Debug.Log("Setting");
        SettingMenuUI.SetActive(true);
        MainMenuImage.SetActive(false);
        MainMenuUI.SetActive(false);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void BackButtonMain()
    {
        SettingMenuUI.SetActive(false);
        MainMenuImage.SetActive(true);
        MainMenuUI.SetActive(true);
    }
    public void VolumeOnClick() //it will toggle to volume off
    {
        VolumeOff.SetActive(true);
        VolumeOn.SetActive(false);
        volumeval.volume = 0f;
        PlayerPrefs.SetInt("Volume", 0);
    }
    public void VolumeOffClick()//it will toggle to volume on
    {
        VolumeOn.SetActive(true);
        VolumeOff.SetActive(false);
        volumeval.volume = 1f;
        PlayerPrefs.SetInt("Volume", 1);
    }

}
