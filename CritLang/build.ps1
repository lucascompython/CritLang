param (
      [switch]$codegeneration = $false,
      [switch]$run = $false,
      [switch]$publish = $false,
      [string]$target = $null,
      [switch]$help = $false,
      [string]$proj = "CritLang.csproj",
      [switch]$version
)

$versionNumber = "0.2.1"


if ($help) {
      Write-Host "Usage: build.ps1 [-codegeneration] [-run <target>] [-publish] [-target <target>] [-help]"
      Write-Host "  -codegeneration: Generate code from the grammar"
      Write-Host "  -run: Run the application"
      Write-Host "  -publish: Publish the application"
      Write-Host "  -target: The target to publish for"
      Write-Host "  -proj: The project to build"
      Write-Host "  -help: Show this help"
      exit
}
if ($version) {
   Write-Host "CritLang version $versionNumber"
   exit
}

if ($codegeneration) {
   if (!(Test-Path "./antlr-4.*")) {
      Write-host "ANTLR not found!"
      Write-host "Downloading ANTLR 4.13.0..."
      wget "https://www.antlr.org/download/antlr-4.13.0-complete.jar"
      mv "./antlr-4.13.0-complete.jar" "./antlr.jar"
   }
   
   java -jar ./antlr.jar -Dlanguage=CSharp ./Content/Crit.g4 -visitor -encoding utf8 -Xexact-output-dir -o ./Content/.antlr/
   Write-Host "Generated code from grammar!"
}
if ($run) {
   if ($target -eq $null) {
      throw "A file to run must be specified"
   }
   return dotnet run --project $proj $target
}

if ($publish) {
   if ($target -eq $null) {
      throw "Target must be specified"
   }
   $singleFile = "true" # for cross-platform
   $projFile = "CritLangCross.csproj"
   
   if ($target -eq "win-x64" -or $target -eq "linux-x64") {
      $singleFile = "false"
      $projFile = "CritLang.csproj"
   }

   return dotnet publish $projFile -c Release --self-contained true -r $target -p:PublishTrimmed=true -p:PublishReadyToRun=true -p:PublishSingleFile=$singleFile
}


