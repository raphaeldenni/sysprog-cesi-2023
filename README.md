# System Programmation CESI Project

The project consists in designing a backup software called EasySave for the publisher ProSoft. The project comprises several versions:

- [1.0 (CLI)](https://github.com/SysProg-CESI-Groupe-B/SysProg-CESI/releases/tag/v1.0.0)
- 1.1 (CLI) (coming soon)
- 2.0 (GUI) (coming soon)

## Documentation

### Installation
Go to [Releases](https://github.com/SysProg-CESI-Groupe-B/SysProg-CESI/releases) and check the last version. Just download the ".exe" and use it in your terminal. You can also clone the repo and build the executable from source.

### Create command
Create a new task

Usage : `easysave.exe create <taskName> <source> <destination> <type>`

### Modify command
Modify an existing task

Usage : `easysave.exe modify <taskName> [taskName|source|dest|type] <string>`

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
