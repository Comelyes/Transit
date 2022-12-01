using System.Net;

namespace Transit.Extensions;

public class ApiRequests
{
    public static string PostRequest(string url, string _data)
    {
        WebRequest request = WebRequest.Create(url);
        request.Method = "POST";
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(_data);
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteArray.Length;
            
        using (Stream dataStream = request.GetRequestStream())
        {
            dataStream.Write(byteArray, 0, byteArray.Length);
        }
        
        WebResponse response =  request.GetResponse();
        string result = GetDataFromStream(response);
        response.Close();
        return result;
    }
    private static string GetDataFromStream(WebResponse _response)
    {
        using (Stream stream = _response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}