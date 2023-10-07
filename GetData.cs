using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetData : MonoBehaviour
{
    private string apiUrl = "API_URL_BURAYA_GIRIN";

    [System.Serializable]
    public class YourDataModel
    {
        // API'nin d�nd��� veri yap�s�na uygun alanlar� burada tan�mlay�n.Mesela string veri geliyorsa public string veri1; diye tan�mlayabilirsiniz.
    }

    [System.Serializable]
    public class APIResponse
    {
        public List<YourDataModel> results;
    }

    void Start()
    {
        // Verileri �ekmek i�in GetData fonksiyonunu ba�lat
        StartCoroutine(GetDataFromAPI());
    }

    IEnumerator GetDataFromAPI()
    {
        // API'den veri �ekmek i�in UnityWebRequest kullan�l�r
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        // �stek hata verirse, hata mesaj�n� kaydet ve hata mesaj�n� g�ster
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // API'den gelen JSON verisini ��z�mle
            APIResponse responseData = JsonUtility.FromJson<APIResponse>(request.downloadHandler.text);
            YourDataModel data = responseData.results[0];

            //Art�k data.key_ismi yazarak veriyi �ekebilirsiniz.

            //NOT : Bu kod farkl� API verilerine g�re d�zenlenmesi gerekebilir.Bu kod https://opentdb.com/api.php?amount=10&category=23&difficulty=medium&type=multiple adresinden gelen veri d�zenine g�re yaz�lm��t�r.
        }
    }
}