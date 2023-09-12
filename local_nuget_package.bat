dotnet pack -c Release
mkdir C:\Users\baran\local-nuget-feed
copy bin\Release\NimbleMediator.1.0.0.nupkg C:\Users\baran\local-nuget-feed
dotnet nuget add source C:\Users\baran\local-nuget-feed -n LocalFeed

REM To add the package to a project:
REM dotnet add package NimbleMediator -s %USERPROFILE%\local-nuget-feed
