# dice_app: the die throwing app

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

## To contribute (workflow)

We are using the feature branch workflow ([details here](https://www.atlassian.com/git/tutorials/comparing-workflows/feature-branch-workflow), or see the summary below)

### 1 - Sync with the remote 

Make sure you're working with the latest version of the project
```
git checkout main
git fetch origin 
git reset --hard origin/main
```

### 2 - Create a new branch

Give your new branch a name referring to an issue (or maybe a group of similar issues)
```
git checkout -b new-feature
```

Regularly, you might want to get all the new code from your main branch, to work with an up-to-date codebase:
```
git pull --rebase origin main
```

### 3 - Code

:fire::technologist::bug::fire:............:white_check_mark:

### 4 - Save your changes to your new branch

For a refresher, see details about `add`, `commit`, `push`, etc. [here](https://www.atlassian.com/git/tutorials/saving-changes)  

It should involve creating a corresponding feature branch on the remote repository
```
git push -u origin new-feature
```

### 5 - Create a Pull Request

On [the repository's main page](https://codefirst.iut.uca.fr/git/alexis.drai/dice_app), or on your new branch's main page, look for a `New Pull Request` button.  

It should then allow you to `merge into: ...:main` and `pull from: ...:new-feature`  

Follow the platform's instructions, until you've made a "work in progress" (WIP) pull request. You can now assign reviewers among your colleagues. They will get familiar with your new code -- and will either accept the branch as it is, or help you arrange it.

## Known issues and limitations

### copies of games
As of now, this app does not allow making copies of a game. We're not trying to make a roguelike, it's just not considered to be a priority feature.
