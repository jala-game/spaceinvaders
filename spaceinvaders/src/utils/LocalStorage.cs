using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace spaceinvaders.utils;

public abstract class LocalStorage
{
    public static void AddUser(User user)
    {
        var data = JsonConvert.DeserializeObject<JArray>(JsonString());

        JObject newUser = new()
        {
            ["name"] = user.Name,
            ["score"] = user.Score
        };

        data.Add(newUser);

        var newJsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText("localstorage/data.json", newJsonString);
    }

    public static List<User> GetUsersPaginator(int quantity, int page = 0)
    {
        var data = JsonConvert.DeserializeObject<JArray>(JsonString());

        var users = ConvertJsonToUserList(data).OrderByDescending(user => user.Score).ToList();

        var totalUsers = users.Count;
        var maxPage = (int)Math.Ceiling((double)totalUsers / quantity);

        if (page > maxPage) return [];

        var startIndex = page * quantity;
        startIndex = Math.Min(startIndex, totalUsers);
        var endIndex = Math.Min(startIndex + quantity, totalUsers);

        List<User> usersList = [];
        for (var i = startIndex; i < endIndex; i++)
        {
            var user = users.ElementAt(i);
            usersList.Add(user);
        }

        return usersList;
    }

    private static IEnumerable<User> ConvertJsonToUserList(JArray data)
    {
        try
        {
            return data.Select(userData => userData.ToObject<User>()).ToList();
        }
        catch (Exception)
        {
            throw new Exception("Error reading json file");
        }
    }

    private static string JsonString()
    {
        return File.ReadAllText("localstorage/data.json");
    }
}