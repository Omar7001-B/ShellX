<p align="center">
  <img src="https://i.imgur.com/2CX2Qpb.jpg"/>
</p>

<p align="center">
    <h1 align="center">SIMPLE-SHELL-AND-FILE-SYSTEM-FAT</h1>
</p>
<p align="center">
    <img src="https://img.shields.io/badge/version-1.0.0-blue.svg?style=flat&color=0080ff" alt="version">
    <img src="https://img.shields.io/github/last-commit/Omar7001-B/ShellX?style=flat&logo=git&logoColor=white&color=0080ff" alt="last-commit">
    <img src="https://img.shields.io/github/languages/top/Omar7001-B/ShellX?style=flat&color=0080ff" alt="repo-top-language">
    <img src="https://img.shields.io/github/stars/Omar7001-B/ShellX?style=flat&color=0080ff" alt="stars">
    <img src="https://img.shields.io/github/forks/Omar7001-B/ShellX?style=flat&color=0080ff" alt="forks">
    <img src="https://img.shields.io/github/watchers/Omar7001-B/ShellX?style=flat&color=0080ff" alt="watchers">
</p>

## Table of Contents

- [Overview](#overview) 📜
- [Installation](#installation) 🔧
- [Usage](#usage) ⚙️
- [Commands](#commands) 💻
  - [General](#general) ℹ️
  - [Directory](#directory) 📁
  - [File](#file) 📄
  - [Debug](#debug) 🛠️
- [Repo Structure](#repo-structure) 🏗️
- [Modules](#modules) 🧩
  - [ShellSystem](#shellsystem) 💼
  - [Disk](#disk) 💾
  - [Entry](#entry) 📝
- [Lessons Learned](#lessons-learned) 📚
- [Demo Video](#demo-video) 🎥
- [Contribution](#contribution-) 🤝


## Overview

Simple Shell and File System Program offers efficient file system management and navigation. It provides a range of commands for directory manipulation, file handling, and debugging tasks. Users can easily execute commands, access detailed help, and exit the shell.

## Installation

1. Clone the repository:
   ~~~bash
   git clone https://github.com/Omar7001-B/ShellX.git
   ~~~
   
2. Navigate to the project directory:
   ~~~bash
   cd ShellX
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
            ├─ 📁Exports
            └─ 📁Imports
~~~

## Modules

| Module                                                                                                                 | Summary                          | Documentation                                                                                       |
| ---------------------------------------------------------------------------------------------------------------------- | -------------------------------- | ---------------------------------------------------------------------------------------------------- |
| [ShellSystem](https://github.com/Omar7001-B/ShellX/tree/master/ShellSystem)                  | Handles shell and file system operations. | [Documentation](https://github.com/Omar7001-B/ShellX/blob/master/ShellSystem/) |
| [Disk](https://github.com/Omar7001-B/ShellX/tree/master/Disk)                                | Manages virtual disk and FAT operations.  | [Documentation](https://github.com/Omar7001-B/ShellX/blob/master/Disk/) |
| [Entry](https://github.com/Omar7001-B/ShellX/tree/master/Entry)                              | Handles directory and file entries.         | [Documentation](https://github.com/Omar7001-B/ShellX/blob/master/Entry/) |





## Lessons Learned
- Object-oriented programming (OOP)
  - Constructors
  - Inheritance
  - Virtual Classes
  - Override
  - Overload
- Recursive algorithms (e.g., BFS)
- Data structures (e.g., dictionaries, lists)
- File system management
- String manipulation
- File I/O operations
- Directory and file manipulation
- Software testing (unit testing)

## Demo Video

https://github.com/Omar7001-B/ShellX/assets/115028809/54d1e2e2-63f5-4bbb-92bd-08e9aeae2d55



# Contribution 🤝

We welcome contributions from the community. Please fork the repository and submit pull requests for any improvements or bug fixes.
