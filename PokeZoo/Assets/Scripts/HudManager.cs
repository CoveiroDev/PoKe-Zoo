using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [Header("GameManager")]
    [HideInInspector] private GameManager gameManager;

    [Header("Characters")]
    [HideInInspector] public GameObject player;
    [HideInInspector] public GameObject animal;

    [Header("Health")]
    [SerializeField] public int HealthPlayer = 100;
    [SerializeField] public int HealthAnimal = 100;
    [SerializeField] public Text healthTextPlayer;
    [SerializeField] public Text healthTextAnimal;

    [Header("Characters")]
    [SerializeField] public Text playerNameText;
    [SerializeField] public Text animalNameText;

    [Header("HUD")]
    [SerializeField] public Button btn_Ataque;
    [SerializeField] public Button btn_Defesa;
    [SerializeField] public Button btn_Fugir;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] damageClips;

    [Header("Combat")]
    [SerializeField] private bool animalIsAngry;
    [SerializeField] private bool animalIsDefending;

    private void OnEnable()
    {
        InitializeGameManager();
    }

    private void InitializeGameManager()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void UpdateTexts()
    {
        playerNameText.text = gameManager.player.name;
        if (gameManager.animal)
            animalNameText.text = gameManager.animal.name;

        healthTextPlayer.text = HealthPlayer.ToString() + "/100";
        healthTextAnimal.text = HealthAnimal.ToString() + "/100";
    }

    public void Ataque()
    {
        StartCoroutine(AtaqueCoroutine());
    }

    public void Defender()
    {
        StartCoroutine(DefenderCoroutine());
    }

    public IEnumerator AtaqueCoroutine()
    {
        SetButtonsInteractable(false);

        yield return new WaitForSeconds(1f);

        DetermineAttackOutcome();

        yield return new WaitForSeconds(1.5f);

        TakeDamage(player);

        yield return new WaitForSeconds(1);

        HandleAnimalAnger();
        HandleCreateItem();

        SetButtonsInteractable(true);
    }

    private void DetermineAttackOutcome()
    {
        int currentAtq = Random.Range(0, 4);
        if (currentAtq == 0 && HealthPlayer >= HealthAnimal && HealthAnimal >= 10)
        {
            BlockDamage(animal);
        }
        else
        {
            TakeDamage(animal);
        }
    }

    public IEnumerator DefenderCoroutine()
    {
        SetButtonsInteractable(false);

        yield return new WaitForSeconds(1.5f);

        BlockDamage(player);

        yield return new WaitForSeconds(1);

        HandleAnimalAnger();
        HandleCreateItem();

        SetButtonsInteractable(true);
    }

    private void HandleAnimalAnger()
    {
        if (HealthAnimal <= 50)
        {
            int currentAnimal = Random.Range(0, 2);
            switch (currentAnimal)
            {
                case 0:
                    animalIsAngry = true;
                    break;
                default:
                    animalIsAngry = false;
                    break;
            }
        }
        else
        {
            animalIsAngry = false;
        }
        animal.GetComponent<Animator>().SetBool("Angry", animalIsAngry);
    }
    private void HandleCreateItem()
    {
        if (HealthPlayer <= HealthAnimal && HealthPlayer <= 50)
        {
            if (HealthAnimal != 0 && HealthPlayer != 0)
            {
                int currentItem = Random.Range(0, 2);
                if (currentItem == 0)
                    gameManager.CreateItem("Health_Item");
            }
        }
    }

    public void SetButtonsInteractable(bool interactable)
    {
        btn_Ataque.interactable = interactable;
        btn_Defesa.interactable = interactable;
        btn_Fugir.interactable = interactable;
    }

    private void TakeDamage(GameObject victim)
    {
        if (victim == player && HealthAnimal != 0)
        {
            HandleDamage(victim, 30, 20);
        }
        if (victim == animal && HealthPlayer != 0)
        {
            HandleDamage(victim, 10, 21);
        }
    }

    private void BlockDamage(GameObject victim)
    {
        if (victim == player)
        {
            HandleBlockDamage(victim);
        }
        if (victim == animal && !animalIsDefending)
        {
            HandleBlockDamage(victim);
        }
    }

    private void HandleDamage(GameObject victim, int damageAngry, int damageNormal)
    {
        victim.GetComponent<Animator>().Play("TakeDamage");
        audioSource.PlayOneShot(damageClips[0]);
        int damage = animalIsAngry ? damageAngry : damageNormal;
        if (victim == player)
        {
            HealthPlayer -= damage;
            if (HealthPlayer <= 0)
            {
                HealthPlayer = 0;
                gameManager.StartCoroutine(gameManager.EndFight());
            }
        }
        if (victim == animal)
        {
            HealthAnimal -= damage;
            if (HealthAnimal <= 0)
            {
                HealthAnimal = 0;
                gameManager.StartCoroutine(gameManager.EndFight());
            }
        }
        UpdateTexts();
    }

    private void HandleBlockDamage(GameObject victim)
    {
        victim.GetComponent<Animator>().Play("BlockDamage");
        audioSource.PlayOneShot(damageClips[0]);
        UpdateTexts();
    }

    public void HealDamage(GameObject victim)
    {
        if (victim == player)
        {
            HandleHealDamage(victim, 15);
        }
        if (victim == animal)
        {
            HandleHealDamage(victim, 14);
        }
    }
        private void HandleHealDamage(GameObject victim, int healAmount)
    {
        victim.GetComponent<Animator>().Play("HealDamage");
        audioSource.PlayOneShot(damageClips[0]);
        if (victim == player)
        {
            HealthPlayer += healAmount;
            if (HealthPlayer >= 100)
                HealthPlayer = 100;
        }
        if (victim == animal && !animalIsDefending)
        {
            HealthAnimal += healAmount;
            if (HealthAnimal >= 100)
                HealthAnimal = 100;
        }
        UpdateTexts();
    }
}
