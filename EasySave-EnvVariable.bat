@echo off
setlocal enabledelayedexpansion

REM Verifie si le script est execute en tant qu'administrateur
>nul 2>&1 net session || (
    echo Le script necessite des privileges administratifs. Redemarrez en tant qu'administrateur.
    pause
    exit /b 1
)

REM Chemin du repertoire contenant EasySave.exe (demande a l'utilisateur)
set /p "easySavePath=Entrez le chemin du repertoire contenant EasySave.exe: "

REM Verifie si le chemin specifie est un repertoire existant
if not exist "!easySavePath!\" (
    echo Le chemin specifie n'est pas valide. Assurez-vous qu'il pointe vers un repertoire existant.
    exit /b 1
)

REM Obtient la valeur actuelle de la variable d'environnement PATH
set "currentPath=!PATH!"

REM Verifie si le chemin n'est pas deja present dans la variable PATH
if not "!currentPath!"=="!currentPath:%easySavePath%=!" (
    echo Le chemin est deja present dans la variable PATH.
) else (
    REM Ajoute le chemin au debut de la variable PATH
    set "newPath=!easySavePath!;!currentPath!"

    REM Met a jour la variable d'environnement PATH
    setx PATH "!newPath!" /M

    echo Le chemin a ete ajoute a la variable PATH.
)

pause
