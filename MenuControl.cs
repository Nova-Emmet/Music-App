using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Parse;
using System.Threading.Tasks;


using Kudan.AR;

	//Script that controls menu behaviour and navigation.
public class MenuControl : MonoBehaviour {

    public Canvas mainMenu;
    public Canvas arUI;
   // public GameObject video;
    public bool arScreen = false;
    public bool bioScreen = false;
    public GameObject sidebar;
    public GameObject accountSettings;
    public GameObject latest;
    public GameObject myTickets;
    public GameObject help;
    public GameObject login;
    public GameObject signUp;
    public GameObject forgotPassword;
    public GameObject asEmail;
    public GameObject asPassword;
    public GameObject asPayment;
    public GameObject asLanguage;
    public GameObject asSocial;
    public GameObject search;
    public GameObject liveCountDown;
	public GameObject findMarker;
	public KudanTracker _kudanTracker;

	public TrackingMethodMarker _markerTracking;
	public TrackingMethodMarkerless _markerlessTracking;	// The reference to the markerless tracking method that lets the tracker know which method it is using


    ParseUser user;
    GameObject accountSettingsPage = null;
    GameObject latestPage = null;
    GameObject myTicketsPage = null;
    GameObject helpPage = null;
    GameObject loginPage = null;
    GameObject signUpPage = null;
    GameObject forgotPasswordPage = null;
    GameObject asEmailPage = null;
    GameObject asPasswordPage = null;
    GameObject asPaymentPage = null;
    GameObject asLanguagePage = null;
    GameObject asSocialPage = null;
    GameObject searchPage = null;
    Canvas loginCanvas;
    int menuState;
    public Canvas accountSettingsCanvas;

    public AccesVerifier accessVerifier;
    public PerformanceManager perfManager;

    bool loggedIn = false;


