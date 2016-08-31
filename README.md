# ReactiveHistory

[![Gitter](https://badges.gitter.im/wieslawsoltes/ReactiveHistory.svg)](https://gitter.im/wieslawsoltes/ReactiveHistory?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

[![Build status](https://ci.appveyor.com/api/projects/status/9mgwu7obsuh89kys?svg=true)](https://ci.appveyor.com/project/wieslawsoltes/reactivehistory)
[![Build Status](https://travis-ci.org/wieslawsoltes/ReactiveHistory.svg?branch=master)](https://travis-ci.org/wieslawsoltes/ReactiveHistory)

[![NuGet](https://img.shields.io/nuget/v/ReactiveHistory.svg)](https://www.nuget.org/packages/ReactiveHistory) [![MyGet](https://img.shields.io/myget/reactivehistory-nightly/vpre/ReactiveHistory.svg?label=myget)](https://www.myget.org/gallery/reactivehistory-nightly) 

**ReactiveHistory** is an undo/redo framework for .NET.

## Building ReactiveHistory

First, clone the repository or download the latest zip.
```
git clone https://github.com/wieslawsoltes/ReactiveHistory.git
git submodule update --init --recursive
```

### Build using IDE

* [Visual Studio Community 2015](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx) for `Windows` builds.
* [MonoDevelop](http://www.monodevelop.com/) for `Linux` builds.

Open `ReactiveHistory.sln` in selected IDE and run `Build` command.

### Build on Windows using script

Open up a Powershell prompt and execute the bootstrapper script:
```PowerShell
PS> .\build.ps1 -Target "Default" -Platform "Any CPU" -Configuration "Release"
```

### Build on Linux/OSX using script

Open up a terminal prompt and execute the bootstrapper script:
```Bash
$ ./build.sh --target "Default" --platform "Any CPU" --configuration "Release"
```

## NuGet

ReactiveHistory is delivered as a NuGet package.

You can find the packages here [NuGet](https://www.nuget.org/packages/ReactiveHistory/) or by using nightly build feed:
* Add `https://www.myget.org/F/reactivehistory-nightly/api/v2` to your package sources
* Update your package using `ReactiveHistory` feed

You can install the package like this:

`Install-Package ReactiveHistory -Pre`

### Package Dependencies

* System.Reactive
* System.Reactive.Core
* System.Reactive.Interfaces
* System.Reactive.Linq
* System.Reactive.PlatformServices

### Package Sources

* https://api.nuget.org/v3/index.json

## License

ReactiveHistory is licensed under the [MIT license](LICENSE.TXT).
