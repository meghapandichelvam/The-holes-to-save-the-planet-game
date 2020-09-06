using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;
public class DialogueGameComplete: MonoBehaviour
{
   
    public Animator AnimP;
    public Animator AnimK;
    public Text textDisplay;
    public GameObject DialogueMenu;
   // public GameObject Character;
    public GameObject GameCompleteImage;
    public GameObject GGameCompleteUI;
    private Color dialogueColor;
    private JsonData dialogue;
    private int index;
    private string speaker;

    private bool inDialogue;

    //Game complete

    public bool LoadDialogue(string path)
    {
        if (!inDialogue)
        {
            index = 0;
            var jsonTextFile = Resources.Load<TextAsset>("Dialogue/" + path);
            dialogue = JsonMapper.ToObject(jsonTextFile.text);
            inDialogue = true;
            return true;
        }
        return false;
    }

    public bool  PrintLine()
    {
        if (inDialogue)
        {
            JsonData line = dialogue[index];
            if (line[0].ToString() == "EOD")
            {
               
                inDialogue = false;
                DialogueMenu.SetActive(false);
                //Character.SetActive(false);
                GameCompleteImage.SetActive(true);
                GGameCompleteUI.SetActive(true);
                textDisplay.text = "";
                return false;
            }
            foreach (JsonData key in line.Keys)
                speaker = key.ToString();
            DialogueTextColor();
            textDisplay.text = speaker + ": " + line[0].ToString();
            index++;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameCompleteImage.SetActive(false);
        GGameCompleteUI.SetActive(false);
        //DialogueMenu.SetActive(true);
       // Character.SetActive(false);
        LoadDialogue("Scene/DialogueGameComplete");
        InvokeRepeating("DialogueFlow", 1f, 5f);
    }

    // Update is called once per frame
  
    public void DialogueFlow()
    {
        PrintLine();
        //ield return new WaitForSeconds(0.5f);
    }
    private void DialogueTextColor()
    {
        if (speaker == "Minister")
        {
            textDisplay.color = Color.green;
            AnimP.SetBool("IsSpeakingP", true);
            AnimK.SetBool("IsSpeakingK", false);
       
        }
        else if (speaker == "Surya The King")
        {
          
            textDisplay.color = Color.yellow;
            AnimP.SetBool("IsSpeakingP", false);
            AnimK.SetBool("IsSpeakingK", true);
        }else
        {
            AnimP.SetBool("IsSpeakingP",false);
            textDisplay.color = Color.white;
            AnimK.SetBool("IsSpeakingK", false);
        }
        //textDisplay.color = GameObject.FindGameObjectWithTag(speaker).GetComponent<CharacterColor>().GetDialogueColor();
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void Restart()
    {
        Debug.Log("Re-Play");
        SceneManager.LoadScene("Main");
    }
    public void Menu()
    {
        Debug.Log("Menu");
        SceneManager.LoadScene("Menu");
    }
}
