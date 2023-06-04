using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    public static string generateId()
    {
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
        int charAmount = Random.Range(7, 7); //set those to the minimum and maximum length of your string
        string id = "";
        for (int i = 0; i < charAmount; i++)
        {
            id += glyphs[Random.Range(0, glyphs.Length)];
        }

        return id;
    }
}