	// Use this for initialization
	void Start () {
        accessVerifier = mainMenu.GetComponent<AccesVerifier>();
        perfManager = arUI.GetComponent<PerformanceManager>();
        sidebar.SetActive(false);
        menuState = 0;
        user = new ParseUser();
        if (ParseUser.CurrentUser != null)
        {
            user = ParseUser.CurrentUser;
            Debug.Log(user.Get<string>("username"));
            loggedIn = true;
        }
        else
        {
            EnableLogin();
            Debug.Log("Enable Login.");
        }
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (menuState)
            {
                case 1:
                    menuState = 0;
                    DisableSidebar();
                    break;
                case 2:
                    menuState = 1;
                    DisableAccountSettings();
                    DisableLatest();
                    DisableMyTickets();
                    DisableHelp();
                    break;
                case 3:
                    menuState = 2;
                    DisableEmailSettings();
                    DisablePasswordSettings();
                    DisablePaymentSettings();
                    DisableLanguageSettings();
                    DisableSocialSettings();
                    break;
                default:
                    break;
            }
            Debug.Log("escape");
			if (mainMenu.GetComponent<Canvas>().enabled == false)
            {
				_kudanTracker.ChangeTrackingMethod(_markerlessTracking);    // Change the current tracking method to markerless tracking

				mainMenu.GetComponent<Canvas>().enabled = true;
				arUI.GetComponent<Canvas>().enabled = false;
				liveCountDown.SetActive(false);
				perfManager.UnloadPerformance();
				perfManager.destroyClone ();
				perfManager.destroyImageClone ();
				findMarker.SetActive (false);
				perfManager.destroyMarkerClone ();
				perfManager.SampleRemoveMarker ();
				perfManager.loadingBoolFalse ();
				Debug.Log ("Escape performance");
            }
        }

	}

    public void onPerformanceSelect()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        mainMenu.GetComponent<Canvas>().enabled = false;
        arUI.GetComponent<Canvas>().enabled = true;
        if (perfManager.GetState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.STOPPED)
        {
            perfManager.PlayPerformance();
        }
    }

    public void onPerformanceSelect(string performance)
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        mainMenu.GetComponent<Canvas>().enabled = false;
        arUI.GetComponent<Canvas>().enabled = true;
        perfManager.LoadPerformance(performance);
        if (perfManager.GetState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.STOPPED)
        {
            perfManager.PlayPerformance();
        }
    }

    public void onBackSelect()
    {
        mainMenu.GetComponent<Canvas>().enabled = true;
		_kudanTracker.ChangeTrackingMethod(_markerlessTracking);    // Change the current tracking method to markerless tracking

        arUI.GetComponent<Canvas>().enabled = false;
        liveCountDown.SetActive(false);
        perfManager.UnloadPerformance();
		perfManager.destroyClone ();
		perfManager.destroyMarkerClone ();
		perfManager.destroyImageClone ();
		perfManager.loadingBoolFalse ();
		findMarker.SetActive (false);


	//	video.GetComponent<MediaPlayerCtrl> ().resetID ("");
    }

    public void EnableMainMenu()
    {
        Debug.Log("EnableMainMenu");
        mainMenu.GetComponent<Canvas>().enabled = true;
        login.SetActive(false);
    }


    public void EnableSignUp()
    {
        if (signUpPage == null)
        {
            signUpPage = Instantiate(signUp, mainMenu.transform);

            Debug.Log("new signup");
        }
        else
        {
            signUpPage.SetActive(true);

            Debug.Log("old signup");
        }
        Debug.Log("enabling signup");
    }

    public void DisableSignup()
    {
        signUpPage.SetActive(false);
    }

    public void EnableLogin()
    {
        if (loginPage == null)
        {
            loginPage = Instantiate(login, mainMenu.transform);
        }
        else
        {
            loginPage.SetActive(true);
        }
    }

    public void DisableLogin()
    {
        Debug.Log("DisableLogin");
        loginPage.SetActive(false);
    }

    public void EnableForgotPassword()
    {
        if (forgotPasswordPage == null)
        {
            forgotPasswordPage = Instantiate(forgotPassword, mainMenu.transform);
        }
        else
        {
            forgotPasswordPage.SetActive(true);
        }
    }

    public void DisableForgotPassword()
    {
        forgotPasswordPage.SetActive(false);
    }

    public void EnableSidebar()
    {
        sidebar.SetActive(true);
        sidebar.GetComponent<Sidebar>().SetName();
        menuState = 1;
    }

    public void DisableSidebar()
    {
        sidebar.SetActive(false);
    }

    public void EnableAccountSettings()
    {
        if (accountSettingsPage == null)
        {
            accountSettingsPage = Instantiate(accountSettings, mainMenu.transform);
        }
        else
        {
            accountSettingsPage.SetActive(true);
        }
        Debug.Log("enabling");
        menuState = 2;
    }

    public void DisableAccountSettings()
    {
        accountSettingsPage.SetActive(false);
    }

    public void enableLatest()
    {
        if (latestPage == null)
        {
            latestPage = Instantiate(latest, mainMenu.transform);
        }
        else
        {
            latestPage.SetActive(true);
        }
        Debug.Log("enabling");
        menuState = 2;
    }

    public void DisableLatest()
    {
        latestPage.SetActive(false);
    }

    //May need to destroy previous latestpage and always instantiate new one based on how mytickets page is generated


    public void EnableMyTickets()
    {
        if (myTicketsPage == null)
        {
            myTicketsPage = Instantiate(myTickets, mainMenu.transform);
        }
        else
        {
            myTicketsPage.SetActive(true);
            myTicketsPage.GetComponent<MyTickets>().Init();
        }
        menuState = 2;
    }
    public void DisableMyTickets()
    {
        myTicketsPage.SetActive(false);
    }

    public void EnableHelp()
    {
        if (helpPage == null)
        {
            helpPage = Instantiate(help, mainMenu.transform);
        }
        else
        {
            helpPage.SetActive(true);
        }
        menuState = 2;
    }

    public void DisableHelp()
    {
        //Check is necessary as this method is called when the phones back button is selected and menuState == 2. 
        if (helpPage != null)
        {
            helpPage.SetActive(false);
        }
    }

    public void EnableSearch()
    {
        if (searchPage == null)
        {
            searchPage = Instantiate(search, mainMenu.transform);
        }
        else
        {
            searchPage.SetActive(true);
            searchPage.GetComponent<MyTickets>().Init();
        }
    }

    public void DisableSearch()
    {
        searchPage.SetActive(false);
    }

    public void EnableEmailSettings()
    {
        if (asEmailPage == null)
        {
            asEmailPage = Instantiate(asEmail, mainMenu.transform);
        }
        else
        {
            asEmailPage.SetActive(true);
        }
        asEmailPage.GetComponent<EmailSettings>().SetCurrentEmail();
        menuState = 3;
    }

    public void DisableEmailSettings()
    {
        asEmailPage.SetActive(false);
    }

    public void EnablePasswordSettings()
    {
        if (asPasswordPage == null)
        {
            asPasswordPage = Instantiate(asPassword, mainMenu.transform);
        }
        else
        {
            asPasswordPage.SetActive(true);
        }
        menuState = 3;
    }

    public void DisablePasswordSettings()
    {
        asPasswordPage.SetActive(false);
    }

    public void EnablePaymentSettings()
    {
        if (asPaymentPage == null)
        {
            asPaymentPage = Instantiate(asPayment, mainMenu.transform);
        }
        else
        {
            asPaymentPage.SetActive(true);
        }
        menuState = 3;
    }

    public void DisablePaymentSettings()
    {
        asPaymentPage.SetActive(false);
    }

    public void EnableLanguageSettings()
    {
        if (asLanguagePage == null)
        {
            asLanguagePage = Instantiate(asLanguage, mainMenu.transform);
        }
        else
        {
            asLanguagePage.SetActive(true);
        }
        menuState = 3;
    }

    public void DisableLanguageSettings()
    {
        asLanguagePage.SetActive(false);
    }

    public void EnableSocialSettings()
    {
        if (asSocialPage == null)
        {
            asSocialPage = Instantiate(asSocial, mainMenu.transform);
        }
        else
        {
            asSocialPage.SetActive(true);
        }
        menuState = 3;
    }

    public void DisableSocialSettings()
    {
        asSocialPage.SetActive(false);
    }

    public bool allowAccess(ParseObject performance)
    {
        var query = ParseObject.GetQuery("tickets")
            .WhereEqualTo("performance", performance);

        return false;
    }

    public void LogUserIn()
    {
        accessVerifier.RemoveTickets();
        accessVerifier.GetUserTickets();
    }

    public void SignOut()
    {
        Debug.Log("logging out.");
        ParseUser.LogOutAsync().ContinueWith(t2 =>
        {
            Debug.Log("Logged out successfully: " + !t2.IsFaulted);
            user = ParseUser.CurrentUser;
        });

        EnableLogin();
        Debug.Log("logging out.");
        accessVerifier.RemoveTickets();
    }

    public void setCurrentUser(ParseUser u)
    {
        user = u;
    }
}
	
