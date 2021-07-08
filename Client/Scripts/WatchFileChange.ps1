Write-Host "Started"
$File = ".\wwwroot\css\app.css"

while ($true) {
    $Before = (Get-Item $File).LastWriteTime
    Start-Sleep -Milliseconds 400
    $After = (Get-Item $File).LastWriteTime
    if ($Before -ne $After) {
        Write-Host "$File changed $(Get-Date)"
        " " | Add-Content $File
    } 
}