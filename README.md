Pour lancer la solution dans visual Studio community :
- choisir la solution Quoridor.sln dans le répertoire Sources
- la faire tourner sous la windows machine

Pour faire le .exe
-dotnet publish Sources/Quoridor.sln -f net9.0-windows10.0.19041.0 -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -p:UseMonoRuntime=false -p:WindowsPackageType=None
Problemes en console :
-Problème d'affichage des murs en console,

Problemes en maui :
- pas de possibilité de relancé une partie sauvegarder car pas de séléction sur la liste de game sauvegardée
- pas de choix du bo fonctionnel
- quelques problèmes d'affichage de l'interfaces(textes coupés, bouton qui n'apparaissent pas)
- pas de bot 
- pas de possibilité de relancer une partie à partir de la page de fin
- probleme de retour au niveau des pages 1vs1Page et sauvegardePage
