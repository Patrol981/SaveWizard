# Save Wizard
Appliaction for backing up issues from github

## Features
- Login via <b>*GitHub Personal Access Token*</b> and get list of all your <b>*private repositories*</b>
- Select by Id repository in order to save <b>*issues*</b> of an repository following by Aes GCM encryption
- Print all your most recent backups *(displaying one for each of backups)*
- Delete backup if not needed anymore
- Restore encrypted data as <b>*new*</b> repository origin named after <b>*repository name*</b> followed by <b>*creation time*</b>

## Example usage
```sh
print repos
select repo some_repo_id
print backups
select backup some_backup_id
```

## How to run
"As is" inside SaveWizard.Presentation/Console
```shell
$ dotnet ef database drop //optional
$ dotnet ef database update // optional
$ dotnet run
```
If compiled just run the .exe

## Db Location
After either updating db or launching app database will be created in local application data 
*(AppData\Local\wizard.db for Windows users)*
