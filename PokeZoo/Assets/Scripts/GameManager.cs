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
    [SerializeField] private Image placaManager;
    [SerializeField] private List<CharacterItem> players = new List<CharacterItem>();
    [SerializeField] private List<CharacterItem> animais = new List<CharacterItem>();

    [Header("Current Selection")]
    [SerializeField] private int currentPlayer;
    [SerializeField] private int currentAnimal;

    [Header("UI")]
    [SerializeField] private HudManager hudManager;
    [SerializeField] private Image fadeImage;

    public GameObject walkingPlayer;

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
        placaManager.enabled = false;


        currentAnimal = Random.Range(0, animais.Count);
    }

    public IEnumerator CreateFight(CharacterItem animalItem)
    {
        yield return StartCoroutine(SetupFadeImage(true));

        DestroyPlayerAndAnimal();

        gameplayManager.SetActive(false);
        menuManager.SetActive(false);
        hudManager.gameObject.SetActive(true);
        hudManager.UpdateTexts();
        battleManager.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        InstantiateAnimal(animalItem, players[currentPlayer]);

        yield return StartCoroutine(SetupFadeImage(false));
    }
    private void InstantiateAnimal(CharacterItem animalItem, CharacterItem playerItem)
    {
        hudManager.gameObject.SetActive(true);
        hudManager.animal = animalItem;
        hudManager.player = playerItem;
        hudManager.animalNameText.text = animalItem.itemName;
        hudManager.playerNameText.text = playerItem.itemName;
        hudManager.animalSprite.sprite = animalItem.itemIcon;
        hudManager.playerSprite.sprite = playerItem.itemIcon;
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


        hudManager.audioSource.PlayOneShot(hudManager.damageClips[0]);
        placaManager.enabled = true;
        placaManager.sprite = hudManager.animal.itemPlaca;
        placaManager.preserveAspect = true;


        yield return new WaitForSeconds(10);

        menuManager.SetActive(false);
        battleManager.SetActive(false);
        hudManager.gameObject.SetActive(false);
        placaManager.enabled = false;
        gameplayManager.SetActive(true);
        InstantiatePlayer();

        yield return StartCoroutine(SetupFadeImage(false));
    }

    private void DestroyPlayerAndAnimal()
    {
        if (walkingPlayer) 
            Destroy(walkingPlayer);
    }

    private void InstantiatePlayer()
    {
        walkingPlayer = Instantiate(Resources.Load<GameObject>("Prefabs/Players/Entity_0" + currentPlayer), cameraSpawn);
        walkingPlayer.transform.parent = null;
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

    public void ChoosePlayer(int playerNumber)
    {
        currentPlayer = playerNumber;
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
