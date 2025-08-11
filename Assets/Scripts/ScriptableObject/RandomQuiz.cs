using Fusion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomQuiz", menuName = "Scriptable Objects/RandomQuiz")]
public class RandomQuiz : ScriptableObject
{
    [SerializeField] List<RandomQuizData> Quiz = new();
}

[Serializable]
class RandomQuizData
{
    public string Quiz;
    public string QuizAnswer;
}