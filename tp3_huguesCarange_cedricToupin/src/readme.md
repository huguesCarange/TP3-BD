## Avant d'ex�cuter l'application web
* Le code utilise une variable d'environement pour conserver la chaine de connexion (voir exemple de chaine dans app.web/appsettings.json)
* D�marrer la BD postgres*
* Faire un build
* S�lectionner le projet app.persistence
* Dans la console "Package Manager", choisir default project: app.persistence
* Cr�er une premi�re migration (Add-Migration)
* Mettre � jour la BD en ex�cutant la migration (Update-Database)
* Ex�cuter l'application web

## Fichier launchSetting.json
* voir launchSettings.json de app.web


