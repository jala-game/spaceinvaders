using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LocalStorage {
    private static readonly string jsonString = File.ReadAllText("localstorage/data.json");

    public static void AddUser(User user) {
        JObject data = JsonConvert.DeserializeObject<JObject>(jsonString);

        JArray users = (JArray)data["users"];

        JObject newUser = new()
        {
            ["name"] = user.Name,
            ["score"] = user.Score
        };

        users.Add(newUser);

        string newJsonString = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText("localstorage/data.json", newJsonString);
    }
}