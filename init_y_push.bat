@echo off
REM ============================================================
REM Script de inicializacion y push del proyecto UHabitacional
REM Ejecutalo desde la carpeta raiz del proyecto:
REM   D:\alejandra\diplomado\claude\UHabitacional\Uhabitacional
REM
REM El repo remoto YA tiene LICENSE y .gitignore, por eso el
REM script hace un merge con --allow-unrelated-histories y
REM resuelve los conflictos prefiriendo los archivos LOCALES
REM (estrategia -X ours), para que tu .gitignore .NET gane.
REM ============================================================

setlocal

echo.
echo === Verificando estado de git ===
if exist ".git" (
    echo El repositorio ya esta inicializado. Saltando "git init".
) else (
    echo Inicializando repositorio en la rama main...
    git init -b main
    if errorlevel 1 (
        REM Compatibilidad con versiones viejas de git que no soportan -b
        git init
        git checkout -b main
    )
)

echo.
echo === Configurando remoto origin ===
git remote remove origin 2>nul
git remote add origin git@github.com:AlejandraEscobar3G/UHabitacional.git
git remote -v

echo.
echo === Configurando identidad (solo para este repo) ===
git config user.email "ale.fesico@gmail.com"
git config user.name "AlejandraEscobar3G"

echo.
echo === Agregando archivos locales (respetando .gitignore) ===
git add .

echo.
echo === Estado antes del commit ===
git status --short

echo.
echo === Creando commit inicial local ===
git commit -m "Commit inicial: proyecto UHabitacional (API + MVC + Database + design + presentacion)"
if errorlevel 1 (
    echo No habia nada que comitear o ya existia el commit. Continuamos.
)

echo.
echo === Trayendo el historial remoto (LICENSE y .gitignore previos) ===
git fetch origin

echo.
echo === Haciendo merge con el remoto (prefiriendo archivos locales en conflictos) ===
git merge origin/main --allow-unrelated-histories --no-edit -X ours
if errorlevel 1 (
    echo.
    echo *** Hubo problemas en el merge. Revisa con: git status
    echo *** Resuelvelos manualmente, haz "git add ." y luego:
    echo ***   git commit -m "Merge con remoto"
    echo ***   git push -u origin main
    pause
    exit /b 1
)

echo.
echo === Subiendo al remoto (push) ===
git push -u origin main
if errorlevel 1 (
    echo.
    echo *** El push fallo. Posibles causas:
    echo ***   1. Las credenciales SSH no estan configuradas correctamente.
    echo ***   2. El repositorio remoto avanzo mientras tanto.
    echo *** Si es lo segundo, ejecuta:  git pull --rebase origin main  y luego  git push
    pause
    exit /b 1
)

echo.
echo === Listo. Proyecto subido a GitHub ===
echo Repo: https://github.com/AlejandraEscobar3G/UHabitacional
pause
endlocal
