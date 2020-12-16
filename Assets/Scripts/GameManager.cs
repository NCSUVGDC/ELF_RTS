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
    //Whether or not the menu is open
    public bool menuOpened;
    // Start is called before the first frame update
    void Start()
    {

        started = false;
        paused = false;
        fastForwarded = false;
        
    }
    public void GameStart(float length)
    {
        gameSelect.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        started = false;
        paused = false;
        menuOpened = false;
        fastForwarded = false;
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
        dateText.text = date;
        daysText.text = days;
        timeText.text = hourString;
        

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
        if (paused)
        {
            paused = false;
            pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
            if (fastForwarded)
            {
                fastForwardButton.GetComponent<Image>().color = new Color(170, 170, 230);
            }
            else
            {
                playButton.GetComponent<Image>().color = new Color(170, 170, 230);
            }
        }
        else
        {
            paused = true;
            playButton.GetComponent<Image>().color = new Color(255, 255, 255);
            fastForwardButton.GetComponent<Image>().color = new Color(255, 255, 255);
            pauseButton.GetComponent<Image>().color = new Color(170, 170, 230);

        }
    }
    //code run on play button click. plays paused game or cancels fast-forward effect
    public void Play()
    {
        started = true;
        fastForwarded = false;
        paused = false;
        playButton.GetComponent<Image>().color = new Color(170, 170, 230);
        pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
        pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
    }
    //code run of fast forward button click. fast forwards paused or normal speed game, or sets fast-forwarded game to normal speed
    public void FastForward()
    {
        if(paused || !fastForwarded)
        {
            paused = false;
            fastForwarded = true;
            fastForwardButton.GetComponent<Image>().color = new Color(170, 170, 230);
            pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
            playButton.GetComponent<Image>().color = new Color(255, 255, 255);

        }
        else
        {
            fastForwarded = false;
            fastForwardButton.GetComponent<Image>().color = new Color(255, 255, 255);
            pauseButton.GetComponent<Image>().color = new Color(255, 255, 255);
            playButton.GetComponent<Image>().color = new Color(170, 170, 230);
        }
    }
    public void Menu()
    {
        menuOpened = true;
        menu.gameObject.SetActive(true);

    }
    public void Resume()
    {
        menuOpened = false;
        menu.gameObject.SetActive(false);
    }
    public void Quit()
    {
        started = false;
        menu.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        started = false;
    }
    public void Restart()
    {
        started = false;
        menuOpened = false;
        menu.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
        gameSelect.gameObject.SetActive(true);
    }
    public void GamePlay()
    {
        mainMenu.gameObject.SetActive(false);
        gameSelect.gameObject.SetActive(true);
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
