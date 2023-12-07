# System Programmation CESI Project

The project consists in designing a backup software called EasySave for the publisher ProSoft. The project comprises several versions:

- [1.0 (CLI)](https://github.com/SysProg-CESI-Groupe-B/SysProg-CESI/releases/tag/v1.0.0)
- [1.1 (CLI)](https://github.com/SysProg-CESI-Groupe-B/SysProg-CESI/releases/tag/v1.1.0)
- 2.0 (GUI) (coming soon)

## Documentation

### Installation
Go to [Releases](https://github.com/SysProg-CESI-Groupe-B/SysProg-CESI/releases) and check the last version. Just download the ".exe" and use it in your terminal. You can also clone the repo and build the executable from source.

There is a [EasySave-EnvVariable.bat](https://github.com/SysProg-CESI-Groupe-B/SysProg-CESI/blob/preprod/EasySave-EnvVariable.bat) that you can you use to add the .exe to your environnement variables (only on Windows, use it only one time, else you have to manually change the path variable)

### Commands comprehension
If you see `[something]` you need to type literally "something", and if it is `<something>` you need to replace this by something which is describe in it.

### Create command
Create a new task

Usage : `easysave.exe create <taskName> <source> <destination> <type>`

### Modify command
Modify an existing task

Usage : `easysave.exe modify <taskName> [name|source|dest|type] <string>`

### Delete command
Delete a task

Usage : `easysave.exe delete <taskName>`

### Execute command
Execute one or more tasks

Usage :
- one task : `easysave.exe execute <taskName>`
- task to task : `easysave.exe execute <taskIdOne>-<taskIdTwo>`
- multiple tasks : `easysave.exe execute <taskIdOne>;<taskIdTwo>;*`

### List command
List all tasks

Usage : `easysave.exe list`

### Help command
Display help

Usage : `easysave.exe help`
