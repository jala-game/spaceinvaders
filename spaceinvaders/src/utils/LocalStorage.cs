using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LocalStorage {
    private static readonly string jsonString = File.ReadAllText("localstorage/data.json");

    public static void AddUser(User user) {
        JArray data = JsonConvert.DeserializeObject<JArray>(jsonString);

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
        JArray data = JsonConvert.DeserializeObject<JArray>(jsonString);

        int totalUsers = data.Count;
        int maxPage = (int)Math.Ceiling((double)totalUsers / quantity);

        if (page > maxPage)
        {
            return [];
        }

        int startIndex = page * quantity;
        startIndex = Math.Min(startIndex, totalUsers);
        int endIndex = Math.Min(startIndex + quantity, totalUsers);

        List<User> usersList = [];
        for (int i = startIndex; i < endIndex; i++)
        {
            User user = data[i].ToObject<User>();
            usersList.Add(user);
        }

        return usersList;
    }
}