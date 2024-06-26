using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    private static AnimalManager _instance;
    public static AnimalManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AnimalManager>();
            }
            return _instance;
        }
    }

    public List<GameObject> Models = new List<GameObject>();
    public List<Transform> Users = new List<Transform>();
    [Header("InGame")]
    public Player Player;
    public bool IsGaming = false;
    public List<Transform> Winners = new List<Transform>();
    public int CheckLineIndex;

    private void Awake()
    {
        Player = FindObjectOfType<Player>();
        if (Player)
            Player.OnPlayerDie.AddListener(PlayerRetire);

        Users.Add(Player.transform);

        SetModels();
    }

    private void SetModels()
    {
        for (int idx = 0; idx < 101; idx++)
        {
            for (int i = 0; i < Models.Count; i++)
            {
                int a = Random.Range(0, Models.Count);
                int b = Random.Range(0, Models.Count);
                var temp = Models[a];
                Models[a] = Models[b];
                Models[b] = temp;
            }
        }

        for (int i = 0; i < Users.Count; i++)
        {
            if (Users[i].TryGetComponent(out Player player))
            {
                GameObject model = Instantiate(Models[i], Users[i]);
                model.name = "Visual";
            }
        }
    }

    private void PlayerRetire()
    {

    }

    public void SetWinner(Transform trm)
    {
        Winners.Add(trm);
        if(trm.TryGetComponent(out Player player))
        {
            CameraManager.Instance.SetModelCam();
            UIManager.Instance.ChangeUIState(UIState.OnResult);
        }
        trm.gameObject.SetActive(false);
    }
}
