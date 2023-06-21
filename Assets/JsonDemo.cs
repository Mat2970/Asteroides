using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using Newtonsoft.Json;

public class JsonDemo : MonoBehaviour
{
    private int score;

    void Start()
    {
        score = 0;
    }

    void Update()
    {
        // Si pulsamos el botón K, incrementamos el score y guardamos en el archivo de guardado
        if (Input.GetKeyDown(KeyCode.K))
        {
            score++;

            // Ruta donde queremos guardar la información
            string saveFilePath = Path.Combine(Application.dataPath, "Json/score.json");

            // Crear un objeto anónimo con los valores a guardar
            var data = new { score = score };

            // Convertir el objeto a formato JSON
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Guardar la cadena de texto en el archivo
            File.WriteAllText(saveFilePath, jsonData);

            Debug.Log("JSON saved to: " + saveFilePath);
        }
    }
}
