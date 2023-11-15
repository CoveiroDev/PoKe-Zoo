using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameObject menuManager;
    [SerializeField] private GameObject choosePlayerManager;
    [SerializeField] private GameObject gameplayManager;
    [SerializeField] private GameObject battleManager;

    [Header("Spawn Points")]
    [SerializeField] private Transform cameraSpawn;
    [SerializeField] private Transform spawnPlayer;
    [SerializeField] private Transform spawnAnimal;

    [Header("Lists")]
    [SerializeField] private List<string> players = new List<string> { "Menino", "Menina" };
    [SerializeField] private List<string> animais = new List<string> { "Arara-Azul", "Ema", "Tucano" };

    [Header("Current Selection")]
    [SerializeField] private string currentPlayer;
    [SerializeField] private int currentAnimal;

    [Header("UI")]
    [SerializeField] private HudManager hudManager;
    [SerializeField] private Image fadeImage;

    public GameObject player;
    public GameObject animal;

    private void Start()
    {
        InitializeManagers();
        StartCoroutine(EnterMenu());
    }

    private void InitializeManagers()
    {
        hudManager = GetComponentInChildren<HudManager>();
        hudManager.gameObject.SetActive(false);
        menuManager.SetActive(false);
        gameplayManager.SetActive(false);
        choosePlayerManager.SetActive(false);
        battleManager.SetActive(false);


        currentAnimal = Random.Range(0, animais.Count);
    }

    public IEnumerator CreateFight(string animalPrefabName)
    {
        yield return StartCoroutine(SetupFadeImage(true));

        gameplayManager.SetActive(false);
        menuManager.SetActive(false);
        hudManager.gameObject.SetActive(true);
        hudManager.UpdateTexts();
        battleManager.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        InstantiateAnimal(animalPrefabName);

        SetupHudManager();
        yield return StartCoroutine(SetupFadeImage(false));
    }

    private void InstantiateAnimal(string animalPrefabName)
    {
        animal = Instantiate(Resources.Load<GameObject>("Prefabs/Animais/" + animalPrefabName), spawnAnimal);
        player.name = player.name;
        animal.name = animalPrefabName;
    }

    private void SetupHudManager()
    {
        hudManager.gameObject.SetActive(true);
        hudManager.player = player;
        hudManager.animal = animal;
        hudManager.HealthAnimal = 100;
        hudManager.HealthPlayer = 100;
        hudManager.UpdateTexts();
        hudManager.SetButtonsInteractable(true);
    }

    private IEnumerator SetupFadeImage(bool enable)
    {
        fadeImage.enabled = enable;
        float fadeDuration = 1.0f;
        Color startColor = fadeImage.color;
        Color visibleColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);
        Color invisibleColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, enable ? visibleColor : invisibleColor, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeImage.color = enable ? visibleColor : invisibleColor;
        fadeImage.enabled = enable;
    }

    public IEnumerator EndFight()
    {
        yield return StartCoroutine(SetupFadeImage(true));

        DestroyPlayerAndAnimal();

        menuManager.SetActive(false);
        hudManager.gameObject.SetActive(false);
        gameplayManager.SetActive(true);
        battleManager.SetActive(false);

        InstantiatePlayer();

        yield return StartCoroutine(SetupFadeImage(false));
    }

    private void DestroyPlayerAndAnimal()
    {
        if (player) Destroy(player);
        if (animal) Destroy(animal);
    }

    private void InstantiatePlayer()
    {
        player = Instantiate(Resources.Load<GameObject>("Prefabs/Players/" + currentPlayer), cameraSpawn);
        player.transform.parent = null;
    }

    public IEnumerator EnterMenu()
    {
        yield return StartCoroutine(SetupFadeImage(false));
        yield return StartCoroutine(SetupFadeImage(true));
        yield return StartCoroutine(SetupFadeImage(false));
        menuManager.SetActive(true);
    }

    public void CreateItem(string itemPrefabName)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Itens/" + itemPrefabName), spawnAnimal);
    }

    public void RunFromGameBTN()
    {
        StartCoroutine(EndFight());
    }

    public void ChoosePlayer(string namePlayer)
    {
        currentPlayer = namePlayer;
        gameplayManager.SetActive(true);
        InstantiatePlayer();
        menuManager.SetActive(false);
        choosePlayerManager.SetActive(false);
        battleManager.SetActive(false);
    }

    public void FindFight()
    {
        currentAnimal = Random.Range(0, animais.Count);
        StartCoroutine(CreateFight(animais[currentAnimal]));
    }

    public void StartGameBTN()
    {
        menuManager.SetActive(false);
        choosePlayerManager.SetActive(true);
        battleManager.SetActive(false);
    }

    public void ExitGameBTN()
    {
        Application.Quit();
    }
}
