//This script is a property of Tech & Pro Company 
//Written by Anis Brachemi Meftah

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using GooglePlayGames;
using TMPro;

public class MoveOnTrack : MonoBehaviour {


    int last_currentPoint;
    int first_currentpoint;
    bool animatedpanel = false;
    bool spawnedrigidbodies = false;
    private TimeSpan maxDuration = TimeSpan.FromSeconds(MAX_TIME_TO_CLICK);
    private TimeSpan minDuration = TimeSpan.FromSeconds(MIN_TIME_TO_CLICK);
    private System.Diagnostics.Stopwatch timer;
    private bool ClickedOnce = false;
    private bool ClickedTwice = false;
    private float lastClick = 0;
    bool StopInstantiate;
    int SpawnEnemies = 0;
    bool canJump = true;
    bool startScreen = true;
    bool tryAgain;
    int sound;
    int TapCount;
    float NewTime;
    Color startColor;
    float first_speed;
    int nMode;
    GameObject go;
    bool NightModeFromStore;
    int deathCount;

    [Header("Ids")]
    public string iosgameId;
    public string andgameId;
    public string leaderboardIOSId;
    public string leaderboardAndroidId;

    [Header("Positions")]
    public Transform Pos1;
    public Transform Pos2;
    public Transform Pos3;
    public Transform Pos4;
    public Transform[] path;
    public int currentPoint;
    public Transform InitialPos;

    [Header("Prefabs")]
    public GameObject EnemyPref;
    public GameObject Thecube;
    public GameObject Themaincamera;
    public GameObject rigidbodies;
    public GameObject CoinPref;
    public GameObject[] Items;

    [Header("Number variables")]
    public float MoveSpeed;
    public float jumpSpeed;
    public float jumpTime;
    public float speed = 5.0f;
    public float elevation = 1.0f;
    public float incerement = 1.0001f;
    public float reachdist = 0.10f;
    public int score;
    public float lerptime = 1;
    public int NightValue = 30;
    public int NightTime = 4;
    public int money = 0;
    public int highScore;
    public const double MAX_TIME_TO_CLICK = 0.5;
    public const double MIN_TIME_TO_CLICK = 0.05;
    public float MaxDubbleTapTime = 0.5f;
    public float uiBaseScreenHeight = 302;
    public int i = 0;
    public int ic = 0;

    [Header("Booleans")]
    public bool testMode;
    public bool deleteplayerprefs;
    public bool jumping;
    public bool nightMode;
    public bool dead;
    public bool grounded;
    public bool pausedGame;
    public bool StoreActive;
    public bool IsDebug { get; set; }
    public bool emptyarray = false;
    public bool emptycoinarray = false;
    public bool ShowStore;
    public bool authenticated;

    [Header("UI Elements")]
    public TextMeshProUGUI Score;
    public GameObject panel;
    public GameObject PausePanel;
    public GameObject StorePanel;
    public GameObject mainMenuPanel;
    public GameObject startTxt;
    public GameObject Pausebtn;
    public TextMeshProUGUI Panel;
    public Text[] texts;
    public GameObject RetryButton;
    public GameObject ContinueButton;
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject playStart;
    public TextMeshProUGUI moneyTxt;

    [Header("Sounds")]
    public AudioClip JumpSound;
    public AudioClip CoinPickSound;
    public AudioClip LostSound;

    [Header("Other")]
    public Texture2D pauseImage;
    public GUISkin MySkin;
    public Renderer Child;
    public string[] _scores;




    void Start() {
        if (deleteplayerprefs) {
            PlayerPrefs.DeleteAll();
        }
        first_currentpoint = currentPoint;
        Score.gameObject.SetActive(false);
        highScore = PlayerPrefs.GetInt("HighScore");
        money = PlayerPrefs.GetInt("money");
        sound = PlayerPrefs.GetInt("sound");
        panel.SetActive(false);
        first_speed = speed;
        TapCount = 0;
        Pausebtn.SetActive(false);
        startColor = Themaincamera.GetComponent<Camera>().backgroundColor;
        if (sound != 0) {
            FindObjectOfType<Camera>().gameObject.GetComponent<AudioSource>().mute = true;
            GetComponent<AudioSource>().mute = true;
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }

        foreach (GameObject item in Items) {
            item.SetActive(false);
        }
        int intvalueofactiveobject = PlayerPrefs.GetInt("box");
        Items[intvalueofactiveobject].SetActive(true);


        SendHighScore();
    }


