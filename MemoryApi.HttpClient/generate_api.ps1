#Only needed to get around an issue with AutoRest

autorest autorest.md
Remove-Item -Path ./Models -Recurse
$codeFiles = Get-ChildItem . *.cs -Recurse
foreach($file in $codeFiles)
{
    (Get-Content $file.PSPath) |
    ForEach-Object { $_ -replace "using Models;", "using MemoryCore.JsonModels;" } |
    Set-Content $file.PSPath
}