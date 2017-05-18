## Avant d'exécuter l'application web
* Le code utilise une variable d'environement pour conserver la chaine de connexion (voir exemple de chaine dans app.web/appsettings.json)
* Démarrer la BD postgres*
* Faire un build
* Sélectionner le projet app.persistence
* Dans la console "Package Manager", choisir default project: app.persistence
* Créer une première migration (Add-Migration)
* Mettre à jour la BD en exécutant la migration (Update-Database)
* Exécuter l'application web

## Fichier launchSetting.json
* voir launchSettings.json de app.web


