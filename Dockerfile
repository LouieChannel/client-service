FROM docker.pkg.github.com/louiechannel/dotnet-buildpack/dotnet-buildpack:0.1.0 as build
WORKDIR /tmp
COPY /src ./src
COPY /tests ./tests
COPY *.sln ./
RUN mono /nuget.exe restore
RUN dotnet publish src/Ascalon.ClientService -c Release -f netcoreapp3.1 -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /tmp/out ./
ENTRYPOINT [ "./Ascalon.ClientService" ]
