#/bin/bash
dotnet ef dbcontext scaffold Name=ClientService \
    --startup-project src/Ascalon.ClientService/Ascalon.ClientService.csproj \
    --project src/Ascalon.ClientService.DataBaseContext \
    --table roles --table users --table tasks\
    --data-annotations \
    --context ClientServiceContext \
    --output-dir ../Ascalon.ClientService.DataBaseContext \
    --force Npgsql.EntityFrameworkCore.PostgreSQL