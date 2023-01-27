using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hold player progression data for scene management
public static class GameState
{
    // TODO abstract this a bit more, to simplify planet loading

    // player level 
    public static int _level { get; set; } = 0;

    // collection targets for each level
    public static int[] _levelGoals { get; set; } = new[] { 5, 10, 15, 20, 25 };

    // assign planets to levels
    public static string[] _prefabList { get; set; } = new[]
    {
        "Prefabs/Planets/Alien/Alien_Design1",
        "Prefabs/Planets/Alien/Alien_Design2",
        "Prefabs/Planets/Alien/Alien_Design3",
        "Prefabs/Planets/Alien/Alien_Design4",
        "Prefabs/Planets/Alien/Alien_Design5",
        "Prefabs/Planets/Alien/Alien_Design6",
        "Prefabs/Planets/Alien/Alien_Design7",
        "Prefabs/Planets/Alien/Alien_Design8",
        "Prefabs/Planets/Alien/Alien_Design9",
        "Prefabs/Planets/Alien/Alien_Design10",

        "Prefabs/Planets/Earth-Like/Earth-Like_Design1",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design2",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design3",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design4",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design5",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design6",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design7",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design8",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design9",
        "Prefabs/Planets/Earth-Like/Earth-Like_Design10",

        "Prefabs/Planets/Tundra/Tundra_Design1",
        "Prefabs/Planets/Tundra/Tundra_Design2",
        "Prefabs/Planets/Tundra/Tundra_Design3",
        "Prefabs/Planets/Tundra/Tundra_Design4",
        "Prefabs/Planets/Tundra/Tundra_Design5",

        "Prefabs/Planets/Desert/Desert_Design1",
        "Prefabs/Planets/Desert/Desert_Design2",
        "Prefabs/Planets/Desert/Desert_Design3",
        "Prefabs/Planets/Desert/Desert_Design4",
        "Prefabs/Planets/Desert/Desert_Design5",

        "Prefabs/Planets/Frozen/Frozen_Design1",
        "Prefabs/Planets/Frozen/Frozen_Design2",
        "Prefabs/Planets/Frozen/Frozen_Design3",
        "Prefabs/Planets/Frozen/Frozen_Design4",
        "Prefabs/Planets/Frozen/Frozen_Design5",

        "Prefabs/Planets/Temperate/Temperate_Design1",
        "Prefabs/Planets/Temperate/Temperate_Design2",
        "Prefabs/Planets/Temperate/Temperate_Design3",
        "Prefabs/Planets/Temperate/Temperate_Design4",
        "Prefabs/Planets/Temperate/Temperate_Design5",
    };

}
