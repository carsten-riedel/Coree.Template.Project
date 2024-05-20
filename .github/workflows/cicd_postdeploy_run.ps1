
######################################################################################
Log-Block -Stage "Call" -Section "Dispatch" -Task "dispatching a other job"

if ($branchNameSegment -ieq "master") {
    $worklowFileName = "pages.yml"
    $uri = "https://api.github.com/repos/$gitOwner/$gitRepo/actions/workflows/$worklowFileName/dispatches"
    $headers = @{
        "Accept" = "application/vnd.github+json"
        "X-GitHub-Api-Version" = "2022-11-28"
        "Authorization" = "Bearer $PAT"
        "Content-Type" = "application/json"
    }
    $body = @{
        ref = "$branchName"
    } | ConvertTo-Json
    
    Invoke-WebRequest -Uri $uri -Method Post -Headers $headers -Body $body -Verbose | Out-Null
}