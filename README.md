# Mod Helpers

Supplies numerous helper functions, classes, services, utilities, and information for other mods. Features include:
* In-game mod control panel UI for reporting bugs, editing mod configs, and seeing mods' status.
* Descriptive mod tags that may be viewed and edited by anyone, for any mod. Mod searches in the browser may be filtered by these tags.
* Per-world mod locking available via. control panel to prevent mod mismatch mishaps.

Credits:
Nvar: Coal item's tile sprite sheet.

Mod recommendations:
ModdersToolkit = Tons of experimenting and tinkering options for modders.
FKTModSettings = Adds a UI option for editing a mod's configs. Must be implemented per mod via. API/dependency.

Modders: Now provides an in-game feedback UI for your mod(s) via. github. Simply add the following lines of code to your Mod class (NO mod dependency required):

```
    public static string GithubUserName { get { return "<your github name>"; } }
    public static string GithubProjectName { get { return "<your github project>"; } }
```

For more information on additional mod helper features, [visit the homepage](https://forums.terraria.org/index.php?threads/mod-helpers.63670/).

[Support my mods!](https://www.patreon.com/hamstar0)