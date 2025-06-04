namespace API.Middlewares;
public class StringValidator
{
    public string GetLastIndexValue(string value)
    {
        string[] arr = value.Split("/");
        string[] lastIndex = arr[^1].Split(".");
        return lastIndex[0];
    }
}
