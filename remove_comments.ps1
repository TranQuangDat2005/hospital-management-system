$files = Get-ChildItem -Path "c:\Users\Admin\Desktop\hospital-management-system" -Filter "*.cs" -Recurse
$regex = '(?m)(?<!https?:)//.*$'
$count = 0

foreach ($file in $files) {
    if ($file.FullName -match "\\obj\\" -or $file.FullName -match "\\bin\\" -or $file.FullName -match "\\Migrations\\") {
        continue
    }

    $content = Get-Content -Path $file.FullName -Raw
    $newContent = [System.Text.RegularExpressions.Regex]::Replace($content, $regex, '')
    
    # Remove empty lines that might have been left behind (optional, but keeps it clean)
    # $newContent = [System.Text.RegularExpressions.Regex]::Replace($newContent, '(?m)^\s*$', '')
    
    if ($content -ne $newContent) {
        Set-Content -Path $file.FullName -Value $newContent
        $count++
    }
}

Write-Output "Cleaned $count files."
