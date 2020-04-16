FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env

WORKDIR /app
COPY ./ ./
WORKDIR /app/Publisher
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build-env /app/Publisher/out .
ENTRYPOINT ["dotnet", "Publisher.dll" ]