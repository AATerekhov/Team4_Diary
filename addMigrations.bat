@echo off
SET MIGRATION_NAME=%1
IF "%MIGRATION_NAME%"=="" (
    echo "Migration name is required."
    exit /b 1
)
dotnet ef migrations add %MIGRATION_NAME% --project "C:\Users\ainur\source\repos\Diary2\Diary\Diary.DataAccess" --startup-project "C:\Users\ainur\source\repos\Diary2\Diary\Diary" --context "EfDbContext"