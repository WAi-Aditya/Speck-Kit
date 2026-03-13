# Create GitHub issues from specs/001-skill-matrix/tasks.md
# Repo: https://github.com/WAi-Aditya/Speck-Kit/issues
# Prerequisite: Install GitHub CLI (gh) and run: gh auth login

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$JsonPath = Join-Path $ScriptDir "github-issues-from-tasks.json"
$Repo = "WAi-Aditya/Speck-Kit"

if (-not (Test-Path $JsonPath)) {
    Write-Error "Not found: $JsonPath"
    exit 1
}

# Check for gh
$gh = Get-Command gh -ErrorAction SilentlyContinue
if (-not $gh) {
    Write-Host "GitHub CLI (gh) is not installed or not in PATH."
    Write-Host "Install: https://cli.github.com/"
    Write-Host "Then run: gh auth login"
    exit 1
}

$tasks = Get-Content $JsonPath -Raw | ConvertFrom-Json
$total = $tasks.Count
$created = 0
$failed = 0

Write-Host "Creating $total issues in $Repo ..."
foreach ($t in $tasks) {
    $title = $t.title
    $body = $t.body -replace "`r`n", "`n"
    $bodyFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllText($bodyFile, $body, [System.Text.UTF8Encoding]::new($false))
        & gh issue create --repo $Repo --title $title --body-file $bodyFile 2>&1 | Out-Null
        if ($LASTEXITCODE -eq 0) {
            $created++
            Write-Host "  [$created/$total] $($t.id) OK"
        } else {
            $failed++
            Write-Warning "  FAILED $($t.id): $title"
        }
    } catch {
        $failed++
        Write-Warning "  FAILED $($t.id): $title"
    } finally {
        Remove-Item $bodyFile -Force -ErrorAction SilentlyContinue
    }
}

Write-Host "Done. Created: $created, Failed: $failed"
