using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { IS_PLAYING, PAUSED, UI_MENU, GAME_OVER, NONE }
    public static GameManager instance;


    public GameState presentState = GameState.UI_MENU; 


    public UIManager UM;
    public EnemySpawner ES;


    public int nextWaveAsteroids = 2;
    public bool enemyDeath;

    int Score;
    public int score
    {
        get { return Score; }
        set
        {
            Score = value;
            SetHUDValues();
            CheckCurrentStage();
        }
    }

    int Waves;
    public int waves
    {
        get { return Waves; }
        set {
            Waves = value; SetHUDValues();
        }
    }

    int Lives;
    public int lives
    {
        get { return Lives; }
        set
        {
            if (lives > 3)
                lives = 3;

            Lives = value;
            UM.SetLivesUI(lives);
        }
    }



    public GameObject asteroid_large;
    public GameObject asteroid_medium;
    public GameObject asteroid_small;


    public GameObject spaceShip_pc;
    public GameObject playerExplosion;


    public GameObject gameFiledObj;

    public ObjectPool astPoolLarge;
    public ObjectPool astPoolMedium;
    public ObjectPool astPoolSmall;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
    void Start()
    {
        score = 0;
        waves = 1;
        lives = 3;
        presentState = GameState.UI_MENU;
        UM.ButtonClick("MAIN_MENU");
    }


    public void EnemyDeath()
	{
        enemyDeath = true;
        if (enemyDeath == true)
        {
            StartCoroutine(GenerateWithDalay(false, Random.Range(20f, 40f)));
        }

    }

    public void CheckCurrentStage()
    {       
        if (IsAllAsteroidsDisabled() && score > 0 && presentState ==  GameState.IS_PLAYING)
        {
            waves++;
            StartCoroutine(GenerateWithDalay(true,2f));
            nextWaveAsteroids++;
        }  
    }
    IEnumerator GenerateWithDalay(bool typeGenerate,float sec)
    {
        yield return new WaitForSeconds(sec);
		if (typeGenerate)
		{
            GenerateAsteroids(AsteroidController.asteroidType.Large, nextWaveAsteroids);
        }
		else if (!typeGenerate)
		{
            ES.EnemySpawn();
            enemyDeath = false;
        }
        yield return null;
    }



    void SetHUDValues()
    {
        UM.hud_score.text = "Счет : " + score.ToString();
        UM.hud_waves.text = "Волна : "+waves.ToString();
    }

    public void GenerateAsteroids(AsteroidController.asteroidType type, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject asteroidsClone = null;
            switch (type)
            {
                case AsteroidController.asteroidType.Large:
                    asteroidsClone = astPoolLarge.GetPoolObj();break;
                case AsteroidController.asteroidType.Medium:
                    asteroidsClone = astPoolMedium.GetPoolObj();break;
                case AsteroidController.asteroidType.Small:
                    asteroidsClone = astPoolSmall.GetPoolObj();break;
            }
            asteroidsClone.SetActive(true);
        }
    }
    public void GenerateAsteroids(AsteroidController.asteroidType type, int count, Vector3 pos)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject asteroidsClone = null;
            switch (type)
            {
                case AsteroidController.asteroidType.Large:
                    asteroidsClone = astPoolLarge.GetPoolObj(); break;
                case AsteroidController.asteroidType.Medium:
                    asteroidsClone = astPoolMedium.GetPoolObj(); break;
                case AsteroidController.asteroidType.Small:
                    asteroidsClone = astPoolSmall.GetPoolObj(); break;
            }
            asteroidsClone.transform.position = pos;
			asteroidsClone.SetActive(true);
		}
    }

    bool IsAllAsteroidsDisabled()
    {
        foreach (Transform child in astPoolLarge.transform)
        {
            if (child.gameObject.activeSelf)
                return false;
        }
        foreach (Transform child in astPoolMedium.transform)
        {
            if (child.gameObject.activeSelf)
                return false;
        }
        foreach (Transform child in astPoolSmall.transform)
        {
            if (child.gameObject.activeSelf)
                return false;
        }

        return true;
    }



    public void StopGamePlay()
    {
        gameFiledObj.SetActive(false);
    }

    public void StartGame()
    {
        ResetGame();
        GenerateAsteroids(AsteroidController.asteroidType.Large, nextWaveAsteroids);
        presentState = GameState.IS_PLAYING;

    }

    public void ResumeGame()
    {
      
        gameFiledObj.SetActive(true);
        presentState = GameState.IS_PLAYING;
    }

    public void EnemyDestroyer(bool ed)
	{
		if (ed == false)
		{
            GameObject enemy = ES.transform.GetChild(0).gameObject;
            Destroy(enemy);
            EnemyDeath();


        }
		else
		{
           StartCoroutine(GenerateWithDalay(false, Random.Range(20f, 40f)));
        }
    }
    public void ResetGame()
    {
        StopAllCoroutines();
        score = 0;
        waves = 1;
        lives = 3;
        nextWaveAsteroids = 2;
	    EnemyDestroyer(enemyDeath);
        UM.ResetLivesUI();
        spaceShip_pc.GetComponent<Rigidbody>().isKinematic = true;
        spaceShip_pc.transform.position = Vector3.zero;
        spaceShip_pc.transform.rotation = Quaternion.identity;
        spaceShip_pc.GetComponent<Rigidbody>().isKinematic = false;

        foreach (Transform child in astPoolLarge.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in astPoolMedium.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in astPoolSmall.transform)
        {
            child.gameObject.SetActive(false);
        }
        spaceShip_pc.SetActive(true);
        spaceShip_pc.GetComponent<SpaceShipController>().Immortal = false;
        spaceShip_pc.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ReplayGame()
    {
        ResetGame();
        gameFiledObj.SetActive(true);
        GenerateAsteroids(AsteroidController.asteroidType.Large, nextWaveAsteroids);
        presentState = GameState.IS_PLAYING;
    }

    public void GameOver()
    {
        if (lives <= 0)
        {
            UM.ButtonClick("GAME_OVER");
            SoundManager.instance.PlayClip(EAudioClip.FAILURE_SFX, 1);
            spaceShip_pc.SetActive(false);
            gameFiledObj.SetActive(false);
            Instantiate(playerExplosion, spaceShip_pc.transform.position, spaceShip_pc.transform.rotation);
            presentState = GameState.GAME_OVER;
        }
        else
        {
            CheckCurrentStage();
        }
    }



}
