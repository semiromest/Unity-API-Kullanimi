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
        // API'nin döndüðü veri yapýsýna uygun alanlarý burada tanýmlayýn.Mesela string veri geliyorsa public string veri1; diye tanýmlayabilirsiniz.
    }

    [System.Serializable]
    public class APIResponse
    {
        public List<YourDataModel> results;
    }

    void Start()
    {
        // Verileri çekmek için GetData fonksiyonunu baþlat
        StartCoroutine(GetDataFromAPI());
    }

    IEnumerator GetDataFromAPI()
    {
        // API'den veri çekmek için UnityWebRequest kullanýlýr
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        // Ýstek hata verirse, hata mesajýný kaydet ve hata mesajýný göster
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // API'den gelen JSON verisini çözümle
            APIResponse responseData = JsonUtility.FromJson<APIResponse>(request.downloadHandler.text);
            YourDataModel data = responseData.results[0];

            //Artýk data.key_ismi yazarak veriyi çekebilirsiniz.

            //NOT : Bu kod farklý API verilerine göre düzenlenmesi gerekebilir.Bu kod https://opentdb.com/api.php?amount=10&category=23&difficulty=medium&type=multiple adresinden gelen veri düzenine göre yazýlmýþtýr.
        }
    }
}