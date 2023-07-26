# Fluid Database

A simple key value pair database for int, float, string, and bool. Easily extendable to custom database types via generics. It is non-implementation specific.

## Usage

```csharp
var db = new DatabaseInstance();

# Edit values
db.Strings.Set("myKey", "my value")
var myVal = db.Strings.Get("myKey", "optional fallback value");

# Supports Bools, Strings, Ints, and Floats

# Do whatever you want with the save
var save = db.Save();

# Load the save
var newDb = new DatabaseInstance();
newDb.Load(save);
```

For full API docs see the following files.

* [DatabaseInstance](https://github.com/ashblue/fluid-database/blob/develop/Assets/com.fluid.database/Runtime/DatabaseInstance.cs)
* [KeyValueDataBase](https://github.com/ashblue/fluid-database/blob/develop/Assets/com.fluid.database/Runtime/KeyValueData/KeyValueDataBase.cs)

### Unity Global Database Object

There is also a global database object you can instantly load into your game with zero effort.

```csharp
GlobalDatabaseManager
    # Calling `Instance` lazy loads a global instance with nothing in it
    .Instance
    .Database
    .Strings.Set("myKey", "myVal)
```

This is useful if you want to maintain a global do not destroy instance between scenes. It also supports saving and loading. Full docs [here](https://github.com/ashblue/fluid-database/blob/develop/Assets/com.fluid.database/Runtime/Globals/GlobalDatabaseManager.cs).

## Installation

Fluid Database is used through [Unity's Package Manager](https://docs.unity3d.com/Manual/CustomPackages.html). In order to use it you'll need to add the following lines to your `Packages/manifest.json` file. After that you'll be able to visually control what specific version of Fluid Database you're using from the package manager window in Unity. This has to be done so your Unity editor can connect to NPM's package registry.

```json
{
  "scopedRegistries": [
    {
      "name": "NPM",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.fluid"
      ]
    }
  ],
  "dependencies": {
    "com.fluid.database": "1.0.0"
  }
}
```

## Releases

Archives of specific versions and release notes are available on the [releases page](https://github.com/ashblue/fluid-database/releases).

## Nightly Builds

To access nightly builds of the `develop` branch that are package manager friendly, you'll need to manually edit your `Packages/manifest.json` as so. 

```json
{
    "dependencies": {
      "com.fluid.database": "https://github.com/ashblue/fluid-database.git#nightly"
    }
}
```

Note that to get a newer nightly build you must delete this line and any related lock data in the manifest, let Unity rebuild, then add it back. As Unity locks the commit hash for Git urls as packages.

## Development Environment

If you wish to run to run the development environment you'll need to install the latest [node.js](https://nodejs.org/en/). Then run the following from the root once.

`npm install`

If you wish to create a build run `npm run build` from the root and it will populate the `dist` folder.

### Making Commits

All commits should be made using [Commitizen](https://github.com/commitizen/cz-cli) (which is automatically installed when running `npm install`). Commits are automatically compiled to version numbers on release so this is very important. PRs that don't have Commitizen based commits will be rejected.

To make a commit type the following into a terminal from the root

```bash
npm run commit
```

---

This project was generated with [Oyster Package Generator](https://github.com/ashblue/oyster-package-generator).

