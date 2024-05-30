<p align="center">

</p>
<p align="center">
  <img src="https://static.vecteezy.com/system/resources/previews/028/033/738/original/command-prompt-icon-free-vector.jpg"/>
    <h1 align="center">SIMPLE-SHELL-AND-FILE-SYSTEM-FAT</h1>
</p>
<p align="center">
    <img src="https://img.shields.io/github/last-commit/Omar7001-B/Simple-Shell-And-File-System-FAT?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
    <img src="https://img.shields.io/github/languages/top/Omar7001-B/Simple-Shell-And-File-System-FAT?style=flat&color=0080ff" alt="repo-top-language">
    <img src="https://img.shields.io/github/languages/count/Omar7001-B/Simple-Shell-And-File-System-FAT?style=flat&color=0080ff" alt="repo-language-count">
</p>

## Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [Usage](#usage)
- [Commands](#commands) 💻
  - [General](#general) ℹ️
  - [Directory](#directory) 📁
  - [File](#file) 📄
  - [Debug](#debug) 🛠️
- [Repo Structure](#repo-structure) 📁
- [Modules](#modules) 🧩
  - [ShellSystem](#shellsystem) 💼
  - [Disk](#disk) 💾
  - [Entry](#entry) 📝
- [Lessons Learned](#lessons-learned) 📚
- [Demo Video](#demo-video) 📹
- [Contribution](#contribution-) 🤝


## Overview

Simple Shell and File System Program offers efficient file system management and navigation. It provides a range of commands for directory manipulation, file handling, and debugging tasks. Users can easily execute commands, access detailed help, and exit the shell.

## Installation

1. Clone the repository:
   ~~~bash
   git clone https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT.git
   ~~~
   
2. Navigate to the project directory:
   ~~~bash
   cd Simple-Shell-And-File-System-FAT
   ~~~

3. Compile and run the program:
   ~~~bash
   dotnet run
   ~~~

## Usage

1. After running the program, you'll be prompted with a shell interface.
2. Enter a command followed by any required arguments.
3. Use the `help` command to see detailed information about each command.
4. Use the `quit` command to exit the shell.

# Commands

### General

| Command | Description                                   |
| ------- | --------------------------------------------- |
| help    | Provides Help information for commands.      |
| cls     | Clear the screen.                            |
| quit    | Quit the shell.                              |

### Directory

| Command | Description                                   |
| ------- | --------------------------------------------- |
| cd      | Changes the current directory.               |
| dir     | List the contents of directory.              |
| copy    | Copies one or more entry to another location.|
| cut     | Cut one or more entry to another location.   |
| md      | Creates a directory.                         |
| rd      | Removes a directory.                         |
| rename  | Renames a file.                              |

### File

| Command | Description                                   |
| ------- | --------------------------------------------- |
| echo    | Displays text or variables, write or append to files.|
| type    | Displays the contents of a text file.       |
| del     | Deletes one or more files.                   |
| import  | Import text file(s) from your computer.      |
| export  | Export text file(s) to your computer.        |

### Debug

| Command | Description                                   |
| ------- | --------------------------------------------- |
| meta    | Show meta data of a file.                    |
| tree    | List the tree of a directory.                |
| fat     | Shows The Fat File System.                   |
| mds     | Creates n directories.                       |
| rds     | Removes n directories.                       |


## Repo Structure
~~~
├── Program.cs
│
├── 📁 Disk
│   ├─ FatTable.cs
│   └─ VirtualDisk.cs
│
├── 📁 Entry
│   ├─ Directory.cs
│   ├─ DirectoryEntry.cs
│   └─ FileEntry.cs
│
├── 📁 ShellSystem
│   ├─ FileSystem.cs
│   └─ Shell.cs
│
└── 📁 bin
    └── 📁 Debug
        └── 📁 net6.0
            ├─ Disk.txt
            ├─ Exports
            └─ Imports
~~~

## 🧩 Modules

### ShellSystem

| File                                                                                                                  | Summary                                               | Documentation                                                  |
| ---                                                                                                                   | ---                                                   | ---                                                             |
| [FileSystem.cs](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/ShellSystem/FileSystem.cs) | Handles file system operations.                        | [Documentation](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Documentation/ShellSystem/FileSystem.cs.md)                     |
| [Shell.cs](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/ShellSystem/Shell.cs)           | Implements the shell interface.                        | [Documentation](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Documentation/ShellSystem/Shell.cs.md)                           |


### Disk

| File                                                                                                             | Summary                                         | Documentation                                            |
| ---                                                                                                              | ---                                             | ---                                                       |
| [VirtualDisk.cs](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Disk/VirtualDisk.cs) | Manages virtual disk operations.               | [Documentation](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Documentation/Disk/VirtualDisk.cs.md)                     |
| [FatTable.cs](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Disk/FatTable.cs)       | Implements the FAT file system.                 | [Documentation](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Documentation/Disk/FatTable.cs.md)   

#### Entry

| File                                                                                                                    | Summary                                             | Documentation                                                |
| ---                                                                                                                     | ---                                                 | ---                                                           |
| [Directory.cs](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Entry/Directory.cs)           | Handles directory operations.                      | [Documentation](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Documentation/Entry/Directory.cs.md)                         |
| [DirectoryEntry.cs](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Entry/DirectoryEntry.cs) | Represents directory entries.                      | [Documentation](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Documentation/Entry/DirectoryEntry.cs.md)                    |
| [FileEntry.cs](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Entry/FileEntry.cs)           | Manages file entries.                              | [Documentation](https://github.com/Omar7001-B/Simple-Shell-And-File-System-FAT/blob/master/Documentation/Entry/FileEntry.cs.md)                        |
                     




## Lessons Learned
- Object-oriented programming
- File system management
- File I/O operations
- Data structures (e.g., dictionaries, lists)
- Software testing (unit testing)
- Directory and file manipulation
- String manipulation
- Recursive algorithms

## Demo Video

[Embed or link to the demo video]

# Contribution 🤝

We welcome contributions from the community. Please fork the repository and submit pull requests for any improvements or bug fixes.
