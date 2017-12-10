using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public GameObject playerPrefab;

    public bool gameOver = false;
    public int quantidadePlayers;
    public int codWinner;
    public int duracaoPartida = 180;
    public int delay = 5;
    public int[] pontuacaoPlayers;
    public List<Player> playersInGame;

    Text txtTime;
    float timer;
    public Text[] txtPlayers;
    public Transform[] spawnPoints;
    Button btnRestart;

	void Start ()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        quantidadePlayers = (int)DirCount(new DirectoryInfo("./Assets/LuaScripts"));
#else
        quantidadePlayers = (int) DirCount(new DirectoryInfo("./LuaScripts"));
#endif

        spawnPoints = new Transform[quantidadePlayers];
        txtPlayers = new Text[quantidadePlayers];
        pontuacaoPlayers = new int[quantidadePlayers];

        for (int i = 0; i < quantidadePlayers; i++)
        {
            spawnPoints[i] = GameObject.Find("SpawnP" + (i + 1)).transform;
            txtPlayers[i] = GameObject.Find("txtPontosP" + (i + 1)).GetComponent<Text>();
        }

        playersInGame = new List<Player>();

        for (int i = 0; i < quantidadePlayers; i++)
        {
            GameObject p = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);
            Player player = p.GetComponent<Player>();
            player.cod = i + 1;
            playersInGame.Add(player);
        }

        txtTime = GameObject.Find("txtTime").GetComponent<Text>();
        timer = duracaoPartida - 1;
        btnRestart = GameObject.Find("btnRestart").GetComponent<Button>();
        btnRestart.onClick.RemoveAllListeners();
        btnRestart.onClick.AddListener(delegate { SceneManager.LoadScene(0); });
    }

    void Update ()
    {
		if(!gameOver)
        {
            Contador();
            AtualizaPlacar();
        }
	}

    public static long DirCount(DirectoryInfo d)
    {
        long i = 0;
        // Add file sizes.
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("lua"))
                i++;
        }
        return i;
    }

#region Regras
    public void Contador()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            txtTime.text = Mathf.RoundToInt(timer).ToString();
        }
        else
        {
            int maxPoints = pontuacaoPlayers.Max();
            int codWinner;

            if (maxPoints > 0)
            {
                codWinner = pontuacaoPlayers.ToList().IndexOf(maxPoints) + 1;
                SetTextUIElement("txtWinner", "Player " + codWinner + " venceu!\nPontos: " + maxPoints);
            }
            else
            {
                SetTextUIElement("txtWinner", "Empate!");
            }

            ToggleUIElement("pnlGameOver", true);
            Time.timeScale = 0f;
            gameOver = true;
        }
    }
    public void Respawn(int _cod)
    {
        StartCoroutine(RespawnAfterTime(_cod, delay));
    }
    IEnumerator RespawnAfterTime(int _cod, float _time)
    {
        yield return new WaitForSeconds(_time);
        //Instantiate(playersPrefabs[_cod], spawnPoints[_cod - 1].position, Quaternion.identity);
        playersInGame[_cod - 1].transform.position = spawnPoints[_cod - 1].position;
        playersInGame[_cod - 1].gameObject.SetActive(true);
        playersInGame[_cod - 1].atirou = false;

        foreach (Bullet _bullet in FindObjectsOfType<Bullet>())
        {
            if (_bullet.ownerCod == _cod)
            {
                Destroy(_bullet.gameObject);
                break;
            }
        }
    }
#endregion

#region Métodos de UI
    void AtualizaPlacar()
    {
        for (int i = 0; i < pontuacaoPlayers.Length; i++)
        {
            txtPlayers[i].text = "P" + (i + 1) + ":" + pontuacaoPlayers[i];
        }
    }
    public void ToggleUIElement(string elementName, bool active)
    {
        CanvasGroup group = GameObject.Find(elementName).GetComponent<CanvasGroup>();

        group.blocksRaycasts = active;
        group.alpha = group.blocksRaycasts ? 1 : 0;
    }
    public void SetTextUIElement(string elementName, string text)
    {
        Text t = GameObject.Find(elementName).GetComponentInChildren<Text>();
        t.text = text;
    }
#endregion
}