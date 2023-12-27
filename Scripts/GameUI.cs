using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public Image fadePlane;
    public GameObject gameOverUI;
    public RectTransform newWaveBanner;
    public Text newWaveTitle;
    public Text newWaveCount;
    public Text socreUI;
    public Text GameOverScoreUI;
    public RectTransform healthBar;

    Spawner spawner;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player> ();
        player.OnDeath += OnGameOver;
    }

    void Awake()
    {
        spawner = FindObjectOfType<Spawner> ();
        spawner.OnNewWave += OnNewWave;
    }

    void Update()
    {
        socreUI.text = ScoreKeeper.score.ToString("D6");
        float healthPercent = 0;
        if (player != null)
        {
            healthPercent = player.health / player.startingHealth;
        }
        healthBar.localScale = new Vector3 (healthPercent, 1, 1);

    }

    void OnNewWave(int waveNumber)
    {
        string[] number = {"One", "Two", "Three", "Four", "Five"};
        newWaveTitle.text = "- Wave " + number[waveNumber - 1] + "-";
        string enemyCountString = spawner.waves[waveNumber - 1].infinity?"infinity" : spawner.waves[waveNumber - 1].enemyCount + "";
        newWaveCount.text = "Enemies: " + enemyCountString;

        //StopCoroutine(AnimateNewWaveBanner());
        StartCoroutine(AnimateNewWaveBanner());
    }

    void OnGameOver()
    {
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, new Color(0,0,0,.95f), 1));
        GameOverScoreUI.text = socreUI.text;
        socreUI.gameObject.SetActive(false);
        healthBar.transform.parent.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    IEnumerator AnimateNewWaveBanner()
    {
        float delayTime = 1f;
        float speed = 3f;
        float animatePercent = 0;
        float dir = 1;

        float endDelayTime = Time.time * 1 / speed * delayTime;

        while (animatePercent < 1)
        {
            animatePercent += Time.deltaTime * speed * dir;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                {
                    dir = -1;
                }
            }

            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-170, 45, animatePercent);
            yield return null;
        }
    }


    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    // UI input
    public void StartNewGame()
    {
        //Application.LoadLevel("Game");
        SceneManager.LoadScene("Game");
    }

    public void RetruToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
