using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //length of the game in minutes
    public float gameLength;
    //the amount of in-game time that has passed in minutes
    private float gameTime;
    //the current in-game minute, used for date/time calculations
    private float currentMinute;
    //the current in-game hour in military time
    private int currentHour;
    //the current in-game day to display
    private int currentDay;
    //the current in-game month's id.
    private int currentMonth;
    //array of months to cycle through for date/time display
    private string[] months = { "Jan.", "Feb.", "Mar.", "Apr.", "May", "Jun.", "Jul.", "Aug.", "Sep.", "Oct.", "Nov.", "Dec." };
    //the current in-game year to display
    private int currentYear;
    //the number of days left before the game will end
    private int daysLeft;
    //the amount of time that will elapse every tick
    private float tickTime;
    //the total amount of in-game time in minutes
    private float totalTime;
    //updates per minute
    private float upm;
    //whether the game has begun or not
    private bool started;
    //whether the game is paused or not
    private bool paused;
    //whether the game is being fastforwarded or not
    private bool fastForwarded;
    //the text box with the given date
    public Text dateText;
    //the text box with the number of days until christmas
    public Text daysText;
    //the text box with the current in-game time
    public Text timeText;
    //the text box with the current score vs the quota
    public Text scoreText;
    //the score box in the menu
    public Text menuScore;
    //the score box in the loss screen
    public Text lossScoreText;
    //the year box in the loss screen
    public Text lossYearText;
    //the score box in the win screen
    public Text winScoreText;
    //the year box in the win screen
    public Text winYearText;
    //the score box in the endless screen
    public Text endlessScoreText;
    //the year box in the endless screen
    public Text endlessYearText;
    //the button to play at normal speed
    public Button playButton;
    //the button to play at double speed
    public Button fastForwardButton;
    //the button to pause 
    public Button pauseButton;
    //the game length selection screen
    public Canvas gameSelect;
    //the actual game ui
    public Canvas gameUI;
    //the In-game pause menu
    public Canvas menu;
    //the main menu
    public Canvas mainMenu;
    //the game loss screen
    public Canvas gameLoss;
    //the game win screen
    public Canvas gameWin;
    //the quota met screen for endless mode
    public Canvas gameWinEndless;
    //the story screen that plays on game start
    public Canvas gameStory;
    //the map generator object
    public GameObject generator;
    //the generated map container
    public GameObject mapContainer;
    //Whether or not the menu is open
    private bool menuOpened;
    //the current score of the game
    private float score;
    //the necessary score to hit to win the game
    private float quota;
    //whether or not you're playing in endless mode
    private bool endless;

    //instance of the GameManager to return
    private static GameManager instance;

    /// <summary>
    /// Static method to get the instance of the GameManager
    /// Can be called from anywhere with GameManager.Instance();
    /// </summary>
    /// <returns>The singleton instance of the GameManager</returns>
    public static GameManager Instance()
    {
        return instance;
    }

    /// <summary>
    /// Awake() runs before Start(). 
    /// It happens after the object has been initialized, but before it has been enabled.
    /// </summary>
    private void Awake()
    {
        //Before enabling the object, we need to make sure that there are no other GameManagers.
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        started = false;
        paused = false;
        fastForwarded = false;
        endless = false;
        
    }
    public void GameStart(float length)
    {
        gameSelect.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        gameStory.gameObject.SetActive(true);
        started = false;
        paused = false;
        menuOpened = false;
        fastForwarded = false;
        score = 0;
        quota = length * 100;
        //the game starts at minute 0
        gameTime = 0;
        //the total number of in-game minutes is equal to 364 day * 24 hours/day * 60 minutes/hour
        totalTime = 364 * 24 * 60;
        //the number of updates per minute using FixedUpdate is equal to 50 fps * 60 seconds/minute
        upm = 50 * 60;
        //game length is given
        gameLength = length;
        //the amount of time that elapses every tick is equal to the total in-game time divided by the number of frames per minute times the real-time game length
        tickTime = totalTime / (upm * gameLength);
        //Set all of the in-game time variables to their original values
        currentMinute = 0;
        currentHour = 0;
        currentMonth = 11;
        currentDay = 26;
        currentYear = 2020;
        daysLeft = 364;
        UpdateTextLabels();
        generator.GetComponent<TileMapGenerator>().GameStart();
        
    }
    public void GameContinue()
    {
        gameWin.gameObject.SetActive(false);
        gameWinEndless.gameObject.SetActive(false);
        quota = quota * quota;
        currentMinute = 0;
        gameTime = 0;
        currentHour = 0;
        currentMonth = 11;
        currentDay = 26;
        daysLeft = 364;
        UpdateTextLabels();
        started = true;
        endless = true;
        Play();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(started && !paused && !menuOpened) {
            UpdateTime();
            if (daysLeft == 0)
            {
                Debug.Log(gameTime);
                GameOver();
            }
        }
    }
    //updates the time functionality
    public void UpdateTime()
    {
        float timeChange = tickTime;
        //double time progression if fast forward is active
        if (fastForwarded)
        {
            timeChange *= 2;
        }
        //increased the total gameTime and in-game minute by one tick's worth
        gameTime += timeChange;
        currentMinute += timeChange;
        //if the current minute exceeds 60, wrap the minutes around and increase the current hour by 1
        if(currentMinute >= 60)
        {
            currentMinute -= 60;
            currentHour++;
            //if the current hour exceeds 24, wrap the hours around and increase the current day by 1
            if(currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                daysLeft--;
                //check to see if the month has ended
                int monthDayCount = monthLength(currentMonth);
                if(currentDay > monthDayCount)
                {
                    currentDay = 1;
                    currentMonth++;
                    if(currentMonth >= 12)
                    {
                        currentMonth = 0;
                        currentYear++;
                    }
                }

            }
        }
        UpdateTextLabels();

    }
    //updates the labels with the current time/day
    private void UpdateTextLabels()
    {
        string date;
        string days;
        string suffix;
        string hourString;
        string scoreString;
        //puts the correct number suffix for the current day of the month.
        if(currentDay == 1)
        {
            suffix = "st";
        }
        else if(currentDay == 2)
        {
            suffix = "nd";
        }
        else if(currentDay == 3)
        {
            suffix = "rd";
        }
        else
        {
            suffix = "th";
        }
        //correctly formats the hour string
        if(currentHour == 0)
        {
            hourString = "12:00 am";
        }
        else if(currentHour == 12)
        {
            hourString = "12:00 pm";
        }
        else if(currentHour < 12)
        {
            hourString = currentHour + ":00 am";
        }
        else
        {
            hourString = (currentHour - 12)  + ":00 pm";
        }
        date = months[currentMonth] + " " + currentDay + suffix + ", " + currentYear;
        days = daysLeft + " Days 'Till Christmas!";
        float scoreRound = score;
        float quotaRound = quota;
        //correctly formats the score number to be displayed
        if(score >= 1000000000)
        {
            scoreRound /= 1000000000;
            scoreRound = (float)System.Math.Round(scoreRound, 0);
            scoreString = "Score: " + scoreRound + "b/";
        }
        else if(score >= 1000000)
        {
            scoreRound /= 1000000;
            scoreRound = (float)System.Math.Round(scoreRound, 0);
            scoreString = "Score: " + scoreRound + "m/";
        }
        else if(score >= 1000)
        {
            scoreRound /= 1000;
            scoreRound = (float)System.Math.Round(scoreRound, 0);
            scoreString = "Score: " + scoreRound + "k/";
        }
        else
        {
            scoreString = "Score: " + scoreRound + "/";
        }
        //correctly formats the quota number to be displayed
        if (quota >= 1000000000)
        {
            quotaRound /= 1000000000;
            quotaRound = (float)System.Math.Round(quotaRound, 0);
            scoreString += quotaRound + "b";
        }
        else if (quota >= 1000000)
        {
            quotaRound /= 1000000;
            quotaRound = (float)System.Math.Round(quotaRound, 0);
            scoreString += quotaRound + "m";
        }
        else if (quota >= 1000)
        {
            quotaRound /= 1000;
            quotaRound = (float)System.Math.Round(quotaRound, 0);
            scoreString += quotaRound + "k";
        }
        else
        {
            scoreString += quotaRound;
        }
        dateText.text = date;
        daysText.text = days;
        timeText.text = hourString;
        scoreText.text = scoreString;
        

    }
    //returns the length of the given month in days
    private int monthLength(int month)
    {
        month++;
        if(month == 2)
        {
            if(currentYear % 4 == 0 && (currentYear % 100 != 0 || currentYear % 400 == 0))
            {
                return 29;
            }
            return 28;
        }
        if(month % 2 == 0)
        {
            if(month <= 7)
            {
                return 30;
            }

            return 31;
        }
        else
        {
            if(month <= 7)
            {
                return 31;
            }
            return 30;
        }
    }
    //Code run on pause button click. Pauses an unpaused game, or restores a paused game to the play or fast-forward state.
    public void Pause()
    {
        if (started)
        {
            if (paused)
            {
                paused = false;
                pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
                if (fastForwarded)
                {
                    fastForwardButton.GetComponent<Image>().color = new Color(70, 70, 130);
                }
                else
                {
                    playButton.GetComponent<Image>().color = new Color(70, 70, 130);
                }
            }
            else
            {
                paused = true;
                playButton.GetComponent<Image>().color = new Color(255, 255, 255);
                fastForwardButton.GetComponent<Image>().color = new Color(255, 255, 255);
                pauseButton.GetComponent<Image>().color = new Color(70, 70, 130);

            }
        }
    }
    //code run on play button click. plays paused game or cancels fast-forward effect
    public void Play()
    {
        if (started)
        {
            fastForwarded = false;
            paused = false;
            playButton.GetComponent<Image>().color = new Color(70, 70, 130);
            pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
            pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
        }
    }
    //code run of fast forward button click. fast forwards paused or normal speed game, or sets fast-forwarded game to normal speed
    public void FastForward()
    {
        if (started)
        {
            if (paused || !fastForwarded)
            {
                paused = false;
                fastForwarded = true;
                fastForwardButton.GetComponent<Image>().color = new Color(70, 70, 130);
                pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
                playButton.GetComponent<Image>().color = new Color(255, 255, 255);

            }
            else
            {
                fastForwarded = false;
                fastForwardButton.GetComponent<Image>().color = new Color(255, 255, 255);
                pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
                playButton.GetComponent<Image>().color = new Color(70, 70, 130);
            }
        }
    }
    //Opens the menu when the menu button is pressed
    public void Menu()
    {
        if (started)
        {
            menuOpened = true;
            menu.gameObject.SetActive(true);
            menuScore.text = "Score: " + score + " | Quota: " + quota;
        }

    }
    //Method to run when the return to game option is chosen from the menu
    public void Resume()
    {
        menuOpened = false;
        menu.gameObject.SetActive(false);
    }
    //Method to run when the quit choice is chosen from the menu or the game over screen
    public void Quit()
    {
        started = false;
        menu.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
        gameLoss.gameObject.SetActive(false);
        gameWin.gameObject.SetActive(false);
        gameWinEndless.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
    //Method to run when the daysLeft counter hits 0
    public void GameOver()
    {
        started = false;
        //if your score is under the quota you lose
        if(score < quota)
        {
            gameUI.gameObject.SetActive(false);
            gameLoss.gameObject.SetActive(true);
            lossScoreText.text = "Score: " + score + " | Quota: " + quota;
            lossYearText.text = "Year: " + currentYear;  
        }
        //if your score is greater than or equal to the quota
        else
        {
            //you can choose to continue endless or quit
            if (endless)
            {
                gameWinEndless.gameObject.SetActive(true);
                endlessScoreText.text = "Score: " + score;
                endlessYearText.text = "Year: " + currentYear;
            }
            //You can choose to begin endless or quit
            else
            {
                gameWin.gameObject.SetActive(true);
                winScoreText.text = "Score: " + score;
                winYearText.text = "Year: " + currentYear;
            }
        }

    }
    //Method to run when the restart button is selected from the menu or from the game over screen
    public void Restart()
    {
        started = false;
        menuOpened = false;
        menu.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
        gameLoss.gameObject.SetActive(false);
        gameSelect.gameObject.SetActive(true);
    }
    //method to run when the play game button is selected on the main menu. 
    public void GamePlay()
    {
        mainMenu.gameObject.SetActive(false);
        gameSelect.gameObject.SetActive(true);
    }
    //closes the game
    public void GameQuit()
    {
        Application.Quit();
    }
    //tests winning functionality
    public void Win()
    {
        score = quota;
        GameOver();
    }
    //tests losing functionality
    public void Lose()
    {
        score = 0;
        GameOver();
    }
    //determines if the game is running and no menus or extra screens are opened
    public bool isRunning()
    {
        return (started && !menuOpened);
    }
    //determines if the game is paused
    public bool isPaused()
    {
        return paused;
    }
    //determines if the game is in fast-forward
    public bool isFastForward()
    {
        return fastForwarded;
    }
    //adds the given amount of points to the score
    public void addPoints(int points)
    {
        score += (float)points;
    }
    //method to run when the story window is closed
    public void StartPlaying()
    {
        started = true;
        Play();
        gameStory.gameObject.SetActive(false);
    }
}
