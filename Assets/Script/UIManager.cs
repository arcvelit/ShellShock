using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private ProgressBar healthBar;
    private ProgressBar cooldownBar;
    private Label weaponIndicator;
    private Label overheatLabel;
    private VisualElement overheatFiller;
    private VisualElement healthFiller;

    public Canvas controlsCanvas;

    public bool flickering;

    private bool released;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;
        healthBar = root.Q("HealthBar") as ProgressBar;
        cooldownBar = root.Q("CooldownBar") as ProgressBar;
        weaponIndicator = root.Q("WeaponIndicator") as Label;
        weaponIndicator.text = "> Machine Gun <";

        overheatLabel = root.Q("OverheatText") as Label;
        overheatLabel.text = "";

        overheatFiller = cooldownBar.Q(className: "unity-progress-bar__progress");
        overheatFiller.style.backgroundColor = Color.white;

        healthFiller = healthBar.Q(className: "unity-progress-bar__progress");
        healthFiller.style.backgroundColor = Color.green;

        StartCoroutine(HideControls(10));


    }

    void Update()
    {
        if (!released) return; 

        if (Input.GetKey(KeyCode.Space) )
        {
            controlsCanvas.gameObject.SetActive(true);
        }
        else {
            controlsCanvas.gameObject.SetActive(false);
        }
    }


    private IEnumerator HideControls(float delay)
    {
        yield return new WaitForSeconds(delay);
        controlsCanvas.gameObject.SetActive(false);
        released = true;
    }

    public void ChangeWeaponTo(string weapon)
    {
        Cooled();
        weaponIndicator.text = weapon;
    }

    public IEnumerator FlickerHealth()
    {
        flickering = true;
        bool flicker = false;
        while (true)
        {
            if (PlayerHealth.Instance.percentHealth < PlayerHealth.Instance.dangerLifeThreshold)
            {
                healthFiller.style.backgroundColor = flicker ? Color.white : Color.green;
                flicker = !flicker;
                yield return new WaitForSeconds(0.25f); // Flicker every 0.25 seconds
            }
            else
            {
                healthFiller.style.backgroundColor = Color.green;
                flickering = false;
                yield return null; // Check every frame
            }
        }
    }

    public void UpdateHealthBar(float percentHealth)
    {
        healthBar.value = percentHealth;
    }

    public void Overheat()
    {
        overheatFiller.style.backgroundColor = Color.red;
        overheatLabel.text = "! OVERHEATED !";

    }

    public void Cooled()
    {
        overheatLabel.text = "";
        overheatFiller.style.backgroundColor = Color.white;
    }


    public void UpdateCoolingBar(float percentOverheat)
    {
        cooldownBar.value = percentOverheat;
    }

    public void Reloading()
    {
        overheatLabel.text = "Reloading...";
    }

}
