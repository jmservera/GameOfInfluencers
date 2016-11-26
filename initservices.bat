sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
sqllocaldb share mssqllocaldb goflocal
sqllocaldb stop MSSQLLocalDB
sqllocaldb start MSSQLLocalDB

"c:\Program Files\Microsoft SQL Server\110\Tools\Binn\SQLCMD.EXE" -S "(localdb)\.\goflocal" -Q"create database goflocal;"

"c:\Program Files\Microsoft SQL Server\110\Tools\Binn\SQLCMD.EXE" -S "(localdb)\.\goflocal" -d "goflocal" -Q"create login [nt authority\network service] FROM windows with DEFAULT_DATABASE=goflocal;use goflocal;exec sp_addrolemember 'db_owner', 'nt authority\network service';"

"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" start