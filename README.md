[![Build Status](https://codefirst.iut.uca.fr/api/badges/alexis.drai/dice_app/status.svg)](https://codefirst.iut.uca.fr/alexis.drai/dice_app)  
[![Quality Gate Status](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=alert_status&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Bugs](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=bugs&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Code Smells](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=code_smells&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Coverage](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=coverage&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Duplicated Lines (%)](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=duplicated_lines_density&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Lines of Code](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=ncloc&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Maintainability Rating](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=sqale_rating&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Reliability Rating](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=reliability_rating&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Security Rating](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=security_rating&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Technical Debt](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=sqale_index&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
[![Vulnerabilities](https://codefirst.iut.uca.fr/sonar/api/project_badges/measure?project=dice-app&metric=vulnerabilities&token=bf024850973b7556eef0b981a1b838867848005c)](https://codefirst.iut.uca.fr/sonar/dashboard?id=dice-app)
# dice_app: the die throwing app

## To use the app

Open the *DiceAppConsole.sln* solution and navigate to the *App* project. The *Program.cs* file has a `Main()` method that can be launched. 

*If you simply load DiceApp.sln, Visual Studio will not load the App project...*

The console prototype loads a stub with a few small games that you can test, and you can create new everything (with a little patience).

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