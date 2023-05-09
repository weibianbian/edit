using Newtonsoft.Json.Linq;
public interface IJsonConvertible
{
    void WriteJson(JObject writer);
    void ReadJson(JObject writer);
}
