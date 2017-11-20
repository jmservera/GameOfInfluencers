# GameOfInfluencers
> You RT or you FAV

## How to run the local app

This Service Fabric project is prepared to run locally, you don't need to install anything in Azure to
test this project, it will work with just the emulators.

### The easy way

You will find a [initservices.bat](initservices.bat) file that does the initialisation work for you, just execute it under *admin* privileges.

### How does the .bat work
In order to use it with the local emulators you will need:

1. Azure Storage Emulator 4.4 or greater. [Download it here](https://azure.microsoft.com/en-us/downloads/)

    Once downloaded, you will need to run it because the SF project is not a "Cloud Project" so it won't
    run the Storage Emulator automatically. you can execute it with this line:

    ```bat
    C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator>AzureStorageEmulator.exe start
    ```

2. To use a LocalDB instance from SF emulator, you need to share it to the network service, with this command:

    ```bat
    sqllocaldb share mssqllocaldb goflocal
    ```

3. As SF runs under NETWORK SERVICE credentials, you will need to add Add \[NETWORK SERVICE] user (or the name is used in your language)
 as dbo to the localdb instance and db in the logins folder (from SSMS)

    ```sql
    create login [nt authority\network service] FROM windows with DEFAULT_DATABASE=goflocal;use goflocal;exec sp_addrolemember 'db_owner', 'nt authority\network service';
    ```

## Needed settings

Besides the basic configuration, you will also need to define some keys.

* Twitter API: just set the needed values in one of the ApplicationParameters *.xml* file, remember to point the corresponding *PublishProfiles* file.
* Cloud services: you will need to change the database and storage connection string.
