using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Basically all the static ish data needed to manage the game
/// </summary>
public class GameState
{
    // TODO abstract this a bit more, to simplify planet loading

    /// <summary>
    /// Player level, as important a thing as their is
    /// </summary>
    public static int _level { get; set; } = 0;

    // collection targets for each level
    /// <summary>
    /// Collection targets for each level...level DIV 5...this covers 30 levels
    /// </summary>
    public static int[] _levelGoals { get; set; } = new[] { 5, 10, 15, 20, 25 };

    /// <summary>
    /// Contains the timer lists for levels...level DIV 5...this covers 30 levels
    /// </summary>
    public static int[] _levelTimers { get; set; } = new[] { 60, 45, 30, 20, 10 };

    /// <summary>
    /// Ordered list of planets, but drawn randomly
    /// </summary>
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

    /// <summary>
    /// Background color for each planet listed above, in the same order
    /// </summary>
    public static Color[] _skyboxList { get; set; } = new[]
    {
        // Alien
        new Color(0,160,240,0),
        new Color(255,157,113,0),
        new Color(0, 183, 253, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 182, 253, 0),
        new Color(0, 176, 252, 0),
        new Color(35, 151, 247, 0),     // option 41 168 109
        new Color(0, 90, 22, 0),        // option 94 123 223
        new Color(138, 175, 65, 0),     // option 0 172 252

        // Earth
        new Color(65, 71, 86, 0),       //
        new Color(0, 91, 157, 0),       // option 52 74 83
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),

        // Tundra
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),

        // Desert
        new Color(105, 162, 244, 0),    // option 33 182 123
        new Color(145, 139, 192, 0),    // option 0 187 223, 3 166 144
        new Color(133, 124, 231, 0),    // option 148 151 236, 77 177 130, 0 161 107
        new Color(0, 181, 213, 0),      //
        new Color(0, 194, 252, 0),      //

        // Frozen
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),

        // Temperate
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0),
        new Color(0, 165, 240, 0)

    };

    /// <summary>
    /// Not working yet - song list
    /// </summary>
    public static string[] _songList { get; set; } = new[]
    {
        "Music/gypsy1",
        "Music/gypsy2",
        "Music/gypsy3"
    };

    /// <summary>
    /// Contains the level type...level mod 6
    /// </summary>
    public static string[] _levelType { get; set; } = new[]
    {
        "Collection",
        "Collection TT",
        "Find",
        "Find TT",
        "Clearcut",
        "Clearcut TT"
    };


}
