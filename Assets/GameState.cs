using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hold player progression data for scene management
public static class GameState
{

    // player level 
    public static int _level { get; set; } = 0;
    // collection targets for each level
    public static int[] _levelGoals { get; set; } = new[] { 5, 10, 15, 20, 25 };

}
