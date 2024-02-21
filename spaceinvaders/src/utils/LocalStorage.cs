using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LocalStorage {
    private static readonly string jsonString = File.ReadAllText("localstorage/data.json");

    public static void AddUser(User user) {
        JObject data = JsonConvert.DeserializeObject<JObject>(jsonString);

        JObject newUser = new()
        {
            ["name"] = user.Name,
            ["score"] = user.Score
        };

        data.Add(newUser);

        string newJsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText("localstorage/data.json", newJsonString);
    }

    public static List<User> GetUsersPaginator(int quantity, int page=0) {
        JObject data = JsonConvert.DeserializeObject<JObject>(jsonString);

        List<User> usersList = [];

        int INITIAL_VALUE = page * quantity;

        for (int i = INITIAL_VALUE; i < INITIAL_VALUE * 2; i++)
        {
            User user = data[i].ToObject<User>();
            usersList.Add(user);
        }

        return usersList;
    }
}