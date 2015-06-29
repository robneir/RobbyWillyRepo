using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu_UIManager : MonoBehaviour {

	public Text inputUsername;

    public GameObject mainMenuPanel;
    private Animator mainMenuPanelAnimator;

    public GameObject loginPanel;
    private Animator loginPanelAnimator;

    public GameObject customizationPanel;
    private Animator customizationPanelAnimator;

    public GameObject createAccountPanel;
    private Animator createAccountPanelAnimator;

    public GameObject optionsPanel;
    private Animator optionsPanelAnimator;

	// Use this for initialization
	void Start () {
        mainMenuPanelAnimator = mainMenuPanel.GetComponent<Animator>();
        loginPanelAnimator = loginPanel.GetComponent<Animator>();
        customizationPanelAnimator = customizationPanel.GetComponent<Animator>();
        createAccountPanelAnimator = createAccountPanel.GetComponent<Animator>();
        optionsPanelAnimator = optionsPanel.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void OnGUI () 
	{

	}

    //Create Account Screen
    public void Back_To_Login_Button_Click()
    {
        createAccountPanelAnimator.SetTrigger("Slide_Out");
        loginPanelAnimator.SetTrigger("Slide_In");
    }

    //Login Screen Button Clicks
    public void Login_Button_Click()
    {
        mainMenuPanelAnimator.SetTrigger("Slide_In");
        loginPanelAnimator.SetTrigger("Slide_Out");
    }

    public void Create_Account_Button_Click()
    {
        createAccountPanelAnimator.SetTrigger("Slide_In");
        loginPanelAnimator.SetTrigger("Slide_Out");
    }


    //Main Menu Buttons Clicks
    public void Server_Browser_Button_Click()
    {
        PhotonNetwork.playerName = inputUsername.text;

        if (inputUsername.text == "")
            PhotonNetwork.playerName = GenerateRandomUsername();

        Application.LoadLevel("Game_Scene");
    }

    public void Customizer_Button_Click()
    {
        customizationPanelAnimator.SetTrigger("Slide_In");
        optionsPanelAnimator.SetTrigger("Slide_Out");
    }

    public void Tutorial_Button_Click()
    {
        optionsPanelAnimator.SetTrigger("Slide_Out");
        customizationPanelAnimator.SetTrigger("Slide_Out");
    }

    public void Options_Button_Click()
    {
        optionsPanelAnimator.SetTrigger("Slide_In");
        customizationPanelAnimator.SetTrigger("Slide_Out");
    }

    public void Logout_Button_Click()
    {
        loginPanelAnimator.SetTrigger("Slide_In");
        mainMenuPanelAnimator.SetTrigger("Slide_Out");
    }

    public void Exit_Button_Click()
    {
        Application.Quit();
    }

    //Utility Methods
    string GenerateRandomUsername()
	{
		string x = "User";

		for(int i = 0; i < 6; ++i)
		{
			x += Random.Range (0, 9).ToString();
		}

		return x;
	}
}
