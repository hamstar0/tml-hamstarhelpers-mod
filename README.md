# Mod Helpers

Supplies numerous helper functions, classes, services, utilities, and information for other mods. Features include:
* In-game mod control panel UI for reporting bugs, editing mod configs, and seeing mods' status.
* Descriptive mod tags that may be viewed and edited by anyone, for any mod. Mod searches in the browser may be filtered by these tags.
* Per-world mod locking available via. control panel to prevent mod mismatch mishaps.


## Modders:
Now provides an in-game feedback UI for your mod(s) via. github. Simply add the following lines of code to your Mod class (NO mod dependency required):

```
    public static string GithubUserName { get { return "<your github name>"; } }
    public static string GithubProjectName { get { return "<your github project>"; } }
```

For more additional modder-relevant features, [visit the modder section of the thread](https://forums.terraria.org/index.php?threads/mod-helpers.63670/#modders).

For usage library code usage instructions and documentation, [visit the wiki](https://github.com/hamstar0/tml-hamstarhelpers-mod/wiki).


**Credits:**

* Nvar: Coal item's tile sprite sheet.


[Please support my mods!](https://www.patreon.com/hamstar0)
