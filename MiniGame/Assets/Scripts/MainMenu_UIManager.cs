using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu_UIManager : MonoBehaviour {

	public Text inputUsername;

    public GameObject mainMenuPanel;

    public GameObject loginPanel;

    public GameObject customizationPanel;

    public GameObject createAccountPanel;

    public GameObject optionsPanel;

    public GameObject serverBrowserPanel;

	// Use this for initialization
	void Start () {

    }
	
	void ActivateUIElement(GameObject element)
    {
        element.GetComponent<UIElement>().SetElementState(UIElement.UIElementState.activated);
    }

    void DeactivateUIElement(GameObject element)
    {
        element.GetComponent<UIElement>().SetElementState(UIElement.UIElementState.disabled);
    }

    //Login Screen Button Clicks
    public void Login_Button_Click()
    {
        ActivateUIElement(mainMenuPanel);
        DeactivateUIElement(loginPanel);
    }

    public void Create_Account_Button_Click()
    {
        DeactivateUIElement(loginPanel);
        ActivateUIElement(createAccountPanel);
    }

    //Main Menu Buttons Clicks
    public void Server_Browser_Button_Click()
    {
        PhotonNetwork.playerName = inputUsername.text;

        if (inputUsername.text == "")
            PhotonNetwork.playerName = GenerateRandomUsername();

        ActivateUIElement(serverBrowserPanel);
        DeactivateUIElement(optionsPanel);
        DeactivateUIElement(customizationPanel);
    }

    public void Customizer_Button_Click()
    {
        ActivateUIElement(customizationPanel);
        DeactivateUIElement(optionsPanel);
        DeactivateUIElement(serverBrowserPanel);
    }

    public void Tutorial_Button_Click()
    {

    }

    public void Options_Button_Click()
    {
        ActivateUIElement(optionsPanel);
        DeactivateUIElement(customizationPanel);
        DeactivateUIElement(serverBrowserPanel);
    }

    public void Logout_Button_Click()
    {
        ActivateUIElement(loginPanel);
        DeactivateUIElement(mainMenuPanel);
        DeactivateUIElement(createAccountPanel);
        DeactivateUIElement(optionsPanel);
        DeactivateUIElement(customizationPanel);
        DeactivateUIElement(serverBrowserPanel);
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
