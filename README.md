# GameOfInfluencers
> You RT or you FAV

## How to run the local app
You will need:
1. Azure Storage Emulator 4.4 or greater. [Download it here](https://azure.microsoft.com/en-us/downloads/)

    Run Storage Emulator with:
    ```bat
    C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator>AzureStorageEmulator.exe start
    ```

2. Share Sql Server LocalDB instance:

    ```bat
    sqllocaldb share mssqllocaldb goflocal
    ```

3. Add \[NETWORK SERVICE] user as dbo to the localdb instance and db in the logins folder (from SSMS)

## The easy way

You will find a [initservices.bat](initservices.bat) file that does the initialisation work for you, just execute it under *admin* privileges.