using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Este script maneja la lógica para interactuar con un ítem del menú de construcción,
// permitiendo al jugador seleccionar y construir torretas.
public class BuildMenuItemScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    // Sprite base del ítem.
    public Sprite BaseSprite;

    // Sprite cuando el ítem está en estado de hover.
    public Sprite HoverSprite;

    // Sprite cuando el ítem está en estado de clic.
    public Sprite ClickSprite;

    // Prototipo de la torreta a construir.
    public GameObject Prototype;

    // Referencias a componentes UI.
    private Text text;
    private Text price;
    private GameObject parent;
    private GameObject rangeSprite;
    private float range;
    private Image image;

    // Bandera para determinar si el ítem está presionado.
    private bool pressed = false;

    // Bandera para determinar si el ítem está deshabilitado.
    private bool disabled = false;

    // Método Update se llama una vez por frame.
    void Update()
    {
        // Verifica si el jugador tiene suficiente dinero para construir la torreta.
        var enoughMoney = GameManager.Instance.EnoughMoneyForTurret(Prototype.tag);

        // Actualiza el texto del precio.
        price.text = "$" + GameManager.Instance.MoneyForTurret(Prototype.tag);

        // Habilita o deshabilita el ítem dependiendo del dinero del jugador.
        if (disabled && enoughMoney)
        {
            disabled = false;
            text.color = new Color(0, 0, 0, 1);
            price.color = new Color(0, 0, 0, 1);
        }
        else if (!disabled && !enoughMoney)
        {
            disabled = true;
            text.color = new Color(0, 0, 0, 0.25f);
            price.color = new Color(0, 0, 0, 0.25f);
        }
    }

    // Método llamado cuando se detecta un clic en el ítem.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (disabled || pressed) return;

        gameObject.transform.Translate(0, -3f, 0);
        image.sprite = ClickSprite;
        pressed = true;
    }

    // Método llamado cuando el puntero entra en el ítem.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Update();
        if (disabled) return;

        image.sprite = HoverSprite;
        rangeSprite.SetActive(true);
        rangeSprite.transform.localScale = new Vector3(16 * range, 16 * range, 1);
    }

    // Método llamado cuando el puntero sale del ítem.
    public void OnPointerExit(PointerEventData eventData)
    {
        rangeSprite.SetActive(false);

        if (disabled) return;

        image.sprite = BaseSprite;

        if (!pressed) return;

        gameObject.transform.Translate(0, 3f, 0);
        pressed = false;
    }

    // Método llamado cuando se suelta el clic en el ítem.
    public void OnPointerUp(PointerEventData eventData)
    {
        if (disabled || !pressed) return;

        var instance = Instantiate(Prototype, parent.transform.position, Quaternion.identity);
        GameManager.Instance.TurretBuilt(instance);

        rangeSprite.SetActive(false);
        parent.SetActive(false);
    }

    // Método Start se llama antes del primer frame.
    void Start()
    {
        // Obtiene referencias a los componentes y objetos necesarios.
        parent = GetComponentInParent<BuildLocationScript>().gameObject;
        image = GetComponent<Image>();
        text = transform.Find("Name").gameObject.GetComponent<Text>();
        price = transform.Find("Price").gameObject.GetComponent<Text>();
        rangeSprite = parent.transform.Find("Range").gameObject;
        range = Prototype.transform.Find("Cannon").GetComponent<CannonScript>().Range;
    }
}
