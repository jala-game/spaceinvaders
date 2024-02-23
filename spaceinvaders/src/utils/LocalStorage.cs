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
        
        List<User> users = ConvertJsonToUserList(data).OrderByDescending(user => user.Score).ToList();

        int totalUsers = users.Count;
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
            User user = users.ElementAt(i);
            usersList.Add(user);
        }

        return usersList;
    }

    private static List<User> ConvertJsonToUserList(JArray data)
    {
        List<User> users = new List<User>();

        foreach (var userData in data)
        {
            users.Add(userData.ToObject<User>()); 
        }

        return users;
    }
}