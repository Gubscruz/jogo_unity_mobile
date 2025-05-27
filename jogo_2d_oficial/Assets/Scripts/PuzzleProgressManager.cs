using System.Collections.Generic;
using UnityEngine;

public class PuzzleProgressManager : MonoBehaviour
{
    public static PuzzleProgressManager Instance { get; private set; }

    public string[] puzzleIds;

    private readonly HashSet<string> solved = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarkSolved(string puzzleId) => solved.Add(puzzleId);

    public bool IsSolved(string puzzleId) => solved.Contains(puzzleId);

    public bool AllSolved(params string[] ids)
    {
        foreach (var id in ids)
            if (!solved.Contains(id)) return false;
        return true;
    }
    
    public void ResetProgress()
    {
        solved.Clear();
    }
}