using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDrawer : MonoBehaviour
{
    public GameObject TextFieldObject; // Referencia al objeto de texto en Unity que mostrará el dinero
    private Text text; // Referencia al componente Text del objeto de texto

    // Método para actualizar el texto mostrado con la cantidad de dinero actual
    public void Draw(int money)
    {
        text.text = money.ToString(); // Actualiza el texto con la cantidad de dinero convertida a string
    }

    // Método llamado cuando el objeto se activa (en el inicio)
    void OnEnable()
    {
        text = TextFieldObject.GetComponent<Text>(); // Obtiene la referencia al componente Text del objeto de texto
    }
}
