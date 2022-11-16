param (
      [switch]$codegeneration = $false,
      [switch]$run = $false,
      [switch]$publish = $false,
      [string]$target = $null,
      [switch]$help = $false
)



if ($help) {
      Write-Host "Usage: build.ps1 [-codegeneration] [-run <target>] [-publish] [-target <target>] [-help]"
      Write-Host "  -codegeneration: Generate code from the grammar"
      Write-Host "  -run: Run the application"
      Write-Host "  -publish: Publish the application"
      Write-Host "  -target: The target to publish for"
      Write-Host "  -help: Show this help"
      exit
}

if ($codegeneration) {
   java -jar .\antlr-4.11.1-complete.jar -Dlanguage=CSharp .\Content\Crit.g4 -visitor -encoding utf8 -Xexact-output-dir -o .\Content\.antlr\
   Write-Host "Generated code from grammar!"
}
if ($run) {
   if ($target -eq $null) {
      throw "A file to run must be specified"
   }
   return dotnet run -- $target
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