    bool sentuser;
    public void Enable() {
        startScreen = false;
        mainMenuPanel.GetComponent<Animator>().SetTrigger("out");
        Pausebtn.SetActive(true);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Time.timeScale = 1;
        startTxt.SetActive(false);
        Score.gameObject.SetActive(true);
        score = 0;
        GetComponent<AdManager>().showTopBanner();
        Invoke("instantiate", 0.5f);
    }
    public void enabler() {
        Invoke("Enable", 0.1f);
    }

    public void instantiate() {
        StopInstantiate = false;
    }
    public void DisableNight() {

        Themaincamera.GetComponent<Camera>().backgroundColor = startColor;
        Renderer[] Rox = Thecube.GetComponentsInChildren<Renderer>();
        Score.color = new Color(0.2264151f, 0.2264151f, 0.2264151f);
        moneyTxt.color = Color.black;
        foreach (Renderer Roxou in Rox) {
            Roxou.enabled = true;
        }
        foreach (GameObject item in Items) {
            item.GetComponent<Renderer>().enabled = true;
        }
        nightMode = false;

    }

    void Update() {

        moneyTxt.text = money.ToString();

        PlayerPrefs.SetInt("money", money);


        if (startScreen) {
            foreach (GameObject item in Items) {
                item.SetActive(false);
            }
            int intvalueofactiveobject = PlayerPrefs.GetInt("box");
            Items[intvalueofactiveobject].SetActive(true);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            StopInstantiate = true;
        }
        nMode = PlayerPrefs.GetInt("nMode");


        if (nMode == 1) {
            nightMode = true;
            NightModeFromStore = true;
        } else {
            nightMode = false;
            NightModeFromStore = false;
        }



        GetComponent<Collider>().enabled = true;
        Child.gameObject.GetComponent<Collider>().enabled = true;


        if (SpawnEnemies <= 0) {
            StopInstantiate = false;
        } else if (SpawnEnemies > 0) {
            StopInstantiate = true;
        }
        Score.text = score.ToString();


        if (Application.platform != RuntimePlatform.Android) {
            if (!pausedGame && !dead && !startScreen) {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {


                    if (Time.time - lastClick < 0.3) {
                        StartCoroutine(DoubleClicks());
                    }
                    lastClick = Time.time;
                }
            }
        }


        grounded = IsGrounded();
        DeathCheck();

        if (GetComponent<Rigidbody>().useGravity == false && score > 0) {
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (nightMode) {
            if (!NightModeFromStore) {
                Invoke("DisableNight", NightTime);
            }
        }



        if (!pausedGame) {
            if (!startScreen) {
                if (!dead) {
                    tryAgain = false;
                    speed = speed * incerement;
                    if (Input.touchCount == 1) {
                        Touch touch = Input.GetTouch(0);

                        if (touch.phase == TouchPhase.Ended) {
                            TapCount += 1;
                        }

                        if (TapCount == 1) {
                            MaxDubbleTapTime = 0.2f;
                            NewTime = Time.time + MaxDubbleTapTime;
                        } else if (TapCount == 2 && Time.time <= NewTime) {

                            StartCoroutine(DoubleClicks());
                            TapCount = 0;
                        }

                    }
                    if (Time.time > NewTime) {
                        TapCount = 0;
                    }

                    panel.SetActive(false);

                    Animator panelAnim = panel.GetComponent<Animator>();
                    panelAnim.SetBool("MakePanelIn", false);

                    if (last_currentPoint != currentPoint) {
                        SpawnEnemies = SpawnEnemies - 1;


                        for (int i = enemies.Count - 1; i > -1; i--) {
                            if (enemies[i] == null) {
                                enemies.RemoveAt(i);
                                emptyarray = true;
                            } else {
                                emptyarray = false;
                                break;
                            }

                            emptyarray = true;
                        }
                        if (!emptyarray) {
                            if (enemies.Count > 1) {
                                Destroy(enemies[0].gameObject);
                                enemies.RemoveAt(0);
                            }
                        }
                        if (!StopInstantiate) {
                            if (score > 1) {
                                InstantiateEnemies();
                            }
                        }




                        for (int i = coins.Count - 1; i > -1; i--) {
                            if (coins[i] == null) {
                                coins.RemoveAt(i);
                                emptycoinarray = true;
                            } else {
                                emptycoinarray = false;
                                break;
                            }

                            emptycoinarray = true;
                        }
                        if (!emptycoinarray) {
                            if (coins.Count > 1) {
                                Destroy(coins[0].gameObject);
                                coins.RemoveAt(0);
                            }
                        }



                        last_currentPoint = currentPoint;
                        if (!startScreen) {
                            score = score + 1;
                            canJump = true;

                            if (score > NightValue) {
                                int randval = UnityEngine.Random.Range(0, 15);
                                if (randval == 1) {
                                    nightMode = true;
                                }
                            }
                        }
                    }


                    if (nightMode) {
                        Themaincamera.GetComponent<Camera>().backgroundColor = Color.black;
                        Renderer[] Rox = Thecube.GetComponentsInChildren<Renderer>();
                        Score.color = Color.white;
                        moneyTxt.color = Color.white;
                        foreach (Renderer Roxou in Rox) {
                            Roxou.enabled = false;
                        }
                        foreach (GameObject item in Items) {
                            item.GetComponent<Renderer>().enabled = false;
                        }

                    } else if (!nightMode) {

                    }


                    if (!jumping) {
                        float dist = Vector3.Distance(path[currentPoint].position, transform.position);
                        transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, Time.deltaTime * speed);
                        if (IsGrounded()) {
                            if (canJump) {
                                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                                    jumping = true;
                                    GetComponent<AudioSource>().PlayOneShot(JumpSound);
                                }
                            }

                        }

                        if (dist < reachdist) {
                            currentPoint--;
                        }
                        if (currentPoint == -1) {
                            currentPoint = path.Length - 1;
                        }
                    } else if (jumping == true) {
                        transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, Time.deltaTime * speed * 2f);
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + jumpSpeed, transform.position.z), Time.deltaTime * speed * 20);
                        Invoke("unjump", jumpTime);
                        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
                            canJump = false;
                        }


                    }

                }
            }
        }

    }

    private void DeathCheck() {
        if (dead) {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            

            DisableNight();
            Pausebtn.SetActive(false);
            StopInstantiate = false;
            if (!spawnedrigidbodies) {
                deathCount++;
                if (deathCount != 0 && deathCount % GetComponent<AdManager>().interstitialReccurrence == 0) {
                    GetComponent<AdManager>().ShowInterstitial();
                    Debug.Log("Interstial Now");
                }
                if (GetComponent<AdManager>().bannerView != null) {
                    GetComponent<AdManager>().bannerView.Hide();
                }
                foreach (GameObject item in Items) {
                    item.SetActive(false);
                }
                int intvalueofactiveobject = PlayerPrefs.GetInt("box");
                Debug.Log(intvalueofactiveobject.ToString());
                Material mat = Items[intvalueofactiveobject].GetComponent<Renderer>().material;
                go = Instantiate(rigidbodies, transform.position, Quaternion.identity) as GameObject;
                go.transform.SetParent(transform);
                Renderer[] gorenderer = go.GetComponentsInChildren<Renderer>();
                foreach (Renderer rend in gorenderer) {
                    rend.material = mat;
                }
                spawnedrigidbodies = true;
            }
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().useGravity = false;
            i = 0;
            foreach (GameObject enem in enemies) {
                if (enem != null) {
                    Destroy(enem);
                }
            }
            enemies.Clear();

            ic = 0;
            foreach (GameObject coi in coins) {
                if (coi != null) {
                    Destroy(coi);
                }
            }
            coins.Clear();
            panel.SetActive(true);
            if (!animatedpanel) {
                Animator panelAnim = panel.GetComponent<Animator>();
                mainMenuPanel.GetComponent<Animator>().SetTrigger("animate");
                panelAnim.SetBool("MakePanelIn", true);
                animatedpanel = true;
            }


            Score.enabled = false;
            if (score > highScore) {
                PlayerPrefs.SetInt("HighScore", score);
                Panel.text = "Amazing ! New HighScore\n" + score.ToString();
                highScore = score;
#if UNITY_IPHONE
		        Social.ReportScore((long)score, leaderboardIOSId, HighScoreCheck);
#elif UNITY_ANDROID
                if (PlayGamesPlatform.Instance.localUser.authenticated) {
                    PlayGamesPlatform.Instance.ReportScore((int)score,
                        leaderboardAndroidId,
                        (bool success) => {
                            Debug.Log("Leaderboard update success: " + success);
                        });
                }
#endif
            } else if (score == highScore) {
                Panel.text = "You reached your highscore\n" + score.ToString();
            } else {
                Panel.text = "Amazing ! Nice Score\n" + score.ToString();
            }

            if (tryAgain) {
                
                RetryButton.GetComponent<Button>().interactable = true;
                ContinueButton.GetComponent<Button>().interactable = true;
                speed = first_speed;
                startTxt.SetActive(true);
                startScreen = true;
                Score.enabled = true;
                spawnedrigidbodies = false;
                Destroy(go);
                panel.SetActive(false);
                Score.gameObject.SetActive(false);
                transform.position = InitialPos.position;
                currentPoint = first_currentpoint;
                animatedpanel = false;
                GetComponent<Rigidbody>().useGravity = true;
                transform.position = InitialPos.position;
                playStart.SetActive(true);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                dead = false;
                tryAgain = false;
            }
        }
    }

    public void unjump() {
        jumping = false;
    }

    public bool DoubleClick() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
            if (!ClickedOnce && !ClickedTwice) {
                timer = System.Diagnostics.Stopwatch.StartNew();
                ClickedOnce = true;
            }
            if (ClickedOnce) {
                if (timer.Elapsed > minDuration && timer.Elapsed < maxDuration) {
                    ClickedTwice = true;
                    ClickedOnce = false;
                    return false;
                } else if (timer.Elapsed > maxDuration) {
                    ClickedOnce = false;

                    return false;
                }
            }
            if (ClickedTwice) {
                if (timer.Elapsed > minDuration && timer.Elapsed < maxDuration) {
                    ClickedTwice = false;
                    return true;
                } else if (timer.Elapsed > maxDuration) {
                    ClickedTwice = false;

                    return false;
                }
            }
        }
        return false;
    }

    public IEnumerator DoubleClicks() {
        if (!dead) {
            yield return new WaitForSeconds(0.01f);
        }
    }

    public bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up,  0.1f);
    }


    private int GetScaledFontSize(int baseFontSize) {
        float uiScale = Screen.height / uiBaseScreenHeight;
        int scaledFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
        return scaledFontSize;
    }

    GameObject enemy;
    GameObject coin;

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> coins = new List<GameObject>();


    void InstantiateEnemies() {
        if (UnityEngine.Random.Range(0, 2) == 1) {
            i = i + 1;
            if (currentPoint == 1) {
                float RandVal = UnityEngine.Random.Range(Pos1.position.z + 0.4f, Pos2.position.z - 0.4f);
                enemy = Instantiate(EnemyPref, new Vector3(Pos2.position.x, Pos2.position.y, RandVal), transform.rotation) as GameObject;
                enemies.Add(enemy);
            }
            if (currentPoint == 3) {
                float RandVal = UnityEngine.Random.Range(Pos4.position.z + 0.4f, Pos3.position.z - 0.4f);
                enemy = Instantiate(EnemyPref, new Vector3(Pos3.position.x, Pos2.position.y, RandVal), transform.rotation) as GameObject;
                enemies.Add(enemy);
            }
            if (currentPoint == 0) {
                float RandVal = UnityEngine.Random.Range(Pos1.position.x + 0.4f, Pos4.position.x - 0.4f);
                enemy = Instantiate(EnemyPref, new Vector3(RandVal, Pos2.position.y, Pos1.position.z), transform.rotation) as GameObject;
                enemies.Add(enemy);
            }
            if (currentPoint == 2) {
                float RandVal = UnityEngine.Random.Range(Pos2.position.x + 0.4f, Pos3.position.x - 0.4f);
                enemy = Instantiate(EnemyPref, new Vector3(RandVal, Pos2.position.y, Pos2.position.z), transform.rotation) as GameObject;
                enemies.Add(enemy);
            }
        } else {
            if (UnityEngine.Random.Range(0, 2) == 1) {
                ic = ic + 1;
                if (currentPoint == 1) {
                    float RandVal = UnityEngine.Random.Range(Pos1.position.z + 0.4f, Pos2.position.z - 0.4f);
                    coin = Instantiate(CoinPref, new Vector3(Pos2.position.x, Pos2.position.y + elevation, RandVal), CoinPref.transform.rotation) as GameObject;
                    coins.Add(coin);
                }
                if (currentPoint == 3) {
                    float RandVal = UnityEngine.Random.Range(Pos4.position.z + 0.4f, Pos3.position.z - 0.4f);
                    coin = Instantiate(CoinPref, new Vector3(Pos3.position.x, Pos2.position.y + elevation, RandVal), CoinPref.transform.rotation) as GameObject;
                    coins.Add(coin);
                }
                if (currentPoint == 0) {
                    float RandVal = UnityEngine.Random.Range(Pos1.position.x + 0.4f, Pos4.position.x - 0.4f);
                    coin = Instantiate(CoinPref, new Vector3(RandVal, Pos2.position.y + elevation, Pos1.position.z), CoinPref.transform.rotation) as GameObject;
                    coins.Add(coin);
                }
                if (currentPoint == 2) {
                    float RandVal = UnityEngine.Random.Range(Pos2.position.x + 0.4f, Pos3.position.x - 0.4f);
                    coin = Instantiate(CoinPref, new Vector3(RandVal, Pos2.position.y + elevation, Pos2.position.z), CoinPref.transform.rotation) as GameObject;
                    coins.Add(coin);
                }
            } 
        }
    }




    public void Pause() {
        if (!dead) {
            if (!pausedGame) {
                pausedGame = true;
                Time.timeScale = 0;
                PausePanel.SetActive(true);
                Score.gameObject.SetActive(false);
            } else {
                if (ShowStore) {
                    pausedGame = false;
                    ShowStore = false;
                    Time.timeScale = 1;
                    StorePanel.SetActive(false);
                }
                pausedGame = false;
                Time.timeScale = 1;
                PausePanel.SetActive(false);
                Score.gameObject.SetActive(true);
            }
        }
    }

    

    public void ContinueDead() {
        GetComponent<AdManager>().WatchRewardedAd(0);
    }

    public void AdCallbackhandlerContinue() {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Pausebtn.SetActive(true);
        int intvalueofactiveobject = PlayerPrefs.GetInt("box");
        spawnedrigidbodies = false;
        Items[intvalueofactiveobject].SetActive(true);
        Destroy(go);
        GetComponent<Rigidbody>().useGravity = true;
        Score.enabled = true;
        panel.SetActive(false);
        dead = false;
        ContinueButton.GetComponent<Button>().interactable = true;
    }


    public void buyCoins() {
        GetComponent<AdManager>().WatchRewardedAd(0);
    }

    public void AdCallbackhandlerBuy() {
        money += 30;
    }

    public void TryAgain() {
        SendHighScore();
        tryAgain = true;
    }




    public void SaveSoundState(bool state) {
        if (!state) {
            PlayerPrefs.SetInt("sound", 1);
        } else {
            PlayerPrefs.SetInt("sound", 0);
        }
    }





    public void didCompleteRewardedVideo() {
        Destroy(enemy);
        dead = false;
        panel.SetActive(false);
    }





    void SendHighScore() {
        //
    }

    void OnGUI() {
        GUI.skin = MySkin;

    }


    public void DisplayLeaderboard() {
        //
    }

    public void DisplayStore() {

        StorePanel.SetActive(true);
        StorePanel.GetComponent<Animator>().SetTrigger("animate");
        StoreActive = true;

    }
    

    public void DiseableStore() {
        StoreActive = false;
    }


}