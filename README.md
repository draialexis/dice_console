# dice_app: the die throwing app

+ [To use the app](#to-use-the-app)
  - [Console prototype](#console-prototype)
  - [DiceApp DB context with stub](#diceapp-db-context-with-stub)
    * [Troubleshooting (VS vs .NET EF)](#troubleshooting-vs-vs-net-ef)
+ [Known issues and limitations](#known-issues-and-limitations)
  - [copies of games](#copies-of-games)

## To use the app

### Console prototype

Open the *DiceAppConsole.sln* solution and navigate to the *App* project. The *Program.cs* file has a `Main()` method that can be launched. 

*If you simply load DiceApp.sln, Visual Studio will not load the App project...*

The console prototype loads a stub with a few small games that you can test, and you can create new everything (with a little patience).

### DiceApp DB context with stub

Still in *Program.cs*, we also now have a nacent data layer, using Entity Framework. 

Open the *DiceAppConsole.sln* solution and navigate to the *App* project. The *Program.cs* file has a `Main()` method that can be launched. 

The NuGet packages are managed in files that are versioned, so you shouldn't need to manage the dependencies yourself. *"The Line"* is taken care of too.

However, you do need to create the migrations and DB (and you probably should delete them everytime you want to reload).

First, in Visual Studio's terminal ("Developer PowerShell"), go to *DiceApp/Sources/Data*, and make sure Entity Framework is installed and / or updated.
```
cd Data
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```
Now the migrations and DB. Since we have a `DiceAppDbContext` *and* and `DiceAppDbContextWithStub`, you will need to specify which one to use.
```
cd Data
dotnet ef migrations add dice_app_db --context DiceAppDbContextWithStub
dotnet ef database update --context DiceAppDbContextWithStub --startup-project ../App
```
Replace `DiceAppDbContextWithStub` with `DiceAppDbContext` if you want to launch an app with an empty DB.

You can now run the *App* program, and check out your local DB. 

You may not want to read tables in the debug window -- in which case, just download [DB Brower for SQLite](https://sqlitebrowser.org/dl/) and open the *.db* file in it.

Ta-da.

#### Troubleshooting (VS vs .NET EF)

**If Visual Studio's embedded terminal refuses to recognize `dotnet ef`, try to fully close and reopen Visual Studio**

## Known issues and limitations

### copies of games
As of now, this app does not allow making copies of a game. We're not trying to make a roguelike, it's just not considered to be a priority feature.
