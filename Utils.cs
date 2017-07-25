using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Utils : MonoBehaviour
{

    #region efficiency
    public static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }

    public static float DistSq(Vector3 pos1, Vector3 pos2)
    {
        return (pos1 - pos2).sqrMagnitude;
    }
    #endregion

    public static string GetStringBetweenChars(string source, Char[] charA, Char[] charB)
    {
        string snippet = "";

        int indexA = source.IndexOfAny(charA);
        int indexB = source.IndexOfAny(charB);
        snippet = source.Substring(indexA + 1, (indexB - indexA) - 1);

        return snippet;
    }

    #region string matches
    public static bool StringListMatch(List<string> listToSearchIn, List<string> list)
    {
        foreach(string s in list)
        {
            if (listToSearchIn.Contains(s)) return true;
        }
        return false;
    }

    public static bool StringListMatch(List<string> listToSearchIn, string s)
    {
        if (listToSearchIn.Contains(s)) return true;        
        return false;
    }
    #endregion



    #region csv control
    

    public static string[,] GenerateArrayFromTxt(TextAsset txt)
    {
        string[] rows = txt.text.Split("\n"[0]);

        int maxRows = rows.Length;
        int maxCols = 0;

        foreach (string r in rows) //find maxCols
        {
            string[] cols = r.Split(","[0]);

            if (cols.Length > maxCols)
            {
                maxCols = cols.Length;
            }
        }
        
        string[,] output = new string[maxCols, maxRows];

        for (int r = 0; r < rows.Length; r++) //write values
        {
            string[] cols = rows[r].Split(","[0]);
            

            for (int c = 0; c < cols.Length; c++)
            {
                output[c, r] = cols[c].Trim();
            }
        }

        return output;
    }

    public static int GetAxisLength(string[,] arr, int axis, int index)
    {
        int length = 0;
        for(int i = 0; i < arr.GetLength(axis); i++)
        {
            int x = i;
            int y = i;

            if (axis == 0)
            {
                y = index;
            }
            else
            {
                x = index;
            }

            if (arr[x, y] != "")
            {
                length++;
            }
            else break;
        }

        return length;
    }

    public static int StringIndexFindInAxis(string toFind, string[,] arr, int axis, int index, int startAt, bool partial = false) //0 = row, 1 = column
    {
        int result = -1;

        for (int i = startAt; i < arr.GetLength(axis); i++)
        {
            int x = i;
            int y = i;

            if (axis == 0)
            {
                y = index;
            }
            else
            {
                x = index;
            }

            //print(toFind + ":::" + arr[col, i]);
            if (partial)
            {
                if (toFind.StartsWith(arr[x, y])) //gets partial match. could put a bool in this to get exact or partial match
                {
                    result = i;
                    return result;
                }
            }
            else
            {
                if (arr[x, y] == toFind)
                {
                    result = i;
                    return result;
                }
            }
        }

        return result;
    }

    public static List<string> CropListFromArray(string[,] arr, int row, int col, int length)
    {
        List<string> result = new List<string>();

        for(int i = 0; i < length; i++)
        {
            result.Add(arr[row + i, col]);
        }

        return result;
    }

    public static List<int> CropListFromArray_Int(string[,] arr, int row, int col, int length)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < length; i++)
        {
            int temp = 0;
            if (arr[row + i, col].Contains("-"))
            {
                temp = 0 - IntParseFast(arr[row + i, col].Substring(1));
            }
            else
            {
                temp = IntParseFast(arr[row + i, col]);
            }
            result.Add(temp);
        }

        return result;
    }

    #endregion
}
